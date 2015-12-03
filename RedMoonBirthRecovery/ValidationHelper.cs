using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery
{
    public class ValidationHelper
    {
        /// <summary>
        /// 验证并返回错误结果JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool validateModel<T>(T t, out string errors)
        {
            var context = new ValidationContext(t, null, null);
            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(t, context, result, true);
            if (!isValid)
            {
                var errorCollection = result.Select(p => new ValidateMsgModel { fieldName = p.MemberNames.First(), errMsg = p.ErrorMessage });
                errors = JsonConvert.SerializeObject(errorCollection);
                return false;
            }
            errors = string.Empty;
            return true;
        }
        /// <summary>
        /// 验证并返回错误列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">需要验证的对象</param>
        /// <param name="errorList">错误列表</param>
        /// <returns></returns>
        public static bool validateModel<T>(T t, out IList<ValidateMsgModel> errorList)
        {
            var context = new ValidationContext(t, null, null);
            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(t, context, result, true);
            if (!isValid)
            {
                errorList = result.Select(p => new ValidateMsgModel { fieldName = p.MemberNames.First(), errMsg = p.ErrorMessage }).ToList<ValidateMsgModel>();

                return errorList.Count < 1;
            }
            errorList = null;
            return true;
        }

    }

    public class ValidateMsgModel
    {
        public string fieldName { get; set; }
        public string errMsg { get; set; }

    }
}
