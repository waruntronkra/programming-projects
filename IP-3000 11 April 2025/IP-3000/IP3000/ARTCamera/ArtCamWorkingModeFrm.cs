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
    public partial class ArtCamWorkingModeFrm : Form
    {
        private CArtCam m_CArtCam = null;
        private ARTCAM_CAMERATYPE m_DllType = 0;
        private CAMERAINFO m_CameraInfo;
        private int m_CursorNumber = 0;
        private bool m_Init = false;

        public struct M_CURSOR
        {
            public int Enable;
            public int SizeX;
            public int SizeY;
            public int PosX;
            public int PosY;
            public int Color;
        }
        private M_CURSOR[] m_Cursor = new M_CURSOR[2];

        public ArtCamWorkingModeFrm(CArtCam a)
        {
            InitializeComponent();
            m_CArtCam = a;
            m_Init = false;
        }

        private void ArtCamWorkingModeFrm_Load(object sender, EventArgs e)
        {
            m_DllType = (ARTCAM_CAMERATYPE)(m_CArtCam.GetDllVersion() >> 16);
            m_CameraInfo.lSize = System.Runtime.InteropServices.Marshal.SizeOf(m_CameraInfo);
            m_CArtCam.GetCameraInfo(ref m_CameraInfo);
            m_Init = false;

            if (1 == m_CArtCam.Monitor_GetColorMode())
            {
                checkColor.Checked = true;
            }
            else
            {
                checkAWB.Enabled = false;
            }

            if (1 == m_CArtCam.Monitor_GetBayerGainAuto())
            {
                checkAWB.Checked = true;

                numericGainR.Enabled = trackGainR.Enabled = false;
                numericGainG.Enabled = trackGainG.Enabled = false;
                numericGainB.Enabled = trackGainB.Enabled = false;
            }

            // Bayer
            InitControl(numericGainR, trackGainR, 0, 1023, m_CArtCam.Monitor_GetBayerGainRed());
            InitControl(numericGainG, trackGainG, 0, 1023, m_CArtCam.Monitor_GetBayerGainGreen());
            InitControl(numericGainB, trackGainB, 0, 1023, m_CArtCam.Monitor_GetBayerGainBlue());

            // Gain, Shutter, Mirror
            InitControl(numericGainAll, trackGainAll, m_CameraInfo.lGlobalGainMin, m_CameraInfo.lGlobalGainMax, m_CArtCam.GetGlobalGain());
            InitControl(numericShutter, trackShutter, m_CameraInfo.lExposureMin, m_CameraInfo.lExposureMax, m_CArtCam.GetExposureTime());

            checkMirrorV.Checked = m_CArtCam.GetMirrorV();
            checkMirrorH.Checked = m_CArtCam.GetMirrorH();

            for (int i = 0; i < 2; i++)
            {
                // CursorMode
                m_Cursor[i].Enable = m_CArtCam.Monitor_GetCrossCursorMode(i);

                // Position
                m_CArtCam.Monitor_GetCrossCursorPos(i, out m_Cursor[i].PosX, out m_Cursor[i].PosY);

                // Size
                m_CArtCam.Monitor_GetCrossCursorSize(i, out m_Cursor[i].SizeX, out m_Cursor[i].SizeY);

                // Color
                m_Cursor[i].Color = m_CArtCam.Monitor_GetCrossCursorColorRGB(i);
            }

            int Width = 1280;
            int Height = 1024;
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_1000MI_HD2 == m_DllType)
            {
                switch (m_CArtCam.Width())
                {
                    case 3664:
                    case 1920:
                        Width = 1920;
                        Height = 1080;
                        break;
                    case 1280:
                        Width = 1280;
                        Height = 720;
                        break;
                }
            }
            else
            {
                int wReg0D = m_CArtCam.Fpga_ReadRegister(0x0D);
                switch (wReg0D)
                {
                    case 0: Width = 1280; Height = 720; break;
                    case 1: Width = 1280; Height = 1024; break;
                }
            }
            InitControl(numericXPos, trackXPos, 0, Width, m_Cursor[m_CursorNumber].PosX);
            InitControl(numericYPos, trackYPos, 0, Height, m_Cursor[m_CursorNumber].PosY);

            InitControl(numericXSize, trackXSize, 0, 7, m_Cursor[m_CursorNumber].SizeX);
            InitControl(numericYSize, trackYSize, 0, 7, m_Cursor[m_CursorNumber].SizeY);

            radioCursor1.Checked = true;
            checkCursorOn.Checked = (1 == m_Cursor[m_CursorNumber].Enable) ? true : false;
            Invalidate();
            m_Init = true;
        }

        private void checkAWB_CheckedChanged(object sender, EventArgs e)
        {
            bool Color = checkColor.Checked;
            bool AWB = checkAWB.Checked;
            if (!m_Init) return;

            m_CArtCam.Monitor_SetColorMode(Color ? 1 : 0);
            m_CArtCam.Monitor_SetBayerGainAuto(AWB ? 1 : 0);

            bool Flg = (Color && !AWB);
            numericGainR.Enabled = trackGainR.Enabled = Flg;
            numericGainG.Enabled = trackGainG.Enabled = Flg;
            numericGainB.Enabled = trackGainB.Enabled = Flg;

            if (Flg)
            {
                numericGainR.Value = m_CArtCam.Monitor_GetBayerGainRed();
                numericGainG.Value = m_CArtCam.Monitor_GetBayerGainGreen();
                numericGainB.Value = m_CArtCam.Monitor_GetBayerGainBlue();
            }

            checkAWB.Enabled = Color;
        }

        private void InitControl(NumericUpDown n, TrackBar t, int Minimum, int Maximum, int Now)
        {
            n.Minimum = Minimum; n.Maximum = Maximum;
            t.Minimum = Minimum; t.Maximum = Maximum;

            if (Now < Minimum || Maximum < Now)
            {
                n.Enabled = t.Enabled = false;
                return;
            }
            t.Value = Now;
            n.Value = Now;
        }

        private void checkColor_CheckedChanged(object sender, EventArgs e)
        {
            checkAWB_CheckedChanged(sender, e);
        }

        private void checkMirrorV_CheckedChanged(object sender, EventArgs e)
        {
            m_CArtCam.SetMirrorV(checkMirrorV.Checked);
        }

        private void checkMirrorH_CheckedChanged(object sender, EventArgs e)
        {
            m_CArtCam.SetMirrorH(checkMirrorH.Checked);
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            numericGainR.Value = 256;
            numericGainG.Value = 256;
            numericGainB.Value = 256;

            numericXPos.Value = 360;
            numericYPos.Value = 360;
            numericXSize.Value = 4;
            numericYSize.Value = 4;

            switch (m_DllType)
            {
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_1000MI_HD2:
                    numericGainAll.Value = 64;
                    numericShutter.Value = 2748;
                    break;
                case ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_150P5_HD2:
                    numericGainAll.Value = 512;
                    numericShutter.Value = 64;
                    break;
            }
        }

        private void buttonInitSettings_Click(object sender, EventArgs e)
        {
            m_CArtCam.Monitor_InitRegisterSettings(0);
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            m_CArtCam.Monitor_SaveCurrentSettings();
        }

        private void pictureColor_MouseUp(object sender, MouseEventArgs e)
        {
            int w = pictureColor.Width / 16;
            int h = pictureColor.Height;
            Point p = Cursor.Position;

            for (int i = 0; i < 16; i++)
            {
                Rectangle rc = new Rectangle(w * i, 0, w, h);

                if (rc.Contains(pictureColor.PointToClient(p)))
                {
                    m_CArtCam.Monitor_SetCrossCursorColorRGB(m_CursorNumber, i);
                    Invalidate();
                    break;
                }
            }
        }
    }
}
