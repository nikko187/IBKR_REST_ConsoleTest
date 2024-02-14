using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBKR_REST_ConsoleTest
{
    // SecDef myDeserializedClass = JsonConvert.DeserializeObject<List<SecDef>>(myJsonResponse);
    public class SecDef
    {
        [JsonProperty("conid")]
        public string conid { get; set; }

        [JsonProperty("companyHeader")]
        public string companyHeader { get; set; }

        [JsonProperty("companyName")]
        public string companyName { get; set; }

        [JsonProperty("symbol")]
        public string symbol { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("restricted")]
        public string restricted { get; set; }

        [JsonProperty("fop")]
        public string fop { get; set; }

        [JsonProperty("opt")]
        public string opt { get; set; }

        [JsonProperty("war")]
        public string war { get; set; }

        [JsonProperty("sections")]
        public List<Section> sections { get; set; }
    }

    public class Section
    {
        [JsonProperty("secType")]
        public string secType { get; set; }

        [JsonProperty("months")]
        public string months { get; set; }

        [JsonProperty("symbol")]
        public string symbol { get; set; }

        [JsonProperty("exchange")]
        public string exchange { get; set; }

        [JsonProperty("legSecType")]
        public string legSecType { get; set; }
    }

}
