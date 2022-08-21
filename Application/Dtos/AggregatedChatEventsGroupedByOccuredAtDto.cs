using Application.Common.Models;

namespace Application.Dtos;

public class AggregatedChatEventsGroupedByOccuredAtDto
{
    public DateTime OccuredAt { get; set; }
    public IList<ChatEventsAggregatedByEventType> ChatEventsGroupedByEventType { get; set; }
}