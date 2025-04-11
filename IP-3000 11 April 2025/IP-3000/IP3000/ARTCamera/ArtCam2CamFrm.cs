using ArtCamSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace IP3000_Control.ARTCamera
{
    public partial class ArtCam2CamFrm : Form
    {
        private byte[][] m_pCapture = new byte[2][];
        private Bitmap[] m_Bitmap = { null, null };
        private int m_PreviewMode = -1;
        public CArtCam[] m_CArtCam = { new CArtCam(), new CArtCam() };
        private int m_DllType = -1;
        private int m_DllCount = 0;
        private int m_DllSata = -1;
        private int m_SataType = -1;

        public ArtCam2CamFrm()
        {
            InitializeComponent();
        }

        private void ArtCam2CamFrm_Load(object sender, EventArgs e)
        {
            // Drawing by double buffer(not to flicker)
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            if (File.Exists("Sample.xml"))
            {
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(int[]));
                System.IO.FileStream fs = new System.IO.FileStream("Sample.xml", System.IO.FileMode.Open);
                int[] Type = new int[2];
                Type = (int[])ser.Deserialize(fs);
                fs.Close();
                m_DllType = Type[0];
                m_SataType = Type[1];
            }
            OnDllReload();
            if (-1 != m_DllType)
            {
                OnDllChange((object)0, System.EventArgs.Empty, m_DllType, m_SataType);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>

        protected override unsafe void WndProc(ref Message m)
        {
            // WM_GRAPHNOTIFY
            if (DLL_MESSAGE.WM_GRAPHNOTIFY == (DLL_MESSAGE)m.Msg)
            {
            }
            // WM_ERROR
            else if (DLL_MESSAGE.WM_ERROR == (DLL_MESSAGE)m.Msg ||
                (DLL_MESSAGE.WM_GRAPHPAINT == (DLL_MESSAGE)m.Msg && null == (GP_INFO*)m.WParam)
                )
            {
            }
            // WM_GRAPHPAINT
            else if (DLL_MESSAGE.WM_GRAPHPAINT == (DLL_MESSAGE)m.Msg)
            {
                GP_INFO* pInfo = (GP_INFO*)m.WParam;
                if (pInfo != null)
                {
                    ImageBox_1.Invalidate();
                    ImageBox_2.Invalidate();
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // Drawing by double buffer(not to flicker)
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            if (File.Exists("Sample.xml"))
            {
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(int[]));
                System.IO.FileStream fs = new System.IO.FileStream("Sample.xml", System.IO.FileMode.Open);
                int[] Type = new int[2];
                Type = (int[])ser.Deserialize(fs);
                fs.Close();
                m_DllType = Type[0];
                m_SataType = Type[1];
            }
            OnDllReload();
            if (-1 != m_DllType)
            {
                OnDllChange((object)0, System.EventArgs.Empty, m_DllType, m_SataType);
            }

            //this.WindowState = FormWindowState.Maximized;
            //this.TopMost = false;
            menuPreview_Click(null, null);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_DllType != -1)
            {
                int[] Type = { (int)m_DllType, (int)m_SataType };
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(int[]));
                System.IO.FileStream fs = new System.IO.FileStream("Sample.xml", System.IO.FileMode.Create);
                ser.Serialize(fs, Type);
                fs.Close();
            }

            //Release();
            //m_CArtCam[0].FreeLibrary();
            //m_CArtCam[1].FreeLibrary();
        }

        private void Release()
        {
            m_CArtCam[0].Release();
            m_CArtCam[1].Release();

            for (int i = 0; i < 2; i++)
            {
                if (m_Bitmap[i] != null)
                {
                    m_Bitmap[i].Dispose();
                    m_Bitmap[i] = null;
                }
            }

            m_PreviewMode = -1;
            timer1.Enabled = false;
        }

        // Save
        private void menuSave_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            m_CArtCam[0].SaveImage("Image1.jpg", FILETYPE.FILETYPE_JPEG_NOMAL);
            m_CArtCam[1].SaveImage("Image2.jpg", FILETYPE.FILETYPE_JPEG_NOMAL);
        }

        // End
        private void menuExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        // Preview Draw automatically
        private void menuPreview_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = false;
            ImagePanel_2.Visible = false;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();

            // Set window to be displayed
            // When setting NULL to hWnd,it is possible to create new window and show it.
            m_CArtCam[0].SetPreviewWindow(this.Handle, 0, 0, this.Width / 2, this.Height);
            m_CArtCam[1].SetPreviewWindow(this.Handle, this.Width / 2, 0, this.Width, this.Height);

            //m_CArtCam[0].SetPreviewWindow(this.Handle, 0, 0, 960, 768);
            //m_CArtCam[1].SetPreviewWindow(this.Handle, 961, 0, 961 + 960, 768);

            m_CArtCam[0].Preview();
            m_CArtCam[1].Preview();

            // Check menu
            menuPreview.Checked = true;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 0;
        }

        // Callback: Obtain image pointer of image and draw its own.
        private void menuCallback_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = true;
            ImagePanel_2.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();


            // If drawing by yourself, set all window size to 0.
            // An automatic display can be performed, if window size is set up even when using CallBackPreview
            m_CArtCam[0].SetPreviewWindow(this.Handle, 0, 0, 0, 0);
            m_CArtCam[1].SetPreviewWindow(this.Handle, 0, 0, 0, 0);

            // Creat area for capturing
            CreateBitmap(0);
            CreateBitmap(1);

            ImageBox_1.SetBounds(0, 0, getWidth(0), getHeight(0));
            ImageBox_2.SetBounds(0, 0, getWidth(1), getHeight(1));

            m_CArtCam[0].CallBackPreview(this.Handle, m_pCapture[0], getSize(0), 1);
            m_CArtCam[1].CallBackPreview(this.Handle, m_pCapture[1], getSize(1), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = true;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 1;
            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();
        }

        // Snapshot
        private void menuSnapshot_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = true;
            ImagePanel_2.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();

            // Creat area for capturing
            CreateBitmap(0);
            CreateBitmap(1);

            ImageBox_1.SetBounds(0, 0, getWidth(0), getHeight(0));
            ImageBox_2.SetBounds(0, 0, getWidth(1), getHeight(1));

            m_CArtCam[0].SnapShot(m_pCapture[0], getSize(0), 1);
            m_CArtCam[1].SnapShot(m_pCapture[1], getSize(1), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 2;

            timer1.Enabled = true;
            timer1.Interval = 100;

            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();

        }

        // Capture
        private void menuCapture_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = true;
            ImagePanel_2.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();

            // Creat area for capturing
            CreateBitmap(0);
            CreateBitmap(1);

            ImageBox_1.SetBounds(0, 0, getWidth(0), getHeight(0));
            ImageBox_2.SetBounds(0, 0, getWidth(1), getHeight(1));

            m_CArtCam[0].Capture();
            m_CArtCam[1].Capture();

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = true;
            menuTrigger.Checked = false;

            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();

            timer1.Interval = 100;
            timer1.Enabled = true;
            m_PreviewMode = 3;
        }

        // Capture timer
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (m_PreviewMode == 3)
            {
                m_CArtCam[0].SnapShot(m_pCapture[0], getSize(0), 1);
                m_CArtCam[1].SnapShot(m_pCapture[1], getSize(1), 1);
            }
            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();
        }

        // Trigger
        private void menuTrigger_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = true;
            ImagePanel_2.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();


            // If drawing by yourself, set all window size to 0.
            // An automatic display can be performed, if window size is set up even when using CallBackPreview
            m_CArtCam[0].SetPreviewWindow(this.Handle, 0, 0, 0, 0);
            m_CArtCam[1].SetPreviewWindow(this.Handle, 0, 0, 0, 0);

            // Creat area for capturing
            CreateBitmap(0);
            CreateBitmap(1);

            ImageBox_1.SetBounds(0, 0, getWidth(0), getHeight(0));
            ImageBox_2.SetBounds(0, 0, getWidth(1), getHeight(1));

            m_CArtCam[0].Trigger(this.Handle, m_pCapture[0], getSize(0), 1);
            m_CArtCam[1].Trigger(this.Handle, m_pCapture[1], getSize(1), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = true;

            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();
            m_PreviewMode = 4;
        }

        // Creat area for capturing
        private void CreateBitmap(int n)
        {
            // Release when locked
            if (null != m_Bitmap[n])
            {
                m_Bitmap[n].Dispose();
            }

            switch (getColorMode(n))
            {
                case 8:
                case 16:
                    m_Bitmap[n] = new Bitmap(getWidth(n), getHeight(n), PixelFormat.Format8bppIndexed);

                    // Pallet modification
                    ColorPalette pal = m_Bitmap[n].Palette;
                    Color[] cpe = m_Bitmap[n].Palette.Entries;

                    for (int i = 0; i < 256; i++)
                    {
                        cpe.SetValue(Color.FromArgb(i, i, i), i);
                        pal.Entries[i] = cpe[i];
                    }
                    m_Bitmap[n].Palette = pal;
                    break;

                case 24: m_Bitmap[n] = new Bitmap(getWidth(n), getHeight(n), PixelFormat.Format24bppRgb); break;
                case 32: m_Bitmap[n] = new Bitmap(getWidth(n), getHeight(n), PixelFormat.Format24bppRgb); break;
                case 48: m_Bitmap[n] = new Bitmap(getWidth(n), getHeight(n), PixelFormat.Format24bppRgb); break;
                case 64: m_Bitmap[n] = new Bitmap(getWidth(n), getHeight(n), PixelFormat.Format24bppRgb); break;
            }

            // Arrangement for capture
            m_pCapture[n] = new Byte[getSize(n)];
        }


        // Camera settings
        private void menuCamera1_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            if (0 != m_CArtCam[0].SetCameraDlg(this.Handle))
            {
                switch (m_PreviewMode)
                {
                    case 0: menuPreview_Click(sender, e); break;
                    case 1: menuCallback_Click(sender, e); break;
                }
            }
        }

        private void menuCamera2_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[1].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            if (0 != m_CArtCam[1].SetCameraDlg(this.Handle))
            {
                switch (m_PreviewMode)
                {
                    case 0: menuPreview_Click(sender, e); break;
                    case 1: menuCallback_Click(sender, e); break;
                }
            }
        }

        // Filter settings
        private void menuFilter1_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            m_CArtCam[0].SetImageDlg(this.Handle);
        }

        private void menuFilter2_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[1].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            m_CArtCam[1].SetImageDlg(this.Handle);
        }

        // Analog settings
        private void menuAnalog1_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            m_CArtCam[0].SetAnalogDlg(this.Handle);
        }

        private void menuAnalog2_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam[1].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            m_CArtCam[1].SetAnalogDlg(this.Handle);
        }

        private void OnDllReload()
        {
            // Delete DLL list
            for (int i = 0; i < m_DllCount; i++)
            {
                menuDLL.MenuItems.RemoveAt(1);
            }
            m_DllCount = 0;
            m_DllSata = -1;

            // Search for DLL
            String szDirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            String[] files = Directory.GetFiles(szDirPath, "Art*.dll");
            foreach (String szFileName in files)
            {
                CArtCam ArtCam = new CArtCam();
                if (ArtCam.LoadLibrary(szFileName))
                {

                    long ver = ArtCam.GetDllVersion() & 0xFFFF;
                    String szMenu = String.Format("{0}\tVersion {1:D4}", Path.GetFileNameWithoutExtension(szFileName), ver);

                    m_DllCount++;
                    if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_SATA == (ARTCAM_CAMERATYPE)(ArtCam.GetDllVersion() >> 16))
                    {

                        MenuItem menuSata = new MenuItem();
                        menuSata.Text = szMenu;
                        String[] CameraName = {
                                            "LVDS",
                                            "300MI",
                                            "500MI",
                                            "MV413",
                                            "800MI",
                                            "036MI",
                                            "150P3",
                                            "267KY",
                                            "274KY",
                                            "625KY",
                                            "130MI",
                                            "200MI",
                                        };
                        for (int i = 0; i < CameraName.Length; i++)
                        {
                            MenuItem mi = new MenuItem();
                            mi.Text = CameraName[i];
                            mi.Click += new System.EventHandler(OnMenuDLLSelect);
                            menuSata.MenuItems.Add(i, mi);
                        }
                        menuDLL.MenuItems.Add(m_DllCount, menuSata);
                        m_DllSata = m_DllCount - 1;
                    }
                    else
                    {
                        MenuItem mi = new MenuItem();
                        mi.Text = szMenu;
                        mi.Click += new System.EventHandler(OnMenuDLLSelect);
                        menuDLL.MenuItems.Add(m_DllCount, mi);
                    }
                }
            }
        }

        private void OnMenuDLLSelect(object sender, System.EventArgs e)
        {
            int id = menuDLL.MenuItems.IndexOf((MenuItem)sender);
            if (id > -1)
            {
                OnDllChange(sender, e, id - 1, -1);
            }
            else
            {
                int type = ((MenuItem)sender).Index;
                if ((int)ARTCAM_CAMERATYPE_SATA.ARTCAM_CAMERATYPE_SATA_LVDS <= type && type <= (int)ARTCAM_CAMERATYPE_SATA.ARTCAM_CAMERATYPE_SATA_200MI)
                {
                    OnDllChange(sender, e, m_DllSata, type);
                }
            }
        }

        private void OnDllChange(object sender, System.EventArgs e, int DllType, int SataType)
        {
            Release();
            m_CArtCam[0].FreeLibrary();
            m_CArtCam[1].FreeLibrary();

            if (0 == m_DllCount) return;
            String stMenu = menuDLL.MenuItems[(int)DllType + 1].Text;
            String[] stArray = stMenu.Split('\t');
            String szDllName = String.Format("{0}.dll", stArray[0]);
            bool res = m_CArtCam[0].LoadLibrary(szDllName);
            if (!res)
            {
                MessageBox.Show("DLL is not found.\nIt may have been relocated after executing.");
                return;
            }
            m_CArtCam[1].LoadLibrary(szDllName);

            // Initialize is to be called first
            // By setting Window Handle here, WM_ERROR can be obtained
            if (!m_CArtCam[0].Initialize(this.Handle))
            {
                MessageBox.Show("Failed to initialize SDK");
                return;
            }
            m_CArtCam[1].Initialize(this.Handle);

            m_DllType = DllType;
            m_SataType = SataType;

            // Select SATA camera type when use Sata.dll
            if (-1 != SataType && ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_SATA == m_CArtCam[0].GetDllType())
            {
                m_CArtCam[0].SetCameraType(SataType);
                m_CArtCam[1].SetCameraType(SataType);
            }

            // It is possible to know if device is connected after obtain device name.
            StringBuilder Temp = new StringBuilder(256);
            if (0 == m_CArtCam[0].GetDeviceName(0, Temp, 256))
            {
                m_CArtCam[0].FreeLibrary();
            }
            if (0 == m_CArtCam[1].GetDeviceName(1, Temp, 256))
            {
                // No device
                // Please note that there is possibility of duplication access to one device
                // if there is access from two classes when only one device detected.
                // In this case, please release class or do not call function.

                // If you release class, it comes to be safe failure even when call function.
                m_CArtCam[1].FreeLibrary();
            }


            m_CArtCam[0].SetDeviceNumber(0);
            m_CArtCam[1].SetDeviceNumber(1);

            // To operate 2 cameras simultenously, set camera clock to half.
            m_CArtCam[0].SetHalfClock(1);
            m_CArtCam[1].SetHalfClock(1);


            // Check menu
            for (int i = 0; i < m_DllCount; i++)
            {
                menuDLL.MenuItems[(int)i + 1].Checked = false;
            }
            if (ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_SATA != m_CArtCam[0].GetDllType())
            {
                menuDLL.MenuItems[(int)DllType + 1].Checked = true;
            }

        }

        private void menuDllReload_Click(object sender, System.EventArgs e)
        {
            OnDllReload();
        }

        private int getSize(int n)
        {
            return ((getWidth(n) * (getColorMode(n) / 8) + 3) & ~3) * getHeight(n);
        }

        private int getWidth(int n)
        {
            int[] Size = { 1, 2, 4, 8 };
            return m_CArtCam[n].Width() / Size[(int)(getSubSample(n))];
        }

        private int getHeight(int n)
        {
            int[] Size = { 1, 2, 4, 8 };
            return m_CArtCam[n].Height() / Size[(int)getSubSample(n)];
        }

        private int getColorMode(int n)
        {
            return ((m_CArtCam[n].GetColorMode() + 7) & ~7);
        }

        private int getSubSample(int n)
        {
            return ((int)m_CArtCam[n].GetSubSample() & 0x03);
        }

        private BitmapData LockBitmap(int n)
        {
            switch (getColorMode(n))
            {
                case 8:
                case 16:
                    return m_Bitmap[n].LockBits(new Rectangle(0, 0, getWidth(n), getHeight(n)), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                case 24:
                case 32:
                case 48:
                case 64:
                    return m_Bitmap[n].LockBits(new Rectangle(0, 0, getWidth(n), getHeight(n)), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }

            return null;
        }

        private void DrawImage(Graphics g, int n)
        {
            if (null == m_Bitmap[n])
            {
                return;
            }

            BitmapData pBitmapData = LockBitmap(n);
            if (null == pBitmapData)
            {
                return;
            }


            unsafe
            {
                byte* pdest = (byte*)pBitmapData.Scan0.ToPointer();

                // In case of 16 bit transfer,convert to 8 bit and display
                if (getColorMode(n) == 16)
                {
                    int size = getWidth(n) * getHeight(n);
                    switch (m_CArtCam[n].GetColorMode())
                    {
                        case 10: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[n][i * 2 + 1] << 6) | (m_pCapture[n][i * 2] >> 2)); break;
                        case 12: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[n][i * 2 + 1] << 4) | (m_pCapture[n][i * 2] >> 4)); break;
                        case 14: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[n][i * 2 + 1] << 2) | (m_pCapture[n][i * 2] >> 6)); break;
                        case 16: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[n][i * 2 + 1] << 0) | (m_pCapture[n][i * 2] >> 8)); break;
                    }
                }
                else if (getColorMode(n) == 32)
                {
                    int size = getWidth(n) * getHeight(n);
                    for (int i = 0; i < size; i++)
                    {
                        pdest[i * 3] = m_pCapture[n][i * 4];
                        pdest[i * 3 + 1] = m_pCapture[n][i * 4 + 1];
                        pdest[i * 3 + 2] = m_pCapture[n][i * 4 + 2];
                    }
                }
                // This is a heavy load. When using 16 (10) bit color, use C language.
                else if (getColorMode(n) == 48 || getColorMode(n) == 64)
                {
                    int bpp = getColorMode(n) / 8;
                    int size = getWidth(n) * getHeight(n);
                    switch (m_CArtCam[n].GetColorMode())
                    {
                        case 42:
                        case 58:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[n][i * bpp + 1] << 6) | (m_pCapture[n][i * bpp] >> 2));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[n][i * bpp + 3] << 6) | (m_pCapture[n][i * bpp + 2] >> 2));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[n][i * bpp + 5] << 6) | (m_pCapture[n][i * bpp + 4] >> 2));
                            }
                            break;
                        case 44:
                        case 60:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[n][i * bpp + 1] << 4) | (m_pCapture[n][i * bpp] >> 4));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[n][i * bpp + 3] << 4) | (m_pCapture[n][i * bpp + 2] >> 4));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[n][i * bpp + 5] << 4) | (m_pCapture[n][i * bpp + 4] >> 4));
                            }
                            break;
                        case 46:
                        case 62:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[n][i * bpp + 1] << 2) | (m_pCapture[n][i * bpp] >> 6));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[n][i * bpp + 3] << 2) | (m_pCapture[n][i * bpp + 2] >> 6));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[n][i * bpp + 5] << 2) | (m_pCapture[n][i * bpp + 4] >> 6));
                            }
                            break;
                        case 48:
                        case 64:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[n][i * bpp + 1] << 0) | (m_pCapture[n][i * bpp] >> 8));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[n][i * bpp + 3] << 0) | (m_pCapture[n][i * bpp + 2] >> 8));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[n][i * bpp + 5] << 0) | (m_pCapture[n][i * bpp + 4] >> 8));
                            }
                            break;
                    }
                }
                else
                {
                    int size = getSize(n);
                    for (int i = 0; i < size; i++)
                    {
                        pdest[i] = m_pCapture[n][i];
                    }
                }
            }

            m_Bitmap[n].UnlockBits(pBitmapData);


            // Image display
            int iWidth = m_Bitmap[n].Width;
            int iHeight = m_Bitmap[n].Height;
            g.DrawImage(m_Bitmap[n], new Rectangle(0, 0, getWidth(n), getHeight(n)));
        }

        private void ImageBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            DrawImage(e.Graphics, 0);
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            ImagePanel_1.SetBounds(0, 0, this.ClientRectangle.Right / 2, this.ClientRectangle.Bottom);
            ImagePanel_2.SetBounds(this.ClientRectangle.Right / 2, 0, this.ClientRectangle.Right / 2, this.ClientRectangle.Bottom);

        }

        private void ImageBox_2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            DrawImage(e.Graphics, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start2Camera()
        {
            menuPreview_Click(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camNo"></param>
        /// <param name="imagePath"></param>
        public void AcquireImage(int camNo, string imagePath)
        {
            Thread.Sleep(30);

            if (!m_CArtCam[0].IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel_1.Visible = true;
            ImagePanel_2.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam[0].Close();
            m_CArtCam[1].Close();

            // Creat area for capturing
            CreateBitmap(0);
            CreateBitmap(1);

            ImageBox_1.SetBounds(0, 0, getWidth(0), getHeight(0));
            ImageBox_2.SetBounds(0, 0, getWidth(1), getHeight(1));

            m_CArtCam[0].SnapShot(m_pCapture[0], getSize(0), 1);
            m_CArtCam[1].SnapShot(m_pCapture[1], getSize(1), 1);

            m_PreviewMode = 2;

            ImageBox_1.Invalidate();
            ImageBox_2.Invalidate();
            int res = camNo == 0 ? m_CArtCam[0].SaveImage(imagePath, FILETYPE.FILETYPE_JPEG_HIGH) : m_CArtCam[1].SaveImage(imagePath, FILETYPE.FILETYPE_JPEG_HIGH);

            Bitmap cloneImage = null;
            using (Bitmap bitMapImage = new Bitmap(imagePath))
            {
                cloneImage = new Bitmap(bitMapImage, 960, 768);
            }

            using (cloneImage)
            {
                int newWidth = 960;
                int newHeight = 768;
                Bitmap newImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage((Image)cloneImage, 0, 0, newWidth, newHeight);
                }
                File.Delete(imagePath);
                cloneImage.Save(imagePath, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private Image ResizeImage(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }


        private void ArtCam2CamFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
