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
    public partial class ArtCamSizeSettingFrm : Form
    {
        private ARTCAM_CAMERATYPE m_DllType = 0;
        private CArtCam m_CArtCam = null;
        private CAMERAINFO m_CameraInfo;

        //public ArtCamSizeSettingFrm()
        //{
        //    InitializeComponent();
        //}

        public ArtCamSizeSettingFrm(CArtCam a)
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

        private void ArtCamSizeSettingFrm_Load(object sender, EventArgs e)
        {
            // Change operation for each DLL
            // For each DLL function, please refer to manual.
            m_DllType = (ARTCAM_CAMERATYPE)(m_CArtCam.GetDllVersion() >> 16);
            m_CameraInfo.lSize = System.Runtime.InteropServices.Marshal.SizeOf(m_CameraInfo);
            m_CArtCam.GetCameraInfo(ref m_CameraInfo);

            // Set size for camera
            InitCameraSize();

            // Set color mode
            InitColorMode();

            // Set information for sub-sampling.
            InitSubSample();

            // Initialize CNV.
            InitCNV();

            // Obtain sub-code
            InitSubCode();


            // Frame rate for Directshow
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS == m_DllType)
            {
                textFps.Text = Convert.ToString(m_CArtCam.Fps());
            }
            // For all others, this is the waiting period between frames
            else
            {
                labelFps.Text = "Waiting time  (ms)";
                textFps.Text = Convert.ToString(m_CArtCam.GetWaitTime());
            }
        }

        // Set size for camera
        private void InitCameraSize()
        {
            int lHT, lHS, lHE, lVT, lVS, lVE;

            // Cameras that do not allow size settings
            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS:
                    textHT.Enabled = false;
                    textHS.Enabled = false;
                    textVT.Enabled = false;
                    textVS.Enabled = false;

                    textHT.Text = Convert.ToString(m_CArtCam.Width());
                    textHE.Text = Convert.ToString(m_CArtCam.Width());
                    textHS.Text = "0";
                    textVT.Text = Convert.ToString(m_CArtCam.Height());
                    textVE.Text = Convert.ToString(m_CArtCam.Height());
                    textVS.Text = "0";
                    break;

                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_098:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_500P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P2:
                    textHT.Enabled = false;
                    textHE.Enabled = false;
                    textHS.Enabled = false;
                    textVT.Enabled = false;
                    textVE.Enabled = false;
                    textVS.Enabled = false;

                    textHT.Text = Convert.ToString(m_CArtCam.Width());
                    textHE.Text = Convert.ToString(m_CArtCam.Width());
                    textHS.Text = "0";
                    textVT.Text = Convert.ToString(m_CArtCam.Height());
                    textVE.Text = Convert.ToString(m_CArtCam.Height());
                    textVS.Text = "0";
                    break;

                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_130MI:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_200MI:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_300MI:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_320P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_200SH:
                    textVT.Enabled = false;
                    textHT.Enabled = false;

                    m_CArtCam.GetCaptureWindowEx(out lHT, out lHS, out lHE, out lVT, out lVS, out lVE);

                    textHT.Text = Convert.ToString(lHT);
                    textHE.Text = Convert.ToString(lHE);
                    textHS.Text = Convert.ToString(lHS);
                    textVT.Text = Convert.ToString(lVT);
                    textVE.Text = Convert.ToString(lVE);
                    textVS.Text = Convert.ToString(lVS);
                    break;

                default:
                    m_CArtCam.GetCaptureWindowEx(out lHT, out lHS, out lHE, out lVT, out lVS, out lVE);

                    textHT.Text = Convert.ToString(lHT);
                    textHE.Text = Convert.ToString(lHE);
                    textHS.Text = Convert.ToString(lHS);
                    textVT.Text = Convert.ToString(lVT);
                    textVE.Text = Convert.ToString(lVE);
                    textVS.Text = Convert.ToString(lVS);
                    break;
            }

        }

        private int getColorMode()
        {
            return ((m_CArtCam.GetColorMode() + 7) & ~7);
        }

        // Set color mode
        private void InitColorMode()
        {
            // Color number
            switch (getColorMode())
            {
                case 8: radioColor08.Checked = true; break;
                case 16: radioColor16.Checked = true; break;
                case 24: radioColor24.Checked = true; break;
                case 32: radioColor32.Checked = true; break;
                case 48: radioColor48.Checked = true; break;
                case 64: radioColor64.Checked = true; break;
            }

            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS:
                    radioColor08.Enabled = false;
                    radioColor16.Enabled = false;
                    radioColor32.Enabled = false;
                    radioColor48.Enabled = false;
                    radioColor64.Enabled = false;
                    break;

                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV:
                    radioColor32.Enabled = false;
                    radioColor48.Enabled = false;
                    radioColor64.Enabled = false;
                    break;

                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_320P:
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_200SH:
                    radioColor16.Enabled = false;
                    radioColor32.Enabled = false;
                    radioColor48.Enabled = false;
                    radioColor64.Enabled = false;
                    break;
            }
        }

        // Set information for sub-sampling.
        private void InitSubSample()
        {
            // Sub-sampling
            switch ((SUBSAMPLE)((int)m_CArtCam.GetSubSample() & 0x03))
            {
                case SUBSAMPLE.SUBSAMPLE_1: radioSubSample1.Checked = true; break;
                case SUBSAMPLE.SUBSAMPLE_2: radioSubSample2.Checked = true; break;
                case SUBSAMPLE.SUBSAMPLE_4: radioSubSample4.Checked = true; break;
                case SUBSAMPLE.SUBSAMPLE_8: radioSubSample8.Checked = true; break;
            }

            if (((int)m_CArtCam.GetSubSample() & 0x10) == 0x10)
            {
                checkBinning.Checked = true;
            }
            else
            {
                checkBinning.Checked = false;
            }
        }

        // Initialize CNV.
        private void InitCNV()
        {
            // For CNV only
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV == m_DllType)
            {
                switch (m_CArtCam.GetVideoFormat())
                {
                    case VIDEOFORMAT.VIDEOFORMAT_NTSC: labelVideo.Text = "VideoFormat : NTSC"; break;
                    case VIDEOFORMAT.VIDEOFORMAT_PAL: labelVideo.Text = "VideoFormat : PAL"; break;
                    case VIDEOFORMAT.VIDEOFORMAT_PALM: labelVideo.Text = "VideoFormat : PALM"; break;
                    case VIDEOFORMAT.VIDEOFORMAT_SECAM: labelVideo.Text = "VideoFormat : SECAM"; break;
                }

                switch (m_CArtCam.GetSamplingRate())
                {
                    case SAMPLING_RATE.WIDE_HISPEED: radioWH.Checked = true; break;
                    case SAMPLING_RATE.WIDE_LOWSPEED: radioWL.Checked = true; break;
                    case SAMPLING_RATE.NORMAL_HISPEED: radioNH.Checked = true; break;
                    case SAMPLING_RATE.NORMAL_LOWSPEED: radioNL.Checked = true; break;
                }
            }
            else
            {
                radioWH.Enabled = false;
                radioWL.Enabled = false;
                radioNH.Enabled = false;
                radioNL.Enabled = false;

                radioChannel1.Enabled = false;
                radioChannel2.Enabled = false;
                radioChannel3.Enabled = false;
                radioChannel4.Enabled = false;
                radioChannel5.Enabled = false;
                radioChannel6.Enabled = false;
            }
        }

        // Obtain sub-code
        private void InitSubCode()
        {
            textCode1.Text = Convert.ToString(m_CArtCam.ReadSromID(0));

            if (0 == m_CArtCam.m_Error)
            {
                textCode1.Enabled = false;
                textCode2.Enabled = false;
                textCode3.Enabled = false;
                textCode4.Enabled = false;
                textCode5.Enabled = false;
                textCode6.Enabled = false;
                textCode7.Enabled = false;
                textCode8.Enabled = false;

                checkCode.Enabled = false;
                return;
            }

            textCode2.Text = Convert.ToString(m_CArtCam.ReadSromID(1));
            textCode3.Text = Convert.ToString(m_CArtCam.ReadSromID(2));
            textCode4.Text = Convert.ToString(m_CArtCam.ReadSromID(3));
            textCode5.Text = Convert.ToString(m_CArtCam.ReadSromID(4));
            textCode6.Text = Convert.ToString(m_CArtCam.ReadSromID(5));
            textCode7.Text = Convert.ToString(m_CArtCam.ReadSromID(6));
            textCode8.Text = Convert.ToString(m_CArtCam.ReadSromID(7));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int lHT, lHS, lHE, lVT, lVS, lVE, lFps;
            lHT = Convert.ToInt32(textHT.Text);
            lHE = Convert.ToInt32(textHE.Text);
            lHS = Convert.ToInt32(textHS.Text);
            lVT = Convert.ToInt32(textVT.Text);
            lVE = Convert.ToInt32(textVE.Text);
            lVS = Convert.ToInt32(textVS.Text);
            lFps = Convert.ToInt32(textFps.Text);

            /*
			// Size 0 represents error
			if(0 == lVT || 0 == lVE || 0 == lHT || 0 == lHE)
			{
				MessageBox.Show("Set size is wrong");
				return;
			}

			// Effective resolution larger than maximum resolution results in error.
			if( (lVT < lVE + lVS) || (lHT < lHE + lHS) )
			{
				MessageBox.Show("Set size is wrong");
				return;
			}

			if ((lVT > m_CameraInfo.lHeight) || (lHT > m_CameraInfo.lWidth))
				{
					MessageBox.Show("Set size is wrong");
					return;
				}

			// When waiting period is set 0, there will be excessive load on CPU.
			if(ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS != m_DllType)
			{
				if(0 >= lFps)
				{
					MessageBox.Show("Set at least 1 for waiting period. /n performance will deteriorate.");
					return;
				}
			}
			*/



            // Size & frame rate settings.
            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_DS:
                    m_CArtCam.SetCaptureWindow(lHE, lVE, lFps);
                    break;

                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_CNV:
                    if (radioWH.Checked) m_CArtCam.SetSamplingRate(SAMPLING_RATE.WIDE_HISPEED);
                    if (radioWL.Checked) m_CArtCam.SetSamplingRate(SAMPLING_RATE.WIDE_LOWSPEED);
                    if (radioNH.Checked) m_CArtCam.SetSamplingRate(SAMPLING_RATE.NORMAL_HISPEED);
                    if (radioNL.Checked) m_CArtCam.SetSamplingRate(SAMPLING_RATE.NORMAL_LOWSPEED);

                    if (radioChannel1.Checked) m_CArtCam.SetCrossbar(0, 0);
                    if (radioChannel2.Checked) m_CArtCam.SetCrossbar(1, 0);
                    if (radioChannel3.Checked) m_CArtCam.SetCrossbar(2, 0);
                    if (radioChannel4.Checked) m_CArtCam.SetCrossbar(3, 0);
                    if (radioChannel5.Checked) m_CArtCam.SetCrossbar(4, 0);
                    if (radioChannel6.Checked) m_CArtCam.SetCrossbar(5, 0);
                    break;
                /*
                                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_098:
                                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_500P:
                                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P2:
                                    break;
                */
                default:
                    m_CArtCam.SetCaptureWindowEx(lHT, lHS, lHE, lVT, lVS, lVE);
                    m_CArtCam.SetWaitTime(lFps);
                    break;
            }

            // Invalid values cannot be set at dialog
            // It reflects without modification
            // There is no problem since inactive camera returns error as well
            if (checkBinning.Checked)
            {
                if (radioSubSample1.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.SUBSAMPLE_1);
                if (radioSubSample2.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.BINNING_2);
                if (radioSubSample4.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.BINNING_4);
            }
            else
            {
                if (radioSubSample1.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.SUBSAMPLE_1);
                if (radioSubSample2.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.SUBSAMPLE_2);
                if (radioSubSample4.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.SUBSAMPLE_4);
                if (radioSubSample8.Checked) m_CArtCam.SetSubSample(SUBSAMPLE.SUBSAMPLE_8);
            }
            if (radioColor08.Checked) m_CArtCam.SetColorMode(8);
            if (radioColor16.Checked) m_CArtCam.SetColorMode(16);
            if (radioColor24.Checked) m_CArtCam.SetColorMode(24);
            if (radioColor32.Checked) m_CArtCam.SetColorMode(32);
            if (radioColor48.Checked) m_CArtCam.SetColorMode(48);
            if (radioColor64.Checked) m_CArtCam.SetColorMode(64);


            // Write sub-code
            // Make sure writing is correct
            if (checkCode.Checked)
            {
                byte[] m_Code = new byte[8];
                m_Code[0] = Convert.ToByte(textCode1.Text);
                m_Code[1] = Convert.ToByte(textCode2.Text);
                m_Code[2] = Convert.ToByte(textCode3.Text);
                m_Code[3] = Convert.ToByte(textCode4.Text);
                m_Code[4] = Convert.ToByte(textCode5.Text);
                m_Code[5] = Convert.ToByte(textCode6.Text);
                m_Code[6] = Convert.ToByte(textCode7.Text);
                m_Code[7] = Convert.ToByte(textCode8.Text);

                m_CArtCam.WriteSromID(0, m_Code[0]);
                m_CArtCam.WriteSromID(1, m_Code[1]);
                m_CArtCam.WriteSromID(2, m_Code[2]);
                m_CArtCam.WriteSromID(3, m_Code[3]);
                m_CArtCam.WriteSromID(4, m_Code[4]);
                m_CArtCam.WriteSromID(5, m_Code[5]);
                m_CArtCam.WriteSromID(6, m_Code[6]);
                m_CArtCam.WriteSromID(7, m_Code[7]);
            }
        }
    }
}
