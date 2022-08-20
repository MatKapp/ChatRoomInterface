using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class InMemoryChatEventStorage : IChatEventStorage
{
    private readonly IList<ChatEvent> _chatEvents = new List<ChatEvent>();

    public IList<ChatEvent> Fetch(DateTime from, DateTime to)
        => _chatEvents.Where(chatEvent => chatEvent.OccuredAt >= from && chatEvent.OccuredAt <= to)
            .ToList();

    public void Add(ChatEvent chatEvent)
        => _chatEvents.Add(chatEvent);
}