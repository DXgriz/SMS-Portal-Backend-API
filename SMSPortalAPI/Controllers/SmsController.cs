using Microsoft.AspNetCore.Mvc;
using SMSPortalAPI.Data;
using SMSPortalAPI.Models;
using Twilio.Security; // optional if using Twilio SDK

[ApiController]
[Route("api/[controller]")]
public class SmsController : ControllerBase
{
    private readonly SmsPDataAccess _db;
    private readonly IConfiguration _config;

    public SmsController(SmsPDataAccess db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("receive")]
    public async Task<IActionResult> Receive()
    {
        // Twilio posts application/x-www-form-urlencoded with keys: From, To, Body, ...
        var form = await Request.ReadFormAsync();
        string from = form["From"];
        string to = form["To"];
        string body = form["Body"];

        // Optional: validate Twilio signature to ensure request is from Twilio
        var twilioToken = _config["Twilio:AuthToken"];
        if (!string.IsNullOrEmpty(twilioToken))
        {
            if (Request.Headers.TryGetValue("X-Twilio-Signature", out var signature))
            {
                var validator = new RequestValidator(twilioToken);
                var url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
                var dict = form.ToDictionary(k => k.Key, k => k.Value.ToString());
                if (!validator.Validate(url, dict, signature))
                {
                    return Forbid(); // signature invalid
                }
            }
        }

        // find user account by assigned number
        var user = _db.UserAccounts.FirstOrDefault(u => u.AssignedNumber == to);

        var msg = new SmsMessage
        {
            From = from,
            To = to,
            Body = body,
            ReceivedAt = DateTime.UtcNow,
            UserAccountId = user?.Id
        };

        _db.SmsMessages.Add(msg);
        await _db.SaveChangesAsync();

        // reply 200 OK quickly. No need to return TwiML in our case.
        return Ok();
    }
}
