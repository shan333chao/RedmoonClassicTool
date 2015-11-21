using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery.FormModel
{
    public class CrackTaskModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime beginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endDate { get; set; }
        /// <summary>
        /// 所有要提交破解的生日
        /// </summary>
        public List<CrackBirthdayModel> creakRequests { get; set; }
        /// <summary>
        /// 执行的顺序
        /// </summary>
        public int currentIndex { get; set; }

        public bool state { get; set; }
    }
}
