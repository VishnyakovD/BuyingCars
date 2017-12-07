using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BuyingMetal
{
	public interface IMailingManager
	{
		void SendMailNewOrder(string tel);
		//string[] mails, string subject, string body
	}


	public static class SendMailEx
	{
		public static Task SendMailExAsyncOrder(string tel, string msg)
		{

			var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
			var sMTPPort = 25;
			int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
			var enableSsl = false;
			bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
			var useDefaultCredentials = true;
			bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
			var userName = WebConfigurationManager.AppSettings["UserName"];
			var userPassword = WebConfigurationManager.AppSettings["UserPassword"];

			var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
			var mailTo = WebConfigurationManager.AppSettings["MailTo"];
			var mailAdmin = WebConfigurationManager.AppSettings["MailAdmin"];
			//var MailTo2 = WebConfigurationManager.AppSettings["MailTo2"];
			var mailSubject = WebConfigurationManager.AppSettings["MailSubject"];
			//var mailBody = WebConfigurationManager.AppSettings["MailBody"];
			var mailBody = "Клиент " + tel +". "+msg;
			var smtp = new SmtpClient(sMTPHost, sMTPPort);

			smtp.EnableSsl = enableSsl;
			smtp.UseDefaultCredentials = useDefaultCredentials;
			smtp.Credentials = new NetworkCredential(userName, userPassword);


			var message = new MailMessage();
			message.From = new MailAddress(mailFrom);
			message.To.Add(new MailAddress(string.Format("{0}", mailTo)));
			message.To.Add(new MailAddress(string.Format("{0}", mailAdmin)));
			message.Subject = string.Format(mailSubject, tel);
			message.Body = mailBody;

			return Task.Run(() => SendMailExImpl(smtp, message));
		}

		public static Task SendMailExAsyncMessage(string subject, string msg)
		{

			var sMTPHost = WebConfigurationManager.AppSettings["SMTPHost"];
			var sMTPPort = 25;
			int.TryParse(WebConfigurationManager.AppSettings["SMTPPort"], out sMTPPort);
			var enableSsl = false;
			bool.TryParse(WebConfigurationManager.AppSettings["EnableSsl"], out enableSsl);
			var useDefaultCredentials = true;
			bool.TryParse(WebConfigurationManager.AppSettings["UseDefaultCredentials"], out useDefaultCredentials);
			var userName = WebConfigurationManager.AppSettings["UserName"];
			var userPassword = WebConfigurationManager.AppSettings["UserPassword"];

			var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
			var mailTo = WebConfigurationManager.AppSettings["MailTo"];
			var mailAdmin = WebConfigurationManager.AppSettings["MailAdmin"];
			var mailSubject = subject;
			var mailBody = subject + ". " + msg;
			var smtp = new SmtpClient(sMTPHost, sMTPPort);

			smtp.EnableSsl = enableSsl;
			smtp.UseDefaultCredentials = useDefaultCredentials;
			smtp.Credentials = new NetworkCredential(userName, userPassword);

			var message = new MailMessage();
			message.From = new MailAddress(mailFrom);
			message.To.Add(new MailAddress(string.Format("{0}", mailTo)));
			message.To.Add(new MailAddress(string.Format("{0}", mailAdmin)));
			message.Subject = string.Format(mailSubject, mailBody);
			message.Body = mailBody;


			return Task.Run(() => SendMailExImpl(smtp, message));
		}

		private static void SendMailExImpl(
			System.Net.Mail.SmtpClient client,
			System.Net.Mail.MailMessage message)
		{
			//token.ThrowIfCancellationRequested();

			var tcs = new TaskCompletionSource<bool>();
			System.Net.Mail.SendCompletedEventHandler handler = null;
			Action unsubscribe = () => client.SendCompleted -= handler;

			handler = async (s, e) =>
			{
				unsubscribe();

				// a hack to complete the handler asynchronously
				await Task.Yield();

				if (e.UserState != tcs)
					tcs.TrySetException(new InvalidOperationException("Unexpected UserState"));
				else if (e.Cancelled)
					tcs.TrySetCanceled();
				else if (e.Error != null)
					tcs.TrySetException(e.Error);
				else
					tcs.TrySetResult(true);
			};

			client.SendCompleted += handler;
			try
			{
				client.SendAsync(message, tcs);
				//using (token.Register(() => client.SendAsyncCancel(), useSynchronizationContext: false))
				//{
				//	await tcs.Task;
				//}
			}
			finally
			{
				unsubscribe();
			}
		}
	}
}