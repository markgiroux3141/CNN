using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvNetForms
{
    public partial class Form1 : Form
    {
        float[,,] image1;
        float[,,] image2;

        float[] targetData1;
        float[] targetData2;

        NetArch cNet;
        NetTest tNet;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            //ConvertImageToArray();
            cNet = new NetArch(image1);
            tNet = new NetTest();
            RunNet();
            //DisplayFilters();
            //DisplayFeatures();
        }

        private void ConvertImageToArray()
        {
            int Isize = 32;
            int Idepth = 3;

            image1 = new float[Isize, Isize, Idepth];
            image2 = new float[Isize, Isize, Idepth];

            Bitmap bmp = new Bitmap(pictureBoxInput.Image);
            Bitmap bmp2 = new Bitmap(pictureBoxInput2.Image);

            for (int x = 0; x < Isize; x++)
            {
                for(int y = 0; y < Isize; y++)
                {
                    image1[x, y, 0] = (float)bmp.GetPixel(x, y).R / 255f;
                    image1[x, y, 1] = (float)bmp.GetPixel(x, y).G / 255f;
                    image1[x, y, 2] = (float)bmp.GetPixel(x, y).B / 255f;

                    //listBox1.Items.Add(image1[x, y, 0] + " , " + image1[x, y, 1] + " , " + image1[x, y, 2]);

                    image2[x, y, 0] = (float)bmp2.GetPixel(x, y).R / 255f;
                    image2[x, y, 1] = (float)bmp2.GetPixel(x, y).G / 255f;
                    image2[x, y, 2] = (float)bmp2.GetPixel(x, y).B / 255f;

                }
            }
        }

        public void RunNet()
        {
            for(int n=0; n < 10000; n++)
            {
                float err = 0;
                err = tNet.RunNet();

                labelEpoch.Text = n.ToString();
                labelError.Text = err.ToString();

                labelEpoch.Refresh();
                labelError.Refresh();
            }
        }

        /*
        public void RunNet()
        {

            float err = 0;

            targetData1 = new float[2];
            targetData2 = new float[2];

            targetData1[0] = 0;
            targetData1[1] = 0;
            targetData2[0] = 0;
            targetData2[1] = 0;

            for (int n=0; n<100; n++)
            {
                err = 0;

                //cNet.SetInputData(image1);
                cNet.SetTargetData(targetData1);
                cNet.RunConvArch(image1);
                err = cNet.TrainConvArch();

                //cNet.SetInputData(image2);
                cNet.SetTargetData(targetData2);
                cNet.RunConvArch(image2);
                err += cNet.TrainConvArch();

                //net.SetLearningRate(ConvNetHelpers.AdjustLearnRate(err, prevErr, net.GetLearningRate()));

                labelEpoch.Text = n.ToString();
                labelError.Text = err.ToString();

                labelEpoch.Refresh();
                labelError.Refresh();
            }

        }
        */

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
