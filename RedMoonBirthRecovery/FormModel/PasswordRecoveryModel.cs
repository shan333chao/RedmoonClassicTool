using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace RedMoonBirthRecovery.FormModel
{
    public class PasswordRecoveryModel
    {
        /// <summary>
        /// 游戏账号
        /// </summary>
        [Required(ErrorMessage ="请输入游戏登录账号")]
        [RegularExpression(@"^\w{3,12}$", ErrorMessage = "请输入3-14位数字、字母、下划线")]
        public string billid { get; set; }
        /// <summary>
        /// 游戏注册时使用的邮箱
        /// </summary>
        [Required(ErrorMessage ="请输入邮箱地址")]
        [RegularExpression(@"^[\w\+\-]+(\.[\w\+\-]+)*@[a-z\d\-]+(\.[a-z\d\-]+)*\.([a-z]{2,4})$", ErrorMessage = "请填写有效的邮箱")]
        public string email { get; set; }

        /// <summary>
        /// 提交默认为Submit
        /// </summary>
 
        public string submit { get { return "Submit"; }  }
    }
}
