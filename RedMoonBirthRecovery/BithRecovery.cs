using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
namespace RedMoonBirthRecovery
{
    public partial class BithRecovery : Form
    {
        public BithRecovery()
        {
            InitializeComponent();
        }
        private CookieContainer cc = new CookieContainer();
        /// <summary>
        /// post
        /// </summary>
        /// <param name="url">登录的url</param>
        /// <param name="data">登录url的参数.可用http工具获取.　</param>
        /// <param name="refe">登录后的网站地址.</param>
        /// <returns></returns>
        private string post(string url, string data, string refe)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = this.cc;
            request.ContentLength = data.Length;
            request.Referer = refe;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
            {
                writer.Write(data);
                writer.Flush();
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            request = null;
            return result;
        }
        private Thread thread = null;
        private List<string> datelist = null;
        private void btnPost_Click(object sender, EventArgs e)
        {
            DateTime  start=DateTime.Parse("1900/01/01");
            DateTime end = DateTime.Parse("1900/01/01");
            bool isok=DateTime.TryParse(dtpBeginDay.Text, out start) && DateTime.TryParse(dtpEndDay.Text, out end);

            if (isok&&end>=start)
	        {
                this.btnPost.Enabled = false;
                //生成破解日期
                MakeBirth(dtpBeginDay.Text, dtpEndDay.Text);
                thread = new Thread(new ThreadStart(CrossThreadFlush));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                MessageBox.Show("截至日期需要大于起始日期");
            }
 


        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
                this.btnPost.Enabled = true;
            }
        }
        private void CrossThreadFlush()
        {
 
            CrackLoop(); 
 
        }
        private delegate void FlushClient();//代理
        /// <summary>
        /// 剩余时间
        /// </summary>
        private TimeSpan surplus = new TimeSpan();
        public void CrackLoop()
        {
            if (lblStatus.InvokeRequired)
            {
                foreach (string item in datelist)
                {

                    surplus = DateTime.Parse(dtpEndDay.Text) - DateTime.Parse(item);
                    lblStatus.Text = "当前" + item + "\t 已经尝试了" + (datelist.Count - surplus.Days) + "次/还剩" + (datelist.Count - (datelist.Count - surplus.Days));
                    FlushClient fc = new FlushClient(CrackLoop);
                    this.Invoke(fc);
                    if (CrackBirth(item))
                    {

                        MessageBox.Show(item);

                        break;
                    }
                }
                this.btnPost.Enabled = true;

            }

        }



        /// <summary>
        /// 生成生日密码字典
        /// </summary>
        /// <param name="beginBirth">开始日期</param>
        /// <param name="endbirth">结束日期</param>
        /// <returns></returns>
        public void MakeBirth(string beginBirth, string endbirth)
        {
            DateTime bdate = DateTime.Parse(beginBirth);
            DateTime edate = DateTime.Parse(endbirth);
            TimeSpan minusdays = edate - bdate;
            int count = minusdays.Days;
            List<string> listdate = new List<string>();
            for (int i = 0; i <= count; i++)
            {
                listdate.Add(bdate.AddDays(i).ToShortDateString());
            }
            datelist = listdate;
        }
        /// <summary>
        /// post提交的参数
        /// </summary>
        private string name = string.Empty;
        /// <summary>
        /// 要访问的页面
        /// </summary>
        private readonly string url = "http://support.redmoonclassic.com/index.php";
        /// <summary>
        /// 返回结果
        /// </summary>
        private string responseResult = string.Empty;

        /// <summary>
        /// 尝试猜解请求
        /// </summary>
        /// <param name="date">日期</param>
        public bool CrackBirth(string date)
        {
            bool ResponseResult = false;
            DateTime bitrh = DateTime.Parse(date);
            name = "account=" + txtaccount.Text + "&password=" + txtPass.Text + "&month=" + bitrh.Month + "&day=" + bitrh.ToString("dd") + "&year=" + bitrh.Year + "&submit=Submit";
            string result = post(url, name, url);
            if (result.Contains("Login"))
            {
                ResponseResult = false;
            }
            else if (result.Contains("Welcome"))
            {

                ResponseResult = true;
            }
            result = string.Empty;
            name = string.Empty;
            return ResponseResult;
        }

        #region 注册游戏帐号
        /// <summary>
        /// 注册帐号的url
        /// </summary>
        private readonly string addUserUrl = "http://www.redmoonclassic.com/beta/add.php";
        /// <summary>
        /// 注册游戏帐号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegUser_Click(object sender, EventArgs e)
        {

            DateTime bitrh = DateTime.Parse(txtBirthday.Text);
            string name = "loginID=" + txtloginID.Text + "&Password=" + txtPassword.Text + "&Password2=" + txtPassword2.Text + "&month=" + bitrh.Month + "&day=" + bitrh.ToString("dd") + "&year=" + bitrh.Year + "&email=" + txtemail.Text + "&question=" + txtquestion.Text + "&answer=" + txtanswer.Text + "&Create=Create";
            name = name.Replace("@", "%40");
            string result = post(addUserUrl, name, url);
            if (result.Contains("You're account has been created"))
            {
                MessageBox.Show("帐号注册成功");
            }
            else
            {
                MessageBox.Show("您填写的信息有误,只能填写字母和数字");
            }
        }

        #endregion

        #region 变身
        private readonly string shapeshit = "http://www.redmoonclassic.com/beta/shapeshift/";

        private void btnChangeSkin_Click(object sender, EventArgs e)
        {
            string name = "Username=" + txtUsername.Text + "&BillID=" + txtBillID.Text + "&Password=" + txtBillPassword.Text + "&face=" + cbface.SelectedIndex + "&Fame=" + (cbFame.SelectedIndex == 1 ? 1000 : -1000) + "&ChangeSkin=Change+Skin";
            string result = post(shapeshit, name, shapeshit);
            if (result.Contains("This event is currently NOT running"))
            {
                MessageBox.Show("活动未开放");
            }
            else if (result.Contains("Your character has been updated"))
            {
                MessageBox.Show("变身成功");
            }
            else if (result.Contains("Invalid ID/Pass"))
            {
                MessageBox.Show("帐号或密码错误！");
            }
        }

        private void tbRedmoonTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFame.SelectedIndex = 0;
            cbface.SelectedIndex = 0;
        }
        #endregion

        /// <summary>
        /// post
        /// </summary>
        /// <param name="url">登录的url</param>
        /// <param name="data">登录url的参数.可用http工具获取.　</param>
        /// <param name="refe">登录后的网站地址.</param>
        /// <returns></returns>
        private Bitmap getVerifyPic(string url, string data, string refe)
        {
            Bitmap result = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = this.cc;
            request.ContentLength = data.Length;
            request.Referer = refe;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
            {
                writer.Write(data);
                writer.Flush();
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream reader = response.GetResponseStream();
                result = new Bitmap(reader, false);
            }

            return result;
        }


        #region 关于
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbStart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string Game = @"C:\Redmoon\Redmoon.exe";
            if (File.Exists(Game))
            {
                Process.Start(Game);
            }
        }
        /// <summary>
        /// 注册游戏帐号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbRedmoonTools.SelectedIndex = 1;
        }
        /// <summary>
        /// 客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkClient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", "http://www.redmoonclassic.com/downloads/RM601.exe");
        }
        /// <summary>
        /// 补丁1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbPatch1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", "http://www.redmoonclassic.com/downloads/601t602.exe");
        }
        /// <summary>
        /// 补丁2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkPatch2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", "http://www.redmoonclassic.com/downloads/602t603.exe");
        }
        #endregion


        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim().Length > 0 || txtContent.Text.Trim().Length > 0)
            {
                //发邮件
                MailHelper mh = new MailHelper(txtTitle.Text, txtContent.Text);
                mh.SendMail();
                MessageBox.Show("发送成功");
            }
            else
            {
                MessageBox.Show("标题和内容不能为空");
            }

        }

 



    }
}
