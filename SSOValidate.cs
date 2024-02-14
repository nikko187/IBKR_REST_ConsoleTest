using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBKR_REST_ConsoleTest
{

    public class SSOValidate
    {
        public int LOGIN_TYPE { get; set; }
        public string USER_NAME { get; set; }
        public int USER_ID { get; set; }
        public int expire { get; set; }
        public bool RESULT { get; set; }
        public long AUTH_TIME { get; set; }
    }

}
