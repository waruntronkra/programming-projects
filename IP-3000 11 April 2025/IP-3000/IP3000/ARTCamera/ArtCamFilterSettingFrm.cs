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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace IP3000_Control.ARTCamera
{
    public partial class ArtCamFilterSettingFrm : Form
    {
        private ARTCAM_CAMERATYPE m_DllType = 0;
        private CArtCam m_CArtCam = null;
        private int m_Preview = -1;
        private CAMERAINFO m_CameraInfo;

        public ArtCamFilterSettingFrm(CArtCam a, int p)
        {
            //
            // This is needed for Windows form designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after calling InitializeComponent.
            //
            m_CArtCam = a;
            m_Preview = p;
        }

        private void ArtCamFilterSettingFrm_Load(object sender, EventArgs e)
        {
            // Change operation for each DLL
            // For each DLL function, please refer to manual.
            m_DllType = (ARTCAM_CAMERATYPE)(m_CArtCam.GetDllVersion() >> 16);
            m_CameraInfo.lSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CAMERAINFO));
            m_CArtCam.GetCameraInfo(ref m_CameraInfo);

            // Range for value is set while determination of availability is done. 
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV == m_DllType)
            {
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BRIGHTNESS, UpDownBrightness, 0, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_CONTRAST, UpDownContrast, 0, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_HUE, UpDownHue, 0, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_SATURATION, UpDownSaturation, 0, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_SHARPNESS, UpDownSharpness, 0, 0);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_GAMMA, UpDownGamma, 0, 0);

                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_R, UpDownBayerGainR, 0, 0);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_G, UpDownBayerGainG, 0, 0);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_B, UpDownBayerGainB, 0, 0);
            }
            else
            {
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BRIGHTNESS, UpDownBrightness, -255, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_CONTRAST, UpDownContrast, -127, 127);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_HUE, UpDownHue, -360, 360);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_SATURATION, UpDownSaturation, -255, 255);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_SHARPNESS, UpDownSharpness, 0, 30);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_GAMMA, UpDownGamma, 0, 200);

                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_R, UpDownBayerGainR, 0, 200);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_G, UpDownBayerGainG, 0, 200);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_B, UpDownBayerGainB, 0, 200);
            }

            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_SATA != m_DllType)
            {
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_GLOBAL_GAIN, UpDownGlobalGain, m_CameraInfo.lGlobalGainMin, m_CameraInfo.lGlobalGainMax);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_COLOR_GAIN_R, UpDownColorGainR, m_CameraInfo.lColorGainMin, m_CameraInfo.lColorGainMax);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_COLOR_GAIN_G1, UpDownColorGainG1, m_CameraInfo.lColorGainMin, m_CameraInfo.lColorGainMax);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_COLOR_GAIN_G2, UpDownColorGainG2, m_CameraInfo.lColorGainMin, m_CameraInfo.lColorGainMax);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_COLOR_GAIN_B, UpDownColorGainB, m_CameraInfo.lColorGainMin, m_CameraInfo.lColorGainMax);
                InitControl(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_EXPOSURETIME, UpDownExposureTime, m_CameraInfo.lExposureMin, m_CameraInfo.lExposureMax);
            }

            // Auto white balance
            int Data = m_CArtCam.GetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYER_GAIN_AUTO);
            if (0 == m_CArtCam.m_Error)
            {
                checkAWB.Enabled = false;
            }
            if (0 != Data)
            {
                checkAWB.Checked = true;
            }


            // Bayer conversion mode
            Data = m_CArtCam.GetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYERMODE);
            if (0 == m_CArtCam.m_Error)
            {
                radioBayer1.Enabled = false;
                radioBayer2.Enabled = false;
                radioBayer3.Enabled = false;
                radioBayer4.Enabled = false;
            }

            switch (Data)
            {
                case 0: radioBayer1.Checked = true; break;
                case 1: radioBayer2.Checked = true; break;
                case 2: radioBayer3.Checked = true; break;
                case 3: radioBayer4.Checked = true; break;
            }


            // Mirror reversal
            if (m_CArtCam.GetMirrorV()) checkMirrorV.Checked = true;
            if (m_CArtCam.GetMirrorH()) checkMirrorH.Checked = true;


            // Vertical reversal is not available in following models.
            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_USTC:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P2:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_098:
                    checkMirrorV.Enabled = false;
                    break;
            }


            // Auto iris
            switch (m_CArtCam.GetAutoIris())
            {
                case AI_TYPE.AI_NONE: radioAI1.Checked = true; break;
                case AI_TYPE.AI_EXPOSURE: radioAI2.Checked = true; break;
                case AI_TYPE.AI_GAIN: radioAI3.Checked = true; break;
            }

            if (0 == m_CArtCam.m_Error)
            {
                radioAI1.Enabled = false;
                radioAI2.Enabled = false;
                radioAI3.Enabled = false;
            }

            // Camera without shutter function
            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_320P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_098:
                    radioAI2.Enabled = false;
                    break;
            }


            // Half clock
            checkHalfClock.Checked = (0 == m_CArtCam.GetHalfClock()) ? false : true;
            if (0 == m_CArtCam.m_Error)
            {
                checkHalfClock.Enabled = false;
            }


            // Channel
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV != m_DllType)
            {
                radioChannel1.Enabled = false;
                radioChannel2.Enabled = false;
                radioChannel3.Enabled = false;
                radioChannel4.Enabled = false;
                radioChannel5.Enabled = false;
                radioChannel6.Enabled = false;
            }
        }

        // 
        private void InitControl(ARTCAM_FILTERTYPE FilterType, NumericUpDown UpDown, int Min, int Max)
        {
            int Data = m_CArtCam.GetFilterValue(FilterType);


            UpDown.Minimum = Min;
            UpDown.Maximum = Max;
            if (Min <= Data && Data <= Max)
            {
                UpDown.Value = Data;
            }


            // Error occurrence(no response)
            if (0 == m_CArtCam.m_Error)
            {
                UpDown.Enabled = false;
                return;
            }

            return;
        }

        private void checkAWB_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetBayerGainAuto(checkAWB.Checked);
        }

        private void radioBayer1_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYERMODE, 0);
        }

        private void radioBayer2_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYERMODE, 1);
        }

        private void radioBayer3_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYERMODE, 2);
        }

        private void radioBayer4_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetFilterValue(ARTCAM_FILTERTYPE.ARTCAM_FILTERTYPE_BAYERMODE, 3);
        }

        private void radioAI1_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetAutoIris(AI_TYPE.AI_NONE);
        }

        private void radioAI2_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetAutoIris(AI_TYPE.AI_EXPOSURE);
        }

        private void radioAI3_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetAutoIris(AI_TYPE.AI_GAIN);
        }

        private void checkMirrorV_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetMirrorV(checkMirrorV.Checked);
        }

        private void checkMirrorH_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetMirrorH(checkMirrorH.Checked);
        }

        private void checkHalfClock_CheckedChanged(object sender, System.EventArgs e)
        {
            if (-1 != m_Preview)
            {
                // To switch clock, display needs to be stopped.
                Cursor.Current = Cursors.WaitCursor;
                m_CArtCam.StopPreview();
            }

            m_CArtCam.SetHalfClock(checkHalfClock.Checked ? 1 : 0);

            if (-1 != m_Preview)
            {
                // Resume display
                m_CArtCam.StartPreview();
                Cursor.Current = Cursors.Default;
            }
        }

        private void radioChannel1_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(0, 0);
        }

        private void radioChannel2_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(1, 0);
        }

        private void radioChannel3_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(2, 0);
        }

        private void radioChannel4_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(3, 0);
        }

        private void radioChannel5_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(4, 0);
        }

        private void radioChannel6_CheckedChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetCrossbar(5, 0);
        }

        private void UpDownBrightness_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetBrightness((int)UpDownBrightness.Value);
        }

        private void UpDownContrast_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetContrast((int)UpDownContrast.Value);
        }

        private void UpDownHue_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetHue((int)UpDownHue.Value);
        }

        private void UpDownSaturation_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetSaturation((int)UpDownSaturation.Value);
        }

        private void UpDownSharpness_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetSharpness((int)UpDownSharpness.Value);
        }

        private void UpDownGamma_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetGamma((int)UpDownGamma.Value);
        }

        private void UpDownBayerGainR_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetBayerGainRed((int)UpDownBayerGainR.Value);
        }

        private void UpDownBayerGainG_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetBayerGainGreen((int)UpDownBayerGainG.Value);
        }

        private void UpDownBayerGainB_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetBayerGainBlue((int)UpDownBayerGainB.Value);
        }

        private void UpDownGlobalGain_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetGlobalGain((int)UpDownGlobalGain.Value);
        }

        private void UpDownColorGainR_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetColorGainRed((int)UpDownColorGainR.Value);
        }

        private void UpDownColorGainG1_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetColorGainGreen1((int)UpDownColorGainG1.Value);
        }

        private void UpDownColorGainG2_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetColorGainGreen2((int)UpDownColorGainG2.Value);
        }

        private void UpDownColorGainB_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void UpDownExposureTime_ValueChanged(object sender, System.EventArgs e)
        {
            m_CArtCam.SetExposureTime((int)UpDownExposureTime.Value);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

    }
}
