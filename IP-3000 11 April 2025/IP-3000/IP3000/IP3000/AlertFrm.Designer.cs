namespace IP3000.IP3000
{
    partial class AlertFrm
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
            this.lbRejTitle = new System.Windows.Forms.Label();
            this.lbRejMessage = new System.Windows.Forms.Label();
            this.pbRejImage = new System.Windows.Forms.PictureBox();
            this.btReject = new System.Windows.Forms.Button();
            this.btAccept = new System.Windows.Forms.Button();
            this.lbRejcetSide = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbRejImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lbRejTitle
            // 
            this.lbRejTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbRejTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRejTitle.Location = new System.Drawing.Point(40, 40);
            this.lbRejTitle.Name = "lbRejTitle";
            this.lbRejTitle.Size = new System.Drawing.Size(1264, 62);
            this.lbRejTitle.TabIndex = 0;
            this.lbRejTitle.Text = "Reject Title";
            this.lbRejTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRejMessage
            // 
            this.lbRejMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbRejMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRejMessage.Location = new System.Drawing.Point(623, 116);
            this.lbRejMessage.Name = "lbRejMessage";
            this.lbRejMessage.Size = new System.Drawing.Size(681, 52);
            this.lbRejMessage.TabIndex = 3;
            this.lbRejMessage.Text = "Reject Message";
            this.lbRejMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbRejImage
            // 
            this.pbRejImage.Location = new System.Drawing.Point(47, 244);
            this.pbRejImage.Name = "pbRejImage";
            this.pbRejImage.Size = new System.Drawing.Size(868, 729);
            this.pbRejImage.TabIndex = 4;
            this.pbRejImage.TabStop = false;
            // 
            // btReject
            // 
            this.btReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btReject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btReject.Location = new System.Drawing.Point(957, 671);
            this.btReject.Name = "btReject";
            this.btReject.Size = new System.Drawing.Size(513, 302);
            this.btReject.TabIndex = 2;
            this.btReject.Text = "Reject";
            this.btReject.UseVisualStyleBackColor = false;
            this.btReject.Click += new System.EventHandler(this.btReject_Click);
            // 
            // btAccept
            // 
            this.btAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAccept.Location = new System.Drawing.Point(957, 244);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(513, 302);
            this.btAccept.TabIndex = 1;
            this.btAccept.Text = "Accept Part";
            this.btAccept.UseVisualStyleBackColor = false;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // lbRejcetSide
            // 
            this.lbRejcetSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbRejcetSide.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRejcetSide.Location = new System.Drawing.Point(41, 116);
            this.lbRejcetSide.Name = "lbRejcetSide";
            this.lbRejcetSide.Size = new System.Drawing.Size(574, 52);
            this.lbRejcetSide.TabIndex = 7;
            this.lbRejcetSide.Text = "Reject Side";
            this.lbRejcetSide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlertFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1848, 1182);
            this.Controls.Add(this.lbRejcetSide);
            this.Controls.Add(this.pbRejImage);
            this.Controls.Add(this.lbRejMessage);
            this.Controls.Add(this.btReject);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.lbRejTitle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertFrm";
            this.Text = "Alert Message Form";
            this.Load += new System.EventHandler(this.AlertFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbRejImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbRejTitle;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btReject;
        private System.Windows.Forms.Label lbRejMessage;
        private System.Windows.Forms.PictureBox pbRejImage;
        private System.Windows.Forms.Label lbRejcetSide;
    }
}