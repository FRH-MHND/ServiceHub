using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
    public class SmsService : ISmsService
    {
        public async Task<NotificationResponse> SendSmsAsync(string serviceProviderName, string phoneNumber, string messageBody)
        {
            // Ensure phone number format
            if (phoneNumber.Contains("+"))
            {
                phoneNumber = phoneNumber.Replace("+", "");
            }
            try { 
                        return await SendViaJOSMS(phoneNumber, messageBody);
                   
                }
            catch (Exception ex) { 
                // Error handling with detailed message
                return new NotificationResponse
                {
                    StatusCode = "500",
                    Message = $"Exception occurred: {ex.Message}"
                };
            }
        }
        private async Task<NotificationResponse> SendViaJOSMS(string phoneNumber, string messageBody)
        {
            using HttpClient client = new();
            string url = $"eyJhbGciOiJIUzI1NiJ9.eyJpZCI6ImU0N2ExYzM2LTQwZjYtNGRhMi05YTk1LTkyMjFmMDAwZDNkMyIsImlhdCI6MTczNDg1ODI1NywiaXNzIjoyMTE2NH0.R7bCXWLk5yluAeulhOqaofT5_XnRevz3ki_d2Min5gU";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return new NotificationResponse
            {
                StatusCode = response.IsSuccessStatusCode ? "200" : "422",
                Message = responseContent,
                ProviderRequestBody = url,
                ProviderResponseBody = responseContent
            };
        }
    }
}
