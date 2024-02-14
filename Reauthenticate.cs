using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBKR_REST_ConsoleTest
{
    public class Reauthenticate
    {
        public bool authenticated { get; set; }
        public bool connected { get; set; }
        public bool competing { get; set; }
        public string fail { get; set; }
        public string message { get; set; }
        public string[] prompts { get; set; }
    }
}
