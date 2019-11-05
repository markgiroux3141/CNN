using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNetForms
{
    class NetTest
    {
        ConvFCLink cLink;
        FCModule net;

        float[] target; // for FC
        float[] targetL; // for Link
        float[] linkOutput; // hotwire link
        float[] linkDeltas; // hotwire link

        float[,,] linkInput;

        public NetTest()
        {
            linkInput = new float[4, 4, 4];
            linkOutput = new float[4];
            linkDeltas = new float[4];
            cLink = new ConvFCLink(4, 4);
            net = new FCModule(cLink.GetOutputSize(), new int[2] { 10, 2 });

            for(int z = 0; z < linkInput.GetLength(0); z++)
            {
                for(int x = 0; x < linkInput.GetLength(1); x++)
                {
                    for(int y = 0; y < linkInput.GetLength(2); y++)
                    {
                        linkInput[x, y, z] = 1f;
                    }
                }
            }

            target = new float[2];
            target[0] = 1f;
            target[1] = 1f;

            targetL = new float[cLink.GetOutputSize()];
            
            for(int i = 0; i < targetL.Length; i++)
            {
                targetL[i] = 1f;
            }

        }

        public float RunNet()
        {
            float err1 = 0;
            float err2 = 0;

            cLink.SetInputLayer(linkInput);
            cLink.RunNet();

            //for(int i = 0; i < linkDeltas.Length; i++)
            //{
            //    linkDeltas[i] = linkOutput[i] - targetL[i];
            //}

            net.SetInputLayer(cLink.GetOutputLayer());
            net.RunNet();

            err1 = net.Train(target);
            cLink.SetOutputDeltas(net.GetInputDeltas());
            err2 = cLink.TrainNet();
            //Console.WriteLine(err2);

            return err1;
        }
    }
}
