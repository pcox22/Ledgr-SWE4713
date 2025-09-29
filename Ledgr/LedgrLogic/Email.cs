namespace LedgrLogic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

public class Email
{
    public static string SendEmail(string fromAddress, string toAddress, string fromName, string toName, string subject, string body, string toReply, string fromPW)
    {
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
}