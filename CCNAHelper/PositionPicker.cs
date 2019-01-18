using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCNAHelper
{
    public partial class PositionPicker : Form
    {
        private PointF pointA;
        private PointF pointB;

        public PointF percentA { get { return new PointF(pointA.X/pictureBox1.Width * 100, pointA.Y / pictureBox1.Height * 100); } }
        public PointF percentB { get { return new PointF(pointB.X / pictureBox1.Width * 100, pointB.Y / pictureBox1.Height * 100); } }
        private float tolerance;

        int selectedPoint = -1;

        public PositionPicker()
        {
            InitializeComponent();
            pointA = new PointF(0, 0);
            pointB = new PointF(200, 50);
            tolerance = 20f;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle r = new Rectangle(Math.Min((int)pointA.X, (int)pointB.X),
               Math.Min((int)pointA.Y, (int)pointB.Y),
               Math.Abs((int)pointA.X - (int)pointB.X),
               Math.Abs((int)pointA.Y - (int)pointB.Y));

            g.DrawRectangle(Pens.Black, r);

            g.DrawRectangle(Pens.Black, pointA.X - tolerance / 2, pointA.Y - tolerance / 2, tolerance, tolerance);
            g.DrawRectangle(Pens.Black, pointB.X - tolerance / 2, pointB.Y - tolerance / 2, tolerance, tolerance);

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (selectedPoint == -1)
            {
                if ((Math.Sqrt((e.X - pointA.X) * (e.X - pointA.X)) < tolerance / 2) && (Math.Sqrt((e.Y - pointA.Y) * (e.Y - pointA.Y)) < tolerance / 2))
                {
                    selectedPoint = 0;
                }
                if ((Math.Sqrt((e.X - pointB.X) * (e.X - pointB.X)) < tolerance / 2) && (Math.Sqrt((e.Y - pointB.Y) * (e.Y - pointB.Y)) < tolerance / 2))
                {
                    selectedPoint = 1;
                }
            }
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            selectedPoint = -1;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPoint != -1)
            {
                if (selectedPoint == 0)
                {
                    pointA = new PointF(e.X, e.Y);
                }
                else
                {
                    pointB = new PointF(e.X, e.Y);
                }
                pictureBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
