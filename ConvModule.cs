using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNet
{
    class ConvModule
    {
        System.Random rand = new System.Random();

        private float[,,] inpMap;
        private float[,,] inpMapDeltas;

        private float[,,,] filter;
        private float[,,,] filterDeltas;
        private float[] filterBias;
        private float[] filterBiasDeltas;

        private float[,,] featureMap;
        private float[,,] featureMapDeltas;

        private float[,,] pooling;
        private float[,,] maxPoolDeltas;
        private float[,,] poolMask;
        private int[,,] maxPoolInd;

        private float learningRate;
        private int inputSize;
        private int inputDepth;
        private int filterSize;
        private int numFilters;
        private int poolingSize;
        private int filterRadSize;

        public ConvModule(int inpSize, int inpDepth, int NumFilters, int FilterSize)
        {
            inputSize = inpSize;
            inputDepth = inpDepth;
            numFilters = NumFilters;
            filterSize = FilterSize;

            inpMap = new float[inputSize, inputSize, inputDepth];
            inpMapDeltas = new float[inputSize, inputSize, inputDepth];

            poolingSize = inputSize / 2;

            filter = new float[filterSize, filterSize, inputDepth, numFilters];
            filterBias = new float[numFilters];
            filterBiasDeltas = new float[numFilters];
            featureMap = new float[inputSize, inputSize, numFilters];
            pooling = new float[poolingSize, poolingSize, numFilters];
            poolMask = new float[inputSize, inputSize, numFilters];
            maxPoolInd = new int[poolingSize, poolingSize, numFilters];

            filterRadSize = (filterSize - 1) / 2;

            maxPoolDeltas = new float[poolingSize, poolingSize, numFilters];
            featureMapDeltas = new float[inputSize, inputSize, numFilters];
            filterDeltas = new float[filterSize, filterSize, inputDepth, numFilters];

            learningRate = ConvNetForms.GlobalVar.LEARNING_RATE;

            initConvNet();

        }

        public void initConvNet()
        {
            InitializeFilters();
        }

        public void RunNet()
        {
            SetMapsToZero();
            Convolve();
            RelU();
            Pool();
        }

        public float TrainNet()
        {
            float error = 0;
            error = Train();
            return error;
        }

        private void InitializeFilters()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for (int q = 0; q < inputDepth; q++)
                {
                    for (int x = 0; x < filterSize; x++)
                    {
                        for (int y = 0; y < filterSize; y++)
                        {
                            filter[x, y, q, z] = 0.5f + (nRand() * 0.1f);
                            //filter[x, y, q, z] = 0;
                        }
                    }
                }
                filterBias[z] = nRand();
            }
        }

        private void Convolve()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        for (int q = 0; q < inputDepth; q++)
                        {
                            for (int xf = -filterRadSize; xf < filterRadSize + 1; xf++)
                            {
                                for (int yf = -filterRadSize; yf < filterRadSize + 1; yf++)
                                {
                                    if (((x + xf) > -1) && ((x + xf) < inputSize) && ((y + yf) > -1) && ((y + yf) < inputSize))
                                    {
                                        featureMap[x, y, z] += inpMap[x + xf, y + yf, q] * filter[xf + filterRadSize, yf + filterRadSize, q, z];
                                    }
                                }
                            }
                        }
                        featureMap[x, y, z] += filterBias[z];
                    }
                }
            }
        }

        private void RelU()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        if (featureMap[x, y, z] < 0)
                        {
                            featureMap[x, y, z] *= 0.001f;
                        }
                    }
                }
            }
        }

        private void Pool()
        {
            int currMaxInd;
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < inputSize; x += 2)
                {
                    for (int y = 0; y < inputSize; y += 2)
                    {
                        currMaxInd = max4Ind(featureMap[x, y, z], featureMap[x + 1, y, z], featureMap[x, y + 1, z], featureMap[x + 1, y + 1, z]);
                        pooling[x / 2, y / 2, z] = max4(featureMap[x, y, z], featureMap[x + 1, y, z], featureMap[x, y + 1, z], featureMap[x + 1, y + 1, z]);
                        if (currMaxInd < 2)
                        {
                            poolMask[x + currMaxInd, y, z] = 1;
                        }
                        else
                        {
                            poolMask[x + (currMaxInd - 2), y + 1, z] = 1;
                        }
                    }
                }
            }
            MaxPoolIndicies(featureMap);
        }

        private float Train()
        {
            float error = 0;

            //Get Max Pooling Layer Deltas
            for (int z = 0; z < numFilters; z++)
            {
                for (int y = 0; y < poolingSize; y++)
                {
                    for (int x = 0; x < poolingSize; x++)
                    {
                        error += Math.Abs(maxPoolDeltas[x, y, z]);
                    }
                }
            }
            //Get FeatureMap Deltas

            int currX, currY;

            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < poolingSize; x++)
                {
                    for (int y = 0; y < poolingSize; y++)
                    {
                        currX = maxPoolInd[x, y, z] / inputSize;
                        currY = maxPoolInd[x, y, z] % inputSize;

                        featureMapDeltas[currX, currY, z] = maxPoolDeltas[x, y, z];
                        maxPoolDeltas[x, y, z] = 0;
                    }
                }
            }

            //Adjust Filter Weight deltas

            for (int z = 0; z < numFilters; z++)
            {
                for (int r = 0; r < inputDepth; r++)
                {
                    for (int x = -filterRadSize; x < filterRadSize + 1; x++)
                    {
                        for (int y = -filterRadSize; y < filterRadSize + 1; y++)
                        {
                            for (int p = 0; p < inputSize; p++)
                            {
                                for (int q = 0; q < inputSize; q++)
                                {
                                    if ((p + x) > -1 && (q + y) > -1 && (p + x) < inputSize && (q + y) < inputSize)
                                    {
                                        filterDeltas[x + filterRadSize, y + filterRadSize, r, z] += featureMapDeltas[p, q, z] * inpMap[p + x, q + y, r];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adjust Filter bias deltas

            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        filterBiasDeltas[z] += featureMapDeltas[x, y, z];
                    }
                }
            }

            //Get Input Map Deltas

            for (int z = 0; z < inputDepth; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        for (int r = 0; r < numFilters; r++)
                        {
                            for (int p = filterRadSize; p > -filterRadSize + 1; p--)
                            {
                                for (int q = filterRadSize; q > -filterRadSize + 1; q--)
                                {
                                    if (((x + p) > -1) && ((x + p) < inputSize) && ((y + q) > -1) && ((y + q) < inputSize))
                                    {
                                        inpMapDeltas[x, y, z] += featureMapDeltas[x + p, y + q, r] * filter[filterRadSize - p, filterRadSize - q, z, r];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adjust Weights and biases

            for (int z = 0; z < numFilters; z++)
            {
                for (int q = 0; q < inputDepth; q++)
                {
                    for (int x = 0; x < filterSize; x++)
                    {
                        for (int y = 0; y < filterSize; y++)
                        {
                            filter[x, y, q, z] -= learningRate * filterDeltas[x, y, q, z];
                            filterDeltas[x, y, q, z] = 0;
                        }
                    }
                }
                filterBias[z] -= learningRate * filterBiasDeltas[z];
                filterBiasDeltas[z] = 0;
            }
            
            return error;
        }

        private void SetMapsToZero()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < inputSize; x++)
                {
                    for (int y = 0; y < inputSize; y++)
                    {
                        featureMap[x, y, z] = 0;
                        poolMask[x, y, z] = 0;
                    }
                }
            }
        }

        private float max4(float num1, float num2, float num3, float num4)
        {
            float temp1, temp2, temp3;
            temp1 = (num1 > num2) ? num1 : num2;
            temp2 = (num3 > num4) ? num3 : num4;
            temp3 = (temp1 > temp2) ? temp1 : temp2;
            return temp3;
        }

        private int max4Ind(float num1, float num2, float num3, float num4)
        {
            float temp1, temp2, temp3, tempNum1, tempNum2;
            temp1 = (num1 > num2) ? 0 : 1;
            temp2 = (num3 > num4) ? 2 : 3;

            tempNum1 = (num1 > num2) ? num1 : num2;
            tempNum2 = (num3 > num4) ? num3 : num4;

            temp3 = (tempNum1 > tempNum2) ? temp1 : temp2;
            return (int)temp3;
        }

        private float nRand()
        {
            return ((float)rand.NextDouble() * 2f) - 1f;
        }

        private float nRandPos()
        {
            return (float)rand.NextDouble();
        }

        private void MaxPoolIndicies(float[,,] featMap)
        {
            int tempInd, index;
            int size = featMap.GetLength(0);
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < size; x += 2)
                {
                    for (int y = 0; y < size; y += 2)
                    {
                        tempInd = max4Ind(featMap[x, y, z], featMap[x + 1, y, z], featMap[x, y + 1, z], featMap[x + 1, y + 1, z]);

                        if (tempInd < 2)
                        {
                            index = tempInd + (y * size);
                            index += x;
                        }
                        else
                        {
                            index = (tempInd - 2) + ((y + 1) * size);
                            index += x;
                        }
                        maxPoolInd[x / 2, y / 2, z] = index;
                    }
                }
            }
        }

        public void SetOutputDeltas(float[,,] inp)
        {
            maxPoolDeltas = inp;
        }

        public float[,,] GetOutputLayer()
        {
            return pooling;
        }

        public int GetOutputSize()
        {
            return pooling.GetLength(0);
        }

        public int GetOutputDepth()
        {
            return pooling.GetLength(2);
        }

        public void SetInputLayer(float[,,] inp)
        {
            inpMap = inp;
        }

        public float[,,] GetInputMapDeltas()
        {
            return inpMapDeltas;
        }

        public float GetLearningRate()
        {
            return learningRate;
        }

        public void SetLearningRate(float lRate)
        {
            learningRate = lRate;
        }

        public int GetInputSize()
        {
            return inputSize;
        }

        public int GetInputDepth()
        {
            return inputDepth;
        }

        public int GetFilterSize()
        {
            return filterSize;
        }

        public int GetNumFilters()
        {
            return numFilters;
        }

        public float[,,,] GetFilters()
        {
            return filter;
        }

        public float[,,] GetFeatures()
        {
            float[,,] temp = new float[inputSize, inputSize, numFilters];
            temp = featureMap;
            return temp;
        }
    }
}
