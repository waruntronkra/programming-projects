namespace IP3000_Control.IP3000
{
    partial class SNInputFrm
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
            this.tbSerialNo = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbSerialNo
            // 
            this.tbSerialNo.BackColor = System.Drawing.Color.White;
            this.tbSerialNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSerialNo.Location = new System.Drawing.Point(84, 116);
            this.tbSerialNo.Multiline = true;
            this.tbSerialNo.Name = "tbSerialNo";
            this.tbSerialNo.Size = new System.Drawing.Size(852, 104);
            this.tbSerialNo.TabIndex = 0;
            this.tbSerialNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSerialNo.TextChanged += new System.EventHandler(this.tbSerialNo_TextChanged);
            // 
            // btOK
            // 
            this.btOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOK.Location = new System.Drawing.Point(193, 251);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(267, 75);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btReset
            // 
            this.btReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btReset.Location = new System.Drawing.Point(476, 251);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(267, 75);
            this.btReset.TabIndex = 2;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = false;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(79, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(446, 51);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please input serial no.";
            // 
            // SNInputFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 383);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbSerialNo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SNInputFrm";
            this.Text = "SNInputFrm";
            this.Load += new System.EventHandler(this.SNInputFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSerialNo;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Label label1;
    }
}