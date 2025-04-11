namespace IP3000_Control.ARTCamera
{
    partial class ArtCam2CamFrm
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
            Release();
            m_CArtCam[0].FreeLibrary();
            m_CArtCam[1].FreeLibrary();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuSave = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuView = new System.Windows.Forms.MenuItem();
            this.menuPreview = new System.Windows.Forms.MenuItem();
            this.menuCallback = new System.Windows.Forms.MenuItem();
            this.menuSnapshot = new System.Windows.Forms.MenuItem();
            this.menuCapture = new System.Windows.Forms.MenuItem();
            this.menuTrigger = new System.Windows.Forms.MenuItem();
            this.menuSet = new System.Windows.Forms.MenuItem();
            this.menuCamera1 = new System.Windows.Forms.MenuItem();
            this.menuCamera2 = new System.Windows.Forms.MenuItem();
            this.menuFilter1 = new System.Windows.Forms.MenuItem();
            this.menuFilter2 = new System.Windows.Forms.MenuItem();
            this.menuAnalog1 = new System.Windows.Forms.MenuItem();
            this.menuAnalog2 = new System.Windows.Forms.MenuItem();
            this.menuDLL = new System.Windows.Forms.MenuItem();
            this.menuDllReload = new System.Windows.Forms.MenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ImagePanel_2 = new System.Windows.Forms.Panel();
            this.ImageBox_2 = new System.Windows.Forms.PictureBox();
            this.ImagePanel_1 = new System.Windows.Forms.Panel();
            this.ImageBox_1 = new System.Windows.Forms.PictureBox();
            this.ImagePanel_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_2)).BeginInit();
            this.ImagePanel_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuView,
            this.menuSet,
            this.menuDLL});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSave,
            this.menuExit});
            this.menuFile.Text = "File(&F)";
            // 
            // menuSave
            // 
            this.menuSave.Index = 0;
            this.menuSave.Text = "Save(&S)";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuExit
            // 
            this.menuExit.Index = 1;
            this.menuExit.Text = "End(&X)";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuView
            // 
            this.menuView.Index = 1;
            this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPreview,
            this.menuCallback,
            this.menuSnapshot,
            this.menuCapture,
            this.menuTrigger});
            this.menuView.Text = "Display(&V)";
            // 
            // menuPreview
            // 
            this.menuPreview.Index = 0;
            this.menuPreview.Text = "Preview(&P)";
            this.menuPreview.Click += new System.EventHandler(this.menuPreview_Click);
            // 
            // menuCallback
            // 
            this.menuCallback.Index = 1;
            this.menuCallback.Text = "Callback(&B)";
            this.menuCallback.Click += new System.EventHandler(this.menuCallback_Click);
            // 
            // menuSnapshot
            // 
            this.menuSnapshot.Index = 2;
            this.menuSnapshot.Text = "Snapshot(&S)";
            this.menuSnapshot.Click += new System.EventHandler(this.menuSnapshot_Click);
            // 
            // menuCapture
            // 
            this.menuCapture.Index = 3;
            this.menuCapture.Text = "Capture(&C)";
            this.menuCapture.Click += new System.EventHandler(this.menuCapture_Click);
            // 
            // menuTrigger
            // 
            this.menuTrigger.Index = 4;
            this.menuTrigger.Text = "Trigger(&T)";
            this.menuTrigger.Click += new System.EventHandler(this.menuTrigger_Click);
            // 
            // menuSet
            // 
            this.menuSet.Index = 2;
            this.menuSet.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuCamera1,
            this.menuCamera2,
            this.menuFilter1,
            this.menuFilter2,
            this.menuAnalog1,
            this.menuAnalog2});
            this.menuSet.Text = "Settings(&S)";
            // 
            // menuCamera1
            // 
            this.menuCamera1.Index = 0;
            this.menuCamera1.Text = "Camera settings1(&C)";
            this.menuCamera1.Click += new System.EventHandler(this.menuCamera1_Click);
            // 
            // menuCamera2
            // 
            this.menuCamera2.Index = 1;
            this.menuCamera2.Text = "Camera settings2(&C)";
            this.menuCamera2.Click += new System.EventHandler(this.menuCamera2_Click);
            // 
            // menuFilter1
            // 
            this.menuFilter1.Index = 2;
            this.menuFilter1.Text = "Filter settings1(&F)";
            this.menuFilter1.Click += new System.EventHandler(this.menuFilter1_Click);
            // 
            // menuFilter2
            // 
            this.menuFilter2.Index = 3;
            this.menuFilter2.Text = "Filter settings2(&F)";
            this.menuFilter2.Click += new System.EventHandler(this.menuFilter2_Click);
            // 
            // menuAnalog1
            // 
            this.menuAnalog1.Index = 4;
            this.menuAnalog1.Text = "Analog settings1(&A)";
            this.menuAnalog1.Click += new System.EventHandler(this.menuAnalog1_Click);
            // 
            // menuAnalog2
            // 
            this.menuAnalog2.Index = 5;
            this.menuAnalog2.Text = "Analog settings2(&A)";
            this.menuAnalog2.Click += new System.EventHandler(this.menuAnalog2_Click);
            // 
            // menuDLL
            // 
            this.menuDLL.Index = 3;
            this.menuDLL.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDllReload});
            this.menuDLL.Text = "DLL(&L)";
            // 
            // menuDllReload
            // 
            this.menuDllReload.Index = 0;
            this.menuDllReload.Text = "Reload";
            this.menuDllReload.Click += new System.EventHandler(this.menuDllReload_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ImagePanel_2
            // 
            this.ImagePanel_2.AutoScroll = true;
            this.ImagePanel_2.BackColor = System.Drawing.SystemColors.Control;
            this.ImagePanel_2.Controls.Add(this.ImageBox_2);
            this.ImagePanel_2.Location = new System.Drawing.Point(401, 63);
            this.ImagePanel_2.Name = "ImagePanel_2";
            this.ImagePanel_2.Size = new System.Drawing.Size(240, 800);
            this.ImagePanel_2.TabIndex = 3;
            // 
            // ImageBox_2
            // 
            this.ImageBox_2.Location = new System.Drawing.Point(48, 32);
            this.ImageBox_2.Name = "ImageBox_2";
            this.ImageBox_2.Size = new System.Drawing.Size(80, 80);
            this.ImageBox_2.TabIndex = 0;
            this.ImageBox_2.TabStop = false;
            // 
            // ImagePanel_1
            // 
            this.ImagePanel_1.AutoScroll = true;
            this.ImagePanel_1.BackColor = System.Drawing.SystemColors.Control;
            this.ImagePanel_1.Controls.Add(this.ImageBox_1);
            this.ImagePanel_1.Location = new System.Drawing.Point(60, 63);
            this.ImagePanel_1.Name = "ImagePanel_1";
            this.ImagePanel_1.Size = new System.Drawing.Size(240, 800);
            this.ImagePanel_1.TabIndex = 2;
            // 
            // ImageBox_1
            // 
            this.ImageBox_1.Location = new System.Drawing.Point(32, 32);
            this.ImageBox_1.Name = "ImageBox_1";
            this.ImageBox_1.Size = new System.Drawing.Size(80, 80);
            this.ImageBox_1.TabIndex = 0;
            this.ImageBox_1.TabStop = false;
            // 
            // ArtCam2CamFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(919, 501);
            this.Controls.Add(this.ImagePanel_2);
            this.Controls.Add(this.ImagePanel_1);
            this.Menu = this.mainMenu1;
            this.Name = "ArtCam2CamFrm";
            this.Text = "ArtCam2CamFrm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ArtCam2CamFrm_FormClosing);
            this.Load += new System.EventHandler(this.ArtCam2CamFrm_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ImagePanel_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_2)).EndInit();
            this.ImagePanel_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImageBox_2;
        private System.Windows.Forms.PictureBox ImageBox_1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuSave;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuView;
        private System.Windows.Forms.MenuItem menuPreview;
        private System.Windows.Forms.MenuItem menuCallback;
        private System.Windows.Forms.MenuItem menuSnapshot;
        private System.Windows.Forms.MenuItem menuCapture;
        private System.Windows.Forms.MenuItem menuTrigger;
        private System.Windows.Forms.MenuItem menuSet;
        private System.Windows.Forms.MenuItem menuCamera1;
        private System.Windows.Forms.MenuItem menuCamera2;
        private System.Windows.Forms.MenuItem menuFilter1;
        private System.Windows.Forms.MenuItem menuFilter2;
        private System.Windows.Forms.MenuItem menuAnalog1;
        private System.Windows.Forms.MenuItem menuAnalog2;
        private System.Windows.Forms.MenuItem menuDLL;
        private System.Windows.Forms.MenuItem menuDllReload;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel ImagePanel_2;
        private System.Windows.Forms.Panel ImagePanel_1;
    }
}