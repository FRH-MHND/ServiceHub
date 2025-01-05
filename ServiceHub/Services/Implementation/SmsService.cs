using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
	public class SmsService : ISmsService
	{
		private readonly string _accountSid = "ACd6105dc31246d570ac419c761e6ece2c";
		private readonly string _authToken = "ce76d061a904858795b24f1c4c4ba689";
		private readonly string _serviceSid = "VA7fe282aa2b94b0b0cae537f63b0ad4d4";

		public SmsService()
		{
			TwilioClient.Init(_accountSid, _authToken);
		}

		public async Task<NotificationResponse> SendSmsAsync(string serviceProviderName, string phoneNumber, string messageBody)
		{
			try
			{
				// Use Twilio Verify's VerificationResource to send the SMS
				var verification = await VerificationResource.CreateAsync(
					to: phoneNumber, // Dynamic phone number passed as a parameter
					channel: "sms", // Using SMS channel
					pathServiceSid: _serviceSid // Verify service SID
				);

				// Return success response with verification SID
				return new NotificationResponse
				{
					ResponseStatusCode = "200",
					ResponseMessage = $"Verification sent successfully. SID: {verification.Sid}"
				};
			}
			catch (Exception ex)
			{
				// Handle errors and return failure response
				return new NotificationResponse
				{
					ResponseStatusCode = "500",
					ResponseMessage = $"Exception occurred: {ex.Message}"
				};
			}
		}
	}
}
