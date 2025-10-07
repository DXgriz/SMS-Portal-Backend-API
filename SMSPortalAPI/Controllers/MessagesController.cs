using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSPortalAPI.Data;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly SmsPDataAccess _db;

    public MessagesController(SmsPDataAccess db) => _db = db;

    [HttpGet]
    public IActionResult GetMyMessages()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _db.UserAccounts.Find(userId);
        if (user == null) return Unauthorized();

        var msgs = _db.SmsMessages
            .Where(m => m.UserAccountId == userId || m.To == user.AssignedNumber)
            .OrderByDescending(m => m.ReceivedAt)
            .Select(m => new { m.Id, m.From, m.Body, m.ReceivedAt, m.To })
            .ToList();

        return Ok(msgs);
    }
}
