using Application.Common.Interfaces;
using Application.Services;
using Domain.Entities;

namespace Application.UnitTests;

public class ChatEventServiceTests
{
    private readonly Mock<IChatEventStorage> _mockChatEventStorage;
    private readonly Mock<IDateTime> _mockDateTime;
    private readonly ChatEventService _chatEventService;

    public ChatEventServiceTests()
    {
        _mockChatEventStorage = new Mock<IChatEventStorage>();
        _mockDateTime = new Mock<IDateTime>();
        _chatEventService = new ChatEventService(_mockChatEventStorage.Object, _mockDateTime.Object);
    }
    
    [Fact]
    public void Fetch_WithCorrectParameters_InvokesStorageFetch()
    {
        // Arrange
        var from = DateTime.UtcNow.AddYears(-1);
        var to = DateTime.UtcNow;

        _mockChatEventStorage.Setup(s => s.Fetch(from, to))
            .Returns(new List<ChatEvent>());
        
        // Act
        _chatEventService.Fetch(from, to);
        
        // Assert
        _mockChatEventStorage.Verify(s => s.Fetch(from, to), Times.Once);
    }
    
    [Fact]
    public void AddEnterRoom_InvokesStorageAdd_WithEnterRoomChatEvent()
    {
        // Arrange
        var userName = "user";
        var occuredAt = DateTime.UtcNow;
        _mockDateTime.Setup(dt => dt.Now)
            .Returns(occuredAt);
        
        // Act
        _chatEventService.AddEnterRoom(userName);
        
        // Assert
        _mockChatEventStorage.Verify(s => s.Add(
                It.Is<EnterRoom>(er => er.OccuredAt == occuredAt && er.UserName == userName)), Times.Once);
    }
    
    [Fact]
    public void AddLeaveRoom_InvokesStorageAdd_WithLeaveRoomChatEvent()
    {
        // Arrange
        var userName = "user";
        var occuredAt = DateTime.UtcNow;
        _mockDateTime.Setup(dt => dt.Now)
            .Returns(occuredAt);
        
        // Act
        _chatEventService.AddLeaveRoom(userName);
        
        // Assert
        _mockChatEventStorage.Verify(s => s.Add(
            It.Is<LeaveRoom>(lr => lr.OccuredAt == occuredAt && lr.UserName == userName)), Times.Once);
    }
    
    [Fact]
    public void AddComment_InvokesStorageAdd_WithCommentChatEvent()
    {
        // Arrange
        var userName = "user";
        var message = "message";
        var occuredAt = DateTime.UtcNow;
        _mockDateTime.Setup(dt => dt.Now)
            .Returns(occuredAt);
        
        // Act
        _chatEventService.AddComment(userName, message);
        
        // Assert
        _mockChatEventStorage.Verify(s => s.Add(
            It.Is<Comment>(c => c.OccuredAt == occuredAt && c.UserName == userName && c.Message == message)), Times.Once);
    }
    
    [Fact]
    public void AddHighFive_InvokesStorageAdd_WithHighFiveEvent()
    {
        // Arrange
        var userName = "user";
        var receiverName = "receiverName";
        var occuredAt = DateTime.UtcNow;
        _mockDateTime.Setup(dt => dt.Now)
            .Returns(occuredAt);
        
        // Act
        _chatEventService.AddHighFive(userName, receiverName);
        
        // Assert
        _mockChatEventStorage.Verify(s => s.Add(
            It.Is<HighFive>(hf => hf.OccuredAt == occuredAt && hf.UserName == userName && hf.ReceiverName == receiverName)), Times.Once);
    }

    [Theory]
    [InlineData("24:00:00", 2)]
    [InlineData("02:00:00", 3)]
    [InlineData("01:00:00", 4)]
    [InlineData("00:01:00", 7)]
    [InlineData("00:00:01", 7)]
    public void FetchAndAggregate_InvokesFetchAndAggregatesCorrectlyByInterval(string intervalToParse, int expectedCount)
    {
        // Arrange
        var from = DateTime.Parse("2021-01-01");
        var to = DateTime.UtcNow;
        var interval = TimeSpan.Parse(intervalToParse);

        _mockChatEventStorage.Setup(s => s.Fetch(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(GenerateChatEvents());

        // Act
        var result = _chatEventService.FetchAndAggregate(from, to, interval);

        // Assert
        result.Should().HaveCount(expectedCount);
    }

    private List<ChatEvent> GenerateChatEvents()
        => new()
        {
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-17T20:00:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T00:00:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T00:30:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T01:00:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T01:30:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T02:00:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T02:30:00") },
            new EnterRoom { OccuredAt = DateTime.Parse("2022-08-18T02:30:00") }
        };
}