using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery.FormModel
{
    public class ShapeShitModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string BillID { get; set; }
        public int face { get; set; }
        public int Fame { get; set; }
        public string ChangeSkin { get { return "Change+Skin"; } }
    }
}
