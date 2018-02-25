using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;


namespace SFT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            graph.Image = new Bitmap(graph.Width, graph.Height);

            FFT ft = new FFT(2, graph);
        }

        public void Draw()
        {
            Bitmap bmp = (Bitmap)graph.Image;
            Random r = new Random();

            Graphics g = Graphics.FromImage(bmp);

            long n = 0;

            Stopwatch s = new Stopwatch();
            s.Start();

            while (true)
            {
                //lock (this)
                //{

                Pen p = new Pen(Color.FromArgb(r.Next(255), r.Next(255), r.Next(255)));

                for (int i = 0; i < 100; i++)
                {
                    g.DrawLine(p, new Point(r.Next(graph.Width), r.Next(graph.Height)), new Point(r.Next(graph.Width), r.Next(graph.Height)));
                }

                //}

                try
                {
                    graph.Image = bmp;
                }
                catch (Exception e)
                {
                }

                n++;

                Console.WriteLine(((float)n) / ((float)s.ElapsedMilliseconds) * 1000);
            }

        }

    }
}
