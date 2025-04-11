namespace IP3000_Control.DL
{
    partial class DLAnomalyCtrl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cbProductList = new System.Windows.Forms.ComboBox();
            this.btStop = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.btHome = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.LB_Status1 = new System.Windows.Forms.Label();
            this.tpVision = new System.Windows.Forms.TabPage();
            this.btSaveImage = new System.Windows.Forms.Button();
            this.lbModelName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Txt_Segmentation_Threshold = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LB_Status2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CB_DeviceList = new System.Windows.Forms.ComboBox();
            this.LB_Yield1 = new System.Windows.Forms.Label();
            this.LB_Yield2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Txt_ClassificationThreshold = new System.Windows.Forms.TextBox();
            this.LB_Total = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BT_Inspection = new System.Windows.Forms.Button();
            this.BT_LoadImage = new System.Windows.Forms.Button();
            this.BT_LoadModel = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.cbStartStopCam = new System.Windows.Forms.CheckBox();
            this.cbLiveview = new System.Windows.Forms.CheckBox();
            this.button_Snap = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.hSmartWindowControl1 = new HalconDotNet.HSmartWindowControl();
            this.Column19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpMCContrl = new System.Windows.Forms.TabPage();
            this.panelAvatar = new System.Windows.Forms.Panel();
            this.btSoftwareReset = new System.Windows.Forms.Button();
            this.btAlarmReset = new System.Windows.Forms.Button();
            this.DGV_ImageLists = new System.Windows.Forms.DataGridView();
            this.Txt_MinArea = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.tpVision.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpMCContrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ImageLists)).BeginInit();
            this.SuspendLayout();
            // 
            // cbProductList
            // 
            this.cbProductList.AutoCompleteCustomSource.AddRange(new string[] {
            "0195-065C Top",
            "0195-065C Bottom",
            "Bright Top",
            "Bright Bottom"});
            this.cbProductList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProductList.FormattingEnabled = true;
            this.cbProductList.Items.AddRange(new object[] {
            "0195-06C Top",
            "0195-06C Bottom",
            "0239-04C Brigth Top",
            "0239-04C Brigth Bottom",
            "0315-82A Top",
            "0315-82A Bottom"});
            this.cbProductList.Location = new System.Drawing.Point(56, 40);
            this.cbProductList.Name = "cbProductList";
            this.cbProductList.Size = new System.Drawing.Size(187, 28);
            this.cbProductList.TabIndex = 547;
            // 
            // btStop
            // 
            this.btStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStop.Location = new System.Drawing.Point(56, 235);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(188, 35);
            this.btStop.TabIndex = 545;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = false;
            // 
            // btStart
            // 
            this.btStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStart.Location = new System.Drawing.Point(55, 194);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(188, 35);
            this.btStart.TabIndex = 544;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = false;
            // 
            // btHome
            // 
            this.btHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHome.Location = new System.Drawing.Point(56, 153);
            this.btHome.Name = "btHome";
            this.btHome.Size = new System.Drawing.Size(188, 35);
            this.btHome.TabIndex = 543;
            this.btHome.Text = "Home";
            this.btHome.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 264);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 561;
            this.label8.Text = "Status:";
            // 
            // LB_Status1
            // 
            this.LB_Status1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_Status1.Location = new System.Drawing.Point(78, 259);
            this.LB_Status1.Name = "LB_Status1";
            this.LB_Status1.Size = new System.Drawing.Size(64, 23);
            this.LB_Status1.TabIndex = 555;
            this.LB_Status1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpVision
            // 
            this.tpVision.Controls.Add(this.btSaveImage);
            this.tpVision.Controls.Add(this.lbModelName);
            this.tpVision.Controls.Add(this.label5);
            this.tpVision.Controls.Add(this.Txt_Segmentation_Threshold);
            this.tpVision.Controls.Add(this.label3);
            this.tpVision.Controls.Add(this.label8);
            this.tpVision.Controls.Add(this.label4);
            this.tpVision.Controls.Add(this.LB_Status1);
            this.tpVision.Controls.Add(this.LB_Status2);
            this.tpVision.Controls.Add(this.label7);
            this.tpVision.Controls.Add(this.CB_DeviceList);
            this.tpVision.Controls.Add(this.LB_Yield1);
            this.tpVision.Controls.Add(this.LB_Yield2);
            this.tpVision.Controls.Add(this.label6);
            this.tpVision.Controls.Add(this.Txt_ClassificationThreshold);
            this.tpVision.Controls.Add(this.LB_Total);
            this.tpVision.Controls.Add(this.label2);
            this.tpVision.Controls.Add(this.BT_Inspection);
            this.tpVision.Controls.Add(this.BT_LoadImage);
            this.tpVision.Controls.Add(this.BT_LoadModel);
            this.tpVision.Controls.Add(this.label12);
            this.tpVision.Controls.Add(this.tbExposure);
            this.tpVision.Controls.Add(this.cbStartStopCam);
            this.tpVision.Controls.Add(this.cbLiveview);
            this.tpVision.Controls.Add(this.button_Snap);
            this.tpVision.Location = new System.Drawing.Point(4, 22);
            this.tpVision.Margin = new System.Windows.Forms.Padding(2);
            this.tpVision.Name = "tpVision";
            this.tpVision.Padding = new System.Windows.Forms.Padding(2);
            this.tpVision.Size = new System.Drawing.Size(544, 295);
            this.tpVision.TabIndex = 1;
            this.tpVision.Text = "Camera";
            this.tpVision.UseVisualStyleBackColor = true;
            // 
            // btSaveImage
            // 
            this.btSaveImage.Location = new System.Drawing.Point(304, 163);
            this.btSaveImage.Margin = new System.Windows.Forms.Padding(6);
            this.btSaveImage.Name = "btSaveImage";
            this.btSaveImage.Size = new System.Drawing.Size(126, 31);
            this.btSaveImage.TabIndex = 610;
            this.btSaveImage.Text = "Save Image";
            this.btSaveImage.UseVisualStyleBackColor = true;
            // 
            // lbModelName
            // 
            this.lbModelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbModelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbModelName.Location = new System.Drawing.Point(21, 93);
            this.lbModelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbModelName.Name = "lbModelName";
            this.lbModelName.Size = new System.Drawing.Size(254, 23);
            this.lbModelName.TabIndex = 609;
            this.lbModelName.Text = "PreEBT QSFP";
            this.lbModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 608;
            this.label5.Text = "DL Model";
            // 
            // Txt_Segmentation_Threshold
            // 
            this.Txt_Segmentation_Threshold.Location = new System.Drawing.Point(176, 163);
            this.Txt_Segmentation_Threshold.Name = "Txt_Segmentation_Threshold";
            this.Txt_Segmentation_Threshold.Size = new System.Drawing.Size(49, 20);
            this.Txt_Segmentation_Threshold.TabIndex = 567;
            this.Txt_Segmentation_Threshold.Text = "0";
            this.Txt_Segmentation_Threshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(127, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 32);
            this.label3.TabIndex = 566;
            this.label3.Text = "Seg Th";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 17);
            this.label4.TabIndex = 607;
            this.label4.Text = "Running Device";
            // 
            // LB_Status2
            // 
            this.LB_Status2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_Status2.Location = new System.Drawing.Point(148, 259);
            this.LB_Status2.Name = "LB_Status2";
            this.LB_Status2.Size = new System.Drawing.Size(64, 23);
            this.LB_Status2.TabIndex = 554;
            this.LB_Status2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 233);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 560;
            this.label7.Text = "Yield:";
            // 
            // CB_DeviceList
            // 
            this.CB_DeviceList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(81)))), ((int)(((byte)(130)))));
            this.CB_DeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_DeviceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_DeviceList.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.CB_DeviceList.FormattingEnabled = true;
            this.CB_DeviceList.Items.AddRange(new object[] {
            "Normal",
            "Advance"});
            this.CB_DeviceList.Location = new System.Drawing.Point(21, 40);
            this.CB_DeviceList.Name = "CB_DeviceList";
            this.CB_DeviceList.Size = new System.Drawing.Size(256, 21);
            this.CB_DeviceList.TabIndex = 606;
            // 
            // LB_Yield1
            // 
            this.LB_Yield1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_Yield1.Location = new System.Drawing.Point(78, 228);
            this.LB_Yield1.Name = "LB_Yield1";
            this.LB_Yield1.Size = new System.Drawing.Size(64, 23);
            this.LB_Yield1.TabIndex = 557;
            this.LB_Yield1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LB_Yield2
            // 
            this.LB_Yield2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_Yield2.Location = new System.Drawing.Point(148, 228);
            this.LB_Yield2.Name = "LB_Yield2";
            this.LB_Yield2.Size = new System.Drawing.Size(64, 23);
            this.LB_Yield2.TabIndex = 556;
            this.LB_Yield2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 559;
            this.label6.Text = "Total";
            // 
            // Txt_ClassificationThreshold
            // 
            this.Txt_ClassificationThreshold.Location = new System.Drawing.Point(78, 163);
            this.Txt_ClassificationThreshold.Name = "Txt_ClassificationThreshold";
            this.Txt_ClassificationThreshold.Size = new System.Drawing.Size(40, 20);
            this.Txt_ClassificationThreshold.TabIndex = 565;
            this.Txt_ClassificationThreshold.Text = "0";
            this.Txt_ClassificationThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LB_Total
            // 
            this.LB_Total.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_Total.Location = new System.Drawing.Point(78, 197);
            this.LB_Total.Name = "LB_Total";
            this.LB_Total.Size = new System.Drawing.Size(60, 23);
            this.LB_Total.TabIndex = 558;
            this.LB_Total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(29, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 32);
            this.label2.TabIndex = 564;
            this.label2.Text = "Cls Th";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BT_Inspection
            // 
            this.BT_Inspection.Location = new System.Drawing.Point(193, 123);
            this.BT_Inspection.Name = "BT_Inspection";
            this.BT_Inspection.Size = new System.Drawing.Size(76, 30);
            this.BT_Inspection.TabIndex = 604;
            this.BT_Inspection.Text = "Inspection";
            this.BT_Inspection.UseVisualStyleBackColor = true;
            // 
            // BT_LoadImage
            // 
            this.BT_LoadImage.Location = new System.Drawing.Point(108, 123);
            this.BT_LoadImage.Name = "BT_LoadImage";
            this.BT_LoadImage.Size = new System.Drawing.Size(79, 30);
            this.BT_LoadImage.TabIndex = 603;
            this.BT_LoadImage.Text = "Load Image";
            this.BT_LoadImage.UseVisualStyleBackColor = true;
            // 
            // BT_LoadModel
            // 
            this.BT_LoadModel.Location = new System.Drawing.Point(24, 123);
            this.BT_LoadModel.Name = "BT_LoadModel";
            this.BT_LoadModel.Size = new System.Drawing.Size(79, 30);
            this.BT_LoadModel.TabIndex = 605;
            this.BT_LoadModel.Text = "Load Model";
            this.BT_LoadModel.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(301, 75);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 602;
            this.label12.Text = "Exposure :";
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(370, 72);
            this.tbExposure.Margin = new System.Windows.Forms.Padding(6);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(60, 20);
            this.tbExposure.TabIndex = 601;
            this.tbExposure.Text = "2500";
            // 
            // cbStartStopCam
            // 
            this.cbStartStopCam.AutoSize = true;
            this.cbStartStopCam.Location = new System.Drawing.Point(304, 15);
            this.cbStartStopCam.Name = "cbStartStopCam";
            this.cbStartStopCam.Size = new System.Drawing.Size(113, 17);
            this.cbStartStopCam.TabIndex = 598;
            this.cbStartStopCam.Text = "Start/Stop camera";
            this.cbStartStopCam.UseVisualStyleBackColor = true;
            // 
            // cbLiveview
            // 
            this.cbLiveview.AutoSize = true;
            this.cbLiveview.Location = new System.Drawing.Point(304, 44);
            this.cbLiveview.Name = "cbLiveview";
            this.cbLiveview.Size = new System.Drawing.Size(68, 17);
            this.cbLiveview.TabIndex = 593;
            this.cbLiveview.Text = "Liveview";
            this.cbLiveview.UseVisualStyleBackColor = true;
            // 
            // button_Snap
            // 
            this.button_Snap.Location = new System.Drawing.Point(304, 123);
            this.button_Snap.Margin = new System.Windows.Forms.Padding(6);
            this.button_Snap.Name = "button_Snap";
            this.button_Snap.Size = new System.Drawing.Size(126, 31);
            this.button_Snap.TabIndex = 592;
            this.button_Snap.Text = "Snap";
            this.button_Snap.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(52, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 20);
            this.label14.TabIndex = 546;
            this.label14.Text = "Product";
            // 
            // hSmartWindowControl1
            // 
            this.hSmartWindowControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.hSmartWindowControl1.ForeColor = System.Drawing.Color.White;
            this.hSmartWindowControl1.HDoubleClickToFitContent = true;
            this.hSmartWindowControl1.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl1.HImagePart = new System.Drawing.Rectangle(0, 0, 268, 276);
            this.hSmartWindowControl1.HKeepAspectRatio = true;
            this.hSmartWindowControl1.HMoveContent = true;
            this.hSmartWindowControl1.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.hSmartWindowControl1.Location = new System.Drawing.Point(399, 16);
            this.hSmartWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl1.MaximumSize = new System.Drawing.Size(50000, 50000);
            this.hSmartWindowControl1.MinimumSize = new System.Drawing.Size(100, 100);
            this.hSmartWindowControl1.Name = "hSmartWindowControl1";
            this.hSmartWindowControl1.Size = new System.Drawing.Size(391, 316);
            this.hSmartWindowControl1.TabIndex = 597;
            this.hSmartWindowControl1.WindowSize = new System.Drawing.Size(391, 316);
            // 
            // Column19
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column19.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column19.HeaderText = "Inspection Time(ms)";
            this.Column19.MinimumWidth = 10;
            this.Column19.Name = "Column19";
            this.Column19.ReadOnly = true;
            this.Column19.Width = 200;
            // 
            // Column2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "Min Area";
            this.Column2.MinimumWidth = 10;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Predicted With Condition";
            this.Column1.MinimumWidth = 10;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // DataGridViewTextBoxColumn14
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DataGridViewTextBoxColumn14.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewTextBoxColumn14.HeaderText = "Confinence(%)";
            this.DataGridViewTextBoxColumn14.MinimumWidth = 10;
            this.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14";
            this.DataGridViewTextBoxColumn14.ReadOnly = true;
            this.DataGridViewTextBoxColumn14.Width = 200;
            // 
            // DataGridViewTextBoxColumn13
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DataGridViewTextBoxColumn13.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridViewTextBoxColumn13.HeaderText = "Predicted";
            this.DataGridViewTextBoxColumn13.MinimumWidth = 10;
            this.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13";
            this.DataGridViewTextBoxColumn13.ReadOnly = true;
            this.DataGridViewTextBoxColumn13.Width = 120;
            // 
            // DataGridViewTextBoxColumn11
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridViewTextBoxColumn11.HeaderText = "File Name";
            this.DataGridViewTextBoxColumn11.MinimumWidth = 10;
            this.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11";
            this.DataGridViewTextBoxColumn11.ReadOnly = true;
            this.DataGridViewTextBoxColumn11.Width = 350;
            // 
            // DataGridViewTextBoxColumn10
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle6;
            this.DataGridViewTextBoxColumn10.HeaderText = "No.";
            this.DataGridViewTextBoxColumn10.MinimumWidth = 10;
            this.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10";
            this.DataGridViewTextBoxColumn10.ReadOnly = true;
            this.DataGridViewTextBoxColumn10.Width = 40;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpMCContrl);
            this.tabControl1.Controls.Add(this.tpVision);
            this.tabControl1.Location = new System.Drawing.Point(801, 11);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(552, 321);
            this.tabControl1.TabIndex = 600;
            // 
            // tpMCContrl
            // 
            this.tpMCContrl.Controls.Add(this.panelAvatar);
            this.tpMCContrl.Controls.Add(this.btSoftwareReset);
            this.tpMCContrl.Controls.Add(this.btAlarmReset);
            this.tpMCContrl.Controls.Add(this.cbProductList);
            this.tpMCContrl.Controls.Add(this.label14);
            this.tpMCContrl.Controls.Add(this.btStop);
            this.tpMCContrl.Controls.Add(this.btStart);
            this.tpMCContrl.Controls.Add(this.btHome);
            this.tpMCContrl.Location = new System.Drawing.Point(4, 22);
            this.tpMCContrl.Margin = new System.Windows.Forms.Padding(2);
            this.tpMCContrl.Name = "tpMCContrl";
            this.tpMCContrl.Padding = new System.Windows.Forms.Padding(2);
            this.tpMCContrl.Size = new System.Drawing.Size(544, 295);
            this.tpMCContrl.TabIndex = 0;
            this.tpMCContrl.Text = "MC Control";
            this.tpMCContrl.UseVisualStyleBackColor = true;
            // 
            // panelAvatar
            // 
            this.panelAvatar.Location = new System.Drawing.Point(289, 12);
            this.panelAvatar.Name = "panelAvatar";
            this.panelAvatar.Size = new System.Drawing.Size(241, 251);
            this.panelAvatar.TabIndex = 550;
            // 
            // btSoftwareReset
            // 
            this.btSoftwareReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btSoftwareReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSoftwareReset.Location = new System.Drawing.Point(56, 112);
            this.btSoftwareReset.Name = "btSoftwareReset";
            this.btSoftwareReset.Size = new System.Drawing.Size(188, 35);
            this.btSoftwareReset.TabIndex = 549;
            this.btSoftwareReset.Text = "Software Reset";
            this.btSoftwareReset.UseVisualStyleBackColor = false;
            // 
            // btAlarmReset
            // 
            this.btAlarmReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btAlarmReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAlarmReset.Location = new System.Drawing.Point(56, 71);
            this.btAlarmReset.Name = "btAlarmReset";
            this.btAlarmReset.Size = new System.Drawing.Size(188, 35);
            this.btAlarmReset.TabIndex = 548;
            this.btAlarmReset.Text = "Alarm Reset";
            this.btAlarmReset.UseVisualStyleBackColor = false;
            // 
            // DGV_ImageLists
            // 
            this.DGV_ImageLists.AllowUserToAddRows = false;
            this.DGV_ImageLists.AllowUserToDeleteRows = false;
            this.DGV_ImageLists.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.DGV_ImageLists.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGV_ImageLists.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_ImageLists.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.DGV_ImageLists.ColumnHeadersHeight = 35;
            this.DGV_ImageLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_ImageLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumn10,
            this.DataGridViewTextBoxColumn11,
            this.DataGridViewTextBoxColumn13,
            this.DataGridViewTextBoxColumn14,
            this.Column1,
            this.Column2,
            this.Column19});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_ImageLists.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_ImageLists.EnableHeadersVisualStyles = false;
            this.DGV_ImageLists.GridColor = System.Drawing.Color.Silver;
            this.DGV_ImageLists.Location = new System.Drawing.Point(9, 350);
            this.DGV_ImageLists.MultiSelect = false;
            this.DGV_ImageLists.Name = "DGV_ImageLists";
            this.DGV_ImageLists.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_ImageLists.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DGV_ImageLists.RowHeadersVisible = false;
            this.DGV_ImageLists.RowHeadersWidth = 35;
            this.DGV_ImageLists.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.DGV_ImageLists.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.DGV_ImageLists.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DGV_ImageLists.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.DGV_ImageLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_ImageLists.Size = new System.Drawing.Size(1343, 260);
            this.DGV_ImageLists.TabIndex = 596;
            // 
            // Txt_MinArea
            // 
            this.Txt_MinArea.Location = new System.Drawing.Point(1757, 834);
            this.Txt_MinArea.Name = "Txt_MinArea";
            this.Txt_MinArea.Size = new System.Drawing.Size(88, 20);
            this.Txt_MinArea.TabIndex = 599;
            this.Txt_MinArea.Text = "50";
            this.Txt_MinArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1757, 801);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 598;
            this.label1.Text = "Filter Min Area";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WindowControl
            // 
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.WindowControl.ForeColor = System.Drawing.Color.White;
            this.WindowControl.HDoubleClickToFitContent = true;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 268, 276);
            this.WindowControl.HKeepAspectRatio = true;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.Location = new System.Drawing.Point(7, 16);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(0);
            this.WindowControl.MaximumSize = new System.Drawing.Size(50000, 50000);
            this.WindowControl.MinimumSize = new System.Drawing.Size(100, 100);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(375, 316);
            this.WindowControl.TabIndex = 595;
            this.WindowControl.WindowSize = new System.Drawing.Size(375, 316);
            // 
            // DLAnomalyCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hSmartWindowControl1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.DGV_ImageLists);
            this.Controls.Add(this.Txt_MinArea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WindowControl);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DLAnomalyCtrl";
            this.Size = new System.Drawing.Size(1373, 616);
            this.tpVision.ResumeLayout(false);
            this.tpVision.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpMCContrl.ResumeLayout(false);
            this.tpMCContrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ImageLists)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProductList;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btHome;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LB_Status1;
        private System.Windows.Forms.TabPage tpVision;
        private System.Windows.Forms.Button btSaveImage;
        private System.Windows.Forms.Label lbModelName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Txt_Segmentation_Threshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LB_Status2;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ComboBox CB_DeviceList;
        private System.Windows.Forms.Label LB_Yield1;
        private System.Windows.Forms.Label LB_Yield2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Txt_ClassificationThreshold;
        private System.Windows.Forms.Label LB_Total;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BT_Inspection;
        private System.Windows.Forms.Button BT_LoadImage;
        private System.Windows.Forms.Button BT_LoadModel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.CheckBox cbStartStopCam;
        private System.Windows.Forms.CheckBox cbLiveview;
        private System.Windows.Forms.Button button_Snap;
        private System.Windows.Forms.Label label14;
        internal System.Windows.Forms.FolderBrowserDialog FBD;
        internal HalconDotNet.HSmartWindowControl hSmartWindowControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column19;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn10;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpMCContrl;
        private System.Windows.Forms.Panel panelAvatar;
        private System.Windows.Forms.Button btSoftwareReset;
        private System.Windows.Forms.Button btAlarmReset;
        internal System.Windows.Forms.DataGridView DGV_ImageLists;
        private System.Windows.Forms.TextBox Txt_MinArea;
        private System.Windows.Forms.Label label1;
        internal HalconDotNet.HSmartWindowControl WindowControl;
    }
}
