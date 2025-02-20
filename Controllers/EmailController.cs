using Microsoft.AspNetCore.Mvc;
using otel_advisor_webApp.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
    {
        if (string.IsNullOrEmpty(emailRequest.body) || string.IsNullOrEmpty(emailRequest.to))
        {
            return BadRequest("Email content or receiver mail cannot be empty.");
        }

        try
        {
            await _emailService.SendEmailAsync(emailRequest.to, emailRequest.subject, emailRequest.body);
            return Ok("Email sent successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public class EmailRequest
{
    public string to { get; set; }
    public string subject { get; set; }
    public string body{ get; set; }
}
