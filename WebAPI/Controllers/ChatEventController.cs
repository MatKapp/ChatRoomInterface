using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomInterface.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatEventController : ControllerBase
{
    private readonly ILogger<ChatEventController> _logger;
    private readonly IChatEventService _chatEventService;

    public ChatEventController(ILogger<ChatEventController> logger, IChatEventService chatEventService)
    {
        _logger = logger;
        _chatEventService = chatEventService;
    }

    [HttpGet]
    public IActionResult Get(DateTime from, DateTime to)
    {
        var chatEvents = _chatEventService.Fetch(from, to);

        if (!chatEvents.Any())
        {
            return NotFound();
        }
        
        return Ok(chatEvents);
    }
    
    [HttpGet("Aggregated")]
    public IActionResult GetAggregated(DateTime from, DateTime to, TimeSpan interval)
    {
        var chatEvents = _chatEventService.FetchAndAggregate(from, to, interval);

        if (!chatEvents.Any())
        {
            return NotFound();
        }
        
        return Ok(chatEvents);
    }
    
    [HttpPost("EnterRoom")]
    public IActionResult EnterRoom(string userName)
    {
        _chatEventService.AddEnterRoom(userName);
        return Ok();
    }
    
    [HttpPost("LeaveRoom")]
    public IActionResult LeaveRoom(string userName)
    {
        _chatEventService.AddLeaveRoom(userName);
        return Ok();
    }
    
    [HttpPost("HighFive")]
    public IActionResult HighFive(string userName, string receiverName)
    {
        _chatEventService.AddHighFive(userName, receiverName);
        return Ok();
    }
    
    [HttpPost("Comment")]
    public IActionResult Comment(string userName, string message)
    {
        _chatEventService.AddComment(userName, message);
        return Ok();
    }
}