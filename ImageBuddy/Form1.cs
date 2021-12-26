using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageBuddy
{
    public partial class Form1 : Form
    {
        private Bitmap myImage,prevImage;
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
                myImage.Save(stream, ImageFormat.Png);
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
                if (myImage != null)
                    prevImage = (Bitmap)myImage.Clone();
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

        public Bitmap SetColorFilter(Bitmap bitmap,Color colorFilterType)
        {
            Bitmap bmap = (Bitmap)bitmap.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int nPixelR = 0;
                    int nPixelG = 0;
                    int nPixelB = 0;
                    if (colorFilterType == Color.Red)
                    {
                        nPixelR = c.R;
                        nPixelG = c.G - 255;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == Color.Green)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == Color.Blue)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G - 255;
                        nPixelB = c.B;
                    }
                    nPixelR = Math.Max(nPixelR, 0);
                    nPixelR = Math.Min(255, nPixelR);

                    nPixelG = Math.Max(nPixelG, 0);
                    nPixelG = Math.Min(255, nPixelG);

                    nPixelB = Math.Max(nPixelB, 0);
                    nPixelB = Math.Min(255, nPixelB);

                    bmap.SetPixel(i, j, Color.FromArgb((byte)nPixelR,
                      (byte)nPixelG, (byte)nPixelB));
                }
            }
            return bmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myImage != null)
                prevImage = (Bitmap)myImage.Clone();
            LoadImage(ResizeImage(myImage, (int)numericUpDown1.Value, (int)numericUpDown2.Value));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadImage(SetColorFilter(prevImage, Color.Red));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadImage(SetColorFilter(prevImage, Color.Green));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadImage(SetColorFilter(prevImage, Color.Blue));
        }
    }
}
