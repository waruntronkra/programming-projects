namespace IP3000.ARTCamera
{
    partial class ArtCamCorrectionFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSaveMask = new System.Windows.Forms.Button();
            this.checkDotfilterEnable = new System.Windows.Forms.CheckBox();
            this.buttonUpdateHigh = new System.Windows.Forms.Button();
            this.buttonLoadMask = new System.Windows.Forms.Button();
            this.buttonUpdateLow = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkCorrectionEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSaveMask
            // 
            this.buttonSaveMask.Location = new System.Drawing.Point(26, 275);
            this.buttonSaveMask.Margin = new System.Windows.Forms.Padding(6);
            this.buttonSaveMask.Name = "buttonSaveMask";
            this.buttonSaveMask.Size = new System.Drawing.Size(200, 71);
            this.buttonSaveMask.TabIndex = 4;
            this.buttonSaveMask.Text = "Save";
            this.buttonSaveMask.UseVisualStyleBackColor = true;
            this.buttonSaveMask.Click += new System.EventHandler(this.buttonSaveMask_Click);
            // 
            // checkDotfilterEnable
            // 
            this.checkDotfilterEnable.AutoSize = true;
            this.checkDotfilterEnable.Location = new System.Drawing.Point(26, 96);
            this.checkDotfilterEnable.Margin = new System.Windows.Forms.Padding(6);
            this.checkDotfilterEnable.Name = "checkDotfilterEnable";
            this.checkDotfilterEnable.Size = new System.Drawing.Size(279, 29);
            this.checkDotfilterEnable.TabIndex = 1;
            this.checkDotfilterEnable.Text = "Effective pixel correction";
            this.checkDotfilterEnable.UseVisualStyleBackColor = true;
            this.checkDotfilterEnable.CheckedChanged += new System.EventHandler(this.checkDotfilterEnable_CheckedChanged);
            // 
            // buttonUpdateHigh
            // 
            this.buttonUpdateHigh.Location = new System.Drawing.Point(238, 169);
            this.buttonUpdateHigh.Margin = new System.Windows.Forms.Padding(6);
            this.buttonUpdateHigh.Name = "buttonUpdateHigh";
            this.buttonUpdateHigh.Size = new System.Drawing.Size(198, 71);
            this.buttonUpdateHigh.TabIndex = 3;
            this.buttonUpdateHigh.Text = "Register(bright)";
            this.buttonUpdateHigh.UseVisualStyleBackColor = true;
            this.buttonUpdateHigh.Click += new System.EventHandler(this.buttonUpdateHigh_Click);
            // 
            // buttonLoadMask
            // 
            this.buttonLoadMask.Location = new System.Drawing.Point(238, 275);
            this.buttonLoadMask.Margin = new System.Windows.Forms.Padding(6);
            this.buttonLoadMask.Name = "buttonLoadMask";
            this.buttonLoadMask.Size = new System.Drawing.Size(198, 71);
            this.buttonLoadMask.TabIndex = 5;
            this.buttonLoadMask.Text = "Load";
            this.buttonLoadMask.UseVisualStyleBackColor = true;
            this.buttonLoadMask.Click += new System.EventHandler(this.buttonLoadMask_Click);
            // 
            // buttonUpdateLow
            // 
            this.buttonUpdateLow.Location = new System.Drawing.Point(26, 169);
            this.buttonUpdateLow.Margin = new System.Windows.Forms.Padding(6);
            this.buttonUpdateLow.Name = "buttonUpdateLow";
            this.buttonUpdateLow.Size = new System.Drawing.Size(200, 71);
            this.buttonUpdateLow.TabIndex = 2;
            this.buttonUpdateLow.Text = "Register(dark)";
            this.buttonUpdateLow.UseVisualStyleBackColor = true;
            this.buttonUpdateLow.Click += new System.EventHandler(this.buttonUpdateLow_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(333, 403);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(150, 48);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkCorrectionEnable
            // 
            this.checkCorrectionEnable.AutoSize = true;
            this.checkCorrectionEnable.Location = new System.Drawing.Point(41, 65);
            this.checkCorrectionEnable.Margin = new System.Windows.Forms.Padding(6);
            this.checkCorrectionEnable.Name = "checkCorrectionEnable";
            this.checkCorrectionEnable.Size = new System.Drawing.Size(285, 29);
            this.checkCorrectionEnable.TabIndex = 8;
            this.checkCorrectionEnable.Text = "Effective mask correction";
            this.checkCorrectionEnable.UseVisualStyleBackColor = true;
            this.checkCorrectionEnable.CheckedChanged += new System.EventHandler(this.checkCorrectionEnable_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSaveMask);
            this.groupBox1.Controls.Add(this.checkDotfilterEnable);
            this.groupBox1.Controls.Add(this.buttonUpdateHigh);
            this.groupBox1.Controls.Add(this.buttonLoadMask);
            this.groupBox1.Controls.Add(this.buttonUpdateLow);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(468, 375);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Correction";
            // 
            // ArtCamCorrectionFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 479);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkCorrectionEnable);
            this.Controls.Add(this.groupBox1);
            this.Name = "ArtCamCorrectionFrm";
            this.Text = "ArtCamCorrectionFrm";
            this.Load += new System.EventHandler(this.ArtCamCorrectionFrm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveMask;
        private System.Windows.Forms.CheckBox checkDotfilterEnable;
        private System.Windows.Forms.Button buttonUpdateHigh;
        private System.Windows.Forms.Button buttonLoadMask;
        private System.Windows.Forms.Button buttonUpdateLow;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkCorrectionEnable;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}