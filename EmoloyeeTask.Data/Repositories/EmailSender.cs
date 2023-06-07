using Castle.Core.Smtp;
using EmoloyeeTask.Data.Interfaces;
using MimeKit;
using MimeKit.Utils;
using System.Net;
using System.Net.Mail;

public class EmailSender : IMailSender
{

    public async Task SendMailMessageAsync(string listenerMail, string code)
    {
        MimeMessage message = new MimeMessage();

        message.From.Add(new MailboxAddress("Менеджер восстановления пароля", "old-fagger@bk.ru"));
        message.To.Add(new MailboxAddress("Пользователь", listenerMail));
        message.Subject = "Востановление пароля";

        var bodyBuilder = new BodyBuilder();

        var image = bodyBuilder.LinkedResources.Add(@"storage/PersonalAccount/company-logo.jpg");

        image.ContentId = MimeUtils.GenerateMessageId();

        bodyBuilder.HtmlBody = string.Format(@"
<!DOCTYPE html>
    <html>
    <head>
    {0}
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <img src=""cid:{1}"" alt=""Your Company Logo"" />
            </div>
            <div class='content'>
                <h2>Восстановление пароля</h2>
                <p>Мы получили запрос на восстановление вашего пароля. Если вы не делали этого запроса, просто проигнорируйте это письмо.</p>
                <p>Ваш код для восстановления пароля:</p>
                <div class='code'>{2}</div>
                <p>Введите этот код в вашем приложении</p>
            </div>
        </div>
    </body>
    </html>", @"<style>
            body {
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f7f7f7;
            }
            .container {
                width: 80%;
                margin: auto;
                background-color: #fff;
                padding: 20px;
                border-radius: 4px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.05);
            }
            .header {
                text-align: center;
                padding: 20px 0;
                border-bottom: 1px solid #eee;
            }
            .content {
                margin: 20px 0;
                line-height: 1.6;
            }
            .code {
                display: inline-block;
                padding: 10px 20px;
                color: #3498db;
                border: 1px solid #3498db;
                border-radius: 4px;
                font-size: 18px;
                font-weight: bold;
            }
        </style>", image.ContentId, code);

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.mail.ru", 465, true);
            await client.AuthenticateAsync("old-fagger@bk.ru", "kx1F04PikrjhHEiBMs95");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

