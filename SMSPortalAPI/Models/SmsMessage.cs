namespace SMSPortalAPI.Models
{
    using System;
    public class SmsMessage
    {
        public int Id { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime ReceivedAt { get; set; }
        public Guid? UserAccountId { get; set; }
        public UserAccount? UserAccount { get; set; }
    }
}
