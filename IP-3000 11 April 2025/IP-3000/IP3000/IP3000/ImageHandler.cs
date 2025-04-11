using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000.IP3000
{
    public class ImageHandler
    {
        public string inputPath = Application.StartupPath + "\\" + "Original_Image" + "\\" + "Demo Software.jpg";
        public string outputPath = Application.StartupPath + "\\" + "Resize_Image" + "\\" + "Resized_image.bmp";
        //public string InputImage { get; set; }
        //public string OutputImage { get; set; } 

        public Bitmap ResizeImage(Image originalImage,float zoomFactor)
        {
            Bitmap resizedImage = null;
            
            if (originalImage != null)
            {
                int newWidth = (int)(originalImage.Width * zoomFactor);
                int newHeight = (int)(originalImage.Height * zoomFactor);

                resizedImage = new Bitmap(newWidth, newHeight);
                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }
                return resizedImage;
            }

            return resizedImage;
        }

        public Image ZoomImage(Image img, float factor, Point mousePos)
        {
            Bitmap bmp = new Bitmap(img, (int)(img.Width * factor), (int)(img.Height * factor));
            Graphics g = Graphics.FromImage(bmp);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));

            return bmp;
        }
    }
}
