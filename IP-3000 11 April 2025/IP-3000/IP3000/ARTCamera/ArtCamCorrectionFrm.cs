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
    public partial class ArtCamCorrectionFrm : Form
    {
        private CArtCam m_CArtCam = null;

        public ArtCamCorrectionFrm(CArtCam a)
        {
            InitializeComponent();
            m_CArtCam = a;
        }

        private void ArtCamCorrectionFrm_Load(object sender, EventArgs e)
        {
            int MaskFilter = m_CArtCam.GetMaskFilter();
            int DotFilter = m_CArtCam.GetDotFilter();

            if (0 != DotFilter) checkDotfilterEnable.Checked = true;
            if (0 != MaskFilter) checkCorrectionEnable.Checked = true;
        }

        private void checkCorrectionEnable_CheckedChanged(object sender, EventArgs e)
        {
            int Enable = 0;
            if (checkCorrectionEnable.Checked == true)
            {
                Enable = 1;
            }
            m_CArtCam.SetMaskFilter(Enable);
        }

        private void checkDotfilterEnable_CheckedChanged(object sender, EventArgs e)
        {
            int Enable = 0;
            if (checkDotfilterEnable.Checked == true)
            {
                Enable = 1;
            }
            m_CArtCam.SetDotFilter(Enable);
        }

        private void buttonUpdateLow_Click(object sender, EventArgs e)
        {
            m_CArtCam.UpdateMaskData(MASKTYPE.MASKTYPE_LOW);
        }

        private void buttonUpdateHigh_Click(object sender, EventArgs e)
        {
            m_CArtCam.UpdateMaskData(MASKTYPE.MASKTYPE_HIGH);
        }

        private void buttonSaveMask_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Default.msk";
            sfd.Filter = "MSK file(*.msk)|*.msk||";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder str = new StringBuilder(256);
                str.Append(sfd.FileName);
                m_CArtCam.SaveMaskFile(str);
            }
        }

        private void buttonLoadMask_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Default.msk";
            ofd.Filter = "MSK file(*.msk)|*.msk||";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder str = new StringBuilder(256);
                str.Append(ofd.FileName);
                m_CArtCam.LoadMaskFile(str);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }
    }
}
