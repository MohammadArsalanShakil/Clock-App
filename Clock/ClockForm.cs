using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class ClockForm : Form
    {
        Timer t = new Timer();
        int WIDTH = 300, HEIGHT = 300, secondshand = 150, minuteshand = 125, hourhand = 75;
        int cx, cy;
        Bitmap bmp;
        Graphics g;
        Bitmap bitmap;

        public ClockForm()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Image image = Image.FromFile("IMAGE_PATH");
            bitmap = new Bitmap(image);
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);
            cx = WIDTH / 2;
            cy = HEIGHT / 2;
            this.BackColor = Color.Black;
            t.Interval = 1000;
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp);
            int seconds = DateTime.Now.Second;
            int minutes = DateTime.Now.Minute;
            int hour = DateTime.Now.Hour;

            int[] clockhands = new int[2];
            g.Clear(Color.Black);

            //draw circle
            TextureBrush textureBrush = new TextureBrush(bitmap);
            g.FillEllipse(textureBrush, 0, 0, WIDTH, HEIGHT);

            //draw figure
            g.DrawString("١٢", new Font("Consolas", 15), Brushes.White, new PointF(140, 2));
            g.DrawString("٣", new Font("Consolas", 15), Brushes.White, new PointF(285, 140));
            g.DrawString("٦", new Font("Consolas", 15), Brushes.White, new PointF(142, 278));
            g.DrawString("٩", new Font("Consolas", 15), Brushes.White, new PointF(0, 140));
            //second hand
            clockhands = minute_secondhands(seconds, secondshand);
            g.DrawLine(new Pen(Color.White, 2f), new Point(cx, cy), new Point(clockhands[0], clockhands[1]));

            //minute hand
            clockhands = minute_secondhands(minutes, minuteshand);
            g.DrawLine(new Pen(Color.White, 3f), new Point(cx, cy), new Point(clockhands[0], clockhands[1]));

            //hour hand
            clockhands = hourhands(hour % 12, minutes, hourhand);
            g.DrawLine(new Pen(Color.White, 4f), new Point(cx, cy), new Point(clockhands[0], clockhands[1]));

            //load bmp in picturebox1
            pictureBox1.Image = bmp;

            //dispose
            g.Dispose();
        }

        //points for minute and second hand
        private int[] minute_secondhands(int val, int hlen)
        {
            int[] points = new int[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                points[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                points[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                points[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                points[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return points;
        }

        //points for hour hand
        private int[] hourhands(int hval, int mval, int hlen)
        {
            int[] points = new int[2];

            //each hour makes 30 degree
            //each min makes 0.5 degree
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                points[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                points[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                points[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                points[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return points;
        }
    }
}
