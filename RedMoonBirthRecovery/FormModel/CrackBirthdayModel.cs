using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery.FormModel
{
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class CrackBirthdayModel
    {
        [JsonProperty]
        public string account { get; set; }
        [JsonProperty]
        public string password { get; set; }
        [JsonProperty]
        public int month { get; set; }
        [JsonProperty]
        public string day { get; set; }
        [JsonProperty]
        public int year { get; set; }
        [JsonProperty]
        public string submit {  get { return "Submit"; } }
        [JsonIgnore] 
        public DateTime CreckDate { get; set; }
    }
}
