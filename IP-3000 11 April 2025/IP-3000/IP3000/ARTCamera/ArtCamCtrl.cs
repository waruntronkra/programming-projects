using ArtCamSdk;
using HalconDotNet;
using IP3000_Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000.ARTCamera
{
    public partial class ArtCamCtrl : UserControl
    {
        private byte[] m_pCapture;
        private Bitmap m_Bitmap = null;
        private int m_PreviewMode = -1;
        private int m_SelectDevice = -1;
        public CArtCam m_CArtCam = new CArtCam();
        private string m_RecName;
        private bool m_SaveFlg = false;
        private bool m_StopFlg = false;

        //public int m_ViewScale = 33;
        public int m_ViewScale = 100;

        private ARTCAM_CAMERATYPE m_CurrentCameraType;
        private int m_DllType = -1;
        private int m_DllCount = 0;
        private int m_DllSata = -1;
        private int m_SataType = -1;


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);
        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        private string _cameraName = string.Empty;

        public Point startPoint;
        public Rectangle cropRect;
        public bool isDragging = false;

        public HImage hImage = null;

        public string CurFileName {  get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        public ArtCamCtrl(string cameraName)
        {
            _cameraName = cameraName;
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArtCamCtrl_Load(object sender, EventArgs e)
        {
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
            OnDllReload(sender, e);
            if (-1 != m_DllType)
            {
                OnDllChange((object)0, System.EventArgs.Empty, m_DllType, m_SataType);
            }
        }

        private void OnDllReload(object sender, System.EventArgs e)
        {
            for (int i = 0; i < m_DllCount; i++)
            {
                menuDLL.MenuItems.RemoveAt(1);
            }
            m_DllCount = 0;
            m_DllSata = -1;

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
            m_CArtCam.FreeLibrary();

            m_CurrentCameraType = 0;

            if (0 == m_DllCount) return;
            String stMenu = menuDLL.MenuItems[(int)DllType + 1].Text;
            String[] stArray = stMenu.Split('\t');
            String szDllName = String.Format("{0}.dll", stArray[0]);
            bool res = m_CArtCam.LoadLibrary(szDllName);
            if (!res)
            {
                MessageBox.Show("DLL is not found.\nIt may have been relocated after executing.");
                return;
            }
            else
            {
                m_CurrentCameraType = (ARTCAM_CAMERATYPE)(m_CArtCam.GetDllVersion() >> 16);
            }
            // Initialize is to be called first
            // By setting Window Handle here, WM_ERROR can be obtained
            if (!m_CArtCam.Initialize(this.Handle))
            {
                MessageBox.Show("Failed to initialize SDK");
                return;
            }
            m_DllType = DllType;
            m_SataType = SataType;

            // Check menu
            for (int i = 0; i < m_DllCount; i++)
            {
                menuDLL.MenuItems[(int)i + 1].Checked = false;
            }
            // Select SATA camera type when use Sata.dll
            if (-1 != SataType && ARTCAM_CAMERATYPE.ARTCAM_CAMERATYPE_SATA == m_CurrentCameraType)
            {
                m_CArtCam.SetCameraType(SataType);
                m_CArtCam.SetDeviceNumber(0);
            }
            else
            {
                menuDLL.MenuItems[(int)DllType + 1].Checked = true;
            }


            // Device modification menu
            for (int i = 0; i < 8; i++)
            {
                StringBuilder Temp = new StringBuilder(256);
                if (0 != m_CArtCam.GetDeviceName(i, Temp, 256))
                {
                    menuDevice.MenuItems[i].Text = Temp.ToString();
                    menuDevice.MenuItems[i].Enabled = true;
                }
                else
                {
                    menuDevice.MenuItems[i].Enabled = false;
                }
            }

            DeviceChange(sender, e, 0);

            //ImageBox.SetBounds(0, 0, getWidth(), getHeight());
            //ImageBox.SetBounds(0, 0, ImagePanel.Width, ImagePanel.Height-18);
            ImageBox.SetBounds(0, 0, 960, 768);
        }

        // Capture timer
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (m_PreviewMode == 3)
            {
                m_CArtCam.SnapShot(m_pCapture, getSize(), 1);
                m_SaveFlg = true;
            }
            ImageBox.Invalidate();
        }

        // Trigger
        private void menuTrigger_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam.Close();

            // If drawing by yourself, set all window size to 0.
            // An automatic display can be performed, if window size is set up even when using CallBackPreview
            m_CArtCam.SetPreviewWindow(this.Handle, 0, 0, 0, 0);

            // Create area for capturing
            CreateBitmap();

            //ImageBox.SetBounds(0, 0, 711, 585);
            ImageBox.SetBounds(0, 0, getWidth(), getHeight());

            // Capture image
            m_CArtCam.Trigger(this.Handle, m_pCapture, getSize(), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = true;

            m_PreviewMode = 4;
            m_StopFlg = false;
            menuStop.Text = "Pause";

            ImageBox.Invalidate();
        }

        // Pause/Resume
        private void menuStop_Click(object sender, System.EventArgs e)
        {
            // Stop only for animated picture
            if (0 == m_PreviewMode || 1 == m_PreviewMode)
            {
                if (m_StopFlg)
                {
                    m_CArtCam.StartPreview();
                    m_StopFlg = false;

                    menuStop.Text = "Pause";
                }
                else
                {
                    m_CArtCam.StopPreview();
                    m_StopFlg = true;

                    menuStop.Text = "Resume";
                }
            }
        }

        // Recording
        private void menuRec_Click(object sender, System.EventArgs e)
        {
            saveFileDialog1.Filter = "AVIFile(*.avi)|*.avi||";
            saveFileDialog1.DefaultExt = "avi";

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                m_RecName = saveFileDialog1.FileName;

                // Pause
                m_CArtCam.Close();

                // Display on different window while recording
                // When "hWnd" is NULL, window is created from SDK side and therefore the size is not important.
                m_CArtCam.SetPreviewWindow(IntPtr.Zero, 0, 0, 0, 0);

                // 5 seconds recording
                m_CArtCam.Record(m_RecName, 5000, 1);

                // Timing for record ending is not sent from SDK and therefore it needs to be stopped by software.
                // Just in case, run the timer for 1 sencond longer.
                timerRec.Interval = 6000;
                timerRec.Enabled = true;
            }
        }

        // Recording timer
        private void timerRec_Tick(object sender, System.EventArgs e)
        {
            timerRec.Enabled = false;
            m_CArtCam.Close();

            if (DialogResult.Yes == MessageBox.Show("Recording complete. /n Play file?", "Complete", MessageBoxButtons.YesNo))
            {
                System.Diagnostics.Process.Start(m_RecName);
            }
        }

        // End
        private void menuExit_Click(object sender, System.EventArgs e)
        {
            //Close();
        }

        // Preview Draw automatically
        private void menuPreview_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = false;
            timer1.Enabled = false;

            // Release device
            m_CArtCam.Close();

            // Set window to be displayed
            // When setting NULL to hWnd,it is possible to create new window and show it.

            //this.Width = 223;
            //this.Height = 194;
            //MessageBox.Show(this.Width.ToString(),this.Height.ToString());

            //m_CArtCam.SetPreviewWindow(this.Handle, 0, 0, this.Width, this.Height);
            m_CArtCam.SetPreviewWindow(this.Handle, 0, 0, 960, 768);
            m_CArtCam.Preview();

            // Check menu
            menuPreview.Checked = true;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 0;
            m_SaveFlg = true;
            m_StopFlg = false;
            menuStop.Text = "Pause";

            ImageBox.Invalidate();
        }

        // Callback: Obtain image pointer of image and draw its own.
        private void menuCallback_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam.Close();

            // If drawing by yourself, set all window size to 0.
            // An automatic display can be performed, if window size is set up even when using CallBackPreview
            m_CArtCam.SetPreviewWindow(Handle, 0, 0, 0, 0);

            // Creat area for capturing
            CreateBitmap();

            //ImageBox.SetBounds(0, 0, 711, 585);
            ImageBox.SetBounds(0, 0, getWidth(), getHeight());

            // Capture image
            m_CArtCam.CallBackPreview(Handle, m_pCapture, getSize(), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = true;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 1;
            m_StopFlg = false;
            menuStop.Text = "Pause";

            ImageBox.Invalidate();
        }

        // Snapshot
        private void menuSnapshot_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam.Close();

            // Creat area for capturing
            CreateBitmap();

            //ImageBox.SetBounds(0, 0, 500, 500);
            ImageBox.SetBounds(0, 0, getWidth(), getHeight());

            // Capture image
            m_CArtCam.SnapShot(m_pCapture, getSize(), 1);

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = false;
            menuTrigger.Checked = false;

            m_PreviewMode = 2;
            m_SaveFlg = true;
            m_StopFlg = false;
            menuStop.Text = "Pause";

            timer1.Enabled = true;
            timer1.Interval = 100;

            ImageBox.Invalidate();
        }

        public void AcquireImage()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(AcquireImage));
                return;
            }

            ImageBox.Image = null;
            ImageBox.Invalidate();
            //Thread.Sleep(triggerDelay);
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;
            // Release device
            m_CArtCam.Close();

            // Creat area for capturing
            CreateBitmap();
            //ImageBox.SetBounds(0, 0, getWidth() * m_ViewScale / 100, getHeight() * m_ViewScale / 100);
            //ImageBox.SetBounds(0,0, getWidth(), getHeight());
            //m_CArtCam.SnapShot(m_pCapture, getSize(), 1);
            ImageBox.SetBounds(0, 0, 960, 768);
            m_CArtCam.SnapShot(m_pCapture, getSize(), 1);
            m_PreviewMode = 2;
            ImageBox.Invalidate();

            Thread threadSaveImage = new Thread(() =>
            {
                //m_CArtCam.SaveImage(CurFileName, FILETYPE.FILETYPE_JPEG_NOMAL);
                Bitmap cloneBitmap = null;
                using (var src = new Bitmap(m_Bitmap))
                {
                    cloneBitmap = new Bitmap(src, 960, 768);
                }

                using (var bmp = new Bitmap(960, 768, PixelFormat.Format24bppRgb))
                using (var gr = Graphics.FromImage(bmp))
                {
                    gr.Clear(Color.Blue);
                    gr.DrawImage(cloneBitmap, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    File.Delete(CurFileName);
                    bmp.Save(CurFileName, ImageFormat.Jpeg);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerDelay"></param>
        /// <param name="imagePath"></param>
        public void AcquireImage(int triggerDelay, string imagePath)
        {
            ImageBox.Image = null;
            ImageBox.Invalidate();
            //Thread.Sleep(triggerDelay);
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;
            // Release device
            m_CArtCam.Close();

            CurFileName = imagePath;
            // Creat area for capturing
            CreateBitmap();
            //ImageBox.SetBounds(0, 0, getWidth() * m_ViewScale / 100, getHeight() * m_ViewScale / 100);
            //ImageBox.SetBounds(0,0, getWidth(), getHeight());
            //m_CArtCam.SnapShot(m_pCapture, getSize(), 1);
            ImageBox.SetBounds(0, 0, 960, 768);
            m_CArtCam.SnapShot(m_pCapture, getSize(), 1);
            m_PreviewMode = 2;
            ImageBox.Invalidate();
            //Application.DoEvents();

            #region unused 
            ////m_CArtCam.SaveImage(imagePath, FILETYPE.FILETYPE_JPEG_NOMAL);
            //////Halcon crop image
            ////new HDevelopExport(imagePath);
            //Bitmap cloneImage = null;
            //using (Bitmap bitMapImage = new Bitmap(imagePath))
            //{
            //    cloneImage = new Bitmap(bitMapImage, 1000, 808);
            ////    //cloneImage = new Bitmap(bitMapImage);
            //}

            //////C# crop image
            //using (cloneImage)
            //{
            //    Bitmap croppedBitmap = null;
            //    //    float zoomfac = (float)m_ViewScale / (float)100;
            //    //    if (cropRect.Width == 0 || cropRect.Height == 0) return;

            //    //    resizedImage = ResizeImage(cloneImage, zoomfac);
            //    //    ImageBox.DrawToBitmap(resizedImage, ImageBox.ClientRectangle);
            //    croppedBitmap = cloneImage.Clone(new Rectangle(20, 20, 960, 768), cloneImage.PixelFormat);
            //    //    ImageBox.Image = croppedBitmap;
            //    //    ImageBox.Size = croppedBitmap.Size;
            //    //    resizedImage = croppedBitmap;
            //    //    //ImageBox.DrawToBitmap(resizedImage, ImageBox.ClientRectangle);

            //    File.Delete(imagePath);
            //    croppedBitmap.Save(imagePath, ImageFormat.Jpeg);
            //}
            //ImageBox.Invalidate();
            #endregion
        }

        private HImage ConvertBitmapToHImage(Bitmap bitmap)
        {
            HImage hImage = new HImage();

            // Handle grayscale images (8-bit, 1-channel)
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // Lock the bitmap's bits
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

                // Get the pointer to the bitmap data
                IntPtr ptr = bmpData.Scan0;

                // Create the HImage using GenImage1 for grayscale
                hImage.GenImage1("byte", bitmap.Width, bitmap.Height, ptr);

                // Unlock the bits
                bitmap.UnlockBits(bmpData);
            }
            // Handle RGB images (24-bit, 3-channel)
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                // Lock the bitmap's bits
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // Get the pointer to the bitmap data
                IntPtr ptr = bmpData.Scan0;

                // Create the HImage using GenImageInterleaved for RGB
                hImage.GenImageInterleaved(
                    ptr,                      // Pointer to the pixel data
                    "bgr",                    // Color format
                    bitmap.Width,             // Original width
                    bitmap.Height,            // Original height
                    -1,                       // Alignment (-1 for default)
                    "byte",                   // Type ("byte" for 8-bit channels)
                    bitmap.Width,             // Image width (same as original)
                    bitmap.Height,            // Image height (same as original)
                    0,                        // Start at row 0
                    0,                        // Start at column 0
                    8,                        // Bits per channel (8-bit)
                    0                         // Bit shift (no shift)
                );

                // Unlock the bits
                bitmap.UnlockBits(bmpData);
            }
            else
            {
                throw new NotSupportedException("Only 8-bit grayscale and 24-bit RGB images are supported.");
            }

            return hImage;
        }

        private Bitmap ResizeImage(Image originalImage, float zoomFactor)
        {
            Bitmap resizedImage = null;

            if (originalImage != null)
            {
                int newWidth = (int)(originalImage.Width * zoomFactor);
                int newHeight = (int)(originalImage.Height * zoomFactor);

                resizedImage = new Bitmap(newWidth, newHeight);
                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }
                return resizedImage;
            }

            return resizedImage;
        }

        private Bitmap ZoomImage(Bitmap img, float factor)
        {
            Bitmap bmp = new Bitmap((Image)img, (int)(img.Width * factor), (int)(img.Height * factor));
            Graphics g = Graphics.FromImage(bmp);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));

            return bmp;
        }

        // Create bit-map
        private void CreateBitmap()
        {
            // In case bitmap is already created, release.
            if (null != m_Bitmap)
            {
                m_Bitmap.Dispose();
            }

            switch (getColorMode())
            {
                case 8:
                case 16:
                    m_Bitmap = new Bitmap(getWidth(), getHeight(), PixelFormat.Format8bppIndexed);

                    // Pallet modification
                    ColorPalette pal = m_Bitmap.Palette;
                    Color[] cpe = m_Bitmap.Palette.Entries;

                    for (int i = 0; i < 256; i++)
                    {
                        cpe.SetValue(Color.FromArgb(i, i, i), i);
                        pal.Entries[i] = cpe[i];
                    }
                    m_Bitmap.Palette = pal;
                    break;

                case 24: m_Bitmap = new Bitmap(getWidth(), getHeight(), PixelFormat.Format24bppRgb); break;
                case 32: m_Bitmap = new Bitmap(getWidth(), getHeight(), PixelFormat.Format24bppRgb); break;
                case 48: m_Bitmap = new Bitmap(getWidth(), getHeight(), PixelFormat.Format24bppRgb); break;
                case 64: m_Bitmap = new Bitmap(getWidth(), getHeight(), PixelFormat.Format24bppRgb); break;
            }

            // Arrangement for capture
            m_pCapture = new Byte[getSize()];
        }

        private void Release()
        {
            m_CArtCam.Release();

            if (m_Bitmap != null)
            {
                m_Bitmap.Dispose();
                m_Bitmap = null;
            }

            m_PreviewMode = -1;
            timer1.Enabled = false;
            timerRec.Enabled = false;
            m_SaveFlg = false;
            m_StopFlg = false;
        }

        public void SetPreviewCamera(int camNo)
        {
            if (camNo == 0)
            {
                menuDevice0_Click(null, null);
            }
            else
            {
                menuDevice1_Click(null, null);
            }
            menuPreview_Click(null, null);
        }

        private int getSize()
        {
            return ((getWidth() * (getColorMode() / 8) + 3) & ~3) * getHeight();
        }

        private int getWidth()
        {
            int[] Size = { 1, 2, 4, 8 };
            return m_CArtCam.Width() / Size[(int)(getSubSample())];
        }

        private int getHeight()
        {
            int[] Size = { 1, 2, 4, 8 };
            return m_CArtCam.Height() / Size[(int)getSubSample()];
        }

        private int getColorMode()
        {
            return ((m_CArtCam.GetColorMode() + 7) & ~7);
        }

        private int getSubSample()
        {
            return ((int)m_CArtCam.GetSubSample() & 0x03);
        }

        private BitmapData LockBitmap()
        {
            switch (getColorMode())
            {
                case 8:
                case 16:
                    return m_Bitmap.LockBits(new Rectangle(0, 0, getWidth(), getHeight()), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                case 24:
                case 32:
                case 48:
                case 64:
                    //return m_Bitmap.LockBits(new Rectangle(0, 0, 960, 768), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    return m_Bitmap.LockBits(new Rectangle(0, 0, getWidth(), getHeight()), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }

            return null;
        }

        private Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //g.Clear(Color.Transparent);
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch
            {
                MessageBox.Show("Bitmap could not be resized");
                return imgToResize;
            }
        }

        private Image ScaleImage(Image image, int width, int height)
        {
            double ratioHeight = (double)height / image.Height;
            double ratioWidth = (double)width / image.Width;

            float ratio = Math.Min(width / image.Width, height / image.Height);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);


            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }

        private void DrawImage(Graphics g)
        {
            if (null == m_Bitmap)
            {
                return;
            }

            BitmapData pBitmapData = LockBitmap();
            if (null == pBitmapData)
            {
                return;
            }

            unsafe
            {
                byte* pdest = (byte*)pBitmapData.Scan0.ToPointer();

                // In case of 16 bit transfer,convert to 8 bit and display
                if (getColorMode() == 16)
                {
                    int size = getWidth() * getHeight();
                    switch (m_CArtCam.GetColorMode())
                    {
                        case 10: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[i * 2 + 1] << 6) | (m_pCapture[i * 2] >> 2)); break;
                        case 12: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[i * 2 + 1] << 4) | (m_pCapture[i * 2] >> 4)); break;
                        case 14: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[i * 2 + 1] << 2) | (m_pCapture[i * 2] >> 6)); break;
                        case 16: for (int i = 0; i < size; i++) pdest[i] = (byte)((m_pCapture[i * 2 + 1] << 0) | (m_pCapture[i * 2] >> 8)); break;
                    }
                }
                else if (getColorMode() == 32)
                {
                    int size = getWidth() * getHeight();
                    for (int i = 0; i < size; i++)
                    {
                        pdest[i * 3] = m_pCapture[i * 4];
                        pdest[i * 3 + 1] = m_pCapture[i * 4 + 1];
                        pdest[i * 3 + 2] = m_pCapture[i * 4 + 2];
                    }
                }
                // This is a heavy load. When using 16 (10) bit color, use C language.
                else if (getColorMode() == 48 || getColorMode() == 64)
                {
                    int bpp = getColorMode() / 8;
                    int size = getWidth() * getHeight();
                    switch (m_CArtCam.GetColorMode())
                    {
                        case 42:
                        case 58:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[i * bpp + 1] << 6) | (m_pCapture[i * bpp] >> 2));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[i * bpp + 3] << 6) | (m_pCapture[i * bpp + 2] >> 2));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[i * bpp + 5] << 6) | (m_pCapture[i * bpp + 4] >> 2));
                            }
                            break;
                        case 44:
                        case 60:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[i * bpp + 1] << 4) | (m_pCapture[i * bpp] >> 4));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[i * bpp + 3] << 4) | (m_pCapture[i * bpp + 2] >> 4));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[i * bpp + 5] << 4) | (m_pCapture[i * bpp + 4] >> 4));
                            }
                            break;
                        case 46:
                        case 62:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[i * bpp + 1] << 2) | (m_pCapture[i * bpp] >> 6));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[i * bpp + 3] << 2) | (m_pCapture[i * bpp + 2] >> 6));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[i * bpp + 5] << 2) | (m_pCapture[i * bpp + 4] >> 6));
                            }
                            break;
                        case 48:
                        case 64:
                            for (int i = 0; i < size; i++)
                            {
                                pdest[i * 3] = (byte)((m_pCapture[i * bpp + 1] << 0) | (m_pCapture[i * bpp] >> 8));
                                pdest[i * 3 + 1] = (byte)((m_pCapture[i * bpp + 3] << 0) | (m_pCapture[i * bpp + 2] >> 8));
                                pdest[i * 3 + 2] = (byte)((m_pCapture[i * bpp + 5] << 0) | (m_pCapture[i * bpp + 4] >> 8));
                            }
                            break;
                    }
                }
                else
                {
                    int size = getSize();
                    for (int i = 0; i < size; i++)
                    {
                        pdest[i] = m_pCapture[i];
                    }
                }
            }

            m_Bitmap.UnlockBits(pBitmapData);

            g.DrawImage(m_Bitmap, new Rectangle(0, 0, 960 * m_ViewScale / 100, 768 * m_ViewScale / 100));
           
            //// Image display
            int iWidth = m_Bitmap.Width;
            int iHeight = m_Bitmap.Height;

            if (CurFileName != null && CurFileName != string.Empty)
            {               
                //m_CArtCam.SaveImage(CurFileName, FILETYPE.FILETYPE_JPEG_NOMAL);
                Bitmap cloneBitmap = null;
                using (var src = new Bitmap(m_Bitmap))
                {
                    cloneBitmap = new Bitmap(src, 960, 768);
                }

                using (var bmp = new Bitmap(960, 768, PixelFormat.Format24bppRgb))
                using (var gr = Graphics.FromImage(bmp))
                {
                    gr.Clear(Color.Blue);
                    gr.DrawImage(cloneBitmap, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    //File.Delete(imagePath);
                    bmp.Save(CurFileName, ImageFormat.Jpeg);

                    hImage = new HImage();
                    g.DrawImage(m_Bitmap, new Rectangle(0, 0, 960 * m_ViewScale / 100, 768 * m_ViewScale / 100));

                    //hImage = ConvertBitmapToHImage(m_Bitmap);
                    //hImage = ConvertBitmapToHImage(bmp);
                    hImage.ReadImage(CurFileName);
                    
                    //hImage.WriteImage("jpeg", 0, CurFileName.Replace(".jpg", "_h"));
                    CurFileName = "";
                }
            }
            
            //hImage = hImage.ZoomImageSize(960, 768, "constant");
            //double std;
            //HTuple itens = hImage.Intensity(hImage, out std);
            //double masterMean = 100;
            //double factorMean = masterMean-itens.D;
            //hImage = hImage.ScaleImage(1, factorMean);

            // Resize 2048x1536 => 960x768
            // Auto white balance

            // Line drawing
            Point MousePos = this.PointToClient(Cursor.Position);
            if ((MousePos.X < 0) || (MousePos.Y < 0) || (MousePos.X >= iWidth) || (MousePos.Y >= iHeight)) return;

                //if (cropRect != Rectangle.Empty && isDragging)
                //{
                //    g.DrawRectangle(Pens.Red, cropRect);
                //}

                //if (cropRect.Width > 0 && cropRect.Height > 0)
                //{
                //    Pen pen = new Pen(Color.Red, 1);
                //    g.DrawRectangle(pen, cropRect);
                //}

                //Pen pen = new Pen(Color.Red, 1);
                //Point Pos = new Point(MousePos.X - ImagePanel.AutoScrollPosition.X, MousePos.Y - ImagePanel.AutoScrollPosition.Y);
                //g.DrawLine(pen, Pos.X, 0, Pos.X, getHeight() * m_ViewScale / 100);
                //g.DrawLine(pen, 0, Pos.Y, getWidth() * m_ViewScale / 100, Pos.Y);

            if (1300 <= (m_CArtCam.GetDllVersion() & 0xFFFF))
            {
                int Exposure = m_CArtCam.GetRealExposureTime(); // (micro sec)
                float fExposure = (float)Exposure / 1000.0f;
                sbPanelExp.Text = "Exp: " + fExposure.ToString() + "msec";
            }
            else
            {
                sbPanelExp.Text = "";
            }

        }

        private void ImageBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void ArtCamCtrl_Resize(object sender, EventArgs e)
        {
            //ImagePanel.SetBounds(0, 0, 711, 585);
            //statusBar1.SetBounds(0, 715, 711, 30);
            ImagePanel.SetBounds(0, 0, this.ClientRectangle.Right, this.ClientRectangle.Bottom - 16);
            statusBar1.SetBounds(0, this.ClientRectangle.Bottom - 16, this.ClientRectangle.Right, 16);
        }

        private void DeviceChange(object sender, System.EventArgs e, int Number)
        {
            if (m_CArtCam.IsInit())
            {
                m_CArtCam.Close();
            }

            // To confirm whether the device is connected, use "GetDeviceName"
            // It can be found out easily with "GetDeviceName".
            // When area for obtain name is not secured, it results in error. Prepare alignment length of at least 32.
            StringBuilder Temp = new StringBuilder(256);
            if (0 == m_CArtCam.GetDeviceName(Number, Temp, 256))
            {
                m_PreviewMode = -1;
                m_SelectDevice = -1;
                m_StopFlg = false;
                menuStop.Text = "Pause";
                return;
            }


            // A device will be changed, if a camera is displayed after changing the number of a device now
            // Notes: A device is not changed in this function simple substance
            //   After calling this function, a device is changed by initializing a device
            m_SelectDevice = Number;
            m_CArtCam.SetDeviceNumber(Number);


            for (int i = 0; i < 8; i++)
            {
                menuDevice.MenuItems[i].Checked = false;
            }
            menuDevice.MenuItems[Number].Checked = true;


            switch (m_PreviewMode)
            {
                case 0: menuPreview_Click(sender, e); break;
                case 1: menuCallback_Click(sender, e); break;
                case 3: menuCapture_Click(sender, e); break;
                case 4: menuTrigger_Click(sender, e); break;
            }
        }

        private void menuDevice0_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 0); }
        private void menuDevice1_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 1); }
        private void menuDevice2_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 2); }
        private void menuDevice3_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 3); }
        private void menuDevice4_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 4); }
        private void menuDevice5_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 5); }
        private void menuDevice6_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 6); }
        private void menuDevice7_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 7); }
        private void menuDevice8_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 8); }
        private void menuDevice9_Click(object sender, System.EventArgs e) { DeviceChange(sender, e, 9); }

        // Capture
        private void menuCapture_Click(object sender, System.EventArgs e)
        {
            if (!m_CArtCam.IsInit())
            {
                MessageBox.Show("Select available device");
                return;
            }

            ImagePanel.Visible = true;
            timer1.Enabled = false;

            // Release device
            m_CArtCam.Close();

            // Creat area for capturing
            CreateBitmap();

            //ImageBox.SetBounds(0, 0, 960, 768);
            ImageBox.SetBounds(0, 0, getWidth(), getHeight());

            // Display image
            m_CArtCam.Capture();

            // Check menu
            menuPreview.Checked = false;
            menuCallback.Checked = false;
            menuCapture.Checked = true;
            menuTrigger.Checked = false;

            m_PreviewMode = 3;
            m_StopFlg = false;
            menuStop.Text = "Pause";

            ImageBox.Invalidate();

            timer1.Enabled = true;
            timer1.Interval = 200;
        }

        private void statusBar1_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {
            if (e.StatusBarPanel == sbPanelScale)
            {
                if (e.Button.Equals(System.Windows.Forms.MouseButtons.Left))
                {
                    switch (m_ViewScale)
                    {
                        case 33: m_ViewScale = 50; break;
                        case 50: m_ViewScale = 66; break;
                        case 83: m_ViewScale = 100; break;
                        //case 100: m_ViewScale = 83; break;
                        //case 200: m_ViewScale = 400; break;
                        default: m_ViewScale = 100; break;
                    }
                }
                else
                {
                    switch (m_ViewScale)
                    {
                        case 100: m_ViewScale = 83; break;
                        case 83: m_ViewScale = 66; break;
                        case 66: m_ViewScale = 50; break;
                        case 50: m_ViewScale = 33; break;
                        //case 200: m_ViewScale = 400; break;
                        default: m_ViewScale = 33; break;
                    }
                }
            }
            ImagePanel.AutoScrollPosition = new Point(0, 0);
            //ImageBox.SetBounds(0, 0, 960*m_ViewScale/100, 768*m_ViewScale/100, BoundsSpecified.All);
            ImageBox.SetBounds(0, 0, getWidth() * m_ViewScale / 100, getHeight() * m_ViewScale / 100, BoundsSpecified.All);
            sbPanelScale.Text = m_ViewScale.ToString() + "%";
            //add for preview
            menuCapture_Click(null, null);  
            //menuPreview_Click(sender, e);   

        }

        private void ImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                isDragging = true;
            }
        }

        private void ImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                //label1.Text = "X = " + e.X + "," + "Y = " + e.Y;
                Point endPoint = e.Location;
                cropRect = new Rectangle(
                    Math.Min(startPoint.X, endPoint.X),
                    Math.Min(startPoint.Y, endPoint.Y),
                    Math.Abs(startPoint.X - endPoint.X),
                    Math.Abs(startPoint.Y - endPoint.Y));
                ImageBox.Invalidate();
            }
        }

        private void ImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;

            }
        }

        private Image originalImage;
        public float zoomFactor = 1.0f;

        Bitmap resizedImage;


        public void UpdatePreview()
        {
            ImagePanel.AutoScrollPosition = new Point(0, 0);
            ImageBox.SetBounds(0, 0, getWidth() * m_ViewScale / 100, getHeight() * m_ViewScale / 100, BoundsSpecified.All);
            sbPanelScale.Text = m_ViewScale.ToString() + "%";
            //add for preview
            menuCapture_Click(null, null);
        }
    }
}
