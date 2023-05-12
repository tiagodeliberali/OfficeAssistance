namespace OfficeAssistance.API.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AssistanceController : ControllerBase
{
    private readonly ILogger<AssistanceController> _logger;
    private readonly Assistance assistance;

    public AssistanceController(ILogger<AssistanceController> logger)
    {
        _logger = logger;
        this.assistance = new Assistance(_logger, true);
    }

    [HttpPost(Name = "chat")]
    public async Task<ChatResponse> PostAsync(ChatRequest input)
    {
        var inputString = string.Join("\n", input.Conversation.Select(x => $"{x.Sender}: {x.Payload}"));

        return new ChatResponse()
        {
            BotResponse = (await assistance.Chat(inputString)).ToString(),
        };
    }
}
