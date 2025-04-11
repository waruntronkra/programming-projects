using ArtCamSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000_Control.ARTCamera
{
    public partial class ArtCamIOSettingFrm : Form
    {
        private CArtCam m_CArtCam = null;

        public ArtCamIOSettingFrm(CArtCam a)
        {
            //
            // This is needed for Windows form designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after calling InitializeComponent.
            //
            m_CArtCam = a;
        }

        private void ArtCamIOSettingFrm_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            checkBoxRead1.Enabled = true;
            checkBoxRead2.Enabled = true;
            checkBoxRead3.Enabled = true;
            checkBoxRead4.Enabled = true;
            checkBoxRead5.Enabled = true;
            checkBoxRead6.Enabled = true;
            checkBoxRead7.Enabled = true;
            checkBoxRead8.Enabled = true;

            button2.Enabled = true;
            checkBoxWrite1.Enabled = true;
            checkBoxWrite2.Enabled = true;
            checkBoxWrite3.Enabled = true;
            checkBoxWrite4.Enabled = true;
            checkBoxWrite5.Enabled = true;
            checkBoxWrite6.Enabled = true;
            checkBoxWrite7.Enabled = true;
            checkBoxWrite8.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte Data = 0x00;
            int longdata = 0;
            if (m_CArtCam.GetIOPort(out Data, out longdata, 0))
            {
                checkBoxRead1.Checked = (0 != (Data & 0x01)) ? true : false;
                checkBoxRead2.Checked = (0 != (Data & 0x02)) ? true : false;
                checkBoxRead3.Checked = (0 != (Data & 0x04)) ? true : false;
                checkBoxRead4.Checked = (0 != (Data & 0x08)) ? true : false;
                checkBoxRead5.Checked = (0 != (Data & 0x10)) ? true : false;
                checkBoxRead6.Checked = (0 != (Data & 0x20)) ? true : false;
                checkBoxRead7.Checked = (0 != (Data & 0x40)) ? true : false;
                checkBoxRead8.Checked = (0 != (Data & 0x80)) ? true : false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte Data = 0x00;
            if (checkBoxWrite1.Checked) Data |= 0x01;
            if (checkBoxWrite2.Checked) Data |= 0x02;
            if (checkBoxWrite3.Checked) Data |= 0x04;
            if (checkBoxWrite4.Checked) Data |= 0x08;
            if (checkBoxWrite5.Checked) Data |= 0x10;
            if (checkBoxWrite6.Checked) Data |= 0x20;
            if (checkBoxWrite7.Checked) Data |= 0x40;
            if (checkBoxWrite8.Checked) Data |= 0x80;

            m_CArtCam.SetIOPort(Data, 0, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
