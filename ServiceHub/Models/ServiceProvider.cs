namespace ServiceHub.Models
{
    public class ServiceProvider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string ServiceType { get; set; }
        public string Availability { get; set; }
        public float Rating { get; set; }
        public float PricePerHour { get; set; }
        public string Location { get; set; }

        public string ProfileInfo()
        {
            return $"{Name}, Service Type: {ServiceType}, Rating: {Rating}, Price: {PricePerHour}, Location: {Location}";
        }

        public void ManageAccountSettings(AccountSettings settings)
        {
            // Implement account settings management logic here
        }

        public void UpdateServices(string[] services)
        {
            // Implement services update logic here
        }

        public void ViewNotifications()
        {
            // Implement view notifications logic here
        }

        public void CommunicateWithAdmin(string message)
        {
            // Implement communication with admin logic here
        }

        public void ProvideFeedback(string feedback)
        {
            // Implement feedback logic here
        }
    }
}
