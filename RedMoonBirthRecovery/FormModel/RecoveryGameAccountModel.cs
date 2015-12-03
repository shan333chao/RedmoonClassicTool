using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery.FormModel
{
  public  class RecoveryGameAccountModel
    {
        [Required(ErrorMessage = "请输入邮箱地址")]
        [RegularExpression(@"^[\w\+\-]+(\.[\w\+\-]+)*@[a-z\d\-]+(\.[a-z\d\-]+)*\.([a-z]{2,4})$", ErrorMessage = "请填写有效的邮箱")]
        public string email { get; set; }

        /// <summary>
        /// 提交默认为Submit
        /// </summary> 
        public string submit { get { return "Submit"; } }
    }
}
