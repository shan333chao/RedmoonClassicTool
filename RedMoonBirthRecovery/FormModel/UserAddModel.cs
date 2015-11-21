using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery.FormModel
{
    public class UserAddModel
    {
        public string loginID { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public int month { get; set; }
        public string day { get; set; }
        public int year { get; set; }
        public string email { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string Create { get { return "Create"; } }

    }
}
