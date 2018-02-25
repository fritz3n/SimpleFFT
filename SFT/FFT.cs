using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;

namespace SFT
{
    class FFT
    {
        WaveIn input;
        PictureBox graph;

        public FFT(int Device, PictureBox Graph)
        {
            graph = Graph;

            input = new WaveIn(WaveCallbackInfo.FunctionCallback());
            input.BufferMilliseconds = 50;
            input.DeviceNumber = Device;
            input.WaveFormat = new WaveFormat(50000, 16, 1);
            input.DataAvailable += Transform;

            input.StartRecording();
        }

        private void Transform(object sender, WaveInEventArgs e)
        {
            int[] samples = new int[e.BytesRecorded/2];

            for(int i = 0; i < e.BytesRecorded; i += 2)
            {
                samples[i / 2] = BitConverter.ToInt16(e.Buffer, i);
            }

            using (Bitmap bmp = new Bitmap(1000, 300))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Brush brush = new SolidBrush(Color.White);
                    Rectangle area = new Rectangle(0, 0, 1000, 300);
                    g.FillRectangle(brush, area);

                    Pen p = new Pen(Color.Red);

                    System.Drawing.Point last = new System.Drawing.Point(0, 300);

                    for (int i = 0; i <= 2000; i += 20)
                    {
                        System.Drawing.Point cur = new System.Drawing.Point((int)(i / 2), 290-Transform(samples, i));
                        g.DrawLine(p, last, cur);
                        last = cur;
                    }
                }

                SetImg(bmp);

                GC.Collect();
            }
        }

        private int Transform(int[] samples, double frequency)
        {
            double angMult = 360 / (50000 / frequency);

            double ang = 0;

            Vector v = new Vector();

            foreach(int i in samples)
            {
                v += new Vector(i, 0).Rotate(ang);
                ang = ang + angMult;
            }

            v = v / samples.Length;

            return (int)v.Length;
        }

        delegate void CustomDelegate(Bitmap bmp);
        private void SetImg(Bitmap bmp)
        {
            
            if (graph.InvokeRequired)
            {
                graph.Invoke(new CustomDelegate(SetImg), bmp);
            }
            else
            {
                graph.Image = new Bitmap(bmp);
            }
        }
    }


    public static class VectorExt
    {
        private const double DegToRad = Math.PI / 180;

        public static Vector Rotate(this Vector v, double degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static Vector RotateRadians(this Vector v, double radians)
        {
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            return new Vector(ca * v.X, sa * v.X);
        }
    }
}
