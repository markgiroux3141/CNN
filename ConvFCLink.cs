using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNetForms
{
    class ConvFCLink
    {
        System.Random rand = new System.Random();

        private float[,,] weights;
        private float[,,] weightDeltas;

        private float[,,] input;
        private float[,,] inpDeltas;

        private float[] output;
        private float[] outputDeltas;

        private int inputSize;
        private int inputDepth;

        private float learningRate;

        public ConvFCLink(int inpSize, int inpDepth)
        {

            inputSize = inpSize;
            inputDepth = inpDepth;

            weights = new float[inpSize, inpSize, inpDepth];
            weightDeltas = new float[inpSize, inpSize, inpDepth];

            inpDeltas = new float[inpSize, inpSize, inpDepth];

            output = new float[inputDepth];

            learningRate = ConvNetForms.GlobalVar.LEARNING_RATE;

            initNet();
        }

        private void initNet()
        {
            for (int z = 0; z < inputDepth; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for(int y = 0; y < inputSize; y++)
                    {
                        weights[x,y,z] = Rand();
                    }
                }
            }
        }

        public float[] RunNet()
        {
            for(int z = 0; z < inputDepth; z++)
            {
                output[z] = 0;
                for(int x = 0; x < inputSize; x++)
                {
                    for(int y = 0; y < inputSize; y++)
                    {
                        output[z] += input[x, y, z] * weights[x, y, z];
                    }
                }
            }
            return output;
            //Console.WriteLine(output[0]);
        }

        public float TrainNet()
        {
            float error = 0;

            ZeroInpDeltas();

            for (int z = 0; z < inputDepth; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        weightDeltas[x, y, z] += outputDeltas[z] * input[x, y, z];
                        inpDeltas[x, y, z] += outputDeltas[z] * weights[x, y, z];
                    }
                }
                error += Math.Abs(outputDeltas[z]);
            }

            for (int z = 0; z < inputDepth; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        weights[x, y, z] -= learningRate * weightDeltas[x, y, z];
                        weightDeltas[x, y, z] = 0;
                    }
                }
            }
            return error;
        }

        private void ZeroInpDeltas()
        {
            for (int z = 0; z < inputDepth; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        inpDeltas[x, y, z] = 0;
                    }
                }
            }
        }

        private float Rand()
        {
            return ((float)rand.NextDouble() * 2f) - 1f;
        }

        public void SetInputLayer(float[,,] inp)
        {
            input = inp;
        }

        public void SetOutputDeltas(float[] inp)
        {
            outputDeltas = inp;
        }

        public float[,,] GetInputDeltas()
        {
            return inpDeltas;
        }

        public float[] GetOutputLayer()
        {
            return output;
        }

        public int GetOutputSize()
        {
            return output.Length;
        }

    }
}
