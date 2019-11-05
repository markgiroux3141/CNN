using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNetForms
{
    class FCModule
    {
        System.Random rand = new System.Random();

        private float[][] neuronVal;
        private float[][] neuronBias;
        private float[][][] weights;
        private float[][][] weightsDelta;
        private float[][] biasDelta;
        private float[][] neuronDelta;

        private float learningRate;
        private float momentum = 0f;

        private float outputError = 0;

        private int numLayers = 0;
        private int[] numNPerL;

        public float LearningRate
        {
            set
            {
                learningRate = value;
            }
        }

        public float Momentum
        {
            set
            {
                momentum = value;
            }
        }

        public FCModule(int inpSize, int[] neuronsPerLayer)
        {
            numLayers = neuronsPerLayer.Length + 1;
            numNPerL = new int[numLayers];

            numNPerL[0] = inpSize;

            for(int i = 1; i < numLayers; i++)
            {
                numNPerL[i] = neuronsPerLayer[i - 1];
            }

            neuronVal = new float[numLayers][];
            neuronBias = new float[numLayers][];
            biasDelta = new float[numLayers][];
            weights = new float[numLayers][][];
            weightsDelta = new float[numLayers][][];
            neuronDelta = new float[numLayers][];

            for (int i = 0; i < numLayers; i++)
            {
                neuronVal[i] = new float[numNPerL[i]];
                neuronBias[i] = new float[numNPerL[i]];
                biasDelta[i] = new float[numNPerL[i]];
                neuronDelta[i] = new float[numNPerL[i]];
            }
            for (int i = 1; i < numLayers; i++)
            {
                weights[i] = new float[numNPerL[i]][];
                weightsDelta[i] = new float[numNPerL[i]][];
                for (int n = 0; n < numNPerL[i]; n++)
                {
                    weights[i][n] = new float[numNPerL[i - 1]];
                    weightsDelta[i][n] = new float[numNPerL[i - 1]];
                }
            }

            CreateRandomNet();

            learningRate = ConvNetForms.GlobalVar.LEARNING_RATE;
        }

        public void CreateRandomNet()
        {
            for (int i = 1; i < numLayers; i++)
            {
                for (int n = 0; n < numNPerL[i]; n++)
                {
                    neuronBias[i][n] = RandFloat();
                    neuronDelta[i][n] = 0;
                    biasDelta[i][n] = 0;
                    for (int q = 0; q < numNPerL[i - 1]; q++)
                    {
                        weights[i][n][q] = RandFloat();
                        weightsDelta[i][n][q] = 0;
                    }
                }
            }
        }

        public float[] RunNet()
        {
            float[] output = new float[numNPerL[numLayers - 1]];
            float nSum = 0;

            for (int i = 1; i < numLayers; i++)
            {
                for (int n = 0; n < numNPerL[i]; n++)
                {
                    nSum = 0;
                    for (int q = 0; q < numNPerL[i - 1]; q++)
                    {
                        nSum += neuronVal[i - 1][q] * weights[i][n][q];
                    }
                    neuronVal[i][n] = LeRelU(nSum + neuronBias[i][n]);

                    if (i == numLayers - 1)
                        neuronVal[i][n] *= 0.001f;
                }
            }

            for (int n = 0; n < output.Length; n++)
            {
                output[n] = neuronVal[numLayers - 1][n];
                //Console.WriteLine("output " + output[n]);
            }

            outputError = 0;

            return output;
        }

        public float Train(float[] target)
        {
            float error = 0;
            for (int i = (numLayers - 1); i > -1; i--)
            {
                for (int n = 0; n < numNPerL[i]; n++)
                {
                    if (i == numLayers - 1)
                    {
                        neuronDelta[i][n] = (neuronVal[i][n] - target[n]);
                        //Console.WriteLine(neuronVal[i][n]);
                        error += Math.Abs(neuronDelta[i][n]);
                    }
                    if (i != numLayers - 1)
                    {
                        for (int q = 0; q < numNPerL[i + 1]; q++)
                        {
                            neuronDelta[i][n] += neuronDelta[i + 1][q] * LeReluDeriv(neuronVal[i + 1][q]) * weights[i + 1][q][n];
                        }
                    }
                }
                outputError = error;
            }
           
            for (int i = numLayers - 1; i > 0; i--)
            {
                for (int n = 0; n < numNPerL[i]; n++)
                {
                    biasDelta[i][n] = neuronDelta[i][n] * LeReluDeriv(neuronVal[i][n]);
                    neuronBias[i][n] -= learningRate * biasDelta[i][n];
                    for (int q = 0; q < numNPerL[i - 1]; q++)
                    {
                        weights[i][n][q] -= momentum * weightsDelta[i][n][q];
                        weightsDelta[i][n][q] = (neuronDelta[i][n] * LeReluDeriv(neuronVal[i][n]) * neuronVal[i - 1][q]);
                        weights[i][n][q] -= learningRate * weightsDelta[i][n][q];
                    }
                    //Console.WriteLine(neuronDelta[i][n]);
                    neuronDelta[i][n] = 0;
                }
            }
            //Console.WriteLine(weightsDelta[1][1][1]);
            return error;
        }

        public float RandFloat()
        {
            return ((float)rand.NextDouble() * 2f) - 1f;
        }

        public float Sigmoid(float x)
        {
            return 1f / (1f + (float)Math.Exp(-x));
        }

        public float SigmoidDeriv(float x)
        {
            return Sigmoid(x) * (1f - Sigmoid(x));
        }

        public float LeRelU(float x)
        {
            if(x < 0)
            {
                return x *= ConvNetForms.GlobalVar.LERELU_VAL;
            }
            else
            {
                return x;
            }
        }

        public float LeReluDeriv(float x)
        {
            if(x < 0)
            {
                return ConvNetForms.GlobalVar.LERELU_VAL;
            }
            else
            {
                return 1f;
            }
        }

        public int GetNumLay()
        {
            return neuronVal.Length;
        }

        public void SetInputLayer(float[] inp)
        {
            neuronVal[0] = inp;
        }

        public float[] GetInputDeltas()
        {
            return neuronDelta[0];
        }

        public float GetError()
        {
            return outputError;
        }

    }
}
