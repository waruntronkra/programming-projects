namespace IP3000_Control.ARTCamera
{
    partial class ArtCamFullCtrlFrm
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
            m_CArtCam.FreeLibrary();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerRec = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuSave = new System.Windows.Forms.MenuItem();
            this.menuRec = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuView = new System.Windows.Forms.MenuItem();
            this.menuPreview = new System.Windows.Forms.MenuItem();
            this.menuCallback = new System.Windows.Forms.MenuItem();
            this.menuSnapshot = new System.Windows.Forms.MenuItem();
            this.menuCapture = new System.Windows.Forms.MenuItem();
            this.menuTrigger = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuStop = new System.Windows.Forms.MenuItem();
            this.menuSet = new System.Windows.Forms.MenuItem();
            this.menuCamera = new System.Windows.Forms.MenuItem();
            this.menuFilter = new System.Windows.Forms.MenuItem();
            this.menuAnalog = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.MenuUserCorrection = new System.Windows.Forms.MenuItem();
            this.menuDLL = new System.Windows.Forms.MenuItem();
            this.menuDllReload = new System.Windows.Forms.MenuItem();
            this.menuDevice = new System.Windows.Forms.MenuItem();
            this.menuDevice0 = new System.Windows.Forms.MenuItem();
            this.menuDevice1 = new System.Windows.Forms.MenuItem();
            this.menuDevice2 = new System.Windows.Forms.MenuItem();
            this.menuDevice3 = new System.Windows.Forms.MenuItem();
            this.menuDevice4 = new System.Windows.Forms.MenuItem();
            this.menuDevice5 = new System.Windows.Forms.MenuItem();
            this.menuDevice6 = new System.Windows.Forms.MenuItem();
            this.menuDevice7 = new System.Windows.Forms.MenuItem();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.sbPanelSdkStatus = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelExp = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelFps = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelScale = new System.Windows.Forms.StatusBarPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelSdkStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelFps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelScale)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "doc1";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuView,
            this.menuSet,
            this.menuDLL,
            this.menuDevice});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSave,
            this.menuRec,
            this.menuExit});
            this.menuFile.Text = "File(&F)";
            // 
            // menuSave
            // 
            this.menuSave.Index = 0;
            this.menuSave.Text = "Save(&S)";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuRec
            // 
            this.menuRec.Index = 1;
            this.menuRec.Text = "Recording(&R)";
            this.menuRec.Click += new System.EventHandler(this.menuRec_Click);
            // 
            // menuExit
            // 
            this.menuExit.Index = 2;
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
            this.menuTrigger,
            this.menuItem5,
            this.menuStop});
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
            // menuItem5
            // 
            this.menuItem5.Index = 5;
            this.menuItem5.Text = "-";
            // 
            // menuStop
            // 
            this.menuStop.Index = 6;
            this.menuStop.Text = "Pause";
            // 
            // menuSet
            // 
            this.menuSet.Index = 2;
            this.menuSet.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuCamera,
            this.menuFilter,
            this.menuAnalog,
            this.menuItem1,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem7,
            this.menuItem6,
            this.MenuUserCorrection});
            this.menuSet.Text = "Settings(&S)";
            // 
            // menuCamera
            // 
            this.menuCamera.Index = 0;
            this.menuCamera.Text = "Camera settings(&C)";
            this.menuCamera.Click += new System.EventHandler(this.menuCamera_Click);
            // 
            // menuFilter
            // 
            this.menuFilter.Index = 1;
            this.menuFilter.Text = "Filter settings(&F)";
            this.menuFilter.Click += new System.EventHandler(this.menuFilter_Click);
            // 
            // menuAnalog
            // 
            this.menuAnalog.Index = 2;
            this.menuAnalog.Text = "Analog settings(&A)";
            this.menuAnalog.Click += new System.EventHandler(this.menuAnalog_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.Text = "User size settings";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Text = "User filter settings";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 6;
            this.menuItem4.Text = "User IO settings";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 7;
            this.menuItem7.Text = "User monitor settings";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click_1);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 8;
            this.menuItem6.Text = "-";
            // 
            // MenuUserCorrection
            // 
            this.MenuUserCorrection.Index = 9;
            this.MenuUserCorrection.Text = "Correction setting";
            this.MenuUserCorrection.Click += new System.EventHandler(this.MenuUserCorrection_Click);
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
            this.menuDllReload.Click += new System.EventHandler(this.OnDllReload);
            // 
            // menuDevice
            // 
            this.menuDevice.Index = 4;
            this.menuDevice.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDevice0,
            this.menuDevice1,
            this.menuDevice2,
            this.menuDevice3,
            this.menuDevice4,
            this.menuDevice5,
            this.menuDevice6,
            this.menuDevice7});
            this.menuDevice.Text = "Device(&D)";
            // 
            // menuDevice0
            // 
            this.menuDevice0.Index = 0;
            this.menuDevice0.Text = "0";
            this.menuDevice0.Click += new System.EventHandler(this.menuDevice0_Click);
            // 
            // menuDevice1
            // 
            this.menuDevice1.Index = 1;
            this.menuDevice1.Text = "1";
            this.menuDevice1.Click += new System.EventHandler(this.menuDevice1_Click);
            // 
            // menuDevice2
            // 
            this.menuDevice2.Index = 2;
            this.menuDevice2.Text = "2";
            this.menuDevice2.Click += new System.EventHandler(this.menuDevice2_Click);
            // 
            // menuDevice3
            // 
            this.menuDevice3.Index = 3;
            this.menuDevice3.Text = "3";
            this.menuDevice3.Click += new System.EventHandler(this.menuDevice3_Click);
            // 
            // menuDevice4
            // 
            this.menuDevice4.Index = 4;
            this.menuDevice4.Text = "4";
            this.menuDevice4.Click += new System.EventHandler(this.menuDevice4_Click);
            // 
            // menuDevice5
            // 
            this.menuDevice5.Index = 5;
            this.menuDevice5.Text = "5";
            this.menuDevice5.Click += new System.EventHandler(this.menuDevice5_Click);
            // 
            // menuDevice6
            // 
            this.menuDevice6.Index = 6;
            this.menuDevice6.Text = "6";
            this.menuDevice6.Click += new System.EventHandler(this.menuDevice6_Click);
            // 
            // menuDevice7
            // 
            this.menuDevice7.Index = 7;
            this.menuDevice7.Text = "7";
            this.menuDevice7.Click += new System.EventHandler(this.menuDevice7_Click);
            // 
            // ImagePanel
            // 
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.ImagePanel.Controls.Add(this.ImageBox);
            this.ImagePanel.Location = new System.Drawing.Point(51, 42);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(1104, 800);
            this.ImagePanel.TabIndex = 3;
            this.ImagePanel.Click += new System.EventHandler(this.menuStop_Click);
            // 
            // ImageBox
            // 
            this.ImageBox.Location = new System.Drawing.Point(32, 32);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(80, 80);
            this.ImageBox.TabIndex = 1;
            this.ImageBox.TabStop = false;
            this.ImageBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageBox_Paint);
            // 
            // statusBar1
            // 
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar1.Location = new System.Drawing.Point(51, 935);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbPanelSdkStatus,
            this.sbPanelExp,
            this.sbPanelFps,
            this.sbPanelScale});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1280, 32);
            this.statusBar1.SizingGrip = false;
            this.statusBar1.TabIndex = 4;
            this.statusBar1.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar1_PanelClick);
            // 
            // sbPanelSdkStatus
            // 
            this.sbPanelSdkStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbPanelSdkStatus.Name = "sbPanelSdkStatus";
            this.sbPanelSdkStatus.Text = "Not initialized";
            this.sbPanelSdkStatus.Width = 940;
            // 
            // sbPanelExp
            // 
            this.sbPanelExp.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.sbPanelExp.MinWidth = 100;
            this.sbPanelExp.Name = "sbPanelExp";
            this.sbPanelExp.Width = 120;
            // 
            // sbPanelFps
            // 
            this.sbPanelFps.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.sbPanelFps.Name = "sbPanelFps";
            this.sbPanelFps.Text = "0.0fps";
            this.sbPanelFps.Width = 120;
            // 
            // sbPanelScale
            // 
            this.sbPanelScale.Name = "sbPanelScale";
            this.sbPanelScale.Text = "100%";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ArtCamFullCtrlFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1825, 1134);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.ImagePanel);
            this.Menu = this.mainMenu1;
            this.Name = "ArtCamFullCtrlFrm";
            this.Text = "CameraForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ArtCamForm_FormClosing);
            this.Load += new System.EventHandler(this.ArtCamForm_Load);
            this.Resize += new System.EventHandler(this.ArtCamFullCtrlFrm_Resize);
            this.ImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelSdkStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelFps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerRec;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuSave;
        private System.Windows.Forms.MenuItem menuRec;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuView;
        private System.Windows.Forms.MenuItem menuPreview;
        private System.Windows.Forms.MenuItem menuCallback;
        private System.Windows.Forms.MenuItem menuSnapshot;
        private System.Windows.Forms.MenuItem menuCapture;
        private System.Windows.Forms.MenuItem menuTrigger;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuStop;
        private System.Windows.Forms.MenuItem menuSet;
        private System.Windows.Forms.MenuItem menuCamera;
        private System.Windows.Forms.MenuItem menuFilter;
        private System.Windows.Forms.MenuItem menuAnalog;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem MenuUserCorrection;
        private System.Windows.Forms.MenuItem menuDLL;
        private System.Windows.Forms.MenuItem menuDllReload;
        private System.Windows.Forms.MenuItem menuDevice;
        private System.Windows.Forms.MenuItem menuDevice0;
        private System.Windows.Forms.MenuItem menuDevice1;
        private System.Windows.Forms.MenuItem menuDevice2;
        private System.Windows.Forms.MenuItem menuDevice3;
        private System.Windows.Forms.MenuItem menuDevice4;
        private System.Windows.Forms.MenuItem menuDevice5;
        private System.Windows.Forms.MenuItem menuDevice6;
        private System.Windows.Forms.MenuItem menuDevice7;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel sbPanelSdkStatus;
        private System.Windows.Forms.StatusBarPanel sbPanelExp;
        private System.Windows.Forms.StatusBarPanel sbPanelFps;
        private System.Windows.Forms.StatusBarPanel sbPanelScale;
        private System.Windows.Forms.Timer timer1;
    }
}