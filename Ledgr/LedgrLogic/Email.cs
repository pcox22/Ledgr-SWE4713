namespace LedgrLogic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System;
using System.IO;
using System.Threading;


public class Email
{
    static string[] Scopes = { GmailService.Scope.GmailSend };
    private static string ApplicationName = "My Gmail API App";

    public static string SendEmail(string fromAddress, string toAddress, string fromName, string toName, string subject,
        string body, string toReply, string fromPW)
    {
        try
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Patrick", "10jonathancox@gmail.com"));
            message.To.Add(new MailboxAddress("Another Patrick", "pcox21@students.kennesaw.edu"));
            message.Subject = "OAuth2 Gmail Test";
            message.Body = new TextPart("plain") { Text = "Hello from OAuth2 Gmail API!" };

            using (var ms = new MemoryStream())
            {
                message.WriteTo(ms);
                var raw = Convert.ToBase64String(ms.ToArray())
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");

                var gmailMessage = new Google.Apis.Gmail.v1.Data.Message { Raw = raw };
                service.Users.Messages.Send(gmailMessage, "me").Execute();
                Console.WriteLine("Email sent!");
                return "Success";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    /*
    try
    {
        SmtpClient mySmtpClient = new SmtpClient("smtp.gmail.com", 587);
        mySmtpClient.EnableSsl = true;

        // set smtp-client with basicAuthentication
        mySmtpClient.UseDefaultCredentials = false;

        // Must create a variable for the 2FA apps password (will need to be added to the database)
        System.Net.NetworkCredential basicAuthenticationInfo = new
            System.Net.NetworkCredential("ytglitchroxas@gmail.com", "ijdu oaja bryq jlut");
        mySmtpClient.Credentials = basicAuthenticationInfo;

        MailAddress from = new MailAddress("ytglitchroxas@gmail.com", "Ledgr Systems");
        MailAddress to = new MailAddress(toAddress, toName);
        MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

        // often the toReply might be the sender; will set up a "do-not-replay" email for safeguard
        MailAddress replyTo = new MailAddress(toReply);
        myMail.ReplyToList.Add(replyTo);

        myMail.Subject = subject;
        myMail.SubjectEncoding = System.Text.Encoding.UTF8;

        myMail.Body = body;
        myMail.BodyEncoding = System.Text.Encoding.UTF8;

        // keep this here; harms nothing; if needed, HTML tags can be manually placed into the message for styling
        myMail.IsBodyHtml = true;

        mySmtpClient.Send(myMail);
    }

    catch (SmtpException ex)
    {
        // Consider adjusting message as needed
        return "Smtp Error has occured: " + ex.Message;
    }
    catch (Exception ex)
    {
        return "Failed to send message";
    }
    return "Message sent successfully";
}
*/
    
    
}