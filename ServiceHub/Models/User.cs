namespace ServiceHub.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public string ProfileInfo()
        {
            return $"{FirstName} {LastName}, Age: {Age}, Gender: {Gender}";
        }

        public void Register(string email, string phone, string password)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");
            if (!IsValidPhoneNumber(phone))
                throw new ArgumentException("Invalid phone number format.");
            if (!IsValidPassword(password))
                throw new ArgumentException("Password does not meet complexity requirements.");

            Email = email;
            PhoneNumber = phone;
            Password = password;
        }

        public void UpdateProfile(string name, int age, string gender)
        {
            var names = name.Split(' ');
            FirstName = names[0];
            LastName = names.Length > 1 ? names[1] : "";
            Age = age;
            Gender = gender;
        }

        private bool IsValidEmail(string email)
        {
            // Implement email validation logic
            return true;
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Implement phone number validation logic
            return true;
        }

        private bool IsValidPassword(string password)
        {
            // Implement password complexity validation logic
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public List<ServiceProvider> SearchServiceProviders(string criteria)
        {
            // Implement search logic here
            return new List<ServiceProvider>();
        }

        public void SelectServiceProvider(ServiceProvider provider)
        {
            // Implement selection logic here
        }

        public void ScheduleAppointment(ServiceProvider provider, string time)
        {
            // Implement scheduling logic here
        }

        public void RateAndReview(ServiceProvider provider, int rating, string review)
        {
            // Implement rating and review logic here
        }

        public void ReceiveNotifications()
        {
            // Implement notification logic here
        }
    }
}
