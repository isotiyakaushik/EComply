using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Gst
    {
        public class Contacted
        {
            public string name { get; set; }
            public long mobNum { get; set; }
            public string email { get; set; }
        }

        public class Pradr
        {
            public string adr { get; set; }
        }

        public class GstProfile
        {
            public string ntcrbs { get; set; }
            public Contacted contacted { get; set; }
            public string canclDt { get; set; }
            public string adhrVFlag { get; set; }
            public string lgnm { get; set; }
            public string stj { get; set; }
            public string dty { get; set; }
            public string cxdt { get; set; }
            public string gstin { get; set; }
            public List<string> nba { get; set; }
            public string ekycVFlag { get; set; }
            public string cmpRt { get; set; }
            public string rgdt { get; set; }
            public string ctb { get; set; }
            public Pradr pradr { get; set; }
            public string rsnCd { get; set; }
            public string sts { get; set; }
            public string tradeNam { get; set; }
            public string isFieldVisitConducted { get; set; }
            public string ctj { get; set; }
            public string einvoiceStatus { get; set; }
            public List<string> mbr { get; set; }
        }
    }
}
