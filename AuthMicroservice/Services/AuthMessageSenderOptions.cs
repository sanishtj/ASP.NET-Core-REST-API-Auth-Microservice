namespace AuthMicroservice.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string HealthRecordStackSecret { get; set; }
    }
}
