using Keyence.AutoID.SDK;
using System.Text;
using System;

namespace SR_2000
{
    public partial class Form1 : Form
    {
        private ReaderAccessor m_reader_1 = new ReaderAccessor();
        private ReaderAccessor m_reader_2 = new ReaderAccessor();
        private ReaderSearcher m_searcher = new ReaderSearcher();
        List<NicSearchResult> m_nicList = new List<NicSearchResult>();

        public Form1()
        {
            InitializeComponent();
        
            m_nicList = m_searcher.ListUpNic();

            triggerOnBtn1.Enabled = false;
            triggerOffBtn1.Enabled = false;

            triggerOnBtn2.Enabled = false;
            triggerOffBtn2.Enabled = false;

            if (m_nicList != null)
            {
                for (int i = 0; i < m_nicList.Count; i++)
                {
                    NICcomboBox.Items.Add(m_nicList[i].NicIpAddr);
                }
            }

            NICcomboBox.SelectedIndex = 0;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (!m_searcher.IsSearching)
            {
                m_searcher.SelectedNicSearchResult = m_nicList[NICcomboBox.SelectedIndex];

                NICcomboBox.Enabled = false;
                searchBtn.Enabled = false;

                selectBtn1.Enabled = false;
                selectBtn2.Enabled = false;

                Application.DoEvents();

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                m_searcher.Start((res) =>
                {
                    BeginInvoke(new delegateUserControl(SearchListUp), res.IpAddress);
                });
            }
        }

        private void selectBtn1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                m_reader_1.IpAddress = comboBox1.SelectedItem.ToString();
                System.Diagnostics.Debug.WriteLine(comboBox1.SelectedItem.ToString());

                m_reader_1.Connect((data) =>
                {
                    BeginInvoke(new delegateUserControl(ReceivedDataWrite_1), Encoding.ASCII.GetString(data));
                });

                NICcomboBox.Enabled = false;
                searchBtn.Enabled = false;

                comboBox1.Enabled = false;

                triggerOnBtn1.Enabled = true;
                triggerOffBtn1.Enabled = true;
            }
        }

        private void selectBtn2_Click(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem != null)
            {
                m_reader_2.IpAddress = comboBox2.SelectedItem.ToString();
                System.Diagnostics.Debug.WriteLine(comboBox2.SelectedItem.ToString());

                m_reader_2.Connect((data) =>
                {
                    BeginInvoke(new delegateUserControl(ReceivedDataWrite_2), Encoding.ASCII.GetString(data));
                });

                NICcomboBox.Enabled = false;
                searchBtn.Enabled = false;

                comboBox2.Enabled = false;

                triggerOnBtn2.Enabled = true;
                triggerOffBtn2.Enabled = true;
            }
        }

        private void triggerOnBtn1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                payload1.Text = "Start read 2D from incoming parts...";
                ReceivedDataWrite_1(m_reader_1.ExecCommand("LON"));
            }
        }
        private void triggerOnBtn2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                payload2.Text = "Start read 2D from incoming parts...";
                ReceivedDataWrite_2(m_reader_2.ExecCommand("LON"));
            }
        }

        private void triggerOffBtn1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                payload1.Text = "Stop read 2D from incoming parts...";
                ReceivedDataWrite_1(m_reader_1.ExecCommand("LOFF"));
            }
        }

        private void triggerOffBtn2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                payload2.Text = "Stop read 2D from incoming parts...";
                ReceivedDataWrite_2(m_reader_2.ExecCommand("LOFF"));
            }
        }

        private void triggerAllBtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                payload1.Text = "Start read 2D from incoming parts...";
                ReceivedDataWrite_1(m_reader_1.ExecCommand("LON"));

                payload2.Text = "Start read 2D from incoming parts...";
                ReceivedDataWrite_2(m_reader_2.ExecCommand("LON"));
            }
        }

        private void ReceivedDataWrite_1(string receivedData)
        {
            resultRead1.Text = receivedData;
            payload1.Text = ("[" + m_reader_1.IpAddress + "][" + DateTime.Now + "]" + receivedData);
        }

        private void ReceivedDataWrite_2(string receivedData)
        {
            resultRead2.Text = receivedData;
            payload2.Text = ("[" + m_reader_2.IpAddress + "][" + DateTime.Now + "]" + receivedData);
        }

        private delegate void delegateUserControl(string str);
        private void SearchListUp(string ip)
        {
            if (ip != "")
            {
                comboBox1.Items.Add(ip);
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;

                comboBox2.Items.Add(ip);
                comboBox2.SelectedIndex = comboBox2.Items.Count - 1;

                return;
            }
            else
            {
                searchBtn.Enabled = true;
                selectBtn1.Enabled = true;
                selectBtn2.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_reader_1.Dispose();
            m_reader_2.Dispose();
            m_searcher.Dispose();
        }
    }
}