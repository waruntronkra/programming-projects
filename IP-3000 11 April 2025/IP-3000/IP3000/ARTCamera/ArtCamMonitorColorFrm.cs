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

namespace IP3000.ARTCamera
{
    public partial class ArtCamMonitorColorFrm : Form
    {
        private CArtCam m_CArtCam = null;

        public ArtCamMonitorColorFrm(CArtCam a)
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

        private void ArtCamMonitorColorFrm_Load(object sender, EventArgs e)
        {
            int MonitorColorMode = m_CArtCam.Monitor_GetColorMode();
            if (1 == MonitorColorMode) this.checkBox1.Checked = true;
            else
            {
                this.checkBox2.Enabled = false;
                this.checkBox3.Enabled = false;
            }

            int MonitorBayerGainAuto = m_CArtCam.Monitor_GetBayerGainAuto();
            if (1 == MonitorBayerGainAuto) this.checkBox2.Checked = true;
            else this.checkBox3.Enabled = false;

            int MonitorBayerGainLock = m_CArtCam.Monitor_GetBayerGainLock();
            if (1 == MonitorBayerGainLock) this.checkBox3.Checked = true;


            int MonitorBayerGainRed = m_CArtCam.Monitor_GetBayerGainRed();
            this.numericUpDown1.Value = this.trackBar1.Value = MonitorBayerGainRed;

            int MonitorBayerGainGreen = m_CArtCam.Monitor_GetBayerGainGreen();
            this.numericUpDown2.Value = this.trackBar2.Value = MonitorBayerGainGreen;


            int MonitorBayerGainBlue = m_CArtCam.Monitor_GetBayerGainBlue();
            this.numericUpDown3.Value = this.trackBar3.Value = MonitorBayerGainBlue;

            int GlobalGain = m_CArtCam.GetGlobalGain();
            this.numericUpDown4.Value = this.trackBar4.Value = GlobalGain;
            int ExposureTime = m_CArtCam.GetExposureTime();
            this.numericUpDown5.Value = this.trackBar5.Value = ExposureTime;

            int MonitorCameraClock = m_CArtCam.Monitor_GetCameraClock();
            switch (MonitorCameraClock)
            {
                case 2: this.comboBox1.SelectedIndex = 1; break;
                case 5: this.comboBox1.SelectedIndex = 2; break;
                default: this.comboBox1.SelectedIndex = 0; break;
            }

            int MonWidth, MonHeight;
            m_CArtCam.Monitor_GetPreviewSize(out MonWidth, out MonHeight);

            if (800 == MonWidth || 600 == MonHeight)
                this.comboBox2.SelectedIndex = 0;
            else if (1024 == MonWidth || 768 == MonHeight)
                this.comboBox2.SelectedIndex = 1;
            else
                this.comboBox2.SelectedIndex = 2;

            bool Vflip = m_CArtCam.GetMirrorV();
            this.checkBox4.Checked = Vflip;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = this.trackBar1.Value;
            m_CArtCam.Monitor_SetBayerGainRed(this.trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown2.Value = this.trackBar2.Value;
            m_CArtCam.Monitor_SetBayerGainGreen(this.trackBar2.Value);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown3.Value = this.trackBar3.Value;
            m_CArtCam.Monitor_SetBayerGainBlue(this.trackBar3.Value);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown4.Value = this.trackBar4.Value;
            m_CArtCam.SetGlobalGain(this.trackBar4.Value);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown5.Value = this.trackBar5.Value;
            m_CArtCam.SetExposureTime(this.trackBar5.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int ExposureTime = m_CArtCam.GetExposureTime();
            int GlobalGain = m_CArtCam.GetGlobalGain();
            int MirrorV = m_CArtCam.ReadRegister(0x20);

            m_CArtCam.Fpga_WriteRegister(0xB6, 0x03);

            m_CArtCam.Fpga_WriteRegister(0xC0, 0x09);
            m_CArtCam.Fpga_WriteRegister(0xC1, (int)(ExposureTime >> 8));
            m_CArtCam.Fpga_WriteRegister(0xC2, (int)ExposureTime);

            m_CArtCam.Fpga_WriteRegister(0xC3, 0x35);
            m_CArtCam.Fpga_WriteRegister(0xC4, (int)(GlobalGain >> 8));
            m_CArtCam.Fpga_WriteRegister(0xC5, (int)GlobalGain);

            m_CArtCam.Fpga_WriteRegister(0xC6, 0x20);
            m_CArtCam.Fpga_WriteRegister(0xC7, (int)MirrorV >> 8);
            m_CArtCam.Fpga_WriteRegister(0xC8, (int)MirrorV);


            int wReg0xE4 = m_CArtCam.Fpga_ReadRegister(0xE4);
            wReg0xE4 |= 0x0001;
            m_CArtCam.Fpga_WriteRegister(0xE4, wReg0xE4);
            wReg0xE4 &= 0xFFFE;
            m_CArtCam.Fpga_WriteRegister(0xE4, wReg0xE4);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ExposureTime = m_CArtCam.GetExposureTime();
            int GlobalGain = m_CArtCam.GetGlobalGain();
            int MirrorV = m_CArtCam.ReadRegister(0x20);

            m_CArtCam.Fpga_WriteRegister(0xB6, 0x03);

            m_CArtCam.Fpga_WriteRegister(0xC0, 0x09);
            m_CArtCam.Fpga_WriteRegister(0xC1, (int)(ExposureTime >> 8));
            m_CArtCam.Fpga_WriteRegister(0xC2, (int)ExposureTime);

            m_CArtCam.Fpga_WriteRegister(0xC3, 0x35);
            m_CArtCam.Fpga_WriteRegister(0xC4, (int)(GlobalGain >> 8));
            m_CArtCam.Fpga_WriteRegister(0xC5, (int)GlobalGain);

            m_CArtCam.Fpga_WriteRegister(0xC6, 0x20);
            m_CArtCam.Fpga_WriteRegister(0xC7, (int)MirrorV >> 8);
            m_CArtCam.Fpga_WriteRegister(0xC8, (int)MirrorV);


            int wReg0xE4 = m_CArtCam.Fpga_ReadRegister(0xE4);
            wReg0xE4 |= 0x0001;
            m_CArtCam.Fpga_WriteRegister(0xE4, wReg0xE4);
            wReg0xE4 &= 0xFFFE;
            m_CArtCam.Fpga_WriteRegister(0xE4, wReg0xE4);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex == 0)
                m_CArtCam.Monitor_SetPreviewSize(800, 600);
            else if (this.comboBox2.SelectedIndex == 1)
                m_CArtCam.Monitor_SetPreviewSize(1024, 768);
            else
                m_CArtCam.Monitor_SetPreviewSize(1280, 1024);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int Flg = (true == this.checkBox1.Checked) ? 1 : 0;
            m_CArtCam.Monitor_SetColorMode(Flg);
            this.checkBox2.Enabled = this.checkBox1.Checked;
            this.checkBox3.Enabled = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int Flg = (true == this.checkBox2.Checked) ? 1 : 0;
            m_CArtCam.Monitor_SetBayerGainAuto(Flg);
            this.checkBox3.Enabled = this.checkBox2.Checked;

            int MonitorBayerGainRed = m_CArtCam.Monitor_GetBayerGainRed();
            this.numericUpDown1.Value = this.trackBar1.Value = MonitorBayerGainRed;

            int MonitorBayerGainGreen = m_CArtCam.Monitor_GetBayerGainGreen();
            this.numericUpDown2.Value = this.trackBar2.Value = MonitorBayerGainGreen;

            int MonitorBayerGainBlue = m_CArtCam.Monitor_GetBayerGainBlue();
            this.numericUpDown3.Value = this.trackBar3.Value = MonitorBayerGainBlue;

            this.numericUpDown1.Enabled = !this.checkBox2.Checked;
            this.numericUpDown2.Enabled = !this.checkBox2.Checked;
            this.numericUpDown3.Enabled = !this.checkBox2.Checked;
            this.trackBar1.Enabled = !this.checkBox2.Checked;
            this.trackBar2.Enabled = !this.checkBox2.Checked;
            this.trackBar3.Enabled = !this.checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int Flg = (true == this.checkBox3.Checked) ? 1 : 0;
            m_CArtCam.Monitor_SetBayerGainLock(Flg);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            m_CArtCam.SetMirrorV(checkBox4.Checked);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar1.Value = (int)this.numericUpDown1.Value;
            m_CArtCam.Monitor_SetBayerGainRed((int)this.numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar2.Value = (int)this.numericUpDown2.Value;
            m_CArtCam.Monitor_SetBayerGainGreen((int)this.numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar3.Value = (int)this.numericUpDown3.Value;
            m_CArtCam.Monitor_SetBayerGainBlue((int)this.numericUpDown3.Value);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar4.Value = (int)this.numericUpDown4.Value;
            m_CArtCam.SetGlobalGain((int)this.numericUpDown4.Value);
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar5.Value = (int)this.numericUpDown5.Value;
            m_CArtCam.SetExposureTime((int)this.numericUpDown5.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
