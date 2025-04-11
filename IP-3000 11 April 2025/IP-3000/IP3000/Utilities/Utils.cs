using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP3000.Utilities
{
    public static class Utils
    {
        public static string AppBaseDir = AppDomain.CurrentDomain.BaseDirectory;
        public static string MotionSettingDir = AppBaseDir + "Setting";

        public static void CreateSettingDir()
        {
            if (!Directory.Exists(AppBaseDir))
            {
                Directory.CreateDirectory(AppBaseDir);
            }

            if (!Directory.Exists(MotionSettingDir))
            {
                Directory.CreateDirectory(MotionSettingDir);
            }
        }

        public static void CreateFolder(string path)
        {

        }
    }
}
