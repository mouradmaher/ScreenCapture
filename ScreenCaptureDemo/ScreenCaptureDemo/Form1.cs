using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ScreenCaptureDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            CaptureMyScreen();
        }

        private void CaptureMyScreen()
        {
            try
            {
                //Creating a new Bitmap object
                Bitmap captureBitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);

                //Creating a Rectangle object which will capture our Current Screen
                  Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

                //Creating a New Graphics Object
                  Graphics captureGraphics = Graphics.FromImage(captureBitmap);

                //Copying Image from The Screen
                  captureGraphics.CopyFromScreen(captureRectangle.Left,captureRectangle.Top,0,0,captureRectangle.Size);

                //Saving the Image File (I am here Saving it in My E drive).
                  captureBitmap.Save(@"E:\Capture.jpg",ImageFormat.Jpeg);

                //Displaying the Successfull Result

                  MessageBox.Show("Screen Captured");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
