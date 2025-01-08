// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    public static partial class Service
    {
        public static class Mail
        {
            public static async void Send(MailData mailData)
            {
                try
                {
                    if (string.IsNullOrEmpty(mailData.senderAddress))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(mailData.senderPassword))
                    {
                        return;
                    }

                    await Task.Run(() =>
                    {
                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(mailData.senderAddress, mailData.senderName),
                            Subject = mailData.mailName,
                            Body = mailData.mailBody,
                            IsBodyHtml = false,
                        };
                        mailMessage.To.Add(mailData.targetAddress);

                        var smtpClient = new SmtpClient(mailData.smtpServer, mailData.smtpPort)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(mailData.senderAddress, mailData.senderPassword),
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Timeout = 20000
                        };

                        smtpClient.Send(mailMessage);
                    });

                    Debug.Log("邮件发送成功!");
                }
                catch (SmtpException e)
                {
                    Debug.LogError(Text.Format("邮件发送失败: {0}", e.Message));
                }
            }

            [Serializable]
            public struct MailData
            {
                public string smtpServer;
                public int smtpPort;
                public string senderName;
                public string senderAddress;
                public string senderPassword;
                public string mailName;
                public string mailBody;
                public string targetAddress;
            }
        }
    }
}