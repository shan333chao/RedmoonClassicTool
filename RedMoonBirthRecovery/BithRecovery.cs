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
using RedMoonBirthRecovery.FormModel;
using System.Threading.Tasks;
using System.Linq;

namespace RedMoonBirthRecovery
{
    public partial class BithRecovery : Form
    {
        public BithRecovery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 提交数据队列
        /// </summary>
        public CrackTaskModel crackTask = null;
        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPost_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Parse("1900/01/01");
            DateTime end = DateTime.Parse("1900/01/01");
            bool isok = DateTime.TryParse(dtpBeginDay.Text, out start) && DateTime.TryParse(dtpEndDay.Text, out end);
            if (isok && end >= start)
            {
                this.btnPost.Enabled = false;
                //生成破解日期
                MakeBirth(dtpBeginDay.Text, dtpEndDay.Text);
                if (crackTask != null && crackTask.creakRequests.Count > 0)
                {
                    await CrackLoop();
                }
            }
            else
            {
                MessageBox.Show("截至日期需要大于起始日期");
            }
        }
        /// <summary>
        /// 暂停和恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnStop_Click(object sender, EventArgs e)
        {
            if (btnStop.Text == "暂停")
            {
                stopCrack();
                btnStop.Text = "继续";
            }
            else if (btnStop.Text == "继续")
            {
                if (crackTask != null && crackTask.creakRequests.Count > 0)
                {
                    btnStop.Text = "暂停";
                    crackTask.state = false;
                    await CrackLoop();
                }
            }
            btnPost.Enabled = false;

        }
        /// <summary>
        /// 暂停队列
        /// </summary>
        public void stopCrack()
        {
            if (crackTask != null && crackTask.creakRequests.Count > 0)
            {
                //暂停提交
                crackTask.state = true;
            }
        }
        /// <summary>
        /// 重置队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            stopCrack();
            crackTask = null;
            btnStop.Text = "暂停";
            btnPost.Enabled = true;

        }


        /// <summary>
        /// 异步请求
        /// </summary>
        /// <returns></returns>
        public async Task CrackLoop()
        {
            if (crackTask != null)
            {
                for (; crackTask.currentIndex < crackTask.creakRequests.Count; crackTask.currentIndex++)
                {
                    ///如果状态变了，停止循环
                    if (crackTask.state)
                    {
                        break;
                    }
                    var task = crackTask.creakRequests[crackTask.currentIndex];
                    int shengyu = crackTask.creakRequests.Count - 1 - crackTask.currentIndex;
                    lblStatus.Text = "当前" + task.CreckDate.ToString("yyyy-MM-dd") + "\t 已经尝试了" + (crackTask.currentIndex) + "次/还剩" + shengyu;
                    bool isok = await CrackBirth(task);
                    if (isok)
                    {
                        lblStatus.Text = "正确生日是：" + task.CreckDate.ToString("yyyy-MM-dd");
                        break;
                    }
                    if (shengyu == 0)
                    {
                        lblStatus.Text = "很遗憾没有为你找到账号的生日";
                    }
                }
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
            DateTime bdate = new DateTime();
            DateTime edate = new DateTime();
            bool isConverted = DateTime.TryParse(beginBirth, out bdate) && DateTime.TryParse(endbirth, out edate);
            if (isConverted)
            {
                ///构造所有请求生日的数据
                crackTask = new CrackTaskModel { beginDate = bdate, endDate = edate, currentIndex = 0, state = false };
                crackTask.creakRequests = new List<CrackBirthdayModel>();
                TimeSpan minusdays = edate - bdate;
                for (int i = 0; i <= minusdays.Days; i++)
                {
                    var birth = bdate.AddDays(i);
                    crackTask.creakRequests.Add(new CrackBirthdayModel
                    {
                        account = txtaccount.Text,
                        password = txtPass.Text,
                        month = birth.Month,
                        day = birth.ToString("dd"),
                        year = birth.Year,
                        CreckDate = birth
                    });
                }
            }
        }




        /// <summary>
        /// 尝试猜解生日
        /// </summary>
        /// <param name="crackBirth">生日请求对象</param>
        /// <returns></returns>
        public async Task<bool> CrackBirth(CrackBirthdayModel crackBirth)
        {
            bool ResponseResult = false;
            return await Task.Run<bool>(async () =>
             {
                 ///等待提交返回结果
                 string result = await HttpHelper.PostAsync<CrackBirthdayModel>(RedmoonUri.crackBirthday, crackBirth, RedmoonUri.crackBirthday);
                 if (result.Contains("Login"))
                 {
                     ResponseResult = false;
                 }
                 else if (result.Contains("Welcome"))
                 {

                     ResponseResult = true;
                 }
                 result = string.Empty;
                 return ResponseResult;
             });
        }

        #region 注册游戏帐号

        /// <summary>
        /// 注册游戏帐号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegUser_Click(object sender, EventArgs e)
        {

            DateTime bitrh = DateTime.Parse(txtBirthday.Text);

            UserAddModel user = new UserAddModel
            {
                loginID = txtloginID.Text,
                Password = txtPassword.Text,
                Password2 = txtPassword2.Text,
                answer = txtanswer.Text,
                email = txtemail.Text.Replace("@", "%40"),
                question = txtquestion.Text,
                month = bitrh.Month,
                day = bitrh.ToString("dd"),
                year = bitrh.Year
            };
            //string name = "loginID=" + txtloginID.Text + "&Password=" + txtPassword.Text + "&Password2=" + txtPassword2.Text +
            //    "&month=" + bitrh.Month + "&day=" + bitrh.ToString("dd") + "&year=" + bitrh.Year + "&email=" + txtemail.Text +
            //    "&question=" + txtquestion.Text + "&answer=" + txtanswer.Text + "&Create=Create"; 
            //name = name.Replace("@", "%40");
            string result = HttpHelper.Post<UserAddModel>(RedmoonUri.userRegist, user, RedmoonUri.userRegist);
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


        private void btnChangeSkin_Click(object sender, EventArgs e)
        {
            // string name = "Username=" + txtUsername.Text + "&BillID=" + txtBillID.Text + "&Password=" + txtBillPassword.Text + "&face=" + cbface.SelectedIndex + "&Fame=" + (cbFame.SelectedIndex == 1 ? 1000 : -1000) + "&ChangeSkin=Change+Skin";

            ShapeShitModel shape = new ShapeShitModel
            {
                BillID = txtBillID.Text,
                face = cbface.SelectedIndex,
                Fame = (cbFame.SelectedIndex == 1 ? 1000 : -1000),
                Password = txtBillPassword.Text,
                Username = txtUsername.Text
            };
            string result = HttpHelper.Post<ShapeShitModel>(RedmoonUri.shapeshit, shape, RedmoonUri.shapeshit);
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
            Process.Start("iexplore.exe", RedmoonUri.DownLoadMainProgramUrl);
        }
        /// <summary>
        /// 补丁1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbPatch1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", RedmoonUri.DownLoadPatchFirstUrl);
        }
        /// <summary>
        /// 补丁2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkPatch2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", RedmoonUri.DownLoadPathcSecondUrl);
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
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRecoveryPass_Click(object sender, EventArgs e)
        {
            PasswordRecoveryModel recoveryModel = new PasswordRecoveryModel
            {
                billid = txtRecoveryBillingId.Text,
                email = txtRecoveryEmail.Text
            };
            lblRecoveryMsgStatus.Text = "";
            ///验证输入内容是否正确
            IList<ValidateMsgModel> errMsglist = new List<ValidateMsgModel>();
            string result = string.Empty;

            if (ValidationHelper.validateModel<PasswordRecoveryModel>(recoveryModel, out errMsglist))
            {
                recoveryModel.email = recoveryModel.email.Replace("@", "%40");
                result = await HttpHelper.PostAsync<PasswordRecoveryModel>(RedmoonUri.PasswordRecovery, recoveryModel, RedmoonUri.PasswordRecovery);
          
                string errorEmail = "--Incorrect Information--"; 
                if (result.Contains(txtRecoveryEmail.Text))
                {
                    lblRecoveryMsgStatus.Text = string.Format("请去你的邮箱:{0},查收你帐号的密码",txtRecoveryEmail.Text);
                    MessageBox.Show(string.Format("您的密码已发送到邮箱：{0}", recoveryModel.email),"恭喜您", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else if (result.Contains(errorEmail)) {

                    MessageBox.Show(string.Format("您输入的账号或邮箱不匹配！"));
                }
            }
            else
            {
                var errorResult = string.Empty;
                foreach (var item in errMsglist)
                {
                    errorResult += item.errMsg + "\r\n";
                }
                MessageBox.Show(errorResult, "输入错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private async void btnFindMyGameAccount_Click(object sender, EventArgs e)
        {
            lblAccountRecoveryStatusMsg.Text = "";
           RecoveryGameAccountModel recoveryModel = new RecoveryGameAccountModel
            {
                email = txtRecoveryAccountEmail.Text
            };

            ///验证输入内容是否正确
            IList<ValidateMsgModel> errMsglist = new List<ValidateMsgModel>();
            string result = string.Empty;

            if (ValidationHelper.validateModel<RecoveryGameAccountModel>(recoveryModel, out errMsglist))
            {
                recoveryModel.email = recoveryModel.email.Replace("@", "%40");
                result = await HttpHelper.PostAsync<RecoveryGameAccountModel>(RedmoonUri.GameAccountRecovery, recoveryModel, RedmoonUri.GameAccountRecovery);

                string errorEmail = "we have no accounts for the email address";
                if (result.Contains("An email has been sent to"))//success
                {
                    lblAccountRecoveryStatusMsg.Text = string.Format("请去你的邮箱:{0},请去你的邮箱查看你帐号信息", txtRecoveryAccountEmail.Text);
                    MessageBox.Show(string.Format("您的密码已发送到邮箱：{0}", txtRecoveryAccountEmail.Text), "恭喜您", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result.Contains(errorEmail))
                { 
                    MessageBox.Show(string.Format("没有找到任何使用{0}邮箱注册的账号！",txtRecoveryAccountEmail.Text));
                }
            }
            else
            {
                var errorResult = string.Empty;
                foreach (var item in errMsglist)
                {
                    errorResult += item.errMsg + "\r\n";
                }
                MessageBox.Show(errorResult, "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
