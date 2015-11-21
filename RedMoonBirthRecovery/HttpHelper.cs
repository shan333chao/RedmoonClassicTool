using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedMoonBirthRecovery
{
    public class HttpHelper
    {

        /// <summary>
        /// post
        /// </summary>
        /// <param name="url">登录的url</param>
        /// <param name="data">登录url的参数.可用http工具获取.　</param>
        /// <param name="refe">登录后的网站地址.</param>
        /// <returns></returns>
        public static string Post<T>(string url, T data, string refe)
        {
            string result = string.Empty;
            try
            {
                string postData = BuildRequestBody(data);
                CookieContainer cc = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cc;
                request.ContentLength = postData.Length;
                request.Referer = refe;

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
                {
                    writer.Write(postData);
                    writer.Flush();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public static Task<string> PostAsync<T>(string url, T data, string refe)
        {
            string result = string.Empty;
            return Task.Run<string>(() =>
            {
                try
                {

                    string postData = BuildRequestBody(data);
                    CookieContainer cc = new CookieContainer();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.CookieContainer = cc;
                    request.ContentLength = postData.Length;
                    request.Referer = refe;

                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
                    {
                        writer.Write(postData);
                        writer.Flush();
                    }
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        result = reader.ReadToEnd();
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                return result;
            });

        }

        public static string BuildRequestBody<T>(T t)
        {
            string result = string.Empty;
            if (t != null)
            {
                string obj = JsonConvert.SerializeObject(t);
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj);
                if (dic.Keys.Count > 0)
                {
                    foreach (var key in dic.Keys)
                    {
                        result += key + "=" + dic[key] + "&";
                    }
                    int lastAnd = result.LastIndexOf("&");
                    if (lastAnd > 0)
                    {
                        result = result.Substring(0, lastAnd);
                    }
                }
            }
            return result;

        }
    }
}
