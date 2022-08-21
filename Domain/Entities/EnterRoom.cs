using Application.Common.Models;

namespace Domain.Entities;

public class EnterRoom : ChatEvent
{
    public override ChatEventType EventType => ChatEventType.EnterRoom;
    
    public override string GenerateMessage()
        => $"{UserName} enters the room";
}