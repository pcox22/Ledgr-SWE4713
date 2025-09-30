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
        string body)
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
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

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
    
    public static async Task SendEmailAsync(string fromAddress, string toAddress, string fromName, string toName, string subject,
        string body)
    {
        try
        {
            UserCredential credential;

            //var path = Path.Combine(AppContext.BaseDirectory, "Credentials", "credentials.json");
            Console.WriteLine("Base Directory: " + AppContext.BaseDirectory);
            
            using (var stream = new FileStream(AppContext.BaseDirectory + "credentials.json", FileMode.Open, FileAccess.Read))
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
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var ms = new MemoryStream())
            {
                message.WriteTo(ms);
                var raw = Convert.ToBase64String(ms.ToArray())
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");

                var gmailMessage = new Google.Apis.Gmail.v1.Data.Message { Raw = raw };
                await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
                Console.WriteLine("Email sent!");
                //return "Success";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //return ex.Message;
        }
    }
    
}