namespace SR_2000
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            searchBtn = new Button();
            NICcomboBox = new ComboBox();
            selectBtn1 = new Button();
            triggerOnBtn1 = new Button();
            triggerOffBtn1 = new Button();
            payload1 = new TextBox();
            resultRead1 = new TextBox();
            label1 = new Label();
            comboBox2 = new ComboBox();
            label2 = new Label();
            selectBtn2 = new Button();
            triggerOffBtn2 = new Button();
            triggerOnBtn2 = new Button();
            resultRead2 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            payload2 = new TextBox();
            triggerAllBtn = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(87, 73);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(108, 23);
            comboBox1.TabIndex = 7;
            // 
            // searchBtn
            // 
            searchBtn.Location = new Point(317, 12);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(144, 23);
            searchBtn.TabIndex = 8;
            searchBtn.Text = "Search Device";
            searchBtn.UseVisualStyleBackColor = true;
            searchBtn.Click += searchBtn_Click;
            // 
            // NICcomboBox
            // 
            NICcomboBox.FormattingEnabled = true;
            NICcomboBox.Location = new Point(12, 13);
            NICcomboBox.Name = "NICcomboBox";
            NICcomboBox.Size = new Size(299, 23);
            NICcomboBox.TabIndex = 9;
            // 
            // selectBtn1
            // 
            selectBtn1.Location = new Point(12, 73);
            selectBtn1.Name = "selectBtn1";
            selectBtn1.Size = new Size(69, 23);
            selectBtn1.TabIndex = 10;
            selectBtn1.Text = "Select";
            selectBtn1.UseVisualStyleBackColor = true;
            selectBtn1.Click += selectBtn1_Click;
            // 
            // triggerOnBtn1
            // 
            triggerOnBtn1.BackColor = Color.FromArgb(192, 255, 192);
            triggerOnBtn1.Location = new Point(12, 101);
            triggerOnBtn1.Name = "triggerOnBtn1";
            triggerOnBtn1.Size = new Size(69, 36);
            triggerOnBtn1.TabIndex = 11;
            triggerOnBtn1.Text = "ON";
            triggerOnBtn1.UseVisualStyleBackColor = false;
            triggerOnBtn1.Click += triggerOnBtn1_Click;
            // 
            // triggerOffBtn1
            // 
            triggerOffBtn1.BackColor = Color.FromArgb(255, 192, 192);
            triggerOffBtn1.Location = new Point(126, 102);
            triggerOffBtn1.Name = "triggerOffBtn1";
            triggerOffBtn1.Size = new Size(69, 36);
            triggerOffBtn1.TabIndex = 12;
            triggerOffBtn1.Text = "OFF";
            triggerOffBtn1.UseVisualStyleBackColor = false;
            triggerOffBtn1.Click += triggerOffBtn1_Click;
            // 
            // payload1
            // 
            payload1.Location = new Point(11, 371);
            payload1.Name = "payload1";
            payload1.ReadOnly = true;
            payload1.Size = new Size(450, 23);
            payload1.TabIndex = 13;
            // 
            // resultRead1
            // 
            resultRead1.Location = new Point(12, 294);
            resultRead1.Name = "resultRead1";
            resultRead1.ReadOnly = true;
            resultRead1.Size = new Size(183, 23);
            resultRead1.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(87, 55);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 16;
            label1.Text = "Reader 1";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(353, 73);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(108, 23);
            comboBox2.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(353, 55);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 18;
            label2.Text = "Reader 2";
            // 
            // selectBtn2
            // 
            selectBtn2.Location = new Point(278, 73);
            selectBtn2.Name = "selectBtn2";
            selectBtn2.Size = new Size(69, 23);
            selectBtn2.TabIndex = 19;
            selectBtn2.Text = "Select";
            selectBtn2.UseVisualStyleBackColor = true;
            selectBtn2.Click += selectBtn2_Click;
            // 
            // triggerOffBtn2
            // 
            triggerOffBtn2.BackColor = Color.FromArgb(255, 192, 192);
            triggerOffBtn2.Location = new Point(392, 101);
            triggerOffBtn2.Name = "triggerOffBtn2";
            triggerOffBtn2.Size = new Size(69, 36);
            triggerOffBtn2.TabIndex = 20;
            triggerOffBtn2.Text = "OFF";
            triggerOffBtn2.UseVisualStyleBackColor = false;
            triggerOffBtn2.Click += triggerOffBtn2_Click;
            // 
            // triggerOnBtn2
            // 
            triggerOnBtn2.BackColor = Color.FromArgb(192, 255, 192);
            triggerOnBtn2.Location = new Point(278, 101);
            triggerOnBtn2.Name = "triggerOnBtn2";
            triggerOnBtn2.Size = new Size(69, 36);
            triggerOnBtn2.TabIndex = 21;
            triggerOnBtn2.Text = "ON";
            triggerOnBtn2.UseVisualStyleBackColor = false;
            triggerOnBtn2.Click += triggerOnBtn2_Click;
            // 
            // resultRead2
            // 
            resultRead2.Location = new Point(278, 294);
            resultRead2.Name = "resultRead2";
            resultRead2.ReadOnly = true;
            resultRead2.Size = new Size(183, 23);
            resultRead2.TabIndex = 22;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 353);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 23;
            label3.Text = "Reader 1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 397);
            label4.Name = "label4";
            label4.Size = new Size(52, 15);
            label4.TabIndex = 24;
            label4.Text = "Reader 2";
            // 
            // payload2
            // 
            payload2.Location = new Point(11, 415);
            payload2.Name = "payload2";
            payload2.ReadOnly = true;
            payload2.Size = new Size(450, 23);
            payload2.TabIndex = 25;
            // 
            // triggerAllBtn
            // 
            triggerAllBtn.BackColor = Color.Lime;
            triggerAllBtn.Location = new Point(13, 153);
            triggerAllBtn.Name = "triggerAllBtn";
            triggerAllBtn.Size = new Size(449, 34);
            triggerAllBtn.TabIndex = 26;
            triggerAllBtn.Text = "Trigger (On) All";
            triggerAllBtn.UseVisualStyleBackColor = false;
            triggerAllBtn.Click += triggerAllBtn_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.ForeColor = Color.White;
            button1.Location = new Point(13, 208);
            button1.Name = "button1";
            button1.Size = new Size(449, 34);
            button1.TabIndex = 28;
            button1.Text = "Trigger (Off) All";
            button1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(474, 450);
            Controls.Add(button1);
            Controls.Add(triggerAllBtn);
            Controls.Add(payload2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(resultRead2);
            Controls.Add(triggerOnBtn2);
            Controls.Add(triggerOffBtn2);
            Controls.Add(selectBtn2);
            Controls.Add(label2);
            Controls.Add(comboBox2);
            Controls.Add(label1);
            Controls.Add(resultRead1);
            Controls.Add(payload1);
            Controls.Add(triggerOffBtn1);
            Controls.Add(triggerOnBtn1);
            Controls.Add(selectBtn1);
            Controls.Add(NICcomboBox);
            Controls.Add(searchBtn);
            Controls.Add(comboBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Keyence.AutoID.SDK.LiveviewForm liveviewForm1;
        private ComboBox comboBox1;
        private Button searchBtn;
        private ComboBox NICcomboBox;
        private Button selectBtn1;
        private Button triggerOnBtn1;
        private Button triggerOffBtn1;
        private TextBox payload1;
        private CheckBox checkBox1;
        private TextBox resultRead1;
        private Label label1;
        private ComboBox comboBox2;
        private Label label2;
        private Button selectBtn2;
        private Button triggerOffBtn2;
        private Button triggerOnBtn2;
        private TextBox resultRead2;
        private Label label3;
        private Label label4;
        private TextBox payload2;
        private Button triggerAllBtn;
        private Button button1;
    }
}
