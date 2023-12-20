using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HKQTravellingAuthenication.Services
{
	public class TwilioSettings
	{
		public string AccountSid { get; set; }
		public string AuthToken { get; set; }
		public string PhoneNumber { get; set; }
	}

	public interface ISmsSender
	{
		Task SendSmsAsync(string phoneNumber, string message);
	}

	public class TwilioSmsSender : ISmsSender
	{
		private readonly TwilioSettings _twilioSettings;
		private readonly ILogger<TwilioSmsSender> _logger;

		public TwilioSmsSender(IOptions<TwilioSettings> twilioSettings, ILogger<TwilioSmsSender> logger)
		{
			_twilioSettings = twilioSettings.Value;
			_logger = logger;
			_logger.LogInformation("Create TwilioSmsSender");
		}

		public async Task SendSmsAsync(string phoneNumber, string message)
		{
			try
			{
				TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

				var twilioMessage = await MessageResource.CreateAsync(
					body: message,
					from: new PhoneNumber(_twilioSettings.PhoneNumber),
					to: new PhoneNumber(phoneNumber)
				);

				_logger.LogInformation($"Sent SMS to {phoneNumber} with message: {message}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error sending SMS: {ex.Message}");

				// Xử lý khi gửi tin nhắn không thành công
				// ...
			}
		}
	}
}
