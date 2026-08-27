using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EComply.Common
{
    public class Statics
    {
        public static string AppPath = Application.StartupPath;
        public static string AppDBPath = Path.Combine(Application.StartupPath, "Database", "Master.db");
        public static string MainConnectionString = "Data Source=" + AppDBPath + ";Password=E_Comply@280826";

        public static CookieContainer GSTcookieContainer = new CookieContainer();
        public static string CompanyGSTN { get; set; } = string.Empty;
        public static string GstUserName { get; set; } = string.Empty;
        public static string GstPassword { get; set; } = string.Empty;
    }
}
