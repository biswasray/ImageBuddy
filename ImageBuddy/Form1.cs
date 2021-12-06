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
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Image = pictureBox1.InitialImage;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                myImage = new Bitmap(open.FileName);
                FileInfo fileInfo = new FileInfo(open.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = myImage;
                label1.Text = "Name : "+open.FileName;
                label2.Text = "Size : " + (fileInfo.Length/1024)+ " KB ( "+ fileInfo.Length+" bytes )";
                label3.Text = "Resolution : " + myImage.Width.ToString()+" x "+ myImage.Height.ToString();
            }
        }
    }
}
