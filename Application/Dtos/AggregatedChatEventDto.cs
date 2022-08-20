namespace Application.Dtos;

public class AggregatedChatEventDto
{
    public DateTime OccuredAt { get; set; }
    public IList<string> ChatEventMessages { get; set; }
}