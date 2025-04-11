using CTD;
using HalconDotNet;
using IP3000.ARTCamera;
using IP3000.IP3000;
using IP3000.Recipe;
using IP3000.Utilities;
using IP3000_Control.ARTCamera;
using IP3000_Control.DL;
using IP3000_Control.IP3000;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using XlApp = Microsoft.Office.Interop.Excel.Application;
using XlRang = Microsoft.Office.Interop.Excel.Range;
using XlWorkBook = Microsoft.Office.Interop.Excel._Workbook;
using XlWorkSheet = Microsoft.Office.Interop.Excel._Worksheet;

namespace IP3000_Control
{
    public partial class MainForm : Form
    {
        // define定義
        public const int MAX_AXIS = 3;          // 最大軸数
        public const short AXIS_1 = 0;          // 1軸目
        public const short AXIS_2 = 1;          // 2軸目
        public const short AXIS_3 = 2;          // 3軸目
        public const short AXIS_4 = 3;          // 4軸目

        // グローバル変数の宣言
        //public static short gwBsn = 0x100;						// USPG-28 BSN は 100h ～ 10Fh
        public static short gwBsn = 0x0;                            // USPG-48(H) BSN は 0h ～ Fh
        public static int[] gintEndReq = new int[MAX_AXIS];
        public static byte[] gbytUniversal = new byte[MAX_AXIS]; // 終了リクエスト
        public static byte[] gbytBusy = new byte[MAX_AXIS];         // ドライブ状態	0:停止中 / 1:動作中
        public static byte[] gbytEndStatus = new byte[MAX_AXIS];    // End Status
        public static int gintRepeatMax = 0;                        // 繰り返し最大数
        public static int[] gintRepeatCount = new int[MAX_AXIS];    // 繰り返し回数（今の値)
        public static int[] gintPauseEnd = new int[MAX_AXIS];       // ディレイ時間フラグ
        public static string gstrFileName = "";                     // ファイル名

        static public int[] gintPhase1 = new int[MAX_AXIS];// 処理番号
        static public int[] gintPhase2 = new int[MAX_AXIS];
        static public int[] gintPhase3 = new int[MAX_AXIS];

        // その他パラメータ
        static public byte[] gbytMode1 = new byte[MAX_AXIS];        // MODE 1
        static public byte[] gbytMode2 = new byte[MAX_AXIS];        // MODE 2
        static public byte setMode1 = 0x064;        // MODE 1
        static public byte setMode2 = 0x030;        // MODE 2
        static public short[] gshtSLMEnable = new short[MAX_AXIS];  // 減速停止リミット有効
        static public short[] gshtELMEnable = new short[MAX_AXIS];  // 急停止リミット有効
        static public short[] gshtALMEnable = new short[MAX_AXIS];  // アラームリミット有効

        short wBsn = 0;
        short wAxis;
        short wAxis1;
        short wAxis2;
        short wAxis3;
        short wAxis4;

        int internalcount1 = 0;
        int internalcount2 = 0;
        int internalcount3 = 0;

        public int internalcount_Axis3 = 0;

        SpeedSetting speedSetting = new SpeedSetting();
        public short speed;
        private bool isMachineHomed = false;

        enum Writer : int
        {
            No = 0,
            Block,
            Component,
            Pin,
            Side,
            X_axis,
            Y_axis,
            R_axis,
            Camera,
            Zoom,
            Exposure,
            TriggerDelay,
            Result
        }

        //Deep Learning
        private Size prevsize = new Size();
        System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();

        private bool triggerAOI = false;
        private string curProductName;
        private string curSerialNo;

        private IP3000Results ip3000Result = null;
        private ArtCam2CamFrm artCam2CamFrm = null;

        private int firstPosOffset = 5;
        private int indexAOI = 0;

        private string baseLogPath = AppDomain.CurrentDomain.BaseDirectory + "LogFiles";
        private string baseImagePath = AppDomain.CurrentDomain.BaseDirectory + "LogImages";
        private string imgFolder = string.Empty;
        private string curAoiImageName = string.Empty;
        private HImage hImage = null;

        private ArtCamFullCtrlFrm topCam = null;
        private ArtCamFullCtrlFrm sideCam = null;

        private ArtCamCtrl _topCamCtrl = null;
        private ArtCamCtrl _sideCamCtrl = null;

        private double zoomFactor = 1;
        private int exposure = 50;
        private bool trigExposure = false;

        private int activeStep = 0;
        private int viewScale = 100;
        private int camNo = 0;

        public enum ActiveCamera
        {
            TOP,
            SIDE
        }

        private ActiveCamera activeCamera = ActiveCamera.SIDE;
        private int triggerDelay = 500;
        private ProductSide curProductSide = ProductSide.Bottom;

        public enum RunMode
        {
            Production,
            GRR,
            MasterCheck,
        }

        private XlApp oXL = null;
        private XlWorkBook oWB = null;
        private XlWorkSheet oSheet = null;
        private XlRang oRng = null;

        private double[] listConfinence = null;
        private int roundGrr = 0;
        private int totalRoundGrr = 9;
        private RunMode runMode = RunMode.Production;

        private bool Status1 = true;
        private bool Status2 = true;
        private int PassCounter1 = 0;
        int PassCounter2 = 0;
        private int Counter = 0;

        private string inspectionResult = "Pass";

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetControlArrayByName(Form frm, string name)
        {
            System.Collections.ArrayList ctrs = new System.Collections.ArrayList();
            object obj;
            for (int i = 1; (obj = FindControlByFieldName(frm, name + i.ToString())) != null; i++)
                ctrs.Add(obj);
            if (ctrs.Count == 0) return null;
            else return ctrs.ToArray(ctrs[0].GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object FindControlByFieldName(Form frm, string name)
        {
            System.Type t = frm.GetType();

            System.Reflection.FieldInfo fi = t.GetField(
                name,
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.DeclaredOnly);

            if (fi == null)
                return null;

            return fi.GetValue(frm);
        }

        #region backup
        // グローバル変数の宣言
        //public static short gwBsn = 0x100;						// USPG-28 BSN は 100h ～ 10Fh
        //public static short gwBsn = 0x0;                            // USPG-48(H) BSN は 0h ～ Fh
        //public static int[] gintEndReq = new int[MAX_AXIS];
        //public static byte[] gbytUniversal = new byte[MAX_AXIS]; // 終了リクエスト
        //public static byte[] gbytBusy = new byte[MAX_AXIS];         // ドライブ状態	0:停止中 / 1:動作中
        //public static byte[] gbytEndStatus = new byte[MAX_AXIS];    // End Status
        //public static int gintRepeatMax = 0;                        // 繰り返し最大数
        //public static int[] gintRepeatCount = new int[MAX_AXIS];    // 繰り返し回数（今の値)
        //public static int[] gintPauseEnd = new int[MAX_AXIS];       // ディレイ時間フラグ
        //public static string gstrFileName = "";                     // ファイル名

        // その他パラメータ
        //static public byte[] gbytMode1 = new byte[MAX_AXIS];        // MODE 1
        //static public byte[] gbytMode2 = new byte[MAX_AXIS];        // MODE 2
        //static public byte setMode1 = 0x064;        // MODE 1
        //static public byte setMode2 = 0x030;        // MODE 2
        //static public short[] gshtSLMEnable = new short[MAX_AXIS];  // 減速停止リミット有効
        //static public short[] gshtELMEnable = new short[MAX_AXIS];  // 急停止リミット有効
        //static public short[] gshtALMEnable = new short[MAX_AXIS];  // アラームリミット有効

        //SpeedSetting speedSetting = new SpeedSetting();

        //short wBsn = 0;
        //short wAxis;
        //short wAxis1;
        //short wAxis2;
        //short wAxis3;
        //short wAxis4;
        //short[] wAxisArr = new short[4];
        //public int internalcount_Axis3 = 0;
        //Motor motor = new Motor();
        #endregion

        static public int[] gintPhase = new int[MAX_AXIS];// 処理番号

        // 速度パラメータ (0:通常 / 1:原点復帰)
        static public double[,] gdblLowSpeed = new double[MAX_AXIS, 3]; // 自起動速度
        static public double[,] gdblHiSpeed = new double[MAX_AXIS, 3];  // 最高速度
        static public short[,] gshtAccTime = new short[MAX_AXIS, 3];        // 加減速時間
        static public double[,] gdblSRate = new double[MAX_AXIS, 3];        // S字比率

        int[] internalcount = new int[4];

        int countindex = 0;
        int pulseBoardOut = 0;

        short btnStartStatus = 0;
        string DataReceive = "Stop";

        enum StartStatus
        {
            Stop,
            Start,
        }

        private ListRecipe listRecipe = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HNet_Load(object sender, EventArgs e)
        {
            this.MouseWheel += WindowControl.HSmartWindowControl_MouseWheel;
            prevsize.Width = Width;
            prevsize.Height = Height;
            //HOperatorSet.SetCurrentDir(@"D:\IP3000AOI\bin");
            HOperatorSet.SetCurrentDir(AppDomain.CurrentDomain.BaseDirectory);
            ShowDeviceList();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateProductRecipe()
        {
            //AC1200 NG
            //Create Bottom Side Recipe
            OneSideRecipe ac1200NGBottomRecipe = new OneSideRecipe()
            {
                Name = "AC1200NG Bottom",
                Side = ProductSide.Bottom,
                PathGoodImage = "D:\\DATA\\AC1200 BOTTOM J1 J2",
                PathFailImage = "D:\\DATAFAIL\\AC1200 Bottom J1 J2",
                DLProgramName = "D:\\IP3000\\Anomaly\\bsd00_spvision.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200NG Bottom.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                }
            };
            //Create PCBA Side Recipe
            OneSideRecipe ac1200NGPCBARecipe = new OneSideRecipe()
            {
                Name = "AC1200NG PCBA",
                Side = ProductSide.PCBA,
                PathGoodImage = "D:\\DATA\\AC1200 PCBA CONN CAPA",
                PathFailImage = "D:\\DATAFAIL\\AC1200 PCBA CONN CAPA",
                DLProgramName = "D:\\IP3000\\Anomaly\\pcbd00_spvision.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200NG PCBA.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos30", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 53000 },
                    new PointRecipe() { PosName = "Pos31", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos32", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos33", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                    new PointRecipe() { PosName = "Pos34", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 530000 },
                }
            };
            //Create Top Side Recipe
            OneSideRecipe ac1200NGTopRecipe = new OneSideRecipe()
            {
                Name = "AC1200NG Heatsink",
                Side = ProductSide.Top,
                PathGoodImage = "D:\\DATA\\AC1200_Heat Sink",
                PathFailImage = "D:\\DATAFAIL\\AC1200_Heat Sink",
                DLProgramName = "D:\\IP3000\\Anomaly\\hsd00_spvision.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200NG Heat Sink.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 }
                }
            };

            //AC1200
            //Create Bottom Side -12 No heatsink
            OneSideRecipe ac1200BottomRecipe = new OneSideRecipe()
            {
                Name = "AC1200 Bottom",
                Side = ProductSide.Bottom,
                PathGoodImage = "D:\\DATA\\AC1200 Bottom J1 J2",
                PathFailImage = "D:\\DATAFAIL\\AC1200 Bottom J1 J2",
                DLProgramName = "D:\\IP3000\\Anomaly\\ac1200_bottomside.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200 Bottom.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                }
            };
            //Create PCBA Side Recipe
            OneSideRecipe ac1200PCBARecipe = new OneSideRecipe()
            {
                Name = "AC1200 PCBA",
                Side = ProductSide.Bottom,
                PathGoodImage = "D:\\DATA\\AC1200 PCBA CON CAPA",
                PathFailImage = "D:\\DATAFAIL\\AC1200 PCBA CON CAPA",
                DLProgramName = "D:\\IP3000\\Anomaly\\ac1200_pcba.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200 PCBA.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos30", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos31", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos32", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos33", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos34", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                }
            };
            //Create Top Side Recipe
            OneSideRecipe ac1200TopRecipe = new OneSideRecipe()
            {
                Name = "AC1200 Top",
                Side = ProductSide.Top,
                PathGoodImage = "D:\\DATA\\AC1200 Top",
                PathFailImage = "D:\\DATAFAIL\\AC1200 Top",
                DLProgramName = "D:\\IP3000\\Anomaly\\ac1200_topside.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\AC1200 Top.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 }
                }
            };

