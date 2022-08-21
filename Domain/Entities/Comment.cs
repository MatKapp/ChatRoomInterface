using Application.Common.Models;

namespace Domain.Entities;

public class Comment : ChatEvent
{
    public string Message { get; set; }
    public override ChatEventType EventType => ChatEventType.Comment;

    public override string GenerateMessage()
        => $"{UserName} comments: \"{Message}\"";
}