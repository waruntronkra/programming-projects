using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000_Control.IP3000
{
    public partial class SNInputFrm : Form
    {
        private const int SNLength = 9;
        private string snPrefix = string.Empty;
        private string snSuffix = string.Empty;

        public string SerialNo { get; set; }

        public SNInputFrm()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            SerialNo = tbSerialNo.Text.Trim(new char[] { '\r', '\n' });
            if (SerialNo.Length == SNLength)
            {
                this.Close();
            }
            else
            {
                tbSerialNo.Text = string.Empty;
                MessageBox.Show("Incorrect S/N. Please input again");
            }
        }

        public string GetSerialNo()
        {
            return SerialNo;
        }

        private void tbSerialNo_TextChanged(object sender, EventArgs e)
        {
            if (tbSerialNo.Text.Contains("\r\n"))
            {
                SerialNo = tbSerialNo.Text.Trim(new char[] { '\r', '\n' });
                if(SerialNo.Length == SNLength)
                {
                    this.Close();
                }
                else
                {
                    tbSerialNo.Text = string.Empty;
                    MessageBox.Show("Incorrect S/N. Please input again");
                }
            }
        }

        private void SNInputFrm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tbSerialNo.Focus(); 
            this.TopMost = true;    
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            tbSerialNo.Text = string.Empty;
            tbSerialNo.Focus();
        }
    }
}
