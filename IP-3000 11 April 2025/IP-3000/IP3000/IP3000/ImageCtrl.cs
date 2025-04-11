using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000.IP3000
{
    public partial class ImageCtrl : UserControl
    {
        private ImageHandler _imgHandler = new ImageHandler();

        private Image originalImage;
        public float zoomFactor = 1.0f;

        Bitmap resizedImage;

        public Point startPoint;
        public Rectangle cropRect;
        public bool isDragging = false;

        public ImageCtrl()
        {
            InitializeComponent();
        }

        private void ImageCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                // Load the image
                originalImage = Image.FromFile(_imgHandler.inputPath);
                resizedImage = (Bitmap)originalImage;
                pbInputImage.Image = originalImage;
                pbOutputImage.Image = originalImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
