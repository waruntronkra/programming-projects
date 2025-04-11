namespace IP3000.ARTCamera
{
    partial class ArtCamCtrl
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
            this.components = new System.ComponentModel.Container();
            this.sbPanelScale = new System.Windows.Forms.StatusBarPanel();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.sbPanelSdkStatus = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelExp = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelFps = new System.Windows.Forms.StatusBarPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelScale)).BeginInit();
            this.ImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelSdkStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelFps)).BeginInit();
            this.SuspendLayout();
            // 
            // sbPanelScale
            // 
            this.sbPanelScale.Name = "sbPanelScale";
            this.sbPanelScale.Text = "100%";
            // 
            // ImagePanel
            // 
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.ImagePanel.Controls.Add(this.ImageBox);
            this.ImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagePanel.Location = new System.Drawing.Point(0, 0);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(1003, 814);
            this.ImagePanel.TabIndex = 5;
            // 
            // ImageBox
            // 
            this.ImageBox.Location = new System.Drawing.Point(17, 20);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(960, 768);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImageBox.TabIndex = 1;
            this.ImageBox.TabStop = false;
            this.ImageBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageBox_Paint);
            this.ImageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseDown);
            this.ImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
            this.ImageBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseUp);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 814);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbPanelSdkStatus,
            this.sbPanelExp,
            this.sbPanelFps,
            this.sbPanelScale});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1003, 32);
            this.statusBar1.SizingGrip = false;
            this.statusBar1.TabIndex = 6;
            this.statusBar1.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar1_PanelClick);
            // 
            // sbPanelSdkStatus
            // 
            this.sbPanelSdkStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbPanelSdkStatus.Name = "sbPanelSdkStatus";
            this.sbPanelSdkStatus.Text = "Not initialized";
            this.sbPanelSdkStatus.Width = 663;
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            // 
            // menuRec
            // 
            this.menuRec.Index = 1;
            this.menuRec.Text = "Recording(&R)";
            // 
            // menuExit
            // 
            this.menuExit.Index = 2;
            this.menuExit.Text = "End(&X)";
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
            // 
            // menuCallback
            // 
            this.menuCallback.Index = 1;
            this.menuCallback.Text = "Callback(&B)";
            // 
            // menuSnapshot
            // 
            this.menuSnapshot.Index = 2;
            this.menuSnapshot.Text = "Snapshot(&S)";
            // 
            // menuCapture
            // 
            this.menuCapture.Index = 3;
            this.menuCapture.Text = "Capture(&C)";
            // 
            // menuTrigger
            // 
            this.menuTrigger.Index = 4;
            this.menuTrigger.Text = "Trigger(&T)";
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
            // 
            // menuFilter
            // 
            this.menuFilter.Index = 1;
            this.menuFilter.Text = "Filter settings(&F)";
            // 
            // menuAnalog
            // 
            this.menuAnalog.Index = 2;
            this.menuAnalog.Text = "Analog settings(&A)";
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
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Text = "User filter settings";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 6;
            this.menuItem4.Text = "User IO settings";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 7;
            this.menuItem7.Text = "User monitor settings";
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
            // 
            // menuDevice1
            // 
            this.menuDevice1.Index = 1;
            this.menuDevice1.Text = "1";
            // 
            // menuDevice2
            // 
            this.menuDevice2.Index = 2;
            this.menuDevice2.Text = "2";
            // 
            // menuDevice3
            // 
            this.menuDevice3.Index = 3;
            this.menuDevice3.Text = "3";
            // 
            // menuDevice4
            // 
            this.menuDevice4.Index = 4;
            this.menuDevice4.Text = "4";
            // 
            // menuDevice5
            // 
            this.menuDevice5.Index = 5;
            this.menuDevice5.Text = "5";
            // 
            // menuDevice6
            // 
            this.menuDevice6.Index = 6;
            this.menuDevice6.Text = "6";
            // 
            // menuDevice7
            // 
            this.menuDevice7.Index = 7;
            this.menuDevice7.Text = "7";
            // 
            // ArtCamCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ImagePanel);
            this.Controls.Add(this.statusBar1);
            this.Name = "ArtCamCtrl";
            this.Size = new System.Drawing.Size(1003, 846);
            this.Load += new System.EventHandler(this.ArtCamCtrl_Load);
            this.Resize += new System.EventHandler(this.ArtCamCtrl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelScale)).EndInit();
            this.ImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelSdkStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelFps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBarPanel sbPanelScale;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel sbPanelSdkStatus;
        private System.Windows.Forms.StatusBarPanel sbPanelExp;
        private System.Windows.Forms.StatusBarPanel sbPanelFps;
        private System.Windows.Forms.Timer timer1;
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
    }
}
