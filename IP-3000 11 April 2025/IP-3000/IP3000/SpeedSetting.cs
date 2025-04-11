using CTD;

namespace IP3000_Control
{
    internal class SpeedSetting
    {
        public bool CTD_InitAxis_ORG(short wBsn, short wAxis, byte setmode1, byte setmode2)
        {

            //---------------------------------------------------------------
            // MODE1 SET
            //
            // 減速開始ポイント検出方式              自動
            // パルス出力方式                        ２パルス方式
            // DIR   出力端子                        CWパルス  アクティブ Hi
            // PULSE 出力端子                        CCWパルス アクティブ Hi
            //---------------------------------------------------------------
            if (CTDw.CTDwMode1Write(wBsn, wAxis, setmode1) == 0) return false;
            //---------------------------------------------------------------
            // MODE2 SET
            //
            // EXTERNAL COUNTER 入力仕様             None
            // DEND 入力信号アクティブレベル         High
            // DERR 入力信号アクティブレベル         High
            // -SLM 入力信号アクティブレベル         High
            // +SLM 入力信号アクティブレベル         High
            // -ELM 入力信号アクティブレベル         High
            // +ELM 入力信号アクティブレベル         High
            //---------------------------------------------------------------
            if (CTDw.CTDwMode2Write(wBsn, wAxis, setmode2) == 0) return false;
            //---------------------------------------------------------------           
            // モード設定
            //
            // INPOSITION WAIT MODE RESET
            // ALARM STOP ENABLE MODE SET
            //---------------------------------------------------------------
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INPOSITION_WAIT_MODE_RESET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_ALARM_STOP_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_SET) == 0) return false;
            //---------------------------------------------------------------
            // データ設定
            //
            // RANGE DATA                100  出力周波数設定単位  1000÷100=10PPS
            // START/STOP SPEED DATA     100  開始停止周波数      100×10PPS=1000PPS
            // OBJECT SPEED DATA        4000  目的周波数          4000×10PPS=40000PPS
            // RATE-1 DATA              2046  加速時間設定単位    2046÷(4.096×10^6)=0.5mSec
            //                                加速時間            (4000-100)×0.5mSec = 1.95Sec
            // RATE-2 DATA                    デフォルト値 8191(1FFFh)
            // RATE-3 DATA                    デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 1-2          デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 2-3          デフォルト値 8191(1FFFh)
            //
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_RANGE_WRITE, 300) == 0) return false; //Devider of setting (Original x1000)
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_START_STOP_SPEED_DATA_WRITE, 100) == 0) return false; //Start/Stop Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_OBJECT_SPEED_DATA_WRITE, 5000) == 0) return false; //Motor Speed Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_RATE1_DATA_WRITE, 100) == 0) return false; //Start/Stop Delay
            //---------------------------------------------------------------
            // アドレス設定
            //
            // INTERNAL COUNTER 及び EXTERNAL COUNTER に 0h を書き込みます
            //---------------------------------------------------------------
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_INTERNAL_COUNTER_WRITE, 0) == 0) return false;
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0) == 0) return false;

            return true;
        }
        public bool CTD_InitAxis_Jog(short wBsn, short wAxis, byte setmode1, byte setmode2, short speed)
        {

            //---------------------------------------------------------------
            // MODE1 SET
            //
            // 減速開始ポイント検出方式              自動
            // パルス出力方式                        ２パルス方式
            // DIR   出力端子                        CWパルス  アクティブ Hi
            // PULSE 出力端子                        CCWパルス アクティブ Hi
            //---------------------------------------------------------------
            if (CTDw.CTDwMode1Write(wBsn, wAxis, setmode1) == 0) return false;
            //---------------------------------------------------------------
            // MODE2 SET
            //
            // EXTERNAL COUNTER 入力仕様             None
            // DEND 入力信号アクティブレベル         High
            // DERR 入力信号アクティブレベル         High
            // -SLM 入力信号アクティブレベル         High
            // +SLM 入力信号アクティブレベル         High
            // -ELM 入力信号アクティブレベル         High
            // +ELM 入力信号アクティブレベル         High
            //---------------------------------------------------------------
            if (CTDw.CTDwMode2Write(wBsn, wAxis, setmode2) == 0) return false;
            //---------------------------------------------------------------
            // モード設定
            //
            // INPOSITION WAIT MODE RESET
            // ALARM STOP ENABLE MODE SET
            //---------------------------------------------------------------
            //CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INTERPOLATION_MODE_WRITE);
            //if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INPOSITION_WAIT_MODE1_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INPOSITION_WAIT_MODE_RESET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_ALARM_STOP_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_RESET) == 0) return false;
            //---------------------------------------------------------------
            //CTDw.CTDwSetIpPulse(wBsn, wAxis, 50000);

            // データ設定
            //
            // RANGE DATA                100  出力周波数設定単位  1000÷100=10PPS
            // START/STOP SPEED DATA     100  開始停止周波数      100×10PPS=1000PPS
            // OBJECT SPEED DATA        4000  目的周波数          4000×10PPS=40000PPS
            // RATE-1 DATA              2046  加速時間設定単位    2046÷(4.096×10^6)=0.5mSec
            //                                加速時間            (4000-100)×0.5mSec = 1.95Sec
            // RATE-2 DATA                    デフォルト値 8191(1FFFh)
            // RATE-3 DATA                    デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 1-2          デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 2-3          デフォルト値 8191(1FFFh)
            //
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (speed == 0)
            {
                CTD_InitAxis_Jog_Slow(wBsn, wAxis);
            }
            else if (speed == 1)
            {
                CTD_InitAxis_Jog_Meduim(wBsn, wAxis);
            }
            else if (speed == 2)
            {
                CTD_InitAxis_Jog_Fast(wBsn, wAxis);
            }
            //---------------------------------------------------------------
            // アドレス設定
            //
            // INTERNAL COUNTER 及び EXTERNAL COUNTER に 0h を書き込みます
            //---------------------------------------------------------------
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_INTERNAL_COUNTER_WRITE, 0) == 0) return false;
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0) == 0) return false;

            return true;
        }
        public bool CTD_InitAxis_Jog_Slow(short wBsn, short wAxis)
        {
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_RANGE_WRITE, 500) == 0) return false; //Devider of setting (Original x1000)
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_START_STOP_SPEED_DATA_WRITE, 100) == 0) return false; //Start/Stop Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_OBJECT_SPEED_DATA_WRITE, 2000) == 0) return false; //Motor Speed Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_RATE1_DATA_WRITE, 100) == 0) return false; //Start/Stop Delay
            //---------------------------------------------------------------
            return true;
        }
        public bool CTD_InitAxis_Jog_Meduim(short wBsn, short wAxis)
        {
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_RANGE_WRITE, 100) == 0) return false; //Devider of setting (Original x1000)
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_START_STOP_SPEED_DATA_WRITE, 100) == 0) return false; //Start/Stop Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_OBJECT_SPEED_DATA_WRITE, 2000) == 0) return false; //Motor Speed Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_RATE1_DATA_WRITE, 100) == 0) return false; //Start/Stop Delay
            //---------------------------------------------------------------
            return true;
        }
        public bool CTD_InitAxis_Jog_Fast(short wBsn, short wAxis)
        {
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_RANGE_WRITE, 100) == 0) return false; //Devider of setting (Original x1000)
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_START_STOP_SPEED_DATA_WRITE, 100) == 0) return false; //Start/Stop Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_OBJECT_SPEED_DATA_WRITE, 4000) == 0) return false; //Motor Speed Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_RATE1_DATA_WRITE, 100) == 0) return false; //Start/Stop Delay
            //---------------------------------------------------------------
            return true;
        }
        public bool CTD_InitAxis_Run(short wBsn, short wAxis, byte setmode1, byte setmode2)
        {

            //---------------------------------------------------------------
            // MODE1 SET
            //
            // 減速開始ポイント検出方式              自動
            // パルス出力方式                        ２パルス方式
            // DIR   出力端子                        CWパルス  アクティブ Hi
            // PULSE 出力端子                        CCWパルス アクティブ Hi
            //---------------------------------------------------------------
            if (CTDw.CTDwMode1Write(wBsn, wAxis, setmode1) == 0) return false;
            //---------------------------------------------------------------
            // MODE2 SET
            //
            // EXTERNAL COUNTER 入力仕様             None
            // DEND 入力信号アクティブレベル         High
            // DERR 入力信号アクティブレベル         High
            // -SLM 入力信号アクティブレベル         High
            // +SLM 入力信号アクティブレベル         High
            // -ELM 入力信号アクティブレベル         High
            // +ELM 入力信号アクティブレベル         High
            //---------------------------------------------------------------
            if (CTDw.CTDwMode2Write(wBsn, wAxis, setmode2) == 0) return false;
            //---------------------------------------------------------------
            // モード設定
            //
            // INPOSITION WAIT MODE RESET
            // ALARM STOP ENABLE MODE SET
            //---------------------------------------------------------------
            //if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INPOSITION_WAIT_MODE1_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INPOSITION_WAIT_MODE_RESET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_ALARM_STOP_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_EMERGENCY_LIMIT_ENABLE_MODE_SET) == 0) return false;
            if (CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_SLOW_DOWN_LIMIT_ENABLE_MODE_SET) == 0) return false;
            //---------------------------------------------------------------

            //CTDw.CTDwCommandWrite(wBsn, wAxis, CTDw.CTD_INTERPOLATION_MODE_WRITE);

            //CTDw.CTDwSetIpPulse(wBsn, wAxis, 50000);
            // データ設定
            //
            // RANGE DATA                100  出力周波数設定単位  1000÷100=10PPS
            // START/STOP SPEED DATA     100  開始停止周波数      100×10PPS=1000PPS
            // OBJECT SPEED DATA        4000  目的周波数          4000×10PPS=40000PPS
            // RATE-1 DATA              2046  加速時間設定単位    2046÷(4.096×10^6)=0.5mSec
            //                                加速時間            (4000-100)×0.5mSec = 1.95Sec
            // RATE-2 DATA                    デフォルト値 8191(1FFFh)
            // RATE-3 DATA                    デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 1-2          デフォルト値 8191(1FFFh)
            // RATE CHANGE POINT 2-3          デフォルト値 8191(1FFFh)
            //
            // この設定により RATE-1 DATA による直線加減速となります
            //---------------------------------------------------------------
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_RANGE_WRITE, 102) == 0) return false; //Devider of setting (Original x1000)
            if (CTDw.CTDwDataHalfWrite(wBsn, wAxis, CTDw.CTD_START_STOP_SPEED_DATA_WRITE, 10) == 0) return false; //Start/Stop Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_OBJECT_SPEED_DATA_WRITE, 8159) == 0) return false; //Motor Speed Slow or Fast
            if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_RATE1_DATA_WRITE, 50) == 0) return false; //Start/Stop Delay
            //---------------------------------------------------------------
            // アドレス設定
            //
            // INTERNAL COUNTER 及び EXTERNAL COUNTER に 0h を書き込みます
            //---------------------------------------------------------------
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_INTERNAL_COUNTER_WRITE, 0) == 0) return false;
            //if (CTDw.CTDwDataFullWrite(wBsn, wAxis, CTDw.CTD_EXTERNAL_COUNTER_WRITE, 0) == 0) return false;

            return true;
        }
    }
}