            //Tigris
            //Create Bottom Side No heatsink
            OneSideRecipe TigrisBottomRecipe = new OneSideRecipe()
            {
                Name = "Tigris Bottom",
                Side = ProductSide.Bottom,
                PathGoodImage = "D:\\DATA\\AC1200 Bottom J1 J2",
                PathFailImage = "D:\\DATAFAIL\\AC1200 Bottom J1 J2",
                DLProgramName = "D:\\IP3000\\Anomaly\\tigris_bottomside.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\Tigris Bottom.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                }
            };
            //Create PCBA Side Recipe
            OneSideRecipe TigrisPCBARecipe = new OneSideRecipe()
            {
                Name = "Tigris PCBA",
                Side = ProductSide.Bottom,
                PathGoodImage = "D:\\DATA\\AC1200 PCBA CON CAPA",
                PathFailImage = "D:\\DATAFAIL\\AC1200 PCBA CON CAPA",
                DLProgramName = "D:\\IP3000\\Anomaly\\tigris_pcba.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\Tigris PCBA.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos12", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos13", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos14", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos15", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos16", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos17", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos18", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos19", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos20", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos21", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos22", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos23", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos24", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos25", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos26", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos27", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos28", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos29", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos30", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos31", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos32", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos33", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos34", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                }
            };
            //Create Top Side Recipe
            OneSideRecipe TigrisTopRecipe = new OneSideRecipe()
            {
                Name = "Tigris Top",
                Side = ProductSide.Top,
                PathGoodImage = "D:\\DATA\\AC1200 Top",
                PathFailImage = "D:\\DATAFAIL\\AC1200 Top",
                DLProgramName = "D:\\IP3000\\Anomaly\\tigris_topside.hdl",
                MotionPrgName = "D:\\IP3000\\Program\\Tigris Top.prg",
                ListPointRecipe = new List<PointRecipe>()
                {
                    new PointRecipe() { PosName = "Pos1", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos2", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos3", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos4", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos5", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos6", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos7", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos8", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos9", ClsThreshold = 0.3, SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos10", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 },
                    new PointRecipe() { PosName = "Pos11", ClsThreshold = 0.3,SegmentThreshold = 0.67, AreaThreshold = 30000 }
                }
            };

            listRecipe = new ListRecipe();
            listRecipe.ProductID = 1;
            listRecipe.ProductName = "AC1200";
            listRecipe.Customer = "Acacia";
            listRecipe.OneSideRecipes.Add(ac1200NGBottomRecipe);
            listRecipe.OneSideRecipes.Add(ac1200NGPCBARecipe);
            listRecipe.OneSideRecipes.Add(ac1200NGTopRecipe);
            listRecipe.OneSideRecipes.Add(ac1200BottomRecipe);
            listRecipe.OneSideRecipes.Add(ac1200PCBARecipe);
            listRecipe.OneSideRecipes.Add(ac1200TopRecipe);
            listRecipe.OneSideRecipes.Add(TigrisBottomRecipe);
            listRecipe.OneSideRecipes.Add(TigrisPCBARecipe);
            listRecipe.OneSideRecipes.Add(TigrisTopRecipe);

            pgProductRecipe.SelectedObject = listRecipe;
            pgProductRecipe.ExpandAllGridItems();

            if (!File.Exists("ProductRecipe.xml"))
            {
                using (var writer = new FileStream("ProductRecipe.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ListRecipe));
                    serializer.Serialize(writer, listRecipe);
                }
            }
            else
            {
                const string FILENAME = "ProductRecipe.xml";
                string xml = File.ReadAllText(FILENAME);

                XmlSerializer ser = new XmlSerializer(typeof(ListRecipe));
                StringReader rdr = new StringReader(xml);
                //ListRecipe members = (ListRecipe)ser.Deserialize(rdr);
                listRecipe = (ListRecipe)ser.Deserialize(rdr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            groupBox10.Visible = false;
            groupBox12.Visible = false;
            groupBox13.Visible = false;
            groupBox14.Visible = false;

            runMode = RunMode.Production;
            lbRunMode.Text = "Mode: " + runMode.ToString();

            CreateProductRecipe();
            cbRecipeList.Items.Clear(); 
            foreach(OneSideRecipe oneSideRecipe in listRecipe.OneSideRecipes)
            {
                cbRecipeList.Items.Add(oneSideRecipe.Name);
            }
            cbRecipeList.SelectedIndex = 0;

            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr;

            isMachineHomed = false;
            InitBottomMotionAndDL();
            RunFirstDL();

            cbInspSide.Items.Clear();
            cbInspSide.Items.AddRange(new string[] { "Side A", "Side B" });
            cbInspSide.SelectedIndex = 0;

            cbInspCamera.Items.Clear();
            cbInspCamera.Items.AddRange(new string[] { "Top", "Angle" });
            cbInspCamera.SelectedIndex = 0;

            cbBlockNo.Items.Clear();
            cbBlockNo.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            cbBlockNo.SelectedIndex = 0;

            cbPin.Items.Clear();
            cbPin.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            cbPin.SelectedIndex = 0;

            cbZoom.Items.Clear();
            cbZoom.Items.AddRange(new string[] { "33%", "50%", "66%", "83%", "100%" });
            cbZoom.SelectedIndex = 0;

            //Get Avatar
            Avatar(sender, e);

            short i;
            try
            {
                KeyPreview = true;

                Utils.CreateSettingDir();
                if (CTDw.CTDwDllOpen() == 0)
                {
                    MessageBox.Show("Dll Open error!", "CTD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (CTDw.CTDwCreate(wBsn) == 0)
                {
                    MessageBox.Show("Create error \nPlease Check Machine and USB", "CTD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // パラメータ初期化
                for (i = 0; i < MAX_AXIS; i++)
                {
                    //MODE setting
                    gbytMode1[i] = 0x64;  // MODE 1
                    gbytMode2[i] = 0x30;  // MODE 2
                    gshtSLMEnable[i] = 1; // 減速停止リミット有効
                    gshtELMEnable[i] = 1; // 急停止リミット有効
                    gshtALMEnable[i] = 1; // アラームリミット無効

                    //Speed Setting
                    // 速度パラメータ (0:通常 / 1:原点復帰)
                    //gdblLowSpeed[i, 0] = 1000;    // 自起動速度
                    gdblLowSpeed[i, 0] = 5000;    // 自起動速度
                    gdblHiSpeed[i, 0] = 20000;    // 最高速度
                    gshtAccTime[i, 0] = 100;  // 加減速時間
                    gdblSRate[i, 0] = -1;     // S字比率

                    // 速度パラメータ (0:通常 / 1:原点復帰)
                    //gdblLowSpeed[i, 1] = 100; // 自起動速度
                    gdblLowSpeed[i, 1] = 2000; // 自起動速度
                    gdblHiSpeed[i, 1] = 20000; // 最高速度
                    gshtAccTime[i, 1] = 100;  // 加減速時間
                    gdblSRate[i, 1] = -1;     // S字比率

                    //gdblLowSpeed[i, 2] = 100; // 自起動速度
                    gdblLowSpeed[i, 2] = 2000; // 自起動速度
                    gdblHiSpeed[i, 2] = 50000; // 最高速度
                    gshtAccTime[i, 2] = 500;  // 加減速時間
                    gdblSRate[i, 2] = 100;     // S字比率
                }

                // その他初期設定
                for (i = 0; i < MAX_AXIS; i++)
                {
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_EMERGENCY_STOP);                       // とりあえず急停止
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwMode2Write(gwBsn, i, gbytMode2[i]);
                    CTD.CTDw.CTDwSpeedParameterWrite(gwBsn, i, gdblLowSpeed[i, 2], gdblHiSpeed[i, 2], gshtAccTime[i, 2], gdblSRate[i, 2]);
                }

                //CTD.CTDw.CTDwMode1Write(gwBsn48, AXIS_4, 0x64);
                //CTD.CTDw.CTDwMode2Write(gwBsn48, AXIS_4, 0x30);
                //CTD.CTDw.CTDwUniversalSignalWrite(gwBsn48, AXIS_4, 0x01);

                byte bytTemp = 0;
                CTD.CTDw.CTDwGetUniversalSignal(gwBsn, AXIS_4, ref bytTemp);
                bytTemp |= 0x1;
                CTD.CTDw.CTDwUniversalSignalWrite(gwBsn, AXIS_4, bytTemp);

                //OriginSetting();
                speed = 1;
                btnSlow.BackColor = Color.Gray;
                btnMeduim.BackColor = Color.LimeGreen;
                btnFast.BackColor = Color.Gray;
            }
            catch { }

            //InitializeTopCamera();
            //InitializeSideCamera();

            _topCamCtrl = new ArtCamCtrl("Top Camera");
            panelTopCam.Controls.Clear();
            panelTopCam.Controls.Add(_topCamCtrl);
            _topCamCtrl.SetPreviewCamera(0);
            _topCamCtrl.Dock = DockStyle.Fill;
            _topCamCtrl.m_CArtCam.SetCaptureWindow(960, 768, 15);
            _topCamCtrl.m_ViewScale = 50;
            _topCamCtrl.UpdatePreview();

            _topCamCtrl.m_CArtCam.SetSharpness(15);
            _topCamCtrl.m_CArtCam.SetGamma(70);
            _topCamCtrl.m_CArtCam.SetGlobalGain(40);
            _topCamCtrl.m_CArtCam.SetGrayGainG1(25);
            _topCamCtrl.m_CArtCam.SetGrayGainG2(25);
            _topCamCtrl.m_CArtCam.SetGrayGainR(34);
            _topCamCtrl.m_CArtCam.SetGrayGainB(26);
            _topCamCtrl.m_CArtCam.SetBayerGainAuto(false);

            Thread.Sleep(300);

            _sideCamCtrl = new ArtCamCtrl("Side Camera");
            panelSideCam.Controls.Clear();
            panelSideCam.Controls.Add(_sideCamCtrl);
            _sideCamCtrl.SetPreviewCamera(1);
            _sideCamCtrl.Dock = DockStyle.Fill;
            _sideCamCtrl.m_CArtCam.SetCaptureWindow(960, 768, 15);
            _sideCamCtrl.m_ViewScale = 50;
            _sideCamCtrl.UpdatePreview();

            _sideCamCtrl.m_CArtCam.SetSharpness(15);
            _sideCamCtrl.m_CArtCam.SetGamma(64);
            _sideCamCtrl.m_CArtCam.SetGlobalGain(40);
            _sideCamCtrl.m_CArtCam.SetGrayGainG1(32);
            _sideCamCtrl.m_CArtCam.SetGrayGainG2(32);
            _sideCamCtrl.m_CArtCam.SetGrayGainR(46);
            _sideCamCtrl.m_CArtCam.SetGrayGainB(31);
            _sideCamCtrl.m_CArtCam.SetBayerGainAuto(false);

            curProductName = tbProgramName.Text;

            //string dtStr = DateTime.Now.ToString("yyyyMMdd");
            ////string dtFldName = baseImagePath + "\\" + dtStr;
            //string dtFldName = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }

            imgFolder = dtFldName + "\\" + curSerialNo;
            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBottomMotionAndDL()
        {
            //Load motion program
            LoadProgram("D:\\IP3000\\Program\\AC1200NG Bottom.prg");
            //Deep Learning 
            CenterMethod.GetComputeInfo();
            HNet_Load(null, null);

            //Load DL model and run first inspection
            Cursor = Cursors.WaitCursor;
            DoInitialDLBottom();
            Cursor = Cursors.Default;
            curProductSide = ProductSide.Bottom;
            lbInspSide.Text = "Side: " + curProductSide.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitHeatsinkMotionAndDL()
        {
            //Load motion program
            LoadProgram("D:\\IP3000\\Program\\AC1200NG Heat Sink.prg");
            //Deep Learning 
            CenterMethod.GetComputeInfo();
            HNet_Load(null, null);

            //Load DL model and run first inspection
            Cursor = Cursors.WaitCursor;
            DoInitialDLHeatsink();
            Cursor = Cursors.Default;
            curProductSide = ProductSide.Top;
            lbInspSide.Text = "Side: " + curProductSide.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPCBAMotionAndDL()
        {
            //Load motion program
            LoadProgram("D:\\IP3000\\Program\\AC1200NG PCBA.prg");
            //Deep Learning 
            CenterMethod.GetComputeInfo();
            HNet_Load(null, null);

            //Load DL model and run first inspection
            Cursor = Cursors.WaitCursor;
            DoInitialDLPCBA();
            Cursor = Cursors.Default;
            curProductSide = ProductSide.PCBA;
            lbInspSide.Text = "Side: " + curProductSide.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunFirstDL()
        {
            HImage Image = new HImage();
            HTuple DLResults;

            Image.ReadImage(AppDomain.CurrentDomain.BaseDirectory + "SampleImages\\" + "SampleThread.jpg");
            DoPrepareImage(Image);
            DoInspection(Image, out DLResults);
            ShowImage(Image);
            ShowHeatmapImage(Image, DLResults);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeTopCamera()
        {
            if (topCam == null)
            {
                topCam = new ArtCamFullCtrlFrm();
                topCam.Show();
                topCam.SetPreviewCamera(0);
                topCam.SetTitle("Top Camera");
                topCam.FormClosing += TopCam_FormClosing;
                //topCam.m_CArtCam.SetCaptureWindow(960, 768, 12);
            }
        }

        private void TopCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            topCam = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeSideCamera()
        {
            if (sideCam == null)
            {
                sideCam = new ArtCamFullCtrlFrm();
                sideCam.Show();
                sideCam.SetPreviewCamera(1);
                sideCam.SetTitle("Side Camera");
                sideCam.FormClosing += OnSideCamFormClosing;
            }
        }

        private void OnSideCamFormClosing(object sender, FormClosingEventArgs e)
        {
            sideCam = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OriginSetting()
        {
            short i = AXIS_1;
            short j = AXIS_2;
            short k = AXIS_3;

            gbytMode1[i] = 0x064;
            gbytMode2[i] = 0x030;


            gbytMode1[j] = 0x064;
            gbytMode2[j] = 0x030;

            gbytMode1[k] = 0x064;
            gbytMode2[k] = 0x030;

            Org000Exec_1(i);
            Org000Exec_2(j);
            Org000Exec_3(k);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbComponentName.Text != "")
            {
                string[] list = new string[listView_Model.Columns.Count];

                list[(int)Writer.No] = (listView_Model.Items.Count + 1).ToString();
                list[(int)Writer.Block] = cbBlockNo.GetItemText(cbBlockNo.SelectedItem);
                list[(int)Writer.Component] = tbComponentName.Text;
                list[(int)Writer.Pin] = cbPin.GetItemText(cbPin.SelectedItem);
                list[(int)Writer.Side] = cbInspSide.GetItemText(cbInspSide.SelectedItem);
                list[(int)Writer.X_axis] = txtCounter1.Text;
                list[(int)Writer.Y_axis] = txtCounter2.Text;
                list[(int)Writer.R_axis] = txtCounter3.Text;
                list[(int)Writer.Camera] = cbInspCamera.GetItemText(cbInspCamera.SelectedItem);
                list[(int)Writer.Zoom] = cbZoom.GetItemText(cbZoom.SelectedItem);
                list[(int)Writer.Exposure] = nCamExposure.Value.ToString();
                list[(int)Writer.TriggerDelay] =
                list[(int)Writer.Result] = "Idle";

                ListViewItem m_viewItem = new ListViewItem(list);
                listView_Model.Items.Add(m_viewItem);
            }
            else
            {
                MessageBox.Show("Point's Name is Empty");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tbComponentName.Text != "")
            {
                foreach (ListViewItem item in listView_Model.SelectedItems)
                {
                    int index = item.Index;

                    listView_Model.Items.RemoveAt(item.Index);

                    string[] list = new string[listView_Model.Columns.Count];

                    list[(int)Writer.No] = (index + 1).ToString();
                    list[(int)Writer.Block] = cbBlockNo.GetItemText(cbBlockNo.SelectedItem);
                    list[(int)Writer.Component] = tbComponentName.Text;
                    list[(int)Writer.Pin] = cbPin.GetItemText(cbPin.SelectedItem);
                    list[(int)Writer.Side] = cbInspSide.GetItemText(cbInspSide.SelectedItem);
                    list[(int)Writer.X_axis] = txtCounter1.Text;
                    list[(int)Writer.Y_axis] = txtCounter2.Text;
                    list[(int)Writer.R_axis] = txtCounter3.Text;
                    list[(int)Writer.Camera] = cbInspCamera.GetItemText(cbInspCamera.SelectedItem);
                    list[(int)Writer.Zoom] = cbZoom.GetItemText(cbZoom.SelectedItem);
                    list[(int)Writer.Exposure] = nCamExposure.Value.ToString();
                    list[(int)Writer.TriggerDelay] = triggerDelay.ToString();
                    list[(int)Writer.Result] = "Idle";

                    ListViewItem m_viewItem = new ListViewItem(list);
                    listView_Model.Items.Insert(index, m_viewItem);
                }
            }
            else
            {
                MessageBox.Show("Point's Name is Empty");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_Model.SelectedItems)
            {
                listView_Model.Items.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_Model.SelectedItems)
            {
                int index = item.Index;

                string[] list = new string[listView_Model.Columns.Count];

                list[(int)Writer.No] = (index + 1).ToString();
                list[(int)Writer.Block] = cbBlockNo.GetItemText(cbBlockNo.SelectedItem);
                list[(int)Writer.Component] = tbComponentName.Text;
                list[(int)Writer.Pin] = cbPin.GetItemText(cbPin.SelectedItem);
                list[(int)Writer.Side] = cbInspSide.GetItemText(cbInspSide.SelectedItem);
                list[(int)Writer.X_axis] = txtCounter1.Text;
                list[(int)Writer.Y_axis] = txtCounter2.Text;
                list[(int)Writer.R_axis] = txtCounter3.Text;
                list[(int)Writer.Camera] = cbInspCamera.GetItemText(cbInspCamera.SelectedItem);
                list[(int)Writer.Zoom] = cbZoom.GetItemText(cbZoom.SelectedItem);
                list[(int)Writer.Exposure] = nCamExposure.Value.ToString();
                list[(int)Writer.TriggerDelay] = triggerDelay.ToString();
                list[(int)Writer.Result] = "Idle";

                ListViewItem m_viewItem = new ListViewItem(list);
                listView_Model.Items.Insert(item.Index, m_viewItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            listView_Model.Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isMachineHomed)
            {
                MessageBox.Show("Machine is not home. Please push origin button to home", "Machine Warning");
                return;
            }

            if (tbProgramName.Text == string.Empty)
            {
                MessageBox.Show("Please load program before start run");
                return;
            }

            if (!GetProductSerialNo())
            {
                return;
            }
            else
            {
                curProductName = tbProgramName.Text;

                string dtStr = DateTime.Now.ToString("yyyyMMdd");
                string dtFldName = baseImagePath + "\\" + dtStr;
                if (!Directory.Exists(dtFldName))
                {
                    Directory.CreateDirectory(dtFldName);
                }

                imgFolder = dtFldName + "\\" + curSerialNo;
                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }

                tbCurSerialNo.Text = curSerialNo;
                //ip3000Result = new IP3000Results(curProductName, curSerialNo);   
                ip3000Result = new IP3000Results() { ProductName = curProductName, SerialNo = curSerialNo };
                ip3000Result.BeginTime = DateTime.Now;
                StartProgram();
                ip3000Result.EndTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool GetProductSerialNo()
        {
            try
            {
                SNInputFrm sNInputFrm = new SNInputFrm();
                sNInputFrm.ShowDialog();
                if (sNInputFrm != null)
                {
                    if (sNInputFrm.SerialNo != string.Empty && sNInputFrm.SerialNo.Length == 9)
                    {
                        curSerialNo = sNInputFrm.SerialNo;
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartProgram()
        {
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                if (listView_Model.Items.Count > 0)
                {
                    for (int i = 0; i < listView_Model.Items.Count; i++)
                    {
                        listView_Model.Items[i].SubItems[(int)Writer.Result].Text = "Idle";
                    }
                    foreach (ListViewItem item in listView_Model.SelectedItems)
                    {
                        listView_Model.Items[item.Index].Selected = false;
                    }
                }
            }

            
            countindex = 0;
            indexAOI = 0;
            PassCounter1 = 0;
            PassCounter2 = 0;
            Counter = listView_Model.Items.Count - 1;
            inspectionResult = "Pass"; 

            isMachineHomed = false;
            DataReceive = "Run";
            timerDataReceive.Enabled = true;
            Thread.Sleep(50);

            btnStartStatus = 1;
            timerStart.Enabled = true;

            #region first loop run program
            //if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            //{
            //    if (listView_Model.Items.Count > 0)
            //    {
            //        for (int i = 0; i < listView_Model.Items.Count; i++)
            //        {
            //            listView_Model.Items[i].SubItems[(int)Writer.Result].Text = "-";
            //        }
            //        foreach (ListViewItem item in listView_Model.SelectedItems)
            //        {
            //            listView_Model.Items[item.Index].Selected = false;
            //        }

            //        countindex = 0;
            //        indexAOI = 0;
            //        listView_Model.Items[countindex].Selected = true;

            //        triggerDelay = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.TriggerDelay].Text);
            //        exposure = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Exposure].Text);
            //        if (activeCamera == ActiveCamera.TOP)
            //        {
            //            if (_topCamCtrl != null)
            //                _topCamCtrl.m_CArtCam.SetExposureTime(exposure);
            //        }
            //        else
            //        {
            //            if (_sideCamCtrl != null)
            //                _sideCamCtrl.m_CArtCam.SetExposureTime(exposure);
            //        }
            //        MoveAll_Axis(countindex);
            //        listView_Model.Items[countindex].SubItems[(int)Writer.Result].Text = "OK";
            //        DataReceive = "Run";
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please Load Program");
            //    }
            //}
            //else
            //{
            //    DataReceive = "Stop";
            //    timerDataReceive.Enabled = false;
            //    btnStartStatus = 0;
            //    countindex = 0;
            //    indexAOI = 0;
            //}
            #endregion 
        }

        private void GenImageNamebyPosition()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(GenImageNamebyPosition));
                return;
            }

            string strCAM = listView_Model.Items[countindex].SubItems[(int)Writer.Camera].Text;
            int camNo = strCAM.ToUpper() == "TOP" ? 0 : 1;
            int block = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Block].Text);
            int pin = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Pin].Text);
            string imgPatternName = curSerialNo + "-" + camNo.ToString() + "-{" + block.ToString() + "_-" + pin.ToString() + "-1_-" + (countindex + 1).ToString() + "}.jpg";
            string dtStr = DateTime.Now.ToString("yyyyMMdd");

            //imgFolder = listRecipe.OneSideRecipes[cbRecipeList.SelectedIndex].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;

            //string dtFldName = string.Empty;
            //if (curProductSide == ProductSide.Bottom)
            //{
            //    imgFolder = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            //}
            //else if (curProductSide == ProductSide.PCBA)
            //{
            //    imgFolder = listRecipe.OneSideRecipes[1].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            //}
            //else if (curProductSide == ProductSide.Top)
            //{
            //    imgFolder = listRecipe.OneSideRecipes[2].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            //}

            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }

            //imgFolder = dtFldName + "\\" + dtStr;
            curAoiImageName = imgFolder + "\\" + imgPatternName;
            viewScale = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Zoom].Text.Substring(0, listView_Model.Items[countindex].SubItems[(int)Writer.Zoom].Text.Length - 1));
        }

        //private string curAOIFileName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepNo"></param>
        private void AcquireImage()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(GenImageNamebyPosition));
                return;
            }

            string strCAM = listView_Model.Items[countindex].SubItems[(int)Writer.Camera].Text;
            int camNo = strCAM.ToUpper() == "TOP" ? 0 : 1;
            int block = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Block].Text);
            int pin = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Pin].Text);
            string imgPatternName = curSerialNo + "-" + camNo.ToString() + "-{" + block.ToString() + "_-" + pin.ToString() + "-1_-" + (countindex + 1).ToString() + "}.jpg";
            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            //string dtFldName = string.Empty;
            if (curProductSide == ProductSide.Bottom)
            {
                imgFolder = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            }
            else if (curProductSide == ProductSide.PCBA)
            {
                imgFolder = listRecipe.OneSideRecipes[1].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            }
            else if (curProductSide == ProductSide.Top)
            {
                imgFolder = listRecipe.OneSideRecipes[2].PathGoodImage + "\\" + dtStr + "\\" + curSerialNo;
            }

            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }

            curAoiImageName = imgFolder + "\\" + imgPatternName;
            viewScale = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Zoom].Text.Substring(0, listView_Model.Items[countindex].SubItems[(int)Writer.Zoom].Text.Length - 1));

            if (camNo == 0)
            {
                if (topCam != null)
                {
                    topCam.m_ViewScale = viewScale;
                    topCam.AcquireImage(curAoiImageName);
                }

                if (_topCamCtrl != null)
                {
                    _topCamCtrl.m_ViewScale = viewScale;
                    _topCamCtrl.AcquireImage(triggerDelay, curAoiImageName);
                    _topCamCtrl.Update();
                    hImage = _topCamCtrl.hImage;
                }
            }
            else
            {
                if (sideCam != null)
                {

                    sideCam.m_ViewScale = viewScale;
                    sideCam.AcquireImage(curAoiImageName);
                }

                if (_sideCamCtrl != null)
                {
                    _sideCamCtrl.m_ViewScale = viewScale;
                    _sideCamCtrl.AcquireImage(triggerDelay, curAoiImageName);
                    _sideCamCtrl.Update();
                    hImage = _sideCamCtrl.hImage;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PreviousPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreviousPoint()
        {
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                foreach (ListViewItem item in listView_Model.SelectedItems)
                {
                    countindex = item.Index;
                    if (countindex > 0 && countindex <= listView_Model.Items.Count - 1)
                    {
                        foreach (ListViewItem clear_item in listView_Model.SelectedItems)
                        {
                            listView_Model.Items[clear_item.Index].Selected = false;
                            countindex--;
                            listView_Model.Items[countindex].Selected = true;
                            MoveAll_Axis(countindex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextPoint()
        {
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                foreach (ListViewItem item in listView_Model.SelectedItems)
                {
                    countindex = item.Index;
                    if (countindex >= 0 && countindex < listView_Model.Items.Count - 1)
                    {
                        foreach (ListViewItem clear_item in listView_Model.SelectedItems)
                        {
                            listView_Model.Items[clear_item.Index].Selected = false;
                            countindex++;
                            listView_Model.Items[countindex].Selected = true;
                            MoveAll_Axis(countindex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetProgram();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetProgram()
        {
            try
            {
                if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                {
                    foreach (ListViewItem item in listView_Model.SelectedItems)
                    {
                        listView_Model.Items[item.Index].Selected = false;
                    }
                    countindex = 0;

                    for (int i = 0; i < MAX_AXIS; i++)
                    {
                        gintPhase[i] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBoardIn_Click(object sender, EventArgs e)
        {
            BoardIn();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BoardIn()
        {
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                int lngTemp = 0;
                wAxis = CTDw.CTD_AXIS_2;

                CTDw.CTDwGetInternalCounter(gwBsn, AXIS_2, ref lngTemp);
                if (lngTemp <= 0)
                {
                    if (speedSetting.CTD_InitAxis_Run(wBsn, wAxis, setMode1, setMode2) == false)
                    {
                        //ErrorMessage(wBsn);
                        return;
                    }

                    CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, pulseBoardOut - lngTemp);
                    pulseBoardOut = 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBoardOut_Click(object sender, EventArgs e)
        {
            BoardOut();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public void EtcWrite(short i)
        {
            if (MainForm.gshtSLMEnable[i] == 0)
            {   // 減速停止リミット有効
                CTD.CTDw.CTDwCommandWrite(wBsn, i, CTD.CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET);    // 減速停止リミットは無効
            }
            else
            {
                CTD.CTDw.CTDwCommandWrite(wBsn, i, CTD.CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_SET);      // 減速停止リミットは有効
            }
            if (MainForm.gshtELMEnable[i] == 0)
            {   // 急停止リミット有効
                CTD.CTDw.CTDwCommandWrite(wBsn, i, CTD.CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_RESET);    // 急停止リミットは無効
            }
            else
            {
                CTD.CTDw.CTDwCommandWrite(wBsn, i, CTD.CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET);      // 急停止リミットは有効
            }
            if (MainForm.gshtALMEnable[i] == 0)
            {   // アラームリミット有効
                CTD.CTDw.CTDwCommandWrite(wBsn, i, CTD.CTDw.CTD_ALARM_STOP_ENABLE_MODE_RESET);         // アラームは無効
            }
            else
            {
                CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_ALARM_STOP_ENABLE_MODE_SET);           // アラームは有効
            }
        }

        // ステータス表示用文字列を作成する
        public string StatusDispMake(byte bStatus)
        {
            short i, shtTemp;
            string strData;

            strData = "";
            for (i = 0; i < 8; i++)
            {
                shtTemp = anzBit_8(bStatus, i);
                if (shtTemp == 0)
                {
                    strData = "○" + strData;
                }
                else
                {
                    strData = "●" + strData;
                }
            }
            return strData;
        }

        //'----------------------------------------------------------------
        //'   ＢＩＴ解析処理
        //'----------------------------------------------------------------
        private short anzBit_8(byte bData, short bit)
        {
            short shtRet;

            if ((bData & (1 << bit)) == 0)
            {
                shtRet = 0;
            }
            else
            {
                shtRet = 1;
            }
            return shtRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCount1_Tick(object sender, EventArgs e)
        {
            short i = AXIS_1;
            int lngTemp = 0;
            byte bytTemp = 0;
            string strData;

            // 現在座標取得
            CTD.CTDw.CTDwGetInternalCounter(gwBsn, i, ref lngTemp);
            txtCounter1.Text = lngTemp.ToString();

            CTD.CTDw.CTDwGetDrivePulseCounter(gwBsn, i, ref lngTemp);

            // End Status 取得
            CTD.CTDw.CTDwGetEndStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblEndStatus_1.Text = strData;

            bytTemp &= 0x02;
            gbytEndStatus[i] = bytTemp;                             // グローバル変数にも保存

            // DriveStatus 取得
            CTD.CTDw.CTDwGetDriveStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblDriveStatus_1.Text = strData;

            bytTemp &= 0x01;                                         // BUSY状態だけ抜き出す
            gbytBusy[i] = bytTemp;

            // Mechanical Signal 取得
            CTD.CTDw.CTDwGetMechanicalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblMechanicalSignal_1.Text = strData;

            // Universal Signal 取得								
            CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblUniversalSignal_1.Text = strData;

            gshtSLMEnable[i] = 1; // 減速停止リミット有効
            gshtELMEnable[i] = 1; // 急停止リミット有効
            gshtALMEnable[i] = 1;   // アラームリミット無効

            switch (gintPhase[i])
            {
                case 0:     //何もしない
                    break;

                // ORG開始 HOME↑
                case 200:       // 初期処理
                    EtcWrite(i);
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET);          // 急停止リミットを有効に
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET);        // 減速停止リミットは無効に
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwMode2Write(gwBsn, i, gbytMode2[i]);

                    if (speedSetting.CTD_InitAxis_ORG(wBsn, i, setMode1, setMode2) == false)
                    {
                        //ErrorMessage(wBsn);
                        //return;
                    }

                    // 速度設定
                    //TD.CTDw.CTDwSpeedParameterWrite(gwBsn, i, gdblLowSpeed[i, 1], gdblHiSpeed[i, 1], gshtAccTime[i, 1], gdblSRate[i, 1]);
                    //SpeedSet();
                    CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
                    if ((bytTemp & 0x10) == 0x0)
                    {       // ORGがOFFなら
                        gintPhase[i]++;
                    }
                    else
                    {                               // ORGがONなら
                        gintPhase[i] = 230;
                    }
                    break;
                case 201:       // -高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0xc;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 202:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 230;
                            break;
                        }
                        else if ((gbytEndStatus[i] & 0x02) == 0x02)
                        {       // 急停止リミットで停止したら 見つからなかったと判断する
                            gintPhase[i] = 210;
                            break;
                        }
                        else
                        {                                           // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 210:       // +高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0x4;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↓
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);        // +方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 211:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 220;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 220:       // -高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0xc;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 221:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 230;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 230:       // +低速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0x4;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↓
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);        // +方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 231:       // 低速ドライブ終了待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 249;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 240:       // 異常終了
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    gintPhase[i] = 999;
                    break;
                case 249:       // 正常終了
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_INTERNAL_COUNTER_WRITE, 0);                           // Internal Counter クリア
                    CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0);                           // External Counter クリア
                    gintPhase[i] = 999;
                    break;

                case 999:   // 終了処理
                    gintPhase[i] = 0;
                    gintEndReq[i] = 0;
                    break;
            }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCount2_Tick(object sender, EventArgs e)
        {
            short i = AXIS_2;
            int lngTemp = 0;
            byte bytTemp = 0;
            string strData;


            // 現在座標取得
            CTD.CTDw.CTDwGetInternalCounter(gwBsn, i, ref lngTemp);
            txtCounter2.Text = lngTemp.ToString();

            CTD.CTDw.CTDwGetDrivePulseCounter(gwBsn, i, ref lngTemp);

            // End Status 取得
            CTD.CTDw.CTDwGetEndStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblEndStatus_2.Text = strData;

            bytTemp &= 0x02;
            gbytEndStatus[i] = bytTemp;

            // DriveStatus 取得
            CTD.CTDw.CTDwGetDriveStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblDriveStatus_2.Text = strData;

            bytTemp &= 0x01;
            gbytBusy[i] = bytTemp;                                  // グローバル変数にも保存

            // Mechanical Signal 取得
            CTD.CTDw.CTDwGetMechanicalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblMechanicalSignal_2.Text = strData;

            // Universal Signal 取得
            CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
            bytTemp &= 0x10;
            gbytUniversal[i] = bytTemp;

            // Universal Signal 取得								
            CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblUniversalSignal_2.Text = strData;

            gshtSLMEnable[i] = 1; // 減速停止リミット有効
            gshtELMEnable[i] = 1; // 急停止リミット有効
            gshtALMEnable[i] = 1;	// アラームリミット無効

            switch (gintPhase[i])
            {
                case 0:     //何もしない
                    break;

                // ORG開始 HOME↑
                case 200:       // 初期処理
                    EtcWrite(i);
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET);          // 急停止リミットを有効に
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET);        // 減速停止リミットは無効に
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwMode2Write(gwBsn, i, gbytMode2[i]);

                    if (speedSetting.CTD_InitAxis_ORG(wBsn, i, setMode1, setMode2) == false)
                    {
                        //ErrorMessage(wBsn);
                        //return;
                    }
                    // 速度設定
                    //CTD.CTDw.CTDwSpeedParameterWrite(gwBsn, i, gdblLowSpeed[i, 1], gdblHiSpeed[i, 1], gshtAccTime[i, 1], gdblSRate[i, 1]);
                    //SpeedSet();
                    CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
                    if ((bytTemp & 0x10) == 0x0)
                    {       // ORGがOFFなら
                        gintPhase[i]++;
                    }
                    else
                    {                               // ORGがONなら
                        gintPhase[i] = 230;
                    }
                    break;
                case 201:       // -高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0xc;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 202:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 230;
                            break;
                        }
                        else if ((gbytEndStatus[i] & 0x02) == 0x02)
                        {       // 急停止リミットで停止したら 見つからなかったと判断する
                            gintPhase[i] = 210;
                            break;
                        }
                        else
                        {                                           // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 210:       // +高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0x4;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↓
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);        // +方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 211:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 220;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 220:       // -高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0xc;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 221:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 230;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 230:       // +低速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0x4;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↓
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);        // +方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 231:       // 低速ドライブ終了待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 249;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 240:       // 異常終了
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    gintPhase[i] = 999;
                    break;
                case 249:       // 正常終了
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_INTERNAL_COUNTER_WRITE, 0);                           // Internal Counter クリア
                    CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0);                           // External Counter クリア
                    gintPhase[i] = 999;
                    break;

                case 999:   // 終了処理
                    gintPhase[i] = 0;
                    gintEndReq[i] = 0;
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCount3_Tick(object sender, EventArgs e)
        {
            short i = AXIS_3;
            int lngTemp = 0;
            byte bytTemp = 0;
            string strData;

            // 現在座標取得
            CTD.CTDw.CTDwGetInternalCounter(gwBsn, i, ref lngTemp);
            internalcount_Axis3 = lngTemp;
            txtCounter3.Text = lngTemp.ToString();

            CTD.CTDw.CTDwGetDrivePulseCounter(gwBsn, i, ref lngTemp);

            // End Status 取得
            CTD.CTDw.CTDwGetEndStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblEndStatus_3.Text = strData;

            bytTemp &= 0x02;
            gbytEndStatus[i] = bytTemp;                             // グローバル変数にも保存

            // DriveStatus 取得
            CTD.CTDw.CTDwGetDriveStatus(gwBsn, i, ref bytTemp);

            strData = StatusDispMake(bytTemp);
            lblDriveStatus_3.Text = strData;

            bytTemp &= 0x01;                                         // BUSY状態だけ抜き出す
            gbytBusy[i] = bytTemp;                                  // グローバル変数にも保存

            // Mechanical Signal 取得
            CTD.CTDw.CTDwGetMechanicalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblMechanicalSignal_3.Text = strData;

            // Universal Signal 取得								
            CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
            strData = StatusDispMake(bytTemp);
            lblUniversalSignal_3.Text = strData;

            gshtSLMEnable[i] = 1; // 減速停止リミット有効
            gshtELMEnable[i] = 1; // 急停止リミット有効
            gshtALMEnable[i] = 1;   // アラームリミット無効

            switch (gintPhase[i])
            {
                case 0:     //何もしない
                    break;

                // ORG開始 HOME↑
                case 200:       // 初期処理
                    EtcWrite(i);
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET);          // 急停止リミットを有効に
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET);        // 減速停止リミットは無効に
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    CTD.CTDw.CTDwMode2Write(gwBsn, i, gbytMode2[i]);

                    if (speedSetting.CTD_InitAxis_ORG(wBsn, i, setMode1, setMode2) == false)
                    {
                        //ErrorMessage(wBsn);
                        //return;
                    }
                    // 速度設定
                    //CTD.CTDw.CTDwSpeedParameterWrite(gwBsn, i, gdblLowSpeed[i, 1], gdblHiSpeed[i, 1], gshtAccTime[i, 1], gdblSRate[i, 1]);
                    //SpeedSet();
                    CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
                    if ((bytTemp & 0x10) == 0x0) // ORGがOFFなら
                    {
                        gintPhase[i]++;
                    }
                    else  // ORGがONなら
                    {
                        gintPhase[i] = 230;
                        //gintPhase[i]++;
                    }
                    break;
                case 201:       // -高速ドライブ
                                //bytTemp = gbytMode1[i];
                                //bytTemp &= 0xf0; //00000001 AND 11110000
                                //bytTemp |= 0xc; //bytTemp OR 1100
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x6c);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ

                    gintPhase[i]++;
                    break;
                case 202:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if ((gbytEndStatus[i] & 0x02) == 0x02)
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);
                            gintPhase[i] = 210;
                        }
                        else
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);
                            gintPhase[i] = 249;
                        }
                    }
                    break;
                case 210:       // +高速ドライブ
                    if (gbytBusy[i] == 0)
                    {
                        //bytTemp = gbytMode1[i];
                        //bytTemp &= 0xf0;
                        //bytTemp |= 0x4;
                        CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x6c);                                         // 検出対象 HOME ↓
                        CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);        // +方向高速ドライブ
                        gintPhase[i]++;
                    }
                    break;
                case 211:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);        // +方向高速ドライブ
                            gintPhase[i] = 249;
                        }
                    }
                    break;
                case 220:       // -高速ドライブ
                    bytTemp = gbytMode1[i];
                    bytTemp &= 0xf0;
                    bytTemp |= 0xc;
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, bytTemp);                                         // 検出対象 HOME ↑
                    CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);   // -方向高速ドライブ
                    gintPhase[i]++;
                    break;
                case 221:       // 停止待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            gintPhase[i] = 230;
                            break;
                        }
                        else
                        {       // それ以外の停止なら　異常と判断する
                            gintPhase[i] = 240;
                        }
                    }
                    break;
                case 230:       // +低速ドライブ
                                //bytTemp = gbytMode1[i];
                                //bytTemp &= 0xf0;
                                //bytTemp |= 0x4;
                    if (gbytBusy[i] == 0)
                    {
                        CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x6c);                                         // 検出対象 HOME ↓
                        CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);        // +方向高速ドライブ
                        gintPhase[i]++;
                    }
                    break;

                case 231:
                    if (gbytBusy[i] == 0)
                    {
                        CTD.CTDw.CTDwGetUniversalSignal(gwBsn, i, ref bytTemp);
                        if ((bytTemp & 0x10) == 0x10) // ORGがONなら
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);        // +方向高速ドライブ
                            gintPhase[i] = 249;
                        }
                        else
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);
                            gintPhase[i]++;
                        }
                    }
                    break;

                case 232:       // 低速ドライブ終了待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if ((gbytEndStatus[i] & 0x02) == 0x02)
                        {       // 正常終了したら ORGを検出したと判断する
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);
                            gintPhase[i]++;
                            break;
                        }
                        else
                        {
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x6c);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_MINUS_SIGNAL_SEARCH1_DRIVE);
                            gintPhase[i] = 234;
                        }
                    }
                    break;
                case 233:       // 低速ドライブ終了待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x6c);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH1_DRIVE);
                            gintPhase[i]++;
                            break;
                        }
                        else
                        {
                            CTDw.CTDwDataFullWrite(gwBsn, i, CTDw.CTD_SLOW_DOWN_STOP, 0x1F);
                            gintPhase[i] = 999;
                        }
                    }
                    break;
                case 234:       // 低速ドライブ終了待ち
                    if (gbytBusy[i] == 0)
                    {               // 停止したら
                        if (gbytEndStatus[i] == 0)
                        {       // 正常終了したら ORGを検出したと判断する
                            CTD.CTDw.CTDwMode1Write(gwBsn, i, 0x64);                                         // 検出対象 HOME ↓
                            CTD.CTDw.CTDwCommandWrite(gwBsn, i, CTD.CTDw.CTD_PLUS_SIGNAL_SEARCH2_DRIVE);
                            gintPhase[i] = 249;
                            break;
                        }
                        else
                        {
                            CTDw.CTDwDataFullWrite(gwBsn, i, CTDw.CTD_SLOW_DOWN_STOP, 0x1F);
                            gintPhase[i] = 999;
                        }
                    }
                    break;
                case 240:       // 異常終了
                    EtcWrite(i);
                    CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                    gintPhase[i] = 999;
                    break;
                case 249:       // 正常終了
                    if (gbytBusy[i] == 0)
                    {
                        EtcWrite(i);
                        CTD.CTDw.CTDwMode1Write(gwBsn, i, gbytMode1[i]);
                        CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_INTERNAL_COUNTER_WRITE, 0);                           // Internal Counter クリア
                        CTD.CTDw.CTDwDataFullWrite(gwBsn, i, CTD.CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0);                           // External Counter クリア
                        gintPhase[i] = 999;
                    }
                    break;

                case 999:   // 終了処理
                    gintPhase[i] = 0;
                    gintEndReq[i] = 0;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BoardOut()
        {
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                wAxis = CTDw.CTD_AXIS_2;
                int lngTemp = 0;
                StringBuilder stringBuilder = new StringBuilder();
                CTDw.CTDwGetInternalCounter(gwBsn, AXIS_2, ref lngTemp);

                if (lngTemp > 0)
                {
                    stringBuilder.Append(lngTemp.ToString());
                    pulseBoardOut = int.Parse(stringBuilder.ToString());
                    stringBuilder.Clear();

                    if (speedSetting.CTD_InitAxis_Run(wBsn, wAxis, 0x6c, setMode2) == false)
                    {
                        ErrorMessage(wBsn);
                        return;
                    }

                    EtcWrite(wAxis);
                    CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET);          // 急停止リミットを有効に
                    CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET);        // 減速停止リミットは無効に

                    CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, lngTemp);
                    btnStart.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shtAxis"></param>
        private void Org000Exec_1(short shtAxis)
        {
            if (gintPhase[shtAxis] == 0)
            {
                gintPhase[shtAxis] = 200;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shtAxis"></param>
        private void Org000Exec_2(short shtAxis)
        {
            if (gintPhase[shtAxis] == 0)
            {
                gintPhase[shtAxis] = 200;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shtAxis"></param>
        private void Org000Exec_3(short shtAxis)
        {
            if (gintPhase[shtAxis] == 0)
            {
                gintPhase[shtAxis] = 200;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wBsn"></param>
        private void ErrorMessage(short wBsn)
        {
            int dwRes;
            string szbuf;

            dwRes = CTDw.CTDwGetLastError(wBsn);

            szbuf = "CTD error" + '\n' + '\n' + "Error code: 0x" + dwRes.ToString("x8");
            MessageBox.Show(szbuf, "CTD", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void MoveAll_Axis(int index)
        {
            if (listView_Model.Items.Count > 0)
            {
                int[] pulse = new int[4];

                if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                {
                    for (short axis = 0; axis < 3; axis++)
                    {
                        //pulse[axis] = int.Parse(listView_Model.Items[index].SubItems[axis + 1].Text);
                        pulse[axis] = int.Parse(listView_Model.Items[index].SubItems[axis + firstPosOffset].Text);

                        CTDw.CTDwGetInternalCounter(gwBsn, axis, ref internalcount[axis]);

                        CTD.CTDw.CTDwMode1Write(gwBsn, axis, gbytMode1[axis]);
                        CTD.CTDw.CTDwMode2Write(gwBsn, axis, gbytMode2[axis]);
                        EtcWrite(axis);
                        //CTD.CTDw.CTDwSpeedParameterWrite(gwBsn, axis, gdblLowSpeed[axis, 2], gdblHiSpeed[axis, 2], gshtAccTime[axis, 2], gdblSRate[axis, 2]);
                        if (speedSetting.CTD_InitAxis_Run(gwBsn, axis, setMode1, setMode2) == false)
                        {
                            gintPhase[axis] = 999;
                            ErrorMessage(gwBsn);
                            return;
                        }
                        if (pulse[axis] > internalcount[axis])
                            CTDw.CTDwDataFullWrite(gwBsn, axis, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, pulse[axis] - internalcount[axis]);
                        else if (pulse[axis] < internalcount[axis])
                            CTDw.CTDwDataFullWrite(gwBsn, axis, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, internalcount[axis] - pulse[axis]);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Load Program");
            }
        }

        private void ProcessAOI(int stepNo)
        {
            var confirmResult = MessageBox.Show("AOI Process", "Confirm Continue!!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MovePartOut()
        {
            int[] pulse = new int[4];
            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                for (short axis = 0; axis < 3; axis++)
                {
                    //pulse[axis] = int.Parse(listView_Model.Items[index].SubItems[axis + 1].Text);
                    pulse[axis] = 1000;

                    CTDw.CTDwGetInternalCounter(gwBsn, axis, ref internalcount[axis]);

                    CTD.CTDw.CTDwMode1Write(gwBsn, axis, gbytMode1[axis]);
                    CTD.CTDw.CTDwMode2Write(gwBsn, axis, gbytMode2[axis]);
                    EtcWrite(axis);
                    //CTD.CTDw.CTDwSpeedParameterWrite(gwBsn, axis, gdblLowSpeed[axis, 2], gdblHiSpeed[axis, 2], gshtAccTime[axis, 2], gdblSRate[axis, 2]);
                    if (speedSetting.CTD_InitAxis_Run(gwBsn, axis, setMode1, setMode2) == false)
                    {
                        gintPhase[axis] = 999;
                        ErrorMessage(gwBsn);
                        return;
                    }
                    if (pulse[axis] > internalcount[axis])
                        CTDw.CTDwDataFullWrite(gwBsn, axis, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, pulse[axis] - internalcount[axis]);
                    else if (pulse[axis] < internalcount[axis])
                        CTDw.CTDwDataFullWrite(gwBsn, axis, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, internalcount[axis] - pulse[axis]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTDw.CTDwMode1Write(gwBsn, AXIS_4, 0x64);
            CTDw.CTDwMode2Write(gwBsn, AXIS_4, 0x30);
            CTDw.CTDwUniversalSignalWrite(gwBsn, AXIS_4, 0x00);
        }

        private void btnOriginPoint_Click(object sender, EventArgs e)
        {
            #region unused 
            //int pulsecount1 = 0;
            //int pulsecount2 = 0;
            //int pulsecount3 = 0;

            //wAxis1 = CTDw.CTD_AXIS_1;
            //wAxis2 = CTDw.CTD_AXIS_2;
            //wAxis3 = CTDw.CTD_AXIS_3;


            //try
            //{
            //    for (short i = 0; i < MAX_AXIS; i++)
            //    {
            //        if (speedSetting.CTD_InitAxis_Run(wBsn, i, setMode1, setMode2) == false)
            //        {
            //            ErrorMessage(wBsn);
            //            return;
            //        }
            //    }
            //    CTDw.CTDwGetInternalCounter(wBsn, wAxis1, ref pulsecount1);
            //    CTDw.CTDwGetInternalCounter(wBsn, wAxis2, ref pulsecount2);
            //    CTDw.CTDwGetInternalCounter(wBsn, wAxis3, ref pulsecount3);

            //    if (17130 > pulsecount1)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis1, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, 17130 - pulsecount1);
            //    else if (17130 < pulsecount1)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis1, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, pulsecount1 - 17130);

            //    if (158386 > pulsecount2)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis2, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, 158386 - pulsecount2);
            //    else if (158386 < pulsecount2)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis2, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, pulsecount2 - 158386);

            //    if (0 > pulsecount3)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis3, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, 0 - pulsecount3);
            //    else if (0 < pulsecount3)
            //        CTDw.CTDwDataFullWrite(wBsn, wAxis3, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, pulsecount3 - 0);

            //    isMachineHomed = true;
            //    MessageBox.Show("Machine home completed and ready to start run", "Machine Message");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            #endregion

            OriginSetting();
            isMachineHomed = true;
            if (!timerStart.Enabled)
                timerStart.Enabled = true;
            DGV_ImageLists.DataSource = null;

            MessageBox.Show("Please Waiting MC Home...");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int totalPoints = 0;
            string prgFileName = AppDomain.CurrentDomain.BaseDirectory + "Program";
            if (!Directory.Exists(prgFileName))
            {
                Directory.CreateDirectory(prgFileName);
            }

            openFileDialog1.InitialDirectory = prgFileName;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Program File|*.prg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strResult = Path.GetFileName(openFileDialog1.FileName);
                tbProgramName.Text = Path.GetFileNameWithoutExtension(openFileDialog1.FileName); ;
                string path_Read = openFileDialog1.FileName;

                if (File.Exists(path_Read))
                {
                    listView_Model.Items.Clear();

                    using (StreamReader sr = new StreamReader(path_Read))
                    {
                        string line = "";

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] list = line.Split(',');

                            ListViewItem m_viewItem = new ListViewItem(list);
                            listView_Model.Items.Add(m_viewItem);
                        }
                        sr.Close();
                        totalPoints = listView_Model.Items.Count;
                        if (listConfinence != null)
                            listConfinence = null;
                        listConfinence = new double[totalPoints];
                    }
                }
                else
                {
                    MessageBox.Show("No File Directory");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerStart_Tick(object sender, EventArgs e)
        {

            if (gbytBusy[AXIS_1] == 1 || gbytBusy[AXIS_2] == 1 || gbytBusy[AXIS_3] == 1)
            {
                btnStart.Text = "BUSY";
                btnStart.BackColor = Color.Red;
            }
            else if (btnStartStatus == 0)
            {
                btnStart.Text = "START";
                btnStart.BackColor = Color.LimeGreen;
            }
            else if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
            {
                if (countindex <= listView_Model.Items.Count - 1) {

                    if (listView_Model.Items[countindex].SubItems[(int)Writer.Result].Text == "OK")
                    {
                        DataReceive = "Stop";
                        timerStart.Enabled = false;
                        btnStartStatus = 0;

                        btnStart.Text = "TESTING";
                        btnStart.BackColor = Color.Yellow;
                        Thread.Sleep(triggerDelay);

                        if (countindex <= listView_Model.Items.Count - 2)
                        {
                            //GenImageNamebyPosition();
                            AcquireImage();

                            LoadImageActived = true;

                            DGV_ImageLists.RowCount = indexAOI + 1;
                            DGV_ImageLists.Rows[indexAOI].Cells[0].Value = indexAOI + 1;
                            DGV_ImageLists.Rows[indexAOI].Cells[1].Value = curAoiImageName;

                            LoadImageActived = false;
                            //Cursor = Cursors.Default;

                            if (hImage != null)
                            {
                                DLInspectionImage(indexAOI, hImage);
                                if (ip3000Result != null && ip3000Result.AOIResult != "ok" && cbAlertRejPart.Checked)
                                {
                                    string alertTitle = Path.GetFileNameWithoutExtension(curAoiImageName);
                                    string alertMsg = "Cls Score: " + ip3000Result.ClsScore;  // + " Sement Score: " + ip3000Result.SegmentScore;
                                    AlertFrm alertFrm = new AlertFrm();
                                    alertFrm.SetAlertMessageWithImage(indexAOI, curProductSide.ToString(), alertTitle, alertMsg, curAoiImageName);
                                    var dlgresult = alertFrm.ShowDialog();
                                }

                                DGV_ImageLists.FirstDisplayedScrollingRowIndex = DGV_ImageLists.RowCount - 1;
                                indexAOI++;

                                //countindex++;
                                //btnStartStatus = 1;
                                //timerStart.Enabled = true;
                                //DataReceive = "Run";
                            }
                        }

                        countindex++;
                        btnStartStatus = 1;
                        timerStart.Enabled = true;
                        DataReceive = "Run";
                    }
                }

                #region unused for pause trigger AOI
                //DataReceive = "Stop";
                //if (!triggerAOI)
                //{
                //    triggerAOI = true;
                //    var confirmResult = MessageBox.Show("AOI Process", "Confirm Continue!!", MessageBoxButtons.YesNo);
                //    if (confirmResult == DialogResult.Yes)
                //    {
                //        Thread.Sleep(100);
                //        DataReceive = "Run";
                //        triggerAOI = false;
                //    }
                //}
                #endregion
            }

            if (internalcount_Axis3 >= 76000)
            {
                wAxis = CTDw.CTD_AXIS_3;
                CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_STOP, 100);
                if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                {
                    CTDw.CTDwMode2Write(gwBsn, wAxis, 0x35);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Estop()
        {
            timerDataReceive.Enabled = false;
            btnStartStatus = 0;

            for (int i = 0; i < MAX_AXIS; i++)
            {
                short wAxis = (short)i;
                gintPhase[i] = 0;
                if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_STOP, 0x1F) == 0)
                {
                    //ErrorMessage(wBsn);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmergency_Click(object sender, EventArgs e)
        {
            Estop();
            countindex = 0;
            indexAOI = 0;
            btnStartStatus = 0;
            timerStart.Enabled = false;
            DataReceive = "Stop";
            //timerDataReceive.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save_Program();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Model_DoubleClick(object sender, EventArgs e)
        {
            Select_Model();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Select_Model()
        {
            foreach (ListViewItem item in listView_Model.SelectedItems)
            {
                countindex = item.Index;
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                MoveAll_Axis(item.Index);
                tbComponentName.Text = item.SubItems[(int)Writer.Component].Text;
                cbBlockNo.SelectedIndex = cbBlockNo.FindStringExact(item.SubItems[(int)Writer.Block].Text);
                cbPin.SelectedIndex = cbPin.FindStringExact(item.SubItems[(int)Writer.Pin].Text);
                cbInspSide.SelectedIndex = cbInspSide.FindStringExact(item.SubItems[(int)Writer.Side].Text);
                cbInspCamera.SelectedIndex = cbInspCamera.FindStringExact(item.SubItems[(int)Writer.Camera].Text);
                cbZoom.SelectedIndex = cbZoom.FindStringExact(item.SubItems[(int)Writer.Zoom].Text);
                activeCamera = cbInspCamera.SelectedIndex == 0 ? ActiveCamera.TOP : ActiveCamera.SIDE;
                triggerDelay = Convert.ToInt32(item.SubItems[(int)Writer.TriggerDelay].Text);
                nTriggerDelay.Value = Convert.ToDecimal(triggerDelay);
                exposure = Convert.ToInt32(item.SubItems[(int)Writer.Exposure].Text);
                nCamExposure.Value = exposure;
                int zoomFac = Convert.ToInt32(item.SubItems[(int)Writer.Zoom].Text.Substring(0, item.SubItems[(int)Writer.Zoom].Text.Length - 1));
                if (activeCamera == ActiveCamera.TOP)
                {
                    if (topCam != null)
                    {
                        topCam.m_ViewScale = zoomFac;
                        topCam.m_CArtCam.SetExposureTime(exposure);
                    }

                    if (_topCamCtrl != null)
                    {
                        _topCamCtrl.m_ViewScale = zoomFac;
                        _topCamCtrl.m_CArtCam.SetExposureTime(exposure);
                        _topCamCtrl.UpdatePreview();
                    }
                }
                else
                {
                    if (sideCam != null)
                    {
                        sideCam.m_ViewScale = zoomFac;
                        sideCam.m_CArtCam.SetExposureTime(exposure);
                    }

                    if (_sideCamCtrl != null)
                    {
                        _sideCamCtrl.m_ViewScale = zoomFac;
                        _sideCamCtrl.m_CArtCam.SetExposureTime(exposure);
                        _sideCamCtrl.UpdatePreview();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Origin_Click(object sender, EventArgs e)
        {
            OriginSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_Program();
            //SaveCropAreas();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Save_Program()
        {
            string writer = AppDomain.CurrentDomain.BaseDirectory + "Program";
            if (!Directory.Exists(writer))
            {
                Directory.CreateDirectory(writer);
            }
            string path = "";

            saveFileDialog1.InitialDirectory = writer;
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Program File|*.prg";
            saveFileDialog1.Title = "Save Program File";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName != "")
                {
                    path = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    string strResult = Path.GetFileName(path);
                    tbProgramName.Text = strResult.Replace(".prg", "");

                    fs.Close();
                }
                else
                {
                    MessageBox.Show("Empty File Name\n Please try again!!!");
                }

                // Check if file already exists. If yes, delete it.     
                if (File.Exists(path))
                {
                    string index;
                    string block;
                    string component;
                    string pin;
                    string comment;
                    string x_axis;
                    string y_axis;
                    string r_axis;
                    string camera;
                    string result;
                    string zoom;
                    string trigDelay;
                    string expos;

                    StreamWriter sw = new StreamWriter(path);
                    try
                    {
                        for (int i = 0; i < listView_Model.Items.Count; i++)
                        {
                            index = listView_Model.Items[i].SubItems[(int)Writer.No].Text;
                            block = listView_Model.Items[i].SubItems[(int)Writer.Block].Text;
                            component = listView_Model.Items[i].SubItems[(int)Writer.Component].Text;
                            pin = listView_Model.Items[i].SubItems[(int)Writer.Pin].Text;
                            comment = listView_Model.Items[i].SubItems[(int)Writer.Side].Text;
                            x_axis = listView_Model.Items[i].SubItems[(int)Writer.X_axis].Text;
                            y_axis = listView_Model.Items[i].SubItems[(int)Writer.Y_axis].Text;
                            r_axis = listView_Model.Items[i].SubItems[(int)Writer.R_axis].Text;
                            camera = listView_Model.Items[i].SubItems[(int)Writer.Camera].Text;
                            zoom = listView_Model.Items[i].SubItems[(int)Writer.Zoom].Text;
                            expos = listView_Model.Items[i].SubItems[(int)Writer.Exposure].Text;
                            trigDelay = listView_Model.Items[i].SubItems[(int)Writer.TriggerDelay].Text;
                            result = listView_Model.Items[i].SubItems[(int)Writer.Result].Text;

                            string strLog = index + "," + block + "," + component + "," + pin + "," + comment + "," + x_axis + "," + y_axis + "," + r_axis + "," + camera + "," + zoom + "," + expos + "," + trigDelay + "," + result;
                            sw.WriteLine(strLog);
                        }
                        sw.Flush();
                        sw.Close();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(path + "\r\n can not save" + ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No " + path + "\r\n can not save", "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timerDataReceive_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (DataReceive)
                {
                    case "Stop":

                        break;
                    case "Run":
                        {
                            if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                            {
                                if (btnStartStatus == 1)
                                {
                                    if (countindex == listView_Model.Items.Count)
                                    {
                                        btnStartStatus = 0;
                                        timerDataReceive.Enabled = false;
                                        if (runMode == RunMode.GRR)
                                        {
                                            if (roundGrr < totalRoundGrr)
                                            {
                                                btTest_Click(sender, e);
                                                roundGrr++;
                                                tbroundGrr.Text = roundGrr.ToString();
                                            }
                                            else
                                            {
                                                btSaveGrr_Click(sender, e);
                                            }
                                        }

                                        OriginSetting();
                                        btExportResult_Click(null, null);
                                        DataReceive = "Stop";
                                        countindex = 0;
                                        indexAOI = 0;
                                        isMachineHomed = true;

                                        try
                                        {
                                            int index = cbRecipeList.SelectedIndex;
                                            if((index == 2))
                                                index = 0;
                                            else if(index == 5)
                                                index = 3;
                                            else if(index == 8)
                                                index = 6;

                                            cbRecipeList.SelectedIndex = index; 
                                            string recipeName = cbRecipeList.Items[index].ToString();
                                            LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                                            DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Initialize Recipe Error: " + ex.Message);
                                        }

                                        //int nextProgram =0;
                                        //if (curProductSide == ProductSide.Bottom)
                                        //{
                                        //    ResultFrm popupFrm = new ResultFrm();
                                        //    popupFrm.NextProductSide = ProductSide.PCBA;
                                        //    popupFrm.InspectionResult = inspectionResult;
                                        //    popupFrm.ShowDialog();
                                        //    nextProgram = popupFrm.IndexNextProgram;
                                        //}
                                        //else if (curProductSide == ProductSide.PCBA)
                                        //{
                                        //    ResultFrm popupFrm = new ResultFrm();
                                        //    popupFrm.NextProductSide = ProductSide.Top;
                                        //    popupFrm.InspectionResult = inspectionResult;
                                        //    popupFrm.ShowDialog();
                                        //    nextProgram = popupFrm.IndexNextProgram;
                                        //}
                                        //else if (curProductSide == ProductSide.Top)
                                        //{
                                        //    ResultFrm popupFrm = new ResultFrm();
                                        //    popupFrm.NextProductSide = ProductSide.Bottom;
                                        //    popupFrm.InspectionResult = inspectionResult;   
                                        //    popupFrm.ShowDialog();
                                        //    nextProgram = popupFrm.IndexNextProgram;
                                        //}

                                        //if(nextProgram == 0)
                                        //{
                                        //    InitBottomMotionAndDL();
                                        //}
                                        //else if(nextProgram == 1)
                                        //{
                                        //    InitPCBAMotionAndDL();
                                        //}
                                        //else if(nextProgram == 2)
                                        //{
                                        //    InitHeatsinkMotionAndDL();
                                        //}
                                    }
                                    else if (countindex >= 0 && countindex <= listView_Model.Items.Count - 1)
                                    {
                                        foreach (ListViewItem item in listView_Model.SelectedItems)
                                        {
                                            listView_Model.Items[item.Index].Selected = false;
                                        }

                                        listView_Model.Items[countindex].Selected = true;

                                        triggerDelay = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.TriggerDelay].Text);
                                        exposure = Convert.ToInt32(listView_Model.Items[countindex].SubItems[(int)Writer.Exposure].Text);
                                        activeCamera = listView_Model.Items[countindex].SubItems[(int)Writer.Camera].Text.Contains("Top") ? ActiveCamera.TOP : ActiveCamera.SIDE;
                                        if (activeCamera == ActiveCamera.TOP)
                                        {
                                            if (_topCamCtrl != null)
                                            {
                                                _topCamCtrl.m_CArtCam.SetExposureTime(exposure);
                                            }
                                        }
                                        else
                                        {
                                            if (_sideCamCtrl != null)
                                            {
                                                _sideCamCtrl.m_CArtCam.SetExposureTime(exposure);
                                            }
                                        }

                                        MoveAll_Axis(countindex);
                                        listView_Model.Items[countindex].SubItems[(int)Writer.Result].Text = "OK";
                                        listView_Model.Items[countindex].EnsureVisible();
                                        listView_Model.Refresh();
                                        listView_Model.Update();
                                    }
                                }
                            }
                            break;
                        }

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR " + ex.Message);
            }
        }

        private void InternalCount()
        {
            CTDw.CTDwGetInternalCounter(gwBsn, AXIS_1, ref internalcount1);
            CTDw.CTDwGetInternalCounter(gwBsn, AXIS_2, ref internalcount2);
            CTDw.CTDwGetInternalCounter(gwBsn, AXIS_3, ref internalcount3);
        }

        private void timerMouseDown_Tick(object sender, EventArgs e)
        {
            if (internalcount_Axis3 >= 76000)
            {
                wAxis = CTDw.CTD_AXIS_3;
                CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_STOP, 100);
                if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                {
                    CTDw.CTDwMode2Write(gwBsn, wAxis, 0x35);

                    timerMouseDown.Enabled = false;
                    timerTurnLimitOn.Enabled = true;
                }
            }
        }

        private void timerTurnLimitOn_Tick(object sender, EventArgs e)
        {
            if (internalcount_Axis3 < 76000)
            {
                timerTurnLimitOn.Enabled = false;
                timerMouseDown.Enabled = true;
            }
        }
        private void timerGetInternalCounter_Tick(object sender, EventArgs e)
        {
            int lngTemp = 0;
            CTD.CTDw.CTDwGetInternalCounter(gwBsn, AXIS_3, ref lngTemp);
            internalcount_Axis3 = lngTemp;
        }
        private void btnT_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_2;
            MouseDownTop(wAxis);
        }

        private void btnT_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis);
        }

        private void btnTR_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseDownRight(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseDownTop(wAxis2);
        }

        private void btnTR_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis2);
        }

        private void btnR_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_1;
            MouseDownRight(wAxis);
        }

        private void btnR_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis);
        }

        private void btnBR_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseDownRight(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseDownBottom(wAxis2);
        }

        private void btnBR_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis2);
        }

        private void btnB_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_2;
            MouseDownBottom(wAxis);
        }

        private void btnB_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis);
        }

        private void btnBL_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseDownLeft(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseDownBottom(wAxis2);
        }

        private void btnBL_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis2);
        }

        private void btnL_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_1;
            MouseDownLeft(wAxis);
        }

        private void btnL_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis);
        }

        private void btnTL_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseDownLeft(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseDownTop(wAxis2);
        }

        private void btnTL_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis1 = CTDw.CTD_AXIS_1;
            MouseUpAll(wAxis1);
            wAxis2 = CTDw.CTD_AXIS_2;
            MouseUpAll(wAxis2);
        }

        private void btnCW_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_3;
            MouseDownCW(wAxis);
        }

        private void btnCW_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_3;
            MouseUpAll(wAxis);
        }

        private void btnCCW_MouseDown(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_3;
            MouseDownCCW(wAxis);    
        }

        private void btnCCW_MouseUp(object sender, MouseEventArgs e)
        {
            wAxis = CTDw.CTD_AXIS_3;
            MouseUpAll(wAxis);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            InternalCount();
            if (0 > internalcount3)
                CTDw.CTDwDataFullWrite(wBsn, AXIS_3, CTDw.CTD_PLUS_PRESET_PULSE_DRIVE, 0 - internalcount3);
            else if (0 < internalcount3)
                CTDw.CTDwDataFullWrite(wBsn, AXIS_3, CTDw.CTD_MINUS_PRESET_PULSE_DRIVE, internalcount3 - 0);
        }
        public void MouseDownTop(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_MINUS_CONTINUOUS_DRIVE, 100) == 0)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }
        public void MouseDownBottom(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_PLUS_CONTINUOUS_DRIVE, 100) == 0)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }
        public void MouseDownRight(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_PLUS_CONTINUOUS_DRIVE, 100) == 0)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }
        public void MouseDownLeft(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_MINUS_CONTINUOUS_DRIVE, 100) == 0)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }
        public void MouseDownCW(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_MINUS_CONTINUOUS_DRIVE, 100) == 0)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }
        public void MouseDownCCW(short wAxis)
        {
            if (internalcount_Axis3 < 76000)
            {
                if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
                {
                    //ErrorMessage(wBsn);
                    return;
                }

                if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_PLUS_CONTINUOUS_DRIVE, 100) == 0)
                {
                    //ErrorMessage(wBsn);
                    return;
                }
            }
        }
        public void MouseUpAll(short wAxis)
        {
            if (speedSetting.CTD_InitAxis_Jog(wBsn, wAxis, setMode1, setMode2, speed) == false)
            {
                //ErrorMessage(wBsn);
                return;
            }

            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_STOP, 1) == 100)
            {
                //ErrorMessage(wBsn);
                return;
            }
        }

        private void btnSlow_Click(object sender, EventArgs e)
        {
            speed = 0;
            btnSlow.BackColor = Color.LimeGreen;
            btnMeduim.BackColor = Color.Gray;
            btnFast.BackColor = Color.Gray;
        }

        private void btnMeduim_Click(object sender, EventArgs e)
        {
            speed = 1;
            btnSlow.BackColor = Color.Gray;
            btnMeduim.BackColor = Color.LimeGreen;
            btnFast.BackColor = Color.Gray;
        }

        private void btnFast_Click(object sender, EventArgs e)
        {
            speed = 2;
            btnSlow.BackColor = Color.Gray;
            btnMeduim.BackColor = Color.Gray;
            btnFast.BackColor = Color.LimeGreen;
        }

        private void Motor_Load(object sender, EventArgs e)
        {
            speed = 1;
            btnSlow.BackColor = Color.Gray;
            btnMeduim.BackColor = Color.LimeGreen;
            btnFast.BackColor = Color.Gray;
        }

        #region Inspection
        HDlModel ModelHandle = null;
        HDict DLParam = null;
        bool LoadImageActived = false;
        HDict Sample = new HDict();
        HImage PrepareImage = new HImage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_LoadModel_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DoInitialDL();
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearModel()
        {
            if (ModelHandle != null)
            {
                ModelHandle.ClearHandle();
                ModelHandle.Dispose();
            }
            ModelHandle = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDevice()
        {
            if (ModelHandle == null) return;
            CenterMethod.Set_Device(CB_DeviceList.Text, ref ModelHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadImages_Click(object sender, EventArgs e)
        {

            if (FBD.ShowDialog(this) == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                LoadImageActived = true;
                DGV_ImageLists.Rows.Clear();
                DGV_ImageLists.RowCount = Directory.GetFiles(FBD.SelectedPath, "*", SearchOption.AllDirectories).Length;
                int Count = 0;

                foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(FBD.SelectedPath).GetFiles())
                {
                    DGV_ImageLists.Rows[Count].Cells[0].Value = Count + 1;
                    DGV_ImageLists.Rows[Count].Cells[1].Value = fi.FullName;
                    Count += 1;
                }
                LoadImageActived = false;
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_Inspection_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Status1 = true;
            Status2 = true;
            PassCounter1 = 0;
            PassCounter2 = 0;
            Counter = DGV_ImageLists.RowCount;
            for (int i = 0; i < DGV_ImageLists.RowCount; i++)
            {

                if (DGV_ImageLists.Rows[i].Cells[0] != null)
                {
                    string FileName = DGV_ImageLists.Rows[i].Cells[1].Value.ToString();

                    try
                    {
                        HImage Image = new HImage();
                        HTuple DLResults;
                        HTuple ClassName = "";
                        HTuple FilterName = "";

                        Image.ReadImage(FileName);
                        DoPrepareImage(Image);
                        DoInspection(Image, out DLResults);
                        ShowResult(DLResults, i, ref ClassName, ref FilterName);
                        ShowImage(Image);
                        ShowHeatmapImage(Image, DLResults);
                        if (ClassName != "ok" && Status1 == true) Status1 = false;
                        if (FilterName != "ok" && Status2 == true) Status2 = false;

                        if (ClassName == "ok") PassCounter1 += 1;
                        if (FilterName == "ok") PassCounter2 += 1;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ":" + FileName);
                    }
                }
            }
            LB_Status1.Text = Status1 ? "Pass" : "Fail";
            LB_Status2.Text = Status2 ? "Pass" : "Fail";
            LB_Yield1.Text = Math.Round((PassCounter1 / (Counter + 0.0000000000001)) * 100, 2).ToString();
            LB_Yield2.Text = Math.Round((PassCounter2 / (Counter + 0.0000000000001)) * 100, 2).ToString();
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_ImageLists_SelectionChanged(object sender, EventArgs e)
        {
            if (this.Created == false || LoadImageActived == true) return;
            Cursor = Cursors.WaitCursor;
            HImage Image = new HImage();
            HDict Sample = new HDict();
            HTuple DLResults = new HTuple();
            int Index = DGV_ImageLists.SelectedRows[0].Index;
            string FileName = DGV_ImageLists.Rows[Index].Cells[1].Value.ToString();

            try
            {
                HTuple ClassName = "";
                HTuple FilterName = "";
                Image.ReadImage(FileName);
                DoPrepareImage(Image);
                DoInspection(Image, out DLResults);
                ShowResult(DLResults, Index, ref ClassName, ref FilterName);
                ShowImage(Image);
                ShowHeatmapImage(Image, DLResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + FileName);
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Results"></param>
        /// <param name="Index"></param>
        /// <param name="ClassName"></param>
        /// <param name="FilterName"></param>
        private void ShowResult(HTuple Results, int Index, ref HTuple ClassName, ref HTuple FilterName)
        {
            try
            {
                HDict Result = new HDict(Results.H);
                HTuple Confidences = Result.GetDictTuple("anomaly_score");
                HTuple ClassIds = Result.GetDictTuple("anomaly_class_id");
                ClassName = Result.GetDictTuple("anomaly_class");
                FilterName = Result.GetDictTuple("filter_class");
                HRegion Region = new HRegion(Result.GetDictObject("filter_region"));
                HTuple MinArea = Result.GetDictTuple("min_area");

                DGV_ImageLists.Rows[Index].Cells[2].Value = ClassName.S;
                DGV_ImageLists.Rows[Index].Cells[3].Value = Math.Round(Confidences.D, 2);

                DGV_ImageLists.Rows[Index].Cells[4].Value = FilterName.S;
                DGV_ImageLists.Rows[Index].Cells[5].Value = Math.Round(MinArea.D, 2);
                DGV_ImageLists.Rows[Index].Cells[6].Value = Math.Round(SW.Elapsed.TotalMilliseconds, 2);

                if (ip3000Result != null)
                {
                    ip3000Result.AOIResult = FilterName.S;
                    ip3000Result.ClsScore = Math.Round(Confidences.D, 2).ToString();
                    ip3000Result.FilterName = FilterName.S;
                    ip3000Result.MinArea = Math.Round(MinArea.D, 2).ToString();
                    ip3000Result.AOIProcessTime = Math.Round(SW.Elapsed.TotalMilliseconds, 2).ToString();
                    //if (curProductSide == ProductSide.Bottom)
                    //{
                    //    ip3000Result.ClsScoreSpec = listRecipe.OneSideRecipes[0].ListPointRecipe[Index].ClsThreshold.ToString();
                    //    ip3000Result.SegmentScoreSpec = listRecipe.OneSideRecipes[0].ListPointRecipe[Index].SegmentThreshold.ToString();
                    //}
                    //else if(curProductSide == ProductSide.PCBA)
                    //{
                    //    ip3000Result.ClsScoreSpec = listRecipe.OneSideRecipes[1].ListPointRecipe[Index].ClsThreshold.ToString();
                    //    ip3000Result.SegmentScoreSpec = listRecipe.OneSideRecipes[1].ListPointRecipe[Index].SegmentThreshold.ToString();
                    //}
                    //else if (curProductSide == ProductSide.Top)
                    //{
                    //    ip3000Result.ClsScoreSpec = listRecipe.OneSideRecipes[2].ListPointRecipe[Index].ClsThreshold.ToString();
                    //    ip3000Result.SegmentScoreSpec = listRecipe.OneSideRecipes[2].ListPointRecipe[Index].SegmentThreshold.ToString();
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RawImage"></param>
        /// <param name="ImagePreprocessed"></param>
        private void Prepare_Image(HImage RawImage, ref HImage ImagePreprocessed)
        {
            try
            {

                int ImageWidth = Int32.Parse(DLParam.GetDictTuple("image_width").O.ToString());
                int ImageHeight = Int32.Parse(DLParam.GetDictTuple("image_height").O.ToString());

                int ImageNumChannels = Int32.Parse(DLParam.GetDictTuple("image_num_channels").O.ToString());
                int ImageRangeMin = Int32.Parse(DLParam.GetDictTuple("image_range_min").O.ToString());
                int ImageRangeMax = Int32.Parse(DLParam.GetDictTuple("image_range_max").O.ToString());
                string domain_handling = DLParam.GetDictTuple("domain_handling");
                string normalization_type = DLParam.GetDictTuple("normalization_type");
                HTuple ImageType = RawImage.GetImageType();
                HTuple NumMatches = ImageType.TupleRegexpTest("byte|int|real");
                HTuple InputNumChannels = RawImage.CountChannels();

                if (domain_handling == "crop_domain")
                    RawImage = RawImage.CropDomain();
                else if (domain_handling == "full_domain")
                    RawImage = RawImage.FullDomain();

                //*Convert the images to real and zoom the images.
                RawImage = RawImage.ConvertImageType("real");
                RawImage = RawImage.ZoomImageSize(ImageWidth, ImageHeight, "constant");

                if (normalization_type == "none")
                {
                    Preprocess_DL_Model_Image_None(ImageType, ImageRangeMin, ImageRangeMax, ref RawImage);
                }

                ImagePreprocessed = RawImage;
                ImageType = null;
                NumMatches = null;
                InputNumChannels = null;
            }
            catch (Exception ex)
            {

                ex = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageType"></param>
        /// <param name="ImageRangeMin"></param>
        /// <param name="ImageRangeMax"></param>
        /// <param name="Image"></param>
        private static void Preprocess_DL_Model_Image_None(HTuple ImageType, int ImageRangeMin, int ImageRangeMax, ref HImage Image)
        {
            try
            {
                HTuple Indices = ImageType.TupleFind("byte");
                if (Indices.TupleNotEqual(-1))
                {
                    //*Shift the gray values from[0 - 255] to the expected range for byte images.
                    HTuple RescaleRange = (ImageRangeMax - ImageRangeMin) / 255.0;
                    HImage ImageSelected = Image.SelectObj(Indices + 1);
                    ImageSelected = ImageSelected.ScaleImage(RescaleRange, ImageRangeMin);
                    Image = Image.ReplaceObj(ImageSelected, Indices + 1);
                }
            }
            catch (Exception ex)
            {
                ex = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoInitialDL()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Model";
            dlg.Filter = "Models|*.hdl";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Anomaly";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string modelName = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                lbModelName.Text = "Model Name: " + modelName;

                ClearModel();
                ModelHandle = new HDlModel();
                DLParam = new HDict();
                ModelHandle.ReadDlModel(modelName);
                DLParam.ReadDict(modelName + "_dl_preprocess_params.hdict", new HTuple(), new HTuple());
                SetDevice();
                HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
                HDict _data = new HDict(meta_data.H);
                Txt_ClassificationThreshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_classification_threshold").S), 2).ToString();
                Txt_Segmentation_Threshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_segmentation_threshold").S), 2).ToString();


            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoInitialDLPCBA()
        {
            string modelName = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.BaseDirectory + "Anomaly\\pcbd00_spvision.hdl");
            lbModelName.Text = "Model Name: " + modelName;

            ClearModel();
            ModelHandle = new HDlModel();
            ModelHandle.ReadDlModel(@"Anomaly\" + modelName);
            DLParam = new HDict();
            DLParam.ReadDict(@"Anomaly\" + modelName + "_dl_preprocess_params", new HTuple(), new HTuple());
            SetDevice();
            HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
            HDict _data = new HDict(meta_data.H);
            Txt_ClassificationThreshold.Text = "0.3";
            Txt_Segmentation_Threshold.Text = "0.67";
            Txt_MinArea.Text = "80000";
            //Txt_ClassificationThreshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_classification_threshold").S), 2).ToString();
            //Txt_Segmentation_Threshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_segmentation_threshold").S), 2).ToString();

            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = listRecipe.OneSideRecipes[1].PathGoodImage + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void DoInitialDLHeatsink()
        {
            string modelName = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.BaseDirectory + "Anomaly\\hsd00_spvision.hdl");
            lbModelName.Text = "Model Name: " + modelName;

            ClearModel();
            ModelHandle = new HDlModel();
            ModelHandle.ReadDlModel(@"Anomaly\" + modelName);
            DLParam = new HDict();
            DLParam.ReadDict(@"Anomaly\" + modelName + "_dl_preprocess_params", new HTuple(), new HTuple());
            SetDevice();
            HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
            HDict _data = new HDict(meta_data.H);
            Txt_ClassificationThreshold.Text = "0.3";
            Txt_Segmentation_Threshold.Text = "0.67";
            Txt_MinArea.Text = "90000";
            //Txt_ClassificationThreshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_classification_threshold").S), 2).ToString();
            //Txt_Segmentation_Threshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_segmentation_threshold").S), 2).ToString();

            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = listRecipe.OneSideRecipes[2].PathGoodImage + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void DoInitialDLBottom()
        {
            string modelName = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.BaseDirectory + "Anomaly\\bsd00_spvision.hdl");
            lbModelName.Text = "Model Name: " + modelName;

            ClearModel();
            ModelHandle = new HDlModel();
            DLParam = new HDict();
            ModelHandle.ReadDlModel(@"Anomaly\" + modelName);
            DLParam.ReadDict(@"Anomaly\" + modelName + "_dl_preprocess_params", new HTuple(), new HTuple());
            //ModelHandle.ReadDlModel(@"D:\IP3000\FABRINET_Model Files\Model\Bottom\Model\Jig_AddSample_model_Training-241121-094320_opt.hdl");
            //DLParam.ReadDict(@"D:\IP3000\FABRINET_Model Files\Model\Bottom\Model\Jig_AddSample_model_Training-241121-094320_opt_dl_preprocess_params.hdict", new HTuple(), new HTuple());
            SetDevice();
            HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
            HDict _data = new HDict(meta_data.H);
            Txt_ClassificationThreshold.Text = "0.3";
            Txt_Segmentation_Threshold.Text = "0.67";
            Txt_MinArea.Text = "40000";
            //Txt_ClassificationThreshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_classification_threshold").S), 2).ToString();
            //Txt_Segmentation_Threshold.Text = Math.Round(double.Parse(_data.GetDictTuple("anomaly_segmentation_threshold").S), 2).ToString();

            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }
        }

        private void DoInitializeDLModel(string ModelName)
        {
            string modelName = System.IO.Path.GetFileNameWithoutExtension(ModelName);
            lbModelName.Text = "Model Name: " + modelName;

            ClearModel();
            ModelHandle = new HDlModel();
            DLParam = new HDict();
            ModelHandle.ReadDlModel(@"Anomaly\" + modelName);
            DLParam.ReadDict(@"Anomaly\" + modelName + "_dl_preprocess_params", new HTuple(), new HTuple());
            SetDevice();
            HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
            HDict _data = new HDict(meta_data.H);
            Txt_ClassificationThreshold.Text = "0.25";
            Txt_Segmentation_Threshold.Text = "0.67";
            int minArea = 0;
            if (modelName.Contains("Bottom"))
            {
                minArea = 40000;
            }
            else if (modelName.Contains("PCBA"))
            {
                minArea = 100000;
            }
            else
            {
                minArea = 90000;
            }

            Txt_MinArea.Text = minArea.ToString();
            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = listRecipe.OneSideRecipes[0].PathGoodImage + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        private void DoPrepareImage(HImage Image)
        {
            SW.Restart();
            Prepare_Image(Image, ref PrepareImage);
            Sample.SetDictObject(PrepareImage, "image");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="DLResults"></param>
        private void DoInspection(HImage Image, out HTuple DLResults)
        {
            HOperatorSet.ApplyDlModel(ModelHandle, Sample, new HTuple(), out DLResults);
            Threshold_DL_Anomaly_Results(Image, ref DLResults);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DLResults"></param>
        private void Threshold_DL_Anomaly_Results(HImage Image, ref HTuple DLResults)
        {
            try
            {
                HTuple AnomalySegmentationThreshold = 0;
                HTuple AnomalyClassificationThreshold = 0;
                HTuple meta_data = ModelHandle.GetDlModelParam("meta_data");
                HDict _data = new HDict(meta_data.H);

                AnomalySegmentationThreshold = double.Parse(Txt_Segmentation_Threshold.Text);
                AnomalyClassificationThreshold = double.Parse(Txt_ClassificationThreshold.Text);
                for (int i = 0; i < DLResults.Length; i++)
                {
                    //if (curProductSide == ProductSide.Top)
                    //{
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[0].ListPointRecipe[i].ClsThreshold;
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[0].ListPointRecipe[i].SegmentThreshold;
                    //}
                    //else if (curProductSide == ProductSide.Bottom)
                    //{
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[1].ListPointRecipe[i].ClsThreshold;
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[1].ListPointRecipe[i].SegmentThreshold;
                    //}
                    //else if (curProductSide == ProductSide.PCBA)
                    //{
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[2].ListPointRecipe[i].ClsThreshold;
                    //    AnomalyClassificationThreshold = listRecipe.OneSideRecipes[2].ListPointRecipe[i].SegmentThreshold;
                    //}

                    HDict DLResult = new HDict(DLResults[i].H);
                    HImage AnomalyImage = new HImage(DLResult.GetDictObject("anomaly_image"));
                    HTuple AnomalyScore = DLResult.GetDictTuple("anomaly_score");
                    HRegion AnomalyRegion = AnomalyImage.Threshold(AnomalySegmentationThreshold, new HTuple(1.0));

                    DLResult.SetDictObject(AnomalyRegion, "anomaly_region");
                    //*Classify sample as 'ok' or 'nok'.
                    int ClassID = 0;
                    string ClassName = "nok";
                    if (AnomalyScore >= AnomalyClassificationThreshold)
                    {
                        ClassID = 0;
                        ClassName = "nok";
                        //if (inspectionResult == "Pass")
                        //    inspectionResult = "Fail";
                    }
                    else
                    {
                        ClassID = 1;
                        ClassName = "ok";
                    }

                    DLResult.SetDictTuple("anomaly_class", ClassName);
                    DLResult.SetDictTuple("anomaly_class_id", ClassID);
                    DLResult.SetDictTuple("anomaly_classification_threshold", AnomalyClassificationThreshold);
                    DLResult.SetDictTuple("anomaly_segmentation_threshold", AnomalySegmentationThreshold);
                    Gen_RegionFromHeatmapImage(Image, ref DLResult);

                    DLResults[i].H = DLResult;
                }

            }
            catch (Exception ex)
            {
                ex = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetClassName(int ID)
        {
            string Value = "";
            HTuple GetTupleClassID = ModelHandle.GetDlModelParam("class_ids");
            HTuple GetTupleClassName = ModelHandle.GetDlModelParam("class_names");
            for (int i = 0; i < GetTupleClassID.Length; i++)
            {
                if (GetTupleClassID[i].I == ID)
                {
                    Value = GetTupleClassName[i].S;
                    break;
                }
            }
            return Value;
        }

        private void Gen_RegionFromHeatmapImage(HImage Image, ref HDict Result)
        {
            try
            {
                HImage ImageHeatmap = new HImage(Result.GetDictObject("anomaly_image"));
                HRegion Region = new HRegion(Result.GetDictObject("anomaly_region"));
                Add_ColorMap_To_Image(ImageHeatmap, PrepareImage, "jet", ref ImageHeatmap);

                int SW = 0;
                int SH = 0;
                int DW = 0;
                int DH = 0;

                Image.GetImageSize(out SW, out SH);
                PrepareImage.GetImageSize(out DW, out DH);
                ImageHeatmap = ImageHeatmap.ZoomImageSize(SW, SH, "constant");
                double ZW = SW / (double)DW;
                double ZH = SH / (double)DH;
                Region = Region.ZoomRegion(ZW, ZH);

                Result.SetDictObject(ImageHeatmap, "image_heatmap");

                string ClassName = Result.GetDictTuple("anomaly_class");
                if (ClassName == "ok")
                {
                    Region.GenEmptyRegion();
                    Result.SetDictTuple("min_area", 0);
                    Result.SetDictObject(Region, "filter_region");
                    Result.SetDictTuple("filter_class", "ok");
                    Result.SetDictTuple("filter_class_id", 1);
                    return;
                }

                Region = Region.Connection();
                double MinArea = Convert.ToDouble(Txt_MinArea.Text);
                Region = Region.SelectShape("area", "and", MinArea, 9999999.0);

                if (Region.CountObj() == 0)
                {
                    Result.SetDictTuple("filter_class", "ok");
                    Result.SetDictTuple("filter_class_id", 1);
                    Result.SetDictTuple("min_area", 0);
                    Result.SetDictObject(Region, "filter_region");
                }
                else
                {
                    Result.SetDictTuple("filter_class", "nok");
                    Result.SetDictTuple("filter_class_id", 0);
                    HTuple Area = Region.RegionFeatures("area");
                    Result.SetDictTuple("min_area", Area.TupleMin());
                    Region = Region.Union1();
                    Result.SetDictObject(Region, "filter_region");
                    if (inspectionResult == "Pass")
                        inspectionResult = "Fail";
                }
            }
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region GUI

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        private void ShowImage(HImage Image)
        {
            try
            {
                WindowControl.HalconWindow.ClearWindow();
                WindowControl.HalconWindow.AttachBackgroundToWindow(Image);
                WindowControl.HalconWindow.SetPart(0, 0, -2, -2);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Results"></param>
        private void ShowHeatmapImage(HImage Image, HTuple Results)
        {
            try
            {
                HDict Result = new HDict(Results.H);
                HImage ImageHeatmap = new HImage(Result.GetDictObject("anomaly_image"));
                HRegion Region = new HRegion(Result.GetDictObject("anomaly_region"));
                Add_ColorMap_To_Image(ImageHeatmap, PrepareImage, "jet", ref ImageHeatmap);

                int SW = 0;
                int SH = 0;
                int DW = 0;
                int DH = 0;

                Image.GetImageSize(out SW, out SH);
                PrepareImage.GetImageSize(out DW, out DH);

                ImageHeatmap = ImageHeatmap.ZoomImageSize(SW, SH, "constant");

                double ZW = SW / DW;
                double ZH = SH / DH;
                Region = Region.ZoomRegion(ZW, ZH);


                hSmartWindowControl1.HalconWindow.ClearWindow();
                hSmartWindowControl1.HalconWindow.AttachBackgroundToWindow(ImageHeatmap);
                hSmartWindowControl1.HalconWindow.SetPart(0, 0, -2, -2);

                WindowControl.HalconWindow.SetDraw("margin");
                WindowControl.HalconWindow.SetLineWidth(2);
                WindowControl.HalconWindow.DispRegion(Region);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowDeviceList()
        {
            CB_DeviceList.Items.Clear();
            for (int i = 0; i < CenterMethod.DeviceList.Count; i++)
            {
                CB_DeviceList.Items.Add(CenterMethod.DeviceList[i]);
            }
            CB_DeviceList.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDevice();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GrayValueImage"></param>
        /// <param name="Image"></param>
        /// <param name="HeatmapColorScheme"></param>
        /// <param name="ColoredImage"></param>
        private void Add_ColorMap_To_Image(HImage GrayValueImage, HImage Image, HTuple HeatmapColorScheme, ref HImage ColoredImage)
        {
            try
            {
                // *This procedure adds a gray-value image to a RGB image with a chosen colormap.
                string ImageType = GrayValueImage.GetImageType();
                //* The image LUT needs a byte image. Rescale real images.
                if (ImageType == "real")
                {
                    Scale_Image_Range(GrayValueImage, ref GrayValueImage, 0, 1);
                    GrayValueImage = GrayValueImage.ConvertImageType("byte");
                }

                HImage RGBValueImage = null;
                //*** Apply the chosen color scheme on the gray value.
                Apply_Colorscheme_On_Gray_Value_Image(GrayValueImage, HeatmapColorScheme, ref RGBValueImage);

                //*Convert input image to byte image for visualization.
                HImage Channels = Image.ImageToChannels();
                int NumChannels = Image.CountChannels();
                HImage ChannelsScaled = new HImage();
                ChannelsScaled.GenEmptyObj();

                for (int ChannelIndex = 1; ChannelIndex <= NumChannels; ChannelIndex++)
                {

                    HImage Channel = Channels.SelectObj(ChannelIndex);
                    HImage ChannelScaled = new HImage();
                    Channel.MinMaxGray(Channel, 0, out HTuple ChannelMin, out HTuple ChannelMax, out HTuple Range);
                    Scale_Image_Range(Channel, ref ChannelScaled, ChannelMin, ChannelMax);
                    HImage ChannelScaledByte = ChannelScaled.ConvertImageType("byte");

                    ChannelsScaled = ChannelsScaled.ConcatObj(ChannelScaledByte);

                }

                HImage ImageByte = ChannelsScaled.ChannelsToImage();

                //Note that ImageByte needs to have the same number of channels as
                //* RGBValueImage to display colormap image correctly.
                NumChannels = ImageByte.CountChannels();

                if (NumChannels != 3)
                {
                    //*Just take the first channel and use this to generate
                    //*an image with 3 channels for visualization.
                    HImage ImageByteR = ImageByte.AccessChannel(1);
                    HImage ImageByteG = ImageByteR.CopyImage();
                    HImage ImageByteB = ImageByteR.CopyImage();
                    ImageByte = ImageByteR.Compose3(ImageByteG, ImageByteB);
                }

                RGBValueImage = ImageByte.AddImage(RGBValueImage, 0.5, 0);
                ColoredImage = RGBValueImage;

            }
            catch (Exception ex)
            {
                ex = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="ImageScaled"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        private void Scale_Image_Range(HImage Image, ref HImage ImageScaled, HTuple Min, HTuple Max)
        {
            try
            {
                HTuple LowerLimit, UpperLimit, Mult, Add;
                if (Min.Length == 2)
                {
                    LowerLimit = Min[1];
                    Min = Min[0];
                }
                else
                {
                    LowerLimit = 0.0;
                }
                if (Max.Length == 2)
                {
                    UpperLimit = Max[1];
                    Max = Max[0];
                }
                else
                {
                    UpperLimit = 255.0;
                }

                //*Calculate scaling parameters. Only scale if the scaling range is not zero.
                HTuple DiffMaxMin = (Max - Min).TupleAbs();
                if (DiffMaxMin.TupleLess(1.0E-6).TupleNot())
                {
                    Mult = (UpperLimit - LowerLimit).TupleReal() / (Max - Min);
                    Add = -Mult * Min + LowerLimit;
                    //* Scale image.
                    Image = Image.ScaleImage(Mult, Add);
                }

                //*Clip gray values if necessary.
                //* This must be done for each image and channel separately.
                ImageScaled = new HImage();
                ImageScaled.GenEmptyObj();

                int NumImages = Image.CountObj();
                HImage ImageSelectedScaled = new HImage();
                for (int ImageIndex = 1; ImageIndex <= NumImages; ImageIndex++)
                {
                    HImage ImageSelected = new HImage(Image.SelectObj(ImageIndex));

                    int Channels = ImageSelected.CountChannels();

                    for (int ChannelIndex = 1; ChannelIndex <= Channels; ChannelIndex++)
                    {
                        HImage SelectedChannel = ImageSelected.AccessChannel(ChannelIndex);
                        HTuple MinGray, MaxGray, Range;
                        SelectedChannel.MinMaxGray(SelectedChannel, 0, out MinGray, out MaxGray, out Range);
                        HRegion LowerRegion = SelectedChannel.Threshold(MinGray.TupleMin2(LowerLimit), LowerLimit);
                        HRegion UpperRegion = SelectedChannel.Threshold(UpperLimit, UpperLimit.TupleMax2(MaxGray));
                        SelectedChannel = LowerRegion.PaintRegion(SelectedChannel, LowerLimit, "fill");
                        SelectedChannel = UpperRegion.PaintRegion(SelectedChannel, UpperLimit, "fill");

                        if (ChannelIndex == 1)
                            ImageSelectedScaled = SelectedChannel.CopyObj(1, 1);
                        else
                            ImageSelectedScaled = ImageSelectedScaled.AppendChannel(ImageSelectedScaled);

                    }// end loop for
                    ImageScaled = ImageScaled.ConcatObj(ImageSelectedScaled);
                }// end loop for
            }
            catch (Exception ex)
            {
                ex = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputImage"></param>
        /// <param name="Schema"></param>
        /// <param name="ResultImage"></param>
        private void Apply_Colorscheme_On_Gray_Value_Image(HImage InputImage, HTuple Schema, ref HImage ResultImage)
        {
            try
            {
                //*This procedure generates an RGB ResultImage for a grey-value InputImage.
                //*according to the Schema.
                HTuple X = HTuple.TupleGenSequence(0, 255, 1);
                HOperatorSet.TupleGenConst(256, 0, out HTuple Low);
                HOperatorSet.TupleGenConst(256, 255, out HTuple High);
                HTuple R = null, G = null, B = null;

                if (Schema == "jet")
                {
                    //*Scheme Jet: from blue to red
                    HTuple OffR = 3.0 * 64.0;
                    HTuple OffG = 2.0 * 64.0;
                    HTuple OffB = 64.0;
                    HTuple A1 = -4.0;
                    HTuple A0 = 255.0 + 128.0;

                    R = ((X - OffR).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    G = ((X - OffG).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    B = ((X - OffB).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);

                }
                else if (Schema == "inverse_jet")
                {
                    //* Scheme InvJet: from red to blue.
                    HTuple OffR = 64;
                    HTuple OffG = 2 * 64;
                    HTuple OffB = 3 * 64;
                    HTuple A1 = -4.0;
                    HTuple A0 = 255.0 + 128.0;
                    R = ((X - OffR).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    G = ((X - OffG).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    B = ((X - OffB).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);

                }
                else if (Schema == "hot")
                {
                    //* Scheme Hot.
                    HTuple A1 = 3.0;
                    HTuple A0R = 0.0;
                    HTuple A0G = 1.0 / 3.0 * A1 * 255.0;
                    HTuple A0B = 2.0 / 3.0 * A1 * 255.0;
                    R = (X.TupleAbs() * A1 - A0R).TupleMax2(Low).TupleMin2(High);
                    G = (X.TupleAbs() * A1 - A0G).TupleMax2(Low).TupleMin2(High);
                    B = (X.TupleAbs() * A1 - A0B).TupleMax2(Low).TupleMin2(High);
                }
                else if (Schema == "inverse_hot")
                {
                    //* Scheme Inverse Hot.
                    HTuple A1 = -3.0;
                    HTuple A0R = A1 * 255.0;
                    HTuple A0G = 2.0 / 3.0 * A1 * 255.0;
                    HTuple A0B = 1.0 / 3.0 * A1 * 255.0;
                    R = (X.TupleAbs() * A1 - A0R).TupleMax2(Low).TupleMin2(High);
                    G = (X.TupleAbs() * A1 - A0G).TupleMax2(Low).TupleMin2(High);
                    B = (X.TupleAbs() * A1 - A0B).TupleMax2(Low).TupleMin2(High);
                }


                HImage ImageR = InputImage.LutTrans(R);
                HImage ImageG = InputImage.LutTrans(G);
                HImage ImageB = InputImage.LutTrans(B);

                ResultImage = ImageR.Compose3(ImageG, ImageB);


            }
            catch (Exception ex)
            {
                ex = null;
            }

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Avatar(object sender, EventArgs e)
        {
            try
            {
                string imgsrc = $"http://fits/emp_pic/" + tbEN.Text + $".jpg";
                ////var request = WebRequest.Create(imgsrc);
                ////using (var response = request.GetResponse())
                ////using (var stream = response.GetResponseStream())
                ////{
                ////    pictureBox1.BackgroundImage = Bitmap.FromStream(stream);
                ////    pictureBox1.BackgroundImageLayout = ImageLayout.Center;
                ////}

                //Use area
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(imgsrc, HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var streamReader = new StreamReader(stream))
                    {
                        pictureBox1.BackgroundImage = Bitmap.FromStream(stream);
                        pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                        //pictureBox1.BackgroundImageLayout = ImageLayout.Center;
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExportResult_Click(object sender, EventArgs e)
        {
            string dtStr = DateTime.Now.ToString("yyyyMMdd");
            string dtFldName = baseLogPath + "\\" + dtStr;
            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }

            if(curProductSide == ProductSide.Bottom)
            {
                dtFldName += "\\Bottom";
            }
            else if(curProductSide == ProductSide.PCBA)
            {
                dtFldName += "\\PCBA";
            }
            else if(curProductSide == ProductSide.Top)
            {
                dtFldName += "\\Heatsink";
            }

            if (!Directory.Exists(dtFldName))
            {
                Directory.CreateDirectory(dtFldName);
            }

            if (curSerialNo == null || curSerialNo== string.Empty)
                curSerialNo = dtStr;

            string fileName = dtFldName + "\\" + curSerialNo + ".csv";
            var sb = new StringBuilder();

            if (!File.Exists(fileName))
            {
                var headers = DGV_ImageLists.Columns.Cast<DataGridViewColumn>();
                sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));
                File.WriteAllText(fileName, sb.ToString());
                sb = null;
            }

            sb = new StringBuilder();
            foreach (DataGridViewRow row in DGV_ImageLists.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }
            File.AppendAllText(fileName, sb.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void liveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //artCam2CamFrm = new ArtCam2CamFrm();
            //artCam2CamFrm.Show();
            InitializeTopCamera();  
            InitializeSideCamera();
        }

        private void liveCamera1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArtCamFullCtrlFrm artCamForm = new ArtCamFullCtrlFrm();
            //artCamForm.Show();
            //artCamForm.StartWithCamera(0);
            InitializeTopCamera();
        }

        private void liveCamera2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArtCamFullCtrlFrm artCamForm = new ArtCamFullCtrlFrm();
            //artCamForm.Show();
            //artCamForm.StartWithCamera(1);
            InitializeSideCamera();
        }

        //string imgPatternName = _serialNo + "-" + inspecCamNo.ToString() + "-{" + blockNo.ToString() + "_" + pin.ToString() + "-1_" + stepCheck.ToString() + "}.jpg";

        private void LoadProgram(string programName)
        {
            int totalPoints = 0;
            string prgFileName = System.IO.Path.GetFileNameWithoutExtension(programName);
            lbMotionPrgName.Text = "Motion Name: " + prgFileName;
            //string prgFileName = AppDomain.CurrentDomain.BaseDirectory + "Program"; 
            //if (!Directory.Exists(prgFileName))
            //{
            //    Directory.CreateDirectory(prgFileName);
            //}

            //string programFullName = prgFileName + "\\" + programName + ".prg";
            tbProgramName.Text = prgFileName;
            //tbProgramName.Text = programName.Replace(".prg", "");

            if (File.Exists(programName))
            {
                listView_Model.Items.Clear();
                using (StreamReader sr = new StreamReader(programName))
                {
                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] list = line.Split(',');

                        ListViewItem m_viewItem = new ListViewItem(list);
                        listView_Model.Items.Add(m_viewItem);
                    }
                    sr.Close();
                }
                totalPoints = listView_Model.Items.Count;
                if (listConfinence != null)
                    listConfinence = null;
                listConfinence = new double[totalPoints];
            }
            else
            {
                MessageBox.Show("No prg File in Directory");
            }

        }

        private void tbEN_TextChanged(object sender, EventArgs e)
        {
            Avatar(null, null);
        }

        private void DLInspectionImage(int imageIndex, HImage hImage)
        {
            if (this.Created == false || LoadImageActived == true) return;

            HDict Sample = new HDict();
            HTuple DLResults = new HTuple();
            string FileName = curAoiImageName;

            try
            {
                HTuple ClassName = "";
                HTuple FilterName = "";
                DoPrepareImage(hImage);
                DoInspection(hImage, out DLResults);
                ShowResult(DLResults, imageIndex, ref ClassName, ref FilterName);
                ShowImage(hImage);
                ShowHeatmapImage(hImage, DLResults);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + ":" + FileName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sidePart"></param>
        /// <param name="imageIndex"></param>
        private void DLInspectionImage(int imageIndex, string imageName)
        {
            if (this.Created == false || LoadImageActived == true) return;
            //Cursor = Cursors.WaitCursor;
            HImage Image = new HImage();
            HDict Sample = new HDict();
            HTuple DLResults = new HTuple();
            int Index = DGV_ImageLists.SelectedRows[0].Index;
            string FileName = DGV_ImageLists.Rows[Index].Cells[1].Value.ToString();

            try
            { 
                HTuple ClassName = "";
                HTuple FilterName = "";
                Image.ReadImage(FileName);
                DoPrepareImage(Image);
                DoInspection(Image, out DLResults);
                ShowResult(DLResults, Index, ref ClassName, ref FilterName);
                ShowImage(hImage);
                ShowHeatmapImage(hImage, DLResults);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + ":" + FileName);
            }
        }

        //private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
            //if (tabControl2.SelectedIndex == 3)
            //{
            //    if(_topCamCtrl != null)
            //    {
            //        nTopCamExposure.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetExposureTime());
            //        nTopCamBrightness.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetBrightness());
            //        nTopCamContrast.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetContrast()); 
            //        nTopCamSharpness.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetSharpness());
            //        nTopCamRGain.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetGrayGainR());    
            //        nTopCamG1Gain.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetGrayGainG1());
            //        nTopCamG2Gain.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetGrayGainG2()); 
            //        nTopCamBGain.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetGrayGainB());
            //        nTopCamGamma.Value = Convert.ToDecimal(_topCamCtrl.m_CArtCam.GetGamma());
            //    }

            //    if (_sideCamCtrl != null)
            //    {
            //        nSideCamExposure.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetExposureTime());
            //        nSideCamBrightness.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetBrightness());
            //        nSideCamContrast.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetContrast());
            //        nSideCamSharpness.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetSharpness());
            //        nSideCamRGain.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetGrayGainR());
            //        nSideCamG1Gain.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetGrayGainG1());
            //        nSideCamG2Gain.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetGrayGainG2());
            //        nSideCamBGain.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetGrayGainB());
            //        nSideCamGamma.Value = Convert.ToDecimal(_sideCamCtrl.m_CArtCam.GetGamma()); 
            //    }
                
            //}
        //}

        private void nTopCamExposure_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nSideExposure_ValueChanged(object sender, EventArgs e)
        {
            if (sideCam != null)
                sideCam.m_CArtCam.SetExposureTime((int)nSideCamExposure.Value);

            if (_sideCamCtrl != null)
                _sideCamCtrl.m_CArtCam.SetExposureTime((int)nTopCamExposure.Value);
        }


        private void nCamExposure_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                exposure = (int)nCamExposure.Value;
                if (activeCamera == ActiveCamera.TOP)
                {
                    if (topCam != null)
                        topCam.m_CArtCam.SetExposureTime(exposure);

                    if (_topCamCtrl != null)
                        _topCamCtrl.m_CArtCam.SetExposureTime(exposure);
                }
                else
                {
                    if (sideCam != null)
                        sideCam.m_CArtCam.SetExposureTime(exposure);

                    if (_sideCamCtrl != null)
                        _sideCamCtrl.m_CArtCam.SetExposureTime(exposure);
                }
            }
            catch { }
           
        }

        private void btOneShot_Click(object sender, EventArgs e)
        {
            int i = DGV_ImageLists.RowCount;
            GenImageNamebyPosition();
            AcquireImage();
            Cursor = Cursors.WaitCursor;
            LoadImageActived = true;

            DGV_ImageLists.RowCount = i + 1;
            DGV_ImageLists.Rows[i].Cells[0].Value = i + 1;
            DGV_ImageLists.Rows[i].Cells[1].Value = curAoiImageName;

            LoadImageActived = false;
            Cursor = Cursors.Default;

            DLInspectionImage(i, curAoiImageName);
        }

        private void topCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeTopCamera();
        }

        private void sideCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeSideCamera();
        }

        private void cbLive_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btZoom33_Click(object sender, EventArgs e)
        {
            if(_topCamCtrl != null)
            {
                _topCamCtrl.m_ViewScale = 25;
                _topCamCtrl.SetPreviewCamera(0);
            }
        }

        private void btLiveTopCam_Click(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.StartPreview();
            }
        }

        private void btLiveSideCamera_Click(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.StartPreview(); 
            }
        }

        private void nCam1Brightness_ValueChanged(object sender, EventArgs e)
        {
            if(_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetBrightness((int)nTopCamBrightness.Value);
            }
        }

        private void nCam1Contrast_ValueChanged(object sender, EventArgs e)
        {
            if(_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetContrast((int)nTopCamContrast.Value);
            }
        }

        private void nCam1Exposure_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetExposureTime((int)nTopCamExposure.Value);
            }
        }

        private void nCam1Sharpness_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetSharpness((int)nTopCamSharpness.Value);
            }
        }

        private void nCam1RGain_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetGrayGainR((int)nTopCamRGain.Value);  
            }
        }

        private void nCam1G1Gain_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetGrayGainG1((int)nTopCamG1Gain.Value);
            }
        }

        private void nCam1BGain_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetGrayGainB((int)nTopCamBGain.Value);
            }
        }

        private void nCam1G2Gain_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetGrayGainG2((int)nTopCamG2Gain.Value);
            }
        }

        private void nCam1Gamma_ValueChanged(object sender, EventArgs e)
        {
            if (_topCamCtrl != null)
            {
                _topCamCtrl.m_CArtCam.SetGamma((int)nTopCamGamma.Value);
            }
        }

        private void nCam2Brightness_ValueChanged(object sender, EventArgs e)
        {
            if(_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetBrightness((int)nSideCamBrightness.Value);
            }
        }

        private void nCam2Contrast_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetContrast((int)nSideCamContrast.Value);
            }
        }

        private void nCam2Exposure_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetExposureTime((int)nSideCamExposure.Value);
            }
        }

        private void nCam2Sharpness_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetSharpness((int)nSideCamSharpness.Value);
            }
        }

        private void nCam2RGain_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetGrayGainR((int)nSideCamRGain.Value);
            }
        }

        private void nCam2G1Gain_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetGrayGainG1((int)nSideCamG1Gain.Value);
            }
        }

        private void nCam2BGain_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetGrayGainB((int)nSideCamBGain.Value);
            }
        }

        private void nCam2G2Gain_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetGrayGainG2((int)nSideCamG2Gain.Value);
            }
        }

        private void nCam2Gamma_ValueChanged(object sender, EventArgs e)
        {
            if (_sideCamCtrl != null)
            {
                _sideCamCtrl.m_CArtCam.SetGamma((int)nSideCamGamma.Value);
            }
        }

        private void nTriggerDelay_ValueChanged(object sender, EventArgs e)
        {
            triggerDelay = (int)nTriggerDelay.Value;
        }

        private void cbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string zStr = cbZoom.GetItemText(cbZoom.SelectedItem); 
            int zoomFac = Convert.ToInt32(zStr.Substring(0, zStr.Length - 1));

            if (activeCamera == ActiveCamera.TOP)
            {
                if (_topCamCtrl != null)
                {
                    _topCamCtrl.m_ViewScale = zoomFac;
                    _topCamCtrl.UpdatePreview();
                }
            }
            else
            {
                if (_sideCamCtrl != null)
                {
                    _sideCamCtrl.m_ViewScale = zoomFac;
                    _sideCamCtrl.UpdatePreview();
                }

            }
        }

        private void btClearResults_Click(object sender, EventArgs e)
        {
            DGV_ImageLists.Rows.Clear();
            DGV_ImageLists.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gRRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runMode = RunMode.GRR;
            lbRunMode.Text = "Mode: " + runMode.ToString();

            this.Text += " " + runMode.ToString();
            btOpen_Click(sender, e);    
            btResetGrrCount_Click(sender, e);   

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runMode = RunMode.Production;
            lbRunMode.Text = "Mode: " + runMode.ToString();

            this.Text += " " + runMode.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowseGRRMaster_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;    
                openFileDialog.Filter = "xls (*.xls)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    tbGrrFileName.Text = filePath;
                }
            }
        }

        private void btResetGrrCount_Click(object sender, EventArgs e)
        {
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                var rng = oSheet.get_Range("B11:D39", Type.Missing);
                rng.Clear();

                rng = oSheet.get_Range("F11:H39", Type.Missing);
                rng.Clear();

                rng = oSheet.get_Range("J11:L39", Type.Missing);
                rng.Clear();
                roundGrr = 0;
                tbroundGrr.Text = roundGrr.ToString();

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Add table headers going cell by cell.
                for (int row = 0; row < listConfinence.Length; row++)
                {
                    if (roundGrr < 3)
                    {
                        oSheet.Cells[row + 11, roundGrr + 2] = listConfinence[row];
                    }
                    else if(roundGrr < 6)
                    {
                        oSheet.Cells[row + 11, roundGrr + 3] = listConfinence[row];
                    }
                    else
                    {
                        oSheet.Cells[row + 11, roundGrr + 4] = listConfinence[row];
                    }
                }
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.Message);    
            }

        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                oWB = oXL.Workbooks.Open(tbGrrFileName.Text);
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
            }
            catch (Exception ex)
            {
            }
        }

        private void btSaveGrr_Click(object sender, EventArgs e)
        {
            if (roundGrr == totalRoundGrr)
            {
                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "GRR Results\\" + DateTime.Now.ToString("ddMMyyyy"), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                oXL.Quit();
            }
        }

        private void btStartMove_Click(object sender, EventArgs e)
        {
            int filecount = 0;
            string desPath;
            string bottomBasePath = "D:\\IP3000\\DL\\Training\\Bottom";
            string heatsinkBasePath = "D:\\IP3000\\DL\\Training\\Heatsink";
            string pcbaBasePath = "D:\\IP3000\\DL\\Training\\PCBA";

            string[] dirs = Directory.GetDirectories(tbSrcImages.Text);
            try
            {
                foreach (string dir in dirs)
                {
                    foreach (string f in Directory.GetFiles(dir))
                    {
                        var fName = Path.GetFileName(f);
                        var indx1 = fName.LastIndexOf("_");
                        var indx2 = fName.LastIndexOf("}");
                        var pos = Convert.ToInt32(fName.Substring(indx1 + 1, indx2 - indx1 - 1));

                        if (rdTrainBottom.Checked)
                        {
                            desPath = bottomBasePath + "\\Pos" +  pos.ToString() + "\\" + Path.GetFileName(f);
                            File.Move(f, desPath); // Try to move
                        }
                        else if (rdTrainHeatsink.Checked)
                        {
                            desPath = heatsinkBasePath + "\\Pos" + pos.ToString() + "\\" + Path.GetFileName(f);
                            File.Move(f, desPath); // Try to move
                        }
                        else if(rdTrainPCBA.Checked)
                        {
                            desPath = pcbaBasePath + "\\Pos" + pos.ToString() + "\\" + Path.GetFileName(f);
                            File.Move(f, desPath); // Try to move
                        }
                        filecount++;
                    }
                }
                MessageBox.Show(string.Format("Move training images {0} completed.", filecount));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btBrwsSrcImage_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                var folderName = folderDialog.SelectedPath;
                tbSrcImages.Text = folderName;
            }
        }

        private void masterCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runMode = RunMode.MasterCheck;
            lbRunMode.Text = runMode.ToString();

            this.Text += " " + runMode.ToString();
        }

        private Bitmap ScaleImage(Bitmap image, int width, int height)
        {
            int newWidth = width;
            int newHeight = height;

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipes"></param>
        private void SaveProductRecipe()
        {

        }

        private void btSaveAllRecipe_Click(object sender, EventArgs e)
        {
            pgProductRecipe.Enabled = false;    

            cbCurrentRecipe.Items.Clear();

        }

        private void btEditRecipe_Click(object sender, EventArgs e)
        {
            pgProductRecipe.Enabled = true; 
        }

        private void btAddNewRecipe_Click(object sender, EventArgs e)
        {
            
        }

        private void btInitBottomSide_Click(object sender, EventArgs e)
        {
            InitBottomMotionAndDL();
        }

        private void btInitHeatsink_Click(object sender, EventArgs e)
        {
            InitHeatsinkMotionAndDL();
        }

        private void btInitPCBA_Click(object sender, EventArgs e)
        {
            InitPCBAMotionAndDL();
        }

        private void cbCurrentRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ProductRecipe productRecipe = lstProductRecipe[cbCurrentRecipe.SelectedIndex];
            //pgProductRecipe.SelectedObject = productRecipe;
        }

        //private void btSaveResults_Click(object sender, EventArgs e)
        //{
            //using (var file = File.CreateText(path))
            //{
            //    foreach (var arr in lst)
            //    {
            //        if (String.IsNullOrEmpty(arr)) continue;
            //        file.Write(arr[0]);
            //        for (int i = 1; i < arr.Length; i++)
            //        {
            //            file.Write(',');
            //            file.Write(arr[i]);
            //        }
            //        file.WriteLine();
            //    }
            //}
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInitializeRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }

        }

        private void topSideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //AC1200 Top Side
                cbRecipeList.SelectedIndex = 5;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void bottomSideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //AC1200 Bottom Side
                cbRecipeList.SelectedIndex = 3;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void pCBAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //AC1200 PCBA Side
                cbRecipeList.SelectedIndex = 4;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void topSideToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris Bottom Side
                cbRecipeList.SelectedIndex = 2;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void bottomSideToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris Bottom Side
                cbRecipeList.SelectedIndex = 0;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void pCBAToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris PCBA Side
                cbRecipeList.SelectedIndex = 1;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void cbInspCamera_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bottomSideToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris Bottom Side
                cbRecipeList.SelectedIndex = 6;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void pCBAToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris PCBA Side
                cbRecipeList.SelectedIndex = 7;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

        private void topSideToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //Tigris Top Side
                cbRecipeList.SelectedIndex = 8;
                int index = cbRecipeList.SelectedIndex;
                string recipeName = cbRecipeList.Items[index].ToString();
                LoadProgram(listRecipe.OneSideRecipes[index].MotionPrgName);
                DoInitializeDLModel(listRecipe.OneSideRecipes[index].DLProgramName);
                imgFolder = listRecipe.OneSideRecipes[index].PathGoodImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Recipe Error: " + ex.Message);
            }
        }

    }
}


