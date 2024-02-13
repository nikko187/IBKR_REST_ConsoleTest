using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBKR_REST_ConsoleTest
{

    public class SecDef
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string conid { get; set; }
        public string companyHeader { get; set; }
        public string companyName { get; set; }
        public string symbol { get; set; }
        public string description { get; set; }
        public string restricted { get; set; }
        public object fop { get; set; }
        public string opt { get; set; }
        public string war { get; set; }
        public Section[] sections { get; set; }
        public Issuer[] issuers { get; set; }
        public int bondid { get; set; }
        public string secType { get; set; }
    }

    public class Section
    {
        public string secType { get; set; }
        public string exchange { get; set; }
        public string months { get; set; }
        public string conid { get; set; }
    }

    public class Issuer
    {
        public string id { get; set; }
        public string name { get; set; }
    }


}
