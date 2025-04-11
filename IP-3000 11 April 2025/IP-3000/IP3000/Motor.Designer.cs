namespace IP3000_Control
{
    partial class Motor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Motor));
            this.Control_groupBox = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBR = new System.Windows.Forms.Button();
            this.btnBL = new System.Windows.Forms.Button();
            this.btnTL = new System.Windows.Forms.Button();
            this.btnTR = new System.Windows.Forms.Button();
            this.btnCCW = new System.Windows.Forms.Button();
            this.btnCW = new System.Windows.Forms.Button();
            this.btnR = new System.Windows.Forms.Button();
            this.btnB = new System.Windows.Forms.Button();
            this.btnT = new System.Windows.Forms.Button();
            this.btnL = new System.Windows.Forms.Button();
            this.timerMouseDown = new System.Windows.Forms.Timer(this.components);
            this.timerTurnLimitOn = new System.Windows.Forms.Timer(this.components);
            this.btnSlow = new System.Windows.Forms.Button();
            this.btnMeduim = new System.Windows.Forms.Button();
            this.btnFast = new System.Windows.Forms.Button();
            this.timerGetInternalCounter = new System.Windows.Forms.Timer(this.components);
            this.Control_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Control_groupBox
            // 
            this.Control_groupBox.Controls.Add(this.splitContainer2);
            this.Control_groupBox.Location = new System.Drawing.Point(9, 2);
            this.Control_groupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Control_groupBox.Name = "Control_groupBox";
            this.Control_groupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Control_groupBox.Size = new System.Drawing.Size(449, 302);
            this.Control_groupBox.TabIndex = 3;
            this.Control_groupBox.TabStop = false;
            this.Control_groupBox.Text = "Control";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(4, 20);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.btnBR);
            this.splitContainer2.Panel1.Controls.Add(this.btnBL);
            this.splitContainer2.Panel1.Controls.Add(this.btnTL);
            this.splitContainer2.Panel1.Controls.Add(this.btnTR);
            this.splitContainer2.Panel1.Controls.Add(this.btnCCW);
            this.splitContainer2.Panel1.Controls.Add(this.btnCW);
            this.splitContainer2.Panel1.Controls.Add(this.btnR);
            this.splitContainer2.Panel1.Controls.Add(this.btnB);
            this.splitContainer2.Panel1.Controls.Add(this.btnT);
            this.splitContainer2.Panel1.Controls.Add(this.btnL);
            this.splitContainer2.Size = new System.Drawing.Size(473, 278);
            this.splitContainer2.SplitterDistance = 434;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button1.Location = new System.Drawing.Point(195, 96);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 86);
            this.button1.TabIndex = 24;
            this.button1.Text = "CEN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBR
            // 
            this.btnBR.BackgroundImage = global::IP3000.Properties.Resources.BR;
            this.btnBR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBR.Location = new System.Drawing.Point(271, 190);
            this.btnBR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBR.Name = "btnBR";
            this.btnBR.Size = new System.Drawing.Size(157, 82);
            this.btnBR.TabIndex = 23;
            this.btnBR.UseVisualStyleBackColor = true;
            this.btnBR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBR_MouseDown);
            this.btnBR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBR_MouseUp);
            // 
            // btnBL
            // 
            this.btnBL.BackgroundImage = global::IP3000.Properties.Resources.BL;
            this.btnBL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBL.Location = new System.Drawing.Point(7, 190);
            this.btnBL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBL.Name = "btnBL";
            this.btnBL.Size = new System.Drawing.Size(157, 82);
            this.btnBL.TabIndex = 22;
            this.btnBL.UseVisualStyleBackColor = true;
            this.btnBL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBL_MouseDown);
            this.btnBL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBL_MouseUp);
            // 
            // btnTL
            // 
            this.btnTL.BackgroundImage = global::IP3000.Properties.Resources.TL;
            this.btnTL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTL.Location = new System.Drawing.Point(7, 4);
            this.btnTL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTL.Name = "btnTL";
            this.btnTL.Size = new System.Drawing.Size(157, 82);
            this.btnTL.TabIndex = 21;
            this.btnTL.UseVisualStyleBackColor = true;
            this.btnTL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTL_MouseDown);
            this.btnTL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTL_MouseUp);
            // 
            // btnTR
            // 
            this.btnTR.BackgroundImage = global::IP3000.Properties.Resources.TR;
            this.btnTR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTR.Location = new System.Drawing.Point(271, 4);
            this.btnTR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTR.Name = "btnTR";
            this.btnTR.Size = new System.Drawing.Size(157, 82);
            this.btnTR.TabIndex = 8;
            this.btnTR.UseVisualStyleBackColor = true;
            this.btnTR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTR_MouseDown);
            this.btnTR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTR_MouseUp);
            // 
            // btnCCW
            // 
            this.btnCCW.BackgroundImage = global::IP3000.Properties.Resources.CCW;
            this.btnCCW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCCW.Location = new System.Drawing.Point(255, 96);
            this.btnCCW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCCW.Name = "btnCCW";
            this.btnCCW.Size = new System.Drawing.Size(79, 87);
            this.btnCCW.TabIndex = 20;
            this.btnCCW.UseVisualStyleBackColor = true;
            this.btnCCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCCW_MouseDown);
            this.btnCCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCCW_MouseUp);
            // 
            // btnCW
            // 
            this.btnCW.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCW.BackgroundImage")));
            this.btnCW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCW.Location = new System.Drawing.Point(104, 96);
            this.btnCW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(83, 86);
            this.btnCW.TabIndex = 19;
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCW_MouseDown);
            this.btnCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCW_MouseUp);
            // 
            // btnR
            // 
            this.btnR.BackgroundImage = global::IP3000  .Properties.Resources.Right;
            this.btnR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnR.Location = new System.Drawing.Point(336, 96);
            this.btnR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnR.Name = "btnR";
            this.btnR.Size = new System.Drawing.Size(95, 87);
            this.btnR.TabIndex = 17;
            this.btnR.UseVisualStyleBackColor = true;
            this.btnR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnR_MouseDown);
            this.btnR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnR_MouseUp);
            // 
            // btnB
            // 
            this.btnB.BackgroundImage = global::IP3000.Properties.Resources.Bottom;
            this.btnB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnB.Location = new System.Drawing.Point(171, 190);
            this.btnB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnB.Name = "btnB";
            this.btnB.Size = new System.Drawing.Size(95, 82);
            this.btnB.TabIndex = 16;
            this.btnB.UseVisualStyleBackColor = true;
            this.btnB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnB_MouseDown);
            this.btnB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnB_MouseUp);
            // 
            // btnT
            // 
            this.btnT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnT.BackgroundImage")));
            this.btnT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnT.Location = new System.Drawing.Point(171, 4);
            this.btnT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnT.Name = "btnT";
            this.btnT.Size = new System.Drawing.Size(95, 82);
            this.btnT.TabIndex = 15;
            this.btnT.UseVisualStyleBackColor = true;
            this.btnT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnT_MouseDown);
            this.btnT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnT_MouseUp);
            // 
            // btnL
            // 
            this.btnL.BackgroundImage = global::IP3000.Properties.Resources.Left;
            this.btnL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnL.Location = new System.Drawing.Point(7, 96);
            this.btnL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnL.Name = "btnL";
            this.btnL.Size = new System.Drawing.Size(95, 87);
            this.btnL.TabIndex = 14;
            this.btnL.UseVisualStyleBackColor = true;
            this.btnL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnL_MouseDown);
            this.btnL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnL_MouseUp);
            // 
            // timerMouseDown
            // 
            this.timerMouseDown.Interval = 1;
            this.timerMouseDown.Tick += new System.EventHandler(this.timerMouseDown_Tick);
            // 
            // timerTurnLimitOn
            // 
            this.timerTurnLimitOn.Interval = 1;
            this.timerTurnLimitOn.Tick += new System.EventHandler(this.timerTurnLimitOn_Tick);
            // 
            // btnSlow
            // 
            this.btnSlow.BackColor = System.Drawing.Color.Gray;
            this.btnSlow.Location = new System.Drawing.Point(20, 308);
            this.btnSlow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSlow.Name = "btnSlow";
            this.btnSlow.Size = new System.Drawing.Size(132, 60);
            this.btnSlow.TabIndex = 5;
            this.btnSlow.Text = "SLOW";
            this.btnSlow.UseVisualStyleBackColor = false;
            this.btnSlow.Click += new System.EventHandler(this.btnSlow_Click);
            // 
            // btnMeduim
            // 
            this.btnMeduim.BackColor = System.Drawing.Color.Gray;
            this.btnMeduim.Location = new System.Drawing.Point(160, 308);
            this.btnMeduim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMeduim.Name = "btnMeduim";
            this.btnMeduim.Size = new System.Drawing.Size(132, 60);
            this.btnMeduim.TabIndex = 6;
            this.btnMeduim.Text = "MEDUIM";
            this.btnMeduim.UseVisualStyleBackColor = false;
            this.btnMeduim.Click += new System.EventHandler(this.btnMeduim_Click);
            // 
            // btnFast
            // 
            this.btnFast.BackColor = System.Drawing.Color.Gray;
            this.btnFast.Location = new System.Drawing.Point(312, 308);
            this.btnFast.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFast.Name = "btnFast";
            this.btnFast.Size = new System.Drawing.Size(132, 60);
            this.btnFast.TabIndex = 7;
            this.btnFast.Text = "FAST";
            this.btnFast.UseVisualStyleBackColor = false;
            this.btnFast.Click += new System.EventHandler(this.btnFast_Click);
            // 
            // timerGetInternalCounter
            // 
            this.timerGetInternalCounter.Enabled = true;
            this.timerGetInternalCounter.Interval = 1;
            this.timerGetInternalCounter.Tick += new System.EventHandler(this.timerGetInternalCounter_Tick);
            // 
            // Motor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 372);
            this.ControlBox = false;
            this.Controls.Add(this.btnFast);
            this.Controls.Add(this.btnMeduim);
            this.Controls.Add(this.btnSlow);
            this.Controls.Add(this.Control_groupBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Motor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Motor";
            this.Load += new System.EventHandler(this.Motor_Load);
            this.Control_groupBox.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Control_groupBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnBR;
        private System.Windows.Forms.Button btnBL;
        private System.Windows.Forms.Button btnTL;
        private System.Windows.Forms.Button btnTR;
        private System.Windows.Forms.Button btnCCW;
        private System.Windows.Forms.Button btnCW;
        private System.Windows.Forms.Button btnR;
        private System.Windows.Forms.Button btnB;
        private System.Windows.Forms.Button btnT;
        private System.Windows.Forms.Button btnL;
        private System.Windows.Forms.Timer timerMouseDown;
        private System.Windows.Forms.Timer timerTurnLimitOn;
        private System.Windows.Forms.Button btnSlow;
        private System.Windows.Forms.Button btnMeduim;
        private System.Windows.Forms.Button btnFast;
        private System.Windows.Forms.Timer timerGetInternalCounter;
    }
}