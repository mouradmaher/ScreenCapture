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
    public partial class Capturing : Form
    {
        public Capturing()
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
                ////Creating a new Bitmap object
                //Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                //               Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

                ////Creating a Rectangle object which will capture our Current Screen
                //  //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

                ////Creating a New Graphics Object
                //  Graphics captureGraphics = Graphics.FromImage(captureBitmap);

                ////Copying Image from The Screen
                //  captureGraphics.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                ////Saving the Image File (I am here Saving it in My D drive).
                //captureBitmap.Save(@"D:\Capture.jpg",ImageFormat.Jpeg);







                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    bitmap.Save("D://test.jpg", ImageFormat.Jpeg);
                }






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
