using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageBuddy
{
    public partial class Form1 : Form
    {
        private Bitmap myImage;
        private Stream stream;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Image = pictureBox1.InitialImage;
        }

        private void LoadImage(Bitmap bitmap)
        {
            if (bitmap == null)
                MessageBox.Show("Choose an Image");
            try
            {
                myImage = bitmap;
                stream = (new StreamWriter("tempImageBuudy00.png")).BaseStream;
                myImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                FileInfo fileInfo = new FileInfo("tempImageBuudy00.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = myImage;
                label2.Text = "Size : " + (stream.Length / 1024) + " KB ( " + stream.Length + " bytes )";
                label3.Text = "Resolution : " + myImage.Width.ToString() + " x " + myImage.Height.ToString();
                numericUpDown1.Value = myImage.Width;
                numericUpDown2.Value = myImage.Height;
                //myImage.Dispose();
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap b = new Bitmap(open.FileName);
                LoadImage(b);
                label1.Text = "Name : " + open.FileName;
                //b.Dispose();
            }
        }
        public Bitmap ResizeImage(Bitmap bitmap,int newWidth,int newHeight)
        {
            if (newWidth == 0 || newHeight == 0)
                return null;
            Bitmap temp = new Bitmap(newWidth, newHeight, bitmap.PixelFormat);
            double nWF = (double)bitmap.Width / (double)newWidth;
            double nHF = (double)bitmap.Height / (double)newHeight;
            double fx, fy, nx, ny;
            int cx, cy, fr_x, fr_y;
            Color color1 = new Color();
            Color color2 = new Color();
            Color color3 = new Color();
            Color color4 = new Color();
            byte nRed, nGreen, nBlue,bp1,bp2;
            for(int x=0;x<temp.Width;++x)
            {
                for(int y=0;y<temp.Height;++y)
                {
                    fr_x = (int)Math.Floor(x * nWF);
                    fr_y = (int)Math.Floor(y * nHF);
                    cx = (fr_x + 1) >= bitmap.Width ? fr_x: (fr_x + 1);
                    cy = (fr_y + 1) >= bitmap.Height ? fr_y : (fr_y + 1);
                    fx = x * nWF - fr_x;
                    fy = y * nHF - fr_y;
                    nx = 1.0 - fx;
                    ny = 1.0 - fy;
                    color1 = bitmap.GetPixel(fr_x, fr_y);
                    color2 = bitmap.GetPixel(cx, fr_y);
                    color3 = bitmap.GetPixel(fr_x, cy);
                    color4 = bitmap.GetPixel(cx, cy);
                    bp1 = (byte)(nx * color1.B + fx * color2.B);
                    bp2 = (byte)(nx * color3.B + fx * color4.B);
                    nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));
                    bp1 = (byte)(nx * color1.G + fx * color2.G);
                    bp2 = (byte)(nx * color3.G + fx * color4.G);
                    nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));
                    bp1 = (byte)(nx * color1.R + fx * color2.R);
                    bp2 = (byte)(nx * color3.R + fx * color4.R);
                    nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));
                    temp.SetPixel(x, y, System.Drawing.Color.FromArgb(255, nRed, nGreen, nBlue));
                }
            }
            return temp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadImage(ResizeImage(myImage, (int)numericUpDown1.Value, (int)numericUpDown2.Value));
        }
    }
}
