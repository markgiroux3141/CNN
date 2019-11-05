using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvNetForms
{
    class NetArch
    {

        float[] output;
        float[] target;

        int imageSize = 32;

        int imageDepth = 3;

        ConvNet.ConvModule module1;
        ConvNet.ConvModule module2;
        ConvNet.ConvModule module3;
        ConvNet.ConvModule module4;
        ConvNetForms.ConvFCLink cFCLink;
        ConvNetForms.FCModule fCLayer;

        public NetArch(float[,,] image)
        {
            module1 = new ConvNet.ConvModule(imageSize, imageDepth, 4, 5);
            module2 = new ConvNet.ConvModule(module1.GetOutputSize(), module1.GetOutputDepth(), 8, 3);
            module3 = new ConvNet.ConvModule(module2.GetOutputSize(), module2.GetOutputDepth(), 8, 3);
            module4 = new ConvNet.ConvModule(module3.GetOutputSize(), module3.GetOutputDepth(), 8, 3);
            cFCLink = new ConvNetForms.ConvFCLink(module4.GetOutputSize(), module4.GetOutputDepth());
            fCLayer = new ConvNetForms.FCModule(cFCLink.GetOutputSize(), new int [2]{ 10, 2 });
        }

        public void RunConvArch(float[,,] image)
        {
            module1.SetInputLayer(image);

            module1.RunNet();
            module2.SetInputLayer(module1.GetOutputLayer());

            module2.RunNet();
            module3.SetInputLayer(module2.GetOutputLayer());

            module3.RunNet();
            module4.SetInputLayer(module3.GetOutputLayer());

            module4.RunNet();
            cFCLink.SetInputLayer(module4.GetOutputLayer());

            cFCLink.RunNet();
            fCLayer.SetInputLayer(cFCLink.GetOutputLayer());

            output = fCLayer.RunNet();
        }

        public float TrainConvArch()
        {

            fCLayer.Train(target);
            cFCLink.SetOutputDeltas(fCLayer.GetInputDeltas());

            cFCLink.TrainNet();
            module4.SetOutputDeltas(cFCLink.GetInputDeltas());

            module4.TrainNet();
            module3.SetOutputDeltas(module4.GetInputMapDeltas());

            module3.TrainNet();
            module2.SetOutputDeltas(module3.GetInputMapDeltas());

            module2.TrainNet();
            module1.SetOutputDeltas(module2.GetInputMapDeltas());

            module1.TrainNet();

            return fCLayer.GetError();
        }

        public void SetInputData(float[,,] inp)
        {
            module1.SetInputLayer(inp);
        }

        public float[] GetOutputData()
        {
            return output;
        }

        public void SetTargetData(float[] targ)
        {
            target = targ;
        }

        public int GetImageSize()
        {
            return imageSize;
        }

        public int GetImageDepth()
        {
            return imageDepth;
        }

    }
}
