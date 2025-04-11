using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace IP3000.IP3000
{
    public partial class AlertFrm : Form
    {
        public string AlertMessage {  get; set; }
        public string AlertTitle { get; set; }
        public string ImageName { get; set; }
        public string Position { get; set; }
        public string ProductSide { get; set; } 
        public string RejectType { get; set; }

        public string  PathSaveRejImage { get; set; }

        public AlertFrm()
        {
            InitializeComponent();
        }

        private void btAccept_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void AlertFrm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.ClientSize = new Size(850, 650);
            this.WindowState = FormWindowState.Normal;
            //this.Size = new System.Drawing.Size(1815, 1189);

            PathSaveRejImage = "D:\\DATAFAIL";
        }

        public void SetAlertMessageWithImage(int index, string productSide, string title, string errMsg, string imagePath)
        {
            try
            {
                lbRejTitle.Text = title;
                lbRejcetSide.Text = productSide;
                lbRejMessage.Text = errMsg;

                if (imagePath != null && imagePath != string.Empty)
                {
                    pbRejImage.BackgroundImage = Image.FromFile(imagePath);
                    pbRejImage.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch 
            {
            }   
        }

        private void btReject_Click(object sender, EventArgs e)
        {
            SaveRejectImage();
            this.Close();
        }

        private void SaveRejectImage()
        {
            try
            {
                PathSaveRejImage = "D:\\DATAFAIL" + lbRejTitle.Text + ".jpg";
                pbRejImage.Image.Save(PathSaveRejImage);
            }
            catch 
            { 
            }
        }
    }
}
