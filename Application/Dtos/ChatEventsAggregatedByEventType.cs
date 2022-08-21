using Application.Common.Models;

namespace Application.Dtos;

public class ChatEventsAggregatedByEventType
{
    public ChatEventType EventType { get; set; }
    public IList<string> Messages { get; set; }
}