using IP3000.Recipe;
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
    public partial class ResultFrm : Form
    {
        public string InspectionResult { get; set; }
        public string ImageName { get; set; }

        public ProductSide NextProductSide { get; set; }

        public int IndexNextProgram { get; set; }

        public ResultFrm()
        {
            InitializeComponent();
        }

        private void VisualFrm_Load(object sender, EventArgs e)
        {
            lbResult.Text = InspectionResult;
            if(lbResult.Text.Contains("Fail"))
            {
                lbResult.BackColor = Color.IndianRed;
            }
            else
            {
                lbResult.BackColor = Color.GreenYellow;
            }

            //cbListProgram.Items.Clear();
            //cbListProgram.Items.AddRange(new string[] {"AC12000 Bottom", "AC1200 PCBA", "AC1200 Heatsink"});

            //if(NextProductSide == ProductSide.Bottom)
            //{
            //    cbListProgram.SelectedIndex = 0;
            //}
            //else if(NextProductSide == ProductSide.PCBA)
            //{
            //    cbListProgram.SelectedIndex = 1;
            //}
            //else if(NextProductSide == ProductSide.Top)
            //{
            //    cbListProgram.SelectedIndex=2;
            //}
        }

        private void btPass_Click(object sender, EventArgs e)
        {

        }

        private void btConfirmChagngeProgram_Click(object sender, EventArgs e)
        {
            IndexNextProgram = cbListProgram.SelectedIndex;
            this.Close();
        }
    }
}
