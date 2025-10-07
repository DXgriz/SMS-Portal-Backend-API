namespace SMSPortalAPI.Models
{
    using System;
    using System.Collections.Generic;
    public class UserAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        // E.164 phone number assigned to the user (eg. +2771xxxxxxx)
        public string AssignedNumber { get; set; } = null!;
        public ICollection<SmsMessage> Messages { get; set; } = new List<SmsMessage>();
    }
}
