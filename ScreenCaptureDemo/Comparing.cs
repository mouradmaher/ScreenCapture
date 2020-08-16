using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;

using System.Runtime.InteropServices;

namespace ImageComparison 
{
    public partial class Comparing : Form
    {
        private static class Win32Native
        {
            public const int DESKTOPVERTRES = 0x75;
            public const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);
        }

        int counter ;
        public Comparing()
        {
            WindowState = FormWindowState.Minimized;
            InitializeComponent();
       
            Image1();
            
         
        }
      
       

      

        static string fname1, fname2, fname3;
        Bitmap img1, img2;
        int count1 = 0, count2 = 0;
        bool flag = true;
        public enum CompareResult
        {
            ciCompareOk,
            ciPixelMismatch,
            ciSizeMismatch
        };

      
        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            pictureBox1.Visible = false;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Image1()
        {

            const string file = "D://Original.jpg";
            Bitmap bm3;
            int width, height;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hDC = g.GetHdc();
                width = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPHORZRES);
                height = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPVERTRES);
                g.ReleaseHdc(hDC);
            }

            using (var img = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.CopyFromScreen(0, 0, 0, 0, img.Size);

                }

              
                 bm3 = new Bitmap(img);
            }
            bm3.Save(file, ImageFormat.Jpeg);

            fname1 = file;
            Image2();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Image2()
        {
            int width, height;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hDC = g.GetHdc();
                width = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPHORZRES);
                height = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPVERTRES);
                g.ReleaseHdc(hDC);
            }

            using (var img = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.CopyFromScreen(0, 0, 0, 0, img.Size);
                }

                img.Save("F://image2X10msec.jpg", ImageFormat.Jpeg);
            }
            
            fname2 = "F://image2X10msec.jpg";
            checking();
        }

        private void checking()
        {
            img1 = new Bitmap(fname1);
            img2 = new Bitmap(fname2);

            int filterWidth = 3;
            int filterHeight = 3;
            int w = img2.Width;
            int h = img2.Height;

            double[,] filter = new double[filterWidth, filterHeight];

            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;
            Color[,] result = new Color[w, h];
            progressBar1.Visible = true;

            string img1_ref, img2_ref;
           

            progressBar1.Maximum = img1.Width;
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            count2++;
                            flag = false;
                            break;
                        }
                        count1++;
                    }
                    if (progressBar1.Value>1920) { progressBar1.Value++; } else { progressBar1.Value=1920; }
                  
                }
               
                if (flag == false)
                {

                   
                    
                    int wid = Math.Min(img1.Width, img2.Width);
                    int hgt = Math.Min(img1.Height, img2.Height);
                    Bitmap bm3 ;
                    ////bool are_identical = true;
                    //Color eq_color = Color.Transparent;
                    //Color ne_color = Color.Transparent;


                    for (int x = 0; x < wid; ++x)
                    {
                        for (int y = 0; y < hgt; ++y)
                        {
                            double red = 0.0, green = 0.0, blue = 0.0;

                            //=====[REMOVE LINES]========================================================
                            // Color must be read per filter entry, not per image pixel.
                            //Color imageColor = img2.GetPixel(x, y);
                            //===========================================================================

                            for (int filterX = 0; filterX < filterWidth; filterX++)
                            {
                                for (int filterY = 0; filterY < filterHeight; filterY++)
                                {
                                    int imageX = (x - filterWidth / 2 + filterX + wid) % wid;
                                    int imageY = (y - filterHeight / 2 + filterY + hgt) % hgt;

                                    //=====[INSERT LINES]========================================================
                                    // Get the color here - once per fiter entry and image pixel.
                                    Color imageColor = img2.GetPixel(imageX, imageY);
                                    //===========================================================================

                                    red += imageColor.R * filter[filterX, filterY];
                                    green += imageColor.G * filter[filterX, filterY];
                                    blue += imageColor.B * filter[filterX, filterY];
                                }
                                int r = Math.Min(Math.Max((int)(factor * red + bias), 100), 255);
                                int g = Math.Min(Math.Max((int)(factor * green + bias), 100), 255);
                                int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                                result[x, y] = Color.FromArgb(r, g, b);
                            }
                        }
                    }
                    //for (int x = 0; x < wid; x++)
                    //{
                    //    for (int y = 0; y < hgt; y++)
                    //    {
                    //        if (!img1.GetPixel(x, y).Equals(img2.GetPixel(x, y)))
                    //        {
                    //            img2.SetPixel(x, y, result[x, y]);
                    //        }
                    //        img2.Save("D://Changes.jpg", ImageFormat.Jpeg);
                    //        fname3 = "D://Changes.jpg";
                    //    }
                    //}
                    for (int x = 0; x < wid; x++)
                    {
                        for (int y = 0; y < hgt; y++)
                        {


                          

                            if (!img1.GetPixel(x, y).Equals(img2.GetPixel(x, y)))
                            {
                                img2.SetPixel(x, y, result[x, y]);
                            }


                           
                        }
                    }
                    bm3 = new Bitmap(img2);

                    bm3.Save("D://Changes.jpg", ImageFormat.Jpeg);
                    fname3 = "D://Changes.jpg";
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {


                            var index = i * 10 + j;
                            var imgarray = new Image[100];
                            var img = new Bitmap(fname3); ;
                            imgarray[index] = new Bitmap(192, 108);
                            var graphics = Graphics.FromImage(imgarray[index]);
                            graphics.DrawImage(img, new Rectangle(0, 0, 192, 108), new Rectangle(i * 192, j * 108, 192, 108), GraphicsUnit.Pixel);
                            graphics.Dispose();
                            imgarray[index].Save("D://Cropped" + index +"_"+counter+ ".jpg", ImageFormat.Jpeg);
                        }




                    }






                    counter++;
                    checking();

                 

                }

                else
                {

                    Image1();

                }

            }






        }




       
    }
}