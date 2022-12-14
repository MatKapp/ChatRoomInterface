using Application.Common.Models;

namespace Application.Dtos;

public class ChatEventDto
{
    public DateTime OccuredAt { get; set; }
    public string Message { get; set; }
    public ChatEventType EventType { get; set; }
}