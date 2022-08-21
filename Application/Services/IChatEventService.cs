using Application.Common.Models;
using Application.Dtos;

namespace Application.Services;

public interface IChatEventService
{
    IList<ChatEventDto> Fetch(DateTime from, DateTime to);
    void AddEnterRoom(string userName);
    void AddLeaveRoom(string userName);
    void AddComment(string userName, string message);
    void AddHighFive(string userName, string receiverName);
    IList<AggregatedChatEventsGroupedByOccuredAtDto> FetchAndAggregate(DateTime from, DateTime to, TimeSpan timeSpan);
}