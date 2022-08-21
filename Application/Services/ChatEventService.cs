using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities;

namespace Application.Services;

public class ChatEventService : IChatEventService
{
    private readonly IChatEventStorage _chatEventStorage;
    private readonly IDateTime _dateTime;

    public ChatEventService(IChatEventStorage chatEventStorage, IDateTime dateTime)
    {
        _chatEventStorage = chatEventStorage;
        _dateTime = dateTime;
    }

    public IList<ChatEventDto> Fetch(DateTime from, DateTime to)
        => _chatEventStorage.Fetch(from, to)
            .Select(chatEvent => new ChatEventDto
            {
                OccuredAt = chatEvent.OccuredAt,
                Message = chatEvent.GenerateMessage(),
                EventType = chatEvent.EventType
            }).ToList();

    public void AddEnterRoom(string userName)
    {
        var chatEvent = new EnterRoom
        {
            OccuredAt = _dateTime.Now,
            UserName = userName
        };
        
        _chatEventStorage.Add(chatEvent);
    }
    
    public void AddLeaveRoom(string userName)
    {
        var chatEvent = new LeaveRoom
        {
            OccuredAt = _dateTime.Now,
            UserName = userName
        };
        
        _chatEventStorage.Add(chatEvent);

    }

    public void AddComment(string userName, string message)
    {
        var chatEvent = new Comment
        {
            OccuredAt = _dateTime.Now,
            Message = message,
            UserName = userName
        };
        
        _chatEventStorage.Add(chatEvent);
    }
    
    public void AddHighFive(string userName, string receiverName)
    {
        var chatEvent = new HighFive()
        {
            OccuredAt = _dateTime.Now,
            UserName = userName,
            ReceiverName = receiverName
        };
        
        _chatEventStorage.Add(chatEvent);
    }

    public IList<AggregatedChatEventsGroupedByOccuredAtDto> FetchAndAggregate(DateTime from, DateTime to, TimeSpan timeSpan)
    {
        var chatEvents = Fetch(from, to);

        var aggregatedChatEvents = chatEvents.GroupBy(chatEvent => chatEvent.OccuredAt.Ticks / timeSpan.Ticks)
            .Select(group => new AggregatedChatEventsGroupedByOccuredAtDto
            {
                OccuredAt = new DateTime(group.Key * timeSpan.Ticks),
                ChatEventsGroupedByEventType = group.GroupBy(message => message.EventType).
                    Select(eventGroup => new ChatEventsAggregatedByEventType()
                    {
                        EventType = eventGroup.Key,
                        Messages = eventGroup.Select(chatEvent => chatEvent.Message).ToList()
                    }).ToList()
            }).ToList();

        return aggregatedChatEvents;
    }
}