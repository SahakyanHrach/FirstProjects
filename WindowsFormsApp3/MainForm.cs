using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp3
{
    public partial class MainForm : Form
    {
        static double a0 = -2.1;
        static double b0 = -1.1;
        static double side = 2.8;
        static string xa0 = "";
        static string xb0 = "";
        static string xside = "";
        static int value = 1;
        static string imageText = "";
        static string fileName = "nkar";
        static string ext = ".img";
        static int m = 0;
        static string ReadFileName = "";
        static string openedFileText = "";
        static bool start = false;
        static bool startDrawing = false;
        static DateTime D1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        static long BeginTime;
        static long EndTime;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Form1_Paint);
        }
        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (start)
            {
                DrawMandelbrotSet(sender, e);
                Console.WriteLine(EndTime-BeginTime);
            }
            else if (startDrawing)
            {
                DrawOpenedImage(openedFileText, e);
            }
        }
        static void DrawMandelbrotSet(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            Pen p = new Pen(Color.Red, 7);
            // e.Graphics.DrawLine(p, 150, 200, 151, 200);
            // e.Graphics.DrawArc(p, new Rectangle(100,100,1,1),0,360);
            int it = 0;
            int pixel = 1000;
            //imageText = pixel.ToString() + "\n";
            double gap = side / pixel;
            double x = 0;
            double y = 0; /*c=a+bi*/
            /* z=z^2+c   (x+iy)=x+yi^2 + a+bi 
              a^2 + 2abi - b^2 + a + bi 
              x^2 - y^2 + a + i(2xy+b)
              x = x^2 - y^2 + a 
              y = i(2xy+b)*/
            double a = a0;
            double b = b0;
            double wx = 0;
            int iterNum = 1000;
            for (int i = 1; i <= pixel; i++)
            {
                a = a + gap;
                b = b0;
                for (int j = 1; j <= pixel; j++)
                {
                    x = 0;
                    y = 0;
                    it = 0;
                    b = b + gap;
                    //Console.WriteLine(b);
                    while (it < iterNum && (x * x + y * y) < 4)
                    {
                        wx = x * x - y * y + a;
                        y = 2 * x * y + b;
                        x = wx;
                        it += 1;

                    }
                    // Console.WriteLine(it);
                    Color guyn = GetColor(value, it);
                    p = new Pen(guyn, 1);
                    if (it == iterNum)
                    {
                        e.Graphics.DrawRectangle(Pens.Black, new Rectangle(i, j, 1, 1));
                       // imageText += i.ToString() + ":" + j.ToString() + ":" + 0 + "," + 0 + "," + 0 + ";";
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(p, new Rectangle(i, j, 1, 1));
                      //  imageText += i.ToString() + ":" + j.ToString() + ":" + guyn.R + "," + guyn.G + "," + guyn.B + ";";
                    }
                }
              //  imageText += "\n";
            }
            EndTime = (long)(DateTime.UtcNow - D1970).TotalMilliseconds;
            //  Console.WriteLine(imageText);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            m++;
            DateTime Today = DateTime.UtcNow;
            DateTime Yesterday = new DateTime(2020, 11, 29);
            string currentDateTime  = (Today.Year + "_" +Today.Month + "_" + Today.Day + "_" + Today.Hour + "_" + Today.Minute + "_" + Today.Second).ToString();
            File.WriteAllText(m.ToString() + fileName + currentDateTime + ext, imageText);
            // date time compare
            /*  a0 = -0.235125;
              b0 = 0.827215;
              side = 4.0E-5;
              this.Refresh();*/
        }
        static Color GetColor(int value, int it)
        {
            Color guyn = new Color();
            switch (value)
            {
                /*blue red*/
                case 1: guyn = Color.FromArgb(255 - ((it * it * it) % 256), 255 - (it % 256), 255 - (it % 255)); break;
                /*bw*/
                case 2: guyn = Color.FromArgb(it % 256, it % 256, it % 256); ; break;
                /*blue */
                case 3: guyn = Color.FromArgb(0, 0, (int)(255 * Math.Pow(it, 0.5) % 256)); break;
                /*red */
                case 4: guyn = Color.FromArgb((int)(255 * Math.Pow(it, 0.25) % 256), 0, 0); break;
                /*green*/
                case 5: guyn = Color.FromArgb((int)(255 * Math.Pow(it, 0.25) % 256), (int)(255 * Math.Pow(it, 1) % 256), (int)(255 * Math.Pow(it, 1.4) % 256)); break;
                default:
                    Color.FromArgb(255 - ((it * it * it) % 256), 255 - (it % 256), 255 - (it % 255)); break;
            }

            return guyn;
        }
        public void DrawOpenedImage(string fileName, System.Windows.Forms.PaintEventArgs e)
        {

            // Console.WriteLine("drawing started");
            fileName = openedFileText;
            string[] coordinates = fileName.Split(';');
            int x = 0;
            int y = 0;
            int red = 0;
            int green = 0;
            int blue = 0;
            string[] xyText = new string[2];
            string guynText = "";
            string[] guynArr = new string[3];
            for (int i = 0; i < coordinates.Length - 1; i++)
            {
                xyText = coordinates[i].Split(':');
                x = Convert.ToInt32(xyText[0]);
                y = Convert.ToInt32(xyText[1]);
                guynText = xyText[2];
                guynArr = guynText.Split(',');
                red = Convert.ToInt32(guynArr[0]);
                green = Convert.ToInt32(guynArr[1]);
                blue = Convert.ToInt32(guynArr[2]);
                Pen p = new Pen(Color.FromArgb(red,green,blue),10);
                e.Graphics.DrawRectangle(p, new Rectangle(x,y, 1, 1));
            }

            //     e.Graphics.DrawRectangle(p, new Rectangle(i,j,1,1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            side = side / 2;
            this.Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            side = side / 16;
            this.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            side = side / 4;
            this.Refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            side = side / 8;
            this.Refresh();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            a0 = Convert.ToDouble(xa0);
            b0 = Convert.ToDouble(xb0);
            side = Convert.ToDouble(xside);
            xa0 = "";
            xb0 = "";
            xside = "";
            this.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            xa0 = textBox1.Text;
            Console.WriteLine(xa0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            xb0 = textBox2.Text;
            Console.WriteLine(xb0);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            xside = textBox3.Text;
            Console.WriteLine(xside);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'E' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'E' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'E' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                value = 1;
            }
            this.Refresh();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                value = 4;
            }
            this.Refresh();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                value = 2;
            }
            this.Refresh();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                value = 3;
            }
            this.Refresh();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                value = 5;
            }
            this.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openedFileText = File.ReadAllText(ReadFileName);
            startDrawing = true;
            start = false;
            this.Refresh();
            Console.WriteLine(openedFileText);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            ReadFileName = textBox4.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BeginTime = (long)(DateTime.UtcNow - D1970).TotalMilliseconds;
            start = true;
            startDrawing = false;
            this.Refresh();
        }
    }

}
