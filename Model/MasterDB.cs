using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MasterDB
    {
        public class CompanyMaster
        {
            public int Id { get; set; }
            public string gstin { get; set; } = "";
            public string trade_name { get; set; } = "";
            public string address { get; set; } = "";
            public string? mobile_no { get; set; }
            public string? email { get; set; }
            public string? gst_user_name { get; set; }
            public string? gst_password { get; set; }
            public string? e_user_name { get; set; }
            public string? e_password { get; set; }
        }
    }
}
