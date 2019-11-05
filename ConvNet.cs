using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNet
{
    class ConvNet
    {
        System.Random rand = new System.Random();

        private float[,,] inpImage;
        private float[,,,] filter;
        private float[] filterBias;
        private float[] filterBiasDeltas;
        private float[,,] featureMap;
        private float[,,] pooling;
        private float[,,] poolMask;
        private float[,,] relUMask;
        private int[,,] maxPoolInd;

        private float[,,] target;

        // used for collapsing feature maps into a single node
        private float singleOutPut;
        private float singleTarget;
        private float[,,] weights;
        private float[,,] weightDeltas;
        //

        //max pool to single node of depth z
        private float[] node;
        private float[,,] nodeWeights;
        private float[,,] nodeWeightDeltas;
        private float[] nodeBias;
        private float[] nodeBiasDeltas;
        private int[] nodeId;
        private float[] nodeTargets;

        private float[,,] maxPoolDeltas;
        private float[,,] featureMapDeltas;
        private float[,,,] filterDeltas;

        //Debugging
        private float[,,] debugFeatures;

        private float learningRate;

        private int imageSize = 32;
        private int imageDepth = 3;
        private int filterSize = 3;
        private int numFilters = 2;
        private int poolingSize;
        private int filterRadSize;

        public ConvNet()
        {
            inpImage = new float[imageSize, imageSize, imageDepth];
            poolingSize = imageSize / 2;

            filter = new float[filterSize, filterSize, imageDepth, numFilters];
            filterBias = new float[numFilters];
            filterBiasDeltas = new float[numFilters];
            featureMap = new float[imageSize, imageSize, numFilters];
            pooling = new float[poolingSize, poolingSize, numFilters];
            poolMask = new float[imageSize, imageSize, numFilters];
            relUMask = new float[imageSize, imageSize, numFilters];
            target = new float[poolingSize, poolingSize, numFilters];
            maxPoolInd = new int[poolingSize, poolingSize, numFilters];

            filterRadSize = (filterSize - 1) / 2;

            maxPoolDeltas = new float[poolingSize, poolingSize, numFilters];
            featureMapDeltas = new float[imageSize, imageSize, numFilters];
            filterDeltas = new float[filterSize, filterSize, imageDepth, numFilters];

            weights = new float[poolingSize, poolingSize, numFilters];
            weightDeltas = new float[poolingSize, poolingSize, numFilters];
            singleTarget = 1f;

            node = new float[numFilters];
            nodeWeights = new float[poolingSize, poolingSize,numFilters];
            nodeWeightDeltas = new float[poolingSize, poolingSize, numFilters];
            nodeBias = new float[numFilters];
            nodeBiasDeltas = new float[numFilters];
            nodeId = new int[numFilters];
            nodeTargets = new float[numFilters];

            // Debugging
            debugFeatures = new float[imageSize, imageSize, numFilters];

            learningRate = 0.005f;

        }

        public void initConvNet()
        {
            RandImage();
            InitializeFilters();
            InitializeNodeWeights();
            Convolve();
            RelU();
            Pool();
            //PoolToNode();
            //Console.WriteLine(node[0]);
            //Console.WriteLine(node[1]);
            //DebugFeatures();
            //SetMapsToZero();
        }

        public float RunConvNet()
        {
            float error = 0;
            SetMapsToZero();
            Convolve();
            RelU();
            Pool();
            //CollapseToSTarget();
            PoolToNode();
            error = Train();
            //Console.WriteLine(featureMap[0, 0, 0]);
            return error;
        }

        public void RunConvNetNoTrain()
        {
            SetMapsToZero();
            Convolve();
            RelU();
            Pool();
            //CollapseToSTarget();
            PoolToNode();
        }

        private void RandImage()
        {
            for (int z = 0; z < imageDepth; z++)
            {
                for (int x = 0; x < imageSize; x++)
                {
                    for (int y = 0; y < imageSize; y++)
                    {
                        inpImage[x, y, z] = nRand();
                    }
                }
            }
        }

        public void SetImage(float[,,] inp)
        {
            for (int z = 0; z < inp.GetLength(2); z++)
            {
                for (int x = 0;x < inp.GetLength(0); x++)
                {
                    for (int y = 0; y < inp.GetLength(1); y++)
                    {
                        inpImage[x, y, z] = inp[x, y, z];
                    }
                }
            }
        }

        private void InitializeNodeWeights()
        {
            for(int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < poolingSize; x++)
                {
                    for (int y = 0; y < poolingSize; y++)
                    {
                        nodeWeights[x,y,z] = nRand();
                    }
                }
            }
        }

        private void InitializeFilters()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for(int q = 0; q < imageDepth; q++)
                {
                    for (int x = 0; x < filterSize; x++)
                    {
                        for (int y = 0; y < filterSize; y++)
                        {
                            filter[x, y, q, z] = 0.5f + (nRand() * 0.1f);
                        }
                    }
                }
                filterBias[z] = nRand();
                nodeBias[z] = nRand();
            }
        }

        private void Convolve()
        {
            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < imageSize; x++)
                {
                    for (int y = 0; y < imageSize; y++)
                    {
                        for (int q = 0; q < imageDepth; q++)
                        {
                            for (int xf = -filterRadSize; xf < filterRadSize + 1; xf++)
                            {
                                for (int yf = -filterRadSize; yf < filterRadSize + 1; yf++)
                                {
                                    if (((x + xf) > -1) && ((x + xf) < imageSize) && ((y + yf) > -1) && ((y + yf) < imageSize))
                                    {
                                        featureMap[x, y, z] += inpImage[x + xf, y + yf, q] * filter[xf + filterRadSize, yf + filterRadSize,q,z];
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
                for (int x = 0; x < imageSize; x++)
                {
                    for (int y = 0; y < imageSize; y++)
                    {
                        if (featureMap[x, y, z] < 0)
                        {
                            featureMap[x, y, z] *= 0.001f;

                            relUMask[x, y, z] = 1;
                        }
                        else
                        {
                            relUMask[x, y, z] = 1;
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
                for (int x = 0; x < imageSize; x += 2)
                {
                    for (int y = 0; y < imageSize; y += 2)
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

        private void PoolToNode()
        {
            for(int z = 0; z < numFilters; z++)
            {
                for(int x = 0; x < poolingSize; x++)
                {
                    for(int y = 0; y < poolingSize; y++)
                    {
                        node[z] += (nodeWeights[x,y,z] * pooling[x,y,z]);
                    }
                }
                node[z] += nodeBias[z];
                node[z] /= (poolingSize * poolingSize);
            }
        }

        private void CollapseToSTarget()
        {
            singleOutPut = 0;

            for(int z = 0; z < numFilters; z++)
            {
                for(int x = 0; x < poolingSize; x++)
                {
                    for(int y=0; y < poolingSize; y++)
                    {
                        singleOutPut += weights[x, y, z] * pooling[x,y,z];
                    }
                }
            }
            //singleOutPut = Sigmoid(singleOutPut);
        }

        private float Train()
        {
            float error = 0;

            //Get Max Pooling Layer Deltas
            for(int z = 0; z < numFilters; z++)
            {
                for(int y = 0; y < poolingSize; y++)
                {
                    for(int x = 0; x < poolingSize; x++)
                    {
                        nodeWeightDeltas[x, y, z] = (node[z] - nodeTargets[z]) * pooling[x, y, z];
                        maxPoolDeltas[x, y, z] = (node[z] - nodeTargets[z]) * nodeWeights[x, y, z];
                    }
                }
                nodeBiasDeltas[z] = node[z] - nodeTargets[z];
                error += Math.Abs(node[z] - nodeTargets[z]);
            }
            //Get FeatureMap Deltas

            int currX, currY;

            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < poolingSize; x++)
                {
                    for (int y = 0; y < poolingSize; y++)
                    {
                        currX = maxPoolInd[x, y, z] / imageSize;
                        currY = maxPoolInd[x, y, z] % imageSize;

                        featureMapDeltas[currX, currY, z] = maxPoolDeltas[x, y, z] * relUMask[currX, currY,z];
                        maxPoolDeltas[x, y, z] = 0;
                    }
                }
            }

            //Adjust Filter Weight deltas

            for(int z = 0; z < numFilters; z++)
            {
                for(int r = 0; r < imageDepth; r++)
                {
                    for (int x = -filterRadSize; x < filterRadSize + 1; x++)
                    {
                        for (int y = -filterRadSize; y < filterRadSize + 1; y++)
                        {
                            for (int p = 0; p < imageSize; p++)
                            {
                                for (int q = 0; q < imageSize; q++)
                                {
                                    if ((p + x) > -1 && (q + y) > -1 && (p + x) < imageSize && (q + y) < imageSize)
                                    {
                                        filterDeltas[x + filterRadSize, y + filterRadSize, r, z] += featureMapDeltas[p, q, z] * inpImage[p + x, q + y, r];
                                    }
                                }
                            }
                            //filterDeltas[x + filterRadSize, y + filterRadSize, r, z];
                        }
                    }
                }
            }

            //Adjust Filter bias deltas

            for (int z = 0; z < numFilters; z++)
            {
                for (int x = 0; x < imageSize; x++)
                {
                    for (int y = 0; y < imageSize; y++)
                    {
                        filterBiasDeltas[z] += featureMapDeltas[x, y, z];
                    }
                }
            }

            //Adjust Weights and biases

            for (int z = 0; z < numFilters; z++)
            {
                for(int q = 0; q < imageDepth; q++)
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
            // adjust node weights and biases
            for(int z = 0; z < numFilters; z++)
            {
                for(int x = 0; x < poolingSize; x++)
                {
                    for(int y = 0; y < poolingSize; y++)
                    {
                        nodeWeights[x,y,z] -= learningRate * nodeWeightDeltas[x,y,z];
                        nodeWeightDeltas[x,y,z] = 0;
                    }
                }
                
                nodeBias[z] -= learningRate * nodeBiasDeltas[z];
                nodeBiasDeltas[z] = 0;
            }
            return error;
        }

        private void SetMapsToZero()
        {
            for(int z = 0; z < numFilters; z++)
            {
                for(int x = 0; x < imageSize; x++)
                {
                    for(int y = 0; y < imageSize; y++)
                    {
                        featureMap[x, y, z] = 0;
                        relUMask[x, y, z] = 0;
                        poolMask[x, y, z] = 0;
                    }
                }
                node[z] = 0;
            }
        }

        private float[] FlattenArray(float[,,] inp)
        {
            int xl = inp.GetLength(0);
            int yl = inp.GetLength(1);
            int zl = inp.GetLength(2);

            float[] tempArr = new float[xl * yl * zl];

            for (int z = 0; z < zl; z++)
            {
                for (int y = 0; y < yl; y++)
                {
                    for (int x = 0; x < xl; x++)
                    {
                        tempArr[(xl * yl * z) + y * xl + x] = inp[x, y, z];
                    }
                }
            }
            return tempArr;
        }

        private float maxGeneric(float[,,] num, int zIndex)
        {
            int xLen = num.GetLength(0);
            int yLen = num.GetLength(1);
            float tempMax = 0;
            for (int x = 0; x < xLen; x++)
            {
                for(int y = 0; y < yLen; y++)
                {
                    if(num[x,y,zIndex] > tempMax)
                    {
                        tempMax = num[x, y, zIndex];
                    }
                }
            }
            return tempMax;
        }

        private int maxGenericInd(float[,,] num, int zIndex)
        {
            int xLen = num.GetLength(0);
            int yLen = num.GetLength(1);

            float tempMaxVal = 0;
            int tempMaxInd = 0;

            for(int x = 0; x < xLen; x++)
            {
                for(int y = 0; y < yLen; y++)
                {
                    if(num[x,y,zIndex] > tempMaxVal)
                    {
                        tempMaxVal = num[x, y, zIndex];
                        tempMaxInd = x * yLen + y;
                    }
                }
            }
            return tempMaxInd;
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

        private float Sigmoid(float x)
        {
            return 1f / (1f + (float)Math.Exp(-x));
        }

        private float SigmoidDer(float x)
        {
            return Sigmoid(x) * (1f - Sigmoid(x));
        }

        //Debugging
        public void DebugFeatures()
        {
            for(int z = 0; z < numFilters; z++)
            {
                for(int x = 0; x < imageSize; x++)
                {
                    for(int y = 0; y < imageSize; y++)
                    {
                        debugFeatures[x, y, z] = featureMap[x, y, z];
                    }
                }
            }
        }

        public float[,] DisplayFeature()
        {
            float[,] temp = new float[featureMap.GetLength(0), featureMap.GetLength(1)];
            for(int y = 0; y < featureMap.GetLength(1); y++)
            {
                for(int x = 0; x < featureMap.GetLength(0); x++)
                {
                    temp[x, y] = (float)Math.Floor(featureMap[x, y, 0] * 10);
                }
            }
            return temp;
        }
        
        public float[,] DisplayPoolMask()
        {
            float[,] temp = new float[poolMask.GetLength(0), poolMask.GetLength(1)];
            for (int y = 0; y < poolMask.GetLength(1); y++)
            {
                for (int x = 0; x < poolMask.GetLength(0); x++)
                {
                    temp[x, y] = poolMask[x, y, 0];
                }
            }
            return temp;
        }

        public float GetLearningRate()
        {
            return learningRate;
        }

        public void SetLearningRate(float lRate)
        {
            learningRate = lRate;
        }

        public void SetNodeTargets(int val, int index)
        {
            nodeTargets[index] = val;
        }

        public float[,,] GetNodeWeights()
        {
            return nodeWeights;
        }

        public float[] GetNodeBiases()
        {
            return nodeBias;
        }

        public float[] GetNode()
        {
            return node;
        }

        public int GetImageSize()
        {
            return imageSize;
        }

        public int GetImageDepth()
        {
            return imageDepth;
        }

        public int GetFilterSize()
        {
            return filterSize;
        }

        public int GetNumFilters()
        {
            return numFilters;
        }

        public void SetTarget(float x)
        {
            singleTarget = x;
        }

        public float[,,,] GetFilters()
        {
            return filter;
        }

        public float[,,] GetFeatures()
        {
            float[,,] temp = new float[imageSize, imageSize, numFilters];
            temp = featureMap;
            return temp;
        }

        public float[,,] GetDebugFeatures()
        {
            return debugFeatures;
        }
    }
}
