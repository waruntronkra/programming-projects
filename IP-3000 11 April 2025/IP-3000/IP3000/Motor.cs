using CTD;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IP3000_Control
{
    public partial class Motor : Form
    {
        public Motor()
        {
            InitializeComponent();
        }
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

            if (internalcount_Axis3 >= 76000)
            {
                CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_STOP, 100);
                //if (gbytBusy[AXIS_1] == 0 && gbytBusy[AXIS_2] == 0 && gbytBusy[AXIS_3] == 0)
                //{
                //    CTDw.CTDwMode2Write(gwBsn, wAxis, 0x35);
                //}
            }
            else
            {
                MouseDownCCW(wAxis);
            }          
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
    }
}
