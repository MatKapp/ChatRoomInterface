using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IChatEventStorage
{
    IList<ChatEvent> Fetch(DateTime from, DateTime to);
    void Add(ChatEvent chatEvent);
}