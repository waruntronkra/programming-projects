using System.Runtime.InteropServices;

namespace CTD
{
    // -----------------------------------------------------------------
    // 補間 データ (詳細は、ソフトウエアマニュアルを参照ください。)
    // -----------------------------------------------------------------
    [StructLayout(LayoutKind.Sequential)]
    public struct CTDPIPDRIVEPARAMETER
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[,] bIpAxis;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bIpKind;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bMovekind;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[,] lObjPoint;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[,] lCenterPoint;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public int[] lN2Data1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public int[] lN2Data2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public int[] lN2Data3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bIpconsin;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bEndkind;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bDrawIn;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        //public byte[] bSync_start;

        public void Initialize()
        {
            bIpAxis = new byte[1, 4];
            bIpKind = new byte[1];
            bMovekind = new byte[1];
            lObjPoint = new int[1, 2];
            lCenterPoint = new int[1, 2];
            lN2Data1 = new int[1];
            lN2Data2 = new int[1];
            lN2Data3 = new int[1];
            bIpconsin = new byte[1];
            bEndkind = new byte[1];
            bDrawIn = new byte[1];
            //bSync_start = new byte[1];
        }

    }

    class CTDw
    {
        public const int FCLK = 32768000; //32.768MHz;

        //-----------------------------------------------------------------
        // Error code
        //-----------------------------------------------------------------
        public const ushort CTD_SUCCESS = 0; // 異状なし（正常終了）;
        public const ushort CTD_ERR_NO_DEVICE = 2;// 使用可能なデバイスがありません;
        public const ushort CTD_ERR_IN_USE = 3; // 指定のデバイスは使用中です;
        public const ushort CTD_ERR_INVALID_BSN = 4; // 無効なボードセレクトナンバーです;
        //          デバイスがバス上にないか
        //          使用宣言されていません
        public const ushort CTD_ERR_INVALID_PORT = 6; // 不正なポートを要求した;
        public const ushort CTD_ERR_PARAMETER = 7; // 引数の値が範囲外です;
        public const ushort CTD_ERR_INVALID_AXIS = 50; // 無効な制御軸を要求した;
        public const ushort CTD_ERR_TRANS = 60; // 送信時の通信エラーです;
        public const ushort CTD_ERR_RECEIVE = 61; // 受信時の通信エラーです;
        public const ushort CTD_ERR_USB_REMOVE = 62; // デバイスが取り外されました;
        public const short CTD_ERR_WRAPDLL = -1; // ラッパー関数内エラー;
        //-----------------------------------------------------------------
        // Axis
        //-----------------------------------------------------------------
        public const short CTD_AXIS_1 = 0;
        public const short CTD_AXIS_2 = 1;
        public const short CTD_AXIS_3 = 2;
        public const short CTD_AXIS_4 = 3;
        //-----------------------------------------------------------------
        // I/O Map
        //-----------------------------------------------------------------
        public const ushort CTD_PORT_DATA1 = 0;
        public const ushort CTD_PORT_DATA2 = 1;
        public const ushort CTD_PORT_DATA3 = 2;
        public const ushort CTD_PORT_DATA4 = 3;
        public const ushort CTD_PORT_COMMAND = 4;
        public const ushort CTD_PORT_MODE1 = 5;
        public const ushort CTD_PORT_MODE2 = 6;
        public const ushort CTD_PORT_UNIVERSAL_SIGNAL = 7;
        public const ushort CTD_PORT_DRIVE_STATUS = 4;
        public const ushort CTD_PORT_END_STATUS = 5;
        public const ushort CTD_PORT_MECHANICAL_SIGNAL = 6;
        //-----------------------------------------------------------------
        // command
        //-----------------------------------------------------------------
        public const byte CTD_RANGE_WRITE = 0x0;
        public const byte CTD_RANGE_READ = 0x1;
        public const byte CTD_START_STOP_SPEED_DATA_WRITE = 0x2;
        public const byte CTD_START_STOP_SPEED_DATA_READ = 0x3;
        public const byte CTD_OBJECT_SPEED_DATA_WRITE = 0x4;
        public const byte CTD_OBJECT_SPEED_DATA_READ = 0x5;
        public const byte CTD_RATE1_DATA_WRITE = 0x6;
        public const byte CTD_RATE1_DATA_READ = 0x7;
        public const byte CTD_RATE2_DATA_WRITE = 0x8;
        public const byte CTD_RATE2_DATA_READ = 0x9;
        public const byte CTD_RATE3_DATA_WRITE = 0xA;
        public const byte CTD_RATE3_DATA_READ = 0xB;
        public const byte CTD_RATE_CHANGE_POINT_1_2_WRITE = 0xC;
        public const byte CTD_RATE_CHANGE_POINT_1_2_READ = 0xD;
        public const byte CTD_RATE_CHANGE_POINT_2_3_WRITE = 0xE;
        public const byte CTD_RATE_CHANGE_POINT_2_3_READ = 0xF;
        public const byte CTD_SLOW_DOWN_REAR_PULSE_WRITE = 0x10;
        public const byte CTD_SLOW_DOWN_REAR_PULSE_READ = 0x11;
        public const byte CTD_NOW_SPEED_DATA_READ = 0x12;
        public const byte CTD_DRIVE_PULSE_COUNTER_READ = 0x13;
        public const byte CTD_PRESET_PULSE_DATA_OVERRIDE = 0x14;
        public const byte CTD_PRESET_PULSE_DATA_READ = 0x15;
        public const byte CTD_DEVIATION_DATA_READ = 0x16;
        public const byte CTD_INPOSITION_WAIT_MODE1_SET = 0x17;
        public const byte CTD_INPOSITION_WAIT_MODE2_SET = 0x18;
        public const byte CTD_INPOSITION_WAIT_MODE_RESET = 0x19;
        public const byte CTD_ALARM_STOP_ENABLE_MODE_SET = 0x1A;
        public const byte CTD_ALARM_STOP_ENABLE_MODE_RESET = 0x1B;
        public const byte CTD_SLOW_DOWN_STOP = 0x1E;
        public const byte CTD_EMERGENCY_STOP = 0x1F;
        public const byte CTD_PLUS_PRESET_PULSE_DRIVE = 0x20;
        public const byte CTD_MINUS_PRESET_PULSE_DRIVE = 0x21;
        public const byte CTD_PLUS_CONTINUOUS_DRIVE = 0x22;
        public const byte CTD_MINUS_CONTINUOUS_DRIVE = 0x23;
        public const byte CTD_PLUS_SIGNAL_SEARCH1_DRIVE = 0x24;
        public const byte CTD_MINUS_SIGNAL_SEARCH1_DRIVE = 0x25;
        public const byte CTD_PLUS_SIGNAL_SEARCH2_DRIVE = 0x26;
        public const byte CTD_MINUS_SIGNAL_SEARCH2_DRIVE = 0x27;
        public const byte CTD_INTERNAL_COUNTER_WRITE = 0x28;
        public const byte CTD_INTERNAL_COUNTER_READ = 0x29;
        public const byte CTD_INTERNAL_COMPARATE_DATA_WRITE = 0x2A;
        public const byte CTD_INTERNAL_COMPARATE_DATA_READ = 0x2B;
        public const byte CTD_EXTERNAL_COUNTER_WRITE = 0x2C;
        public const byte CTD_EXTERNAL_COUNTER_READ = 0x2D;
        public const byte CTD_EXTERNAL_COMPARATE_DATA_WRITE = 0x2E;
        public const byte CTD_EXTERNAL_COMPARATE_DATA_READ = 0x2F;
        public const byte CTD_INTERNAL_PRE_SCALE_DATA_WRITE = 0x30;
        public const byte CTD_INTERNAL_PRE_SCALE_DATA_READ = 0x31;
        public const byte CTD_EXTERNAL_PRE_SCALE_DATA_WRITE = 0x32;
        public const byte CTD_EXTERNAL_PRE_SCALE_DATA_READ = 0x33;
        public const byte CTD_CLEAR_SIGNAL_SELECT = 0x34;
        public const byte CTD_ONE_TIME_CLEAR_REQUEST = 0x35;
        public const byte CTD_FULL_TIME_CLEAR_REQUEST = 0x36;
        public const byte CTD_CLEAR_REQUEST_RESET = 0x37;
        public const byte CTD_REVERSE_COUNT_MODE_SET = 0x38;
        public const byte CTD_REVERSE_COUNT_MODE_RESET = 0x39;
        public const byte CTD_NO_OPERATION = 0x3A;
        public const byte CTD_STRAIGHT_ACCELERATE_MODE_SET = 0x84;
        public const byte CTD_US_STRAIGHT_ACCELERATE_MODE_SET = 0x85;
        public const byte CTD_S_CURVE_ACCELERATE_MODE_SET = 0x86;
        public const byte CTD_US_S_CURVE_ACCELERATE_MODE_SET = 0x87;
        public const byte CTD_SW1_DATA_WRITE = 0x88;
        public const byte CTD_SW1_DATA_READ = 0x89;
        public const byte CTD_SW2_DATA_WRITE = 0x8A;
        public const byte CTD_SW2_DATA_READ = 0x8B;
        public const byte CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_SET = 0x8C;
        public const byte CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET = 0x8D;
        public const byte CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET = 0x8E;
        public const byte CTD_EMERGENCY_LIMIT_ENABLE_MODE_RESET = 0x8F;
        public const byte CTD_INITIAL_CLEAR = 0x90;
        public const byte CTD_MODE1_READ = 0xa0;
        public const byte CTD_MODE2_READ = 0xa1;
        public const byte CTD_STATUS_READ = 0xa2;
        public const byte CTD_SLOW_DOWN_STOP2 = 0xa3;
        public const byte CTD_RISE_PULSE_COUNTER_READ = 0xa6;
        public const byte CTD_BI_PHASE_PULSE_SELECT_DATA_WRITE = 0xaa;
        public const byte CTD_BI_PHASE_PULSE_SELECT_DATA_READ = 0xab;
        public const byte CTD_SIGNAL_SERH2_REAR_PULSE_DATA_WRITE = 0xac;
        public const byte CTD_SIGNAL_SERH2_REAR_PULSE_DATA_READ = 0xad;
        public const byte CTD_INTERPOLATION_MODE_WRITE = 0xb0;
        public const byte CTD_NOW_POSITION_X_DATA_WRITE = 0xb2;
        public const byte CTD_NOW_POSITION_Y_DATA_WRITE = 0xb4;
        public const byte CTD_OBJECT_POSITION_X_N1_DATA_WRITE = 0xb6;
        public const byte CTD_OBJECT_POSITION_Y_N2_DATA_WRITE = 0xb8;
        public const byte CTD_DRIVE_PULSE_MEASURE = 0xbb;
        public const byte CTD_DRIVE_PULSE_READ = 0xbc;
        public const byte CTD_NOW_DRIVE_PULSE_AUTO_CLEAR = 0xbd;
        public const byte CTD_XY_INTERPOLATION_SELECT_WRITE = 0xbe;
        public const byte CTD_MANUAL_PULSE_MODE_WRITE = 0xc0;
        public const byte CTD_MANUAL_PULSE_MODE_READ = 0xc1;
        public const byte CTD_SOFT_SYNC_MODE_WRITE = 0xc2;
        public const byte CTD_SOFT_SYNC_MODE_READ = 0xc3;
        public const byte CTD_SOFT_SYNC_EXECUTE = 0xc4;
        //-----------------------------------------------------------------
        // Misc
        //-----------------------------------------------------------------
        public const ushort CTD_MAX_SLOTS = 16;
        //-----------------------------------------------------------------
        // API Functions(CTD20.dll)
        //-----------------------------------------------------------------
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDllOpen();

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDllClose();

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwCreate(short wBsn);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwClose(short wBsn);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwLineFallOut(short wBsn, short wAxis, short wData);

        //IO Module 追加
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetIoRead(short wBsn, ref uint pdwData);
        //IO Module 追加
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetIoReadByte(short wBsn, ref uint pdwData);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSetIoWrite(short wBsn, ushort wData, ref uint pdwData);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSetIoWriteByte(short wBsn, ushort wData, ref uint dwData);


        //PMC制御関数
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwInPort(short wBsn, short wAxis, short wPort, ref byte pbData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOutPort(short wBsn, short wAxis, short wPort, byte bData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetDriveStatus(short wBsn, short wAxis, ref byte pbStatus);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetEndStatus(short wBsn, short wAxis, ref byte pbStatus);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetMechanicalSignal(short wBsn, short wAxis, ref byte pbSignal);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetUniversalSignal(short wBsn, short wAxis, ref byte pbSignal);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwMode1Write(short wBsn, short wAxis, byte bMode);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwMode2Write(short wBsn, short wAxis, byte bMode);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwUniversalSignalWrite(short wBsn, short wAxis, byte bSignal);

        //[DllImport("CTDAPI.dll")]
        //public static extern short CTDwDataRead(short wBsn, short wAxis, byte bCmd, ref byte bData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDataHalfRead(short wBsn, short wAxis, byte bCmd, ref short pwData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDataFullRead(short wBsn, short wAxis, byte bCmd, ref int pdwData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwCommandWrite(short wBsn, short wAxis, byte bCmd);

        //[DllImport("CTDAPI.dll")]
        //public static extern short CTDwDataWrite(short wBsn, short wAxis , byte bCmd, byte bData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDataHalfWrite(short wBsn, short wAxis, byte bCmd, short wData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDataFullWrite(short wBsn, short wAxis, byte bCmd, int dwData);

        //Added by sasaki 2012/04/02
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetAxisStatus(short wBsn, ref byte pbStatus);
        //Added by sasaki 2012/04/02
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetAxisAllPort(short wBsn, short wAxis, ref int pdwStatus);
        //Added by sasaki 2012/04/02
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwStartSignalWrite(short wBsn, short wStart);
        //Added by sasaki 2012/04/02
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwDeviceStatus(short wBsn, ref byte pbStatus);

        //情報取得
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetLibVersion(System.Text.StringBuilder pbLibVer);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetDrvVersion(System.Text.StringBuilder pbDrvVer);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetRomVersion(short wBsn, System.Text.StringBuilder pbRomVer);

        [DllImport("CTDAPI.dll")]
        public static extern int CTDwGetLastError(short wBsn);

        //API関数 (その他)
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetInternalCounter(short wBsn, short wAxis, ref int pdwData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetExternalCounter(short wBsn, short wAxis, ref int pdwData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetNowSpeedData(short wBsn, short wAxis, ref short pwData);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetDrivePulseCounter(short wBsn, short wAxis, ref int pdwData);

        //API関数 (補間)
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSetIpParameter(short wBsn, CTDPIPDRIVEPARAMETER IpDriveParameter, short Index);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpSpeedPush(short wBsn);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpSpeedPop(short wBsn);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpExeSub(short wBsn, short wCmd);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpExe(short wBsn, short wCmd);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetIpPulse(short wBsn, short wAxis, ref int pdwPulse);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSetIpPulse(short wBsn, short wAxis, int dwPulse);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwGetIpStepNo(short wBsn, short wAxis, ref int pdwStepNo);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpReset(short wBsn);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpStraight(short wBsn, ref sbyte bAxis, int lObjX, int lObjY, ref int lN2, byte bDrawIn, byte bSpd);
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpStraightE(short wBsn, ref sbyte bAxis, int lObjX, int lObjY, ref int lN2, byte bDrawIn, byte bSpd);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpParaSet(byte bMovekind,
                                        byte bIpconsin,
                                        byte bDrawIn);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpAxisSet(byte bIpAxis1,
                                        byte bIpAxis2,
                                        byte bIpAxis3,
                                        byte bIpAxis4);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwIpDataSet(short wBsn,
                                            byte bIpkind,
                                            int lObjX,
                                            int lObjY,
                                            int lCentX,
                                            int lCentY,
                                            int lN2D2,
                                            int lN2D3,
                                            int lN2D4,
                                            short wStepNo);

        //速度設定関数
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSpeedParameterWrite(short wBsn,
                                              short wAxis,
                                              double dLowSpeed,
                                              double dHighSpeed,
                                              short sAccTime,
                                              double dSRate);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSpeedParameterRead(short wBsn,
                                            short wAxis,
                                            ref double pdLowSpeed,
                                            ref double pdHighSpeed,
                                            ref short psAccTime,
                                            ref double pdSRate);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSpeedWrite(short wBsn, short wAxis, double dObjSpeed);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwSpeedRead(short wBsn, short wAxis, ref double pdObjSpeed);


        //原点復帰関数
        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_Return(short wBsn, short wAxis, byte bTimer);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_Sqnc0(short wBsn, short wAxis);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_Sqnc1(short wBsn, short wAxis);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_Sqnc2(short wBsn, short wAxis);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_Sqnc3(short wBsn, short wAxis);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_WatchX(short wBsn, short wAxis);

        [DllImport("CTDAPI.dll")]
        public static extern short CTDwOrg_End(short wBsn, short wAxis);


    }
}
