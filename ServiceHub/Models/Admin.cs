namespace ServiceHub.Models
{
    public class Admin : User
    {
        public int AdminId { get; set; }

        public void ViewManageBookings()
        {
            // Implement view and manage bookings logic here
        }

        public void ViewFeedbackInquiries()
        {
            // Implement view feedback and inquiries logic here
        }

        public void DeactivateDeleteAccounts()
        {
            // Implement deactivate or delete accounts logic here
        }
    }

}
