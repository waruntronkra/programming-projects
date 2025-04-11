namespace IP3000_Control.IP3000
{
    partial class ResultFrm
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
            this.lbTitle = new System.Windows.Forms.Label();
            this.btConfirmChagngeProgram = new System.Windows.Forms.Button();
            this.cbListProgram = new System.Windows.Forms.ComboBox();
            this.lbResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(59, 437);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(798, 86);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "Please change unit to:";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btConfirmChagngeProgram
            // 
            this.btConfirmChagngeProgram.BackColor = System.Drawing.Color.SkyBlue;
            this.btConfirmChagngeProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConfirmChagngeProgram.Location = new System.Drawing.Point(59, 698);
            this.btConfirmChagngeProgram.Name = "btConfirmChagngeProgram";
            this.btConfirmChagngeProgram.Size = new System.Drawing.Size(789, 157);
            this.btConfirmChagngeProgram.TabIndex = 3;
            this.btConfirmChagngeProgram.Text = "Confirm";
            this.btConfirmChagngeProgram.UseVisualStyleBackColor = false;
            this.btConfirmChagngeProgram.Click += new System.EventHandler(this.btConfirmChagngeProgram_Click);
            // 
            // cbListProgram
            // 
            this.cbListProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbListProgram.FormattingEnabled = true;
            this.cbListProgram.Location = new System.Drawing.Point(59, 550);
            this.cbListProgram.Name = "cbListProgram";
            this.cbListProgram.Size = new System.Drawing.Size(789, 75);
            this.cbListProgram.TabIndex = 4;
            // 
            // lbResult
            // 
            this.lbResult.BackColor = System.Drawing.Color.YellowGreen;
            this.lbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResult.Location = new System.Drawing.Point(60, 76);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(797, 263);
            this.lbResult.TabIndex = 5;
            this.lbResult.Text = "label1";
            this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResultFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 976);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.cbListProgram);
            this.Controls.Add(this.btConfirmChagngeProgram);
            this.Controls.Add(this.lbTitle);
            this.Name = "ResultFrm";
            this.Text = "Inspection Result";
            this.Load += new System.EventHandler(this.VisualFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btConfirmChagngeProgram;
        private System.Windows.Forms.ComboBox cbListProgram;
        private System.Windows.Forms.Label lbResult;
    }
}