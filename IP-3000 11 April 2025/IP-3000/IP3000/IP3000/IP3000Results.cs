using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3000_Control.IP3000
{
    public class IP3000Results
    {
        public string ProductName { get; set; }
        public string SerialNo { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MotionPrgName { get; set; }
        public string XPos { get; set; }
        public string YPos { get; set; }
        public string RPos { get; set; }
        public string DelayCapture { get; set; }
        public string Exposure { get; set; }
        public string AOIResult { get; set; }
        public string ClsScore { get; set; }
        public string SegmentScore { get; set; }
        public string FilterName { get; set; }
        public string MinArea { get; set; }
        public string ClsScoreSpec { get; set; }
        public string SegmentScoreSpec { get; set; }
        public string MinAreaSpec { get; set; }
        public string AOIProcessTime { get; set; }

        //public List<IP3000Results> ListResult { get; set; } = new List<IP3000Results>();

        /// <summary>
        /// 
        /// </summary>
        public IP3000Results() 
        {
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="serialNo"></param>
        public IP3000Results(string productName, string serialNo)
        {
          
        }
    }

    public class IP3000DataItem
    {
        public int JobNo { get; set; }
        public string JobTitle { get; set; }
        public string Side { get; set; }
        public string XPos { get; set; }
        public string YPos { get; set; }
        public string RPos { get; set; }
        public string LossScore { get; set; }
        public string InferenceTime { get; set; }
        public string Result { get; set; }

    }
}
