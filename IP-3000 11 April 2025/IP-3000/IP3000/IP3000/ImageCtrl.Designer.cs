namespace IP3000.IP3000
{
    partial class ImageCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbInputImage = new System.Windows.Forms.PictureBox();
            this.pbOutputImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbInputImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutputImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbInputImage
            // 
            this.pbInputImage.Location = new System.Drawing.Point(77, 76);
            this.pbInputImage.Name = "pbInputImage";
            this.pbInputImage.Size = new System.Drawing.Size(620, 484);
            this.pbInputImage.TabIndex = 0;
            this.pbInputImage.TabStop = false;
            // 
            // pbOutputImage
            // 
            this.pbOutputImage.Location = new System.Drawing.Point(840, 76);
            this.pbOutputImage.Name = "pbOutputImage";
            this.pbOutputImage.Size = new System.Drawing.Size(620, 484);
            this.pbOutputImage.TabIndex = 1;
            this.pbOutputImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(275, 601);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1045, 601);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 37);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Image";
            // 
            // ImageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbOutputImage);
            this.Controls.Add(this.pbInputImage);
            this.Name = "ImageCtrl";
            this.Size = new System.Drawing.Size(1564, 683);
            this.Load += new System.EventHandler(this.ImageCtrl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbInputImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutputImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbInputImage;
        private System.Windows.Forms.PictureBox pbOutputImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
