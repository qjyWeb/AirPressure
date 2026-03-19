using System.Runtime.InteropServices;
using System.Text;

namespace AirPressure
{
    public static class Ini
    {
        [DllImport("kernel32")]
        private static extern long
            WritePrivateProfileString(string section, string key, string val, string filepath);

        [DllImport("kernel32")]
        private static extern int
            GetPrivateProfileString(string section, string key, string def, byte[] retval, int size, string filePath);
        public static void WriteToini(string section, string key, string val, string filepath)
        {
            try
            {
                WritePrivateProfileString(section, key, val, filepath);
            }
            catch { }
        }
        public static string ReadToini(string section, string key, string filepath)
        {
            try
            {
                byte[] temp = new byte[1024];
                int len = GetPrivateProfileString(section, key, "", temp, temp.Length, filepath);
                return Encoding.UTF8.GetString(temp, 0, len);
            }
            catch { }
            return "";
        }
        public static void Write(string section, string key, string val, string filepath)
        {
            if (val == null) val = "";
            WritePrivateProfileString(section, key, val, filepath);
        }
        public static void Write(string section, string key, bool val, string filepath)
        {
            WritePrivateProfileString(section, key, (val ? "1" : "0"), filepath);
        }
        public static void Write(string section, string key, byte val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, short val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, ushort val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, int val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, long val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, float val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, double val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        public static void Write(string section, string key, decimal val, string filepath)
        {
            WritePrivateProfileString(section, key, val.ToString(), filepath);
        }
        static string ReadToIni(string section, string key, string filepath)
        {
            byte[] temp = new byte[1024];
            int len = GetPrivateProfileString(section, key, "", temp, temp.Length, filepath);
            if (temp == null) return "";
            return Encoding.Default.GetString(temp, 0, len);
        }
        public static void Read(string section, string key, ref string val, string filepath)
        {
            val = ReadToIni(section, key, filepath);
        }
        public static void Read(string section, string key, ref bool val, string filepath)
        {
            val = false;
            string ret = ReadToIni(section, key, filepath);
            if (ret == "1") val = true;
            else if (ret.ToLower() == "true") val = true;
        }
        public static void Read(string section, string key, ref byte val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            byte.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref short val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            short.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref ushort val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            ushort.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref int val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            int.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref long val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            long.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref float val, string filepath)
        {
            val = 0f;
            string ret = ReadToIni(section, key, filepath);
            float.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref double val, string filepath)
        {
            val = 0d;
            string ret = ReadToIni(section, key, filepath);
            double.TryParse(ret, out val);
        }
        public static void Read(string section, string key, ref decimal val, string filepath)
        {
            val = 0;
            string ret = ReadToIni(section, key, filepath);
            decimal.TryParse(ret, out val);
        }
    }

}
