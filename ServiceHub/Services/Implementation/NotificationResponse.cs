namespace ServiceHub.Services.Implementation
{
    public class NotificationResponse
    {
            public string StatusCode { get; set; }         // Status of the request (e.g., "200" for success)
            public string Message { get; set; }            // Message or data from the response
            public string ProviderRequestBody { get; set; } // Optional: details of the request
            public string ProviderResponseBody { get; set; } // Optional: response details from provider
        }
    }

