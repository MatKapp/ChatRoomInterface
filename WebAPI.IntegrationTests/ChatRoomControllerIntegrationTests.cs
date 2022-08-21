using System.Net;
using System.Net.Http.Json;
using Application.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace WebAPI.IntegrationTests;

public class ChatRoomControllerIntegrationTests
{
    private const string ChatEventControllerPath = "ChatEvent";
    private const string AggregatedEndpointPath = "Aggregated";
    private const string EnterRoomEndpointPath = "EnterRoom";
    private const string LeaveRoomEndpointPath = "LeaveRoom";
    private const string HighFiveEndpointPath = "HighFive";
    private const string CommentEndpointPath = "Comment";
    private readonly HttpClient _httpClient;

    public ChatRoomControllerIntegrationTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }
    
    [Fact]
    public async Task GetChatEvents_ReturnsNotFoundAfterStart()
    {
        // ARRANGE
        // ACT
        var response = await _httpClient.GetAsync(ChatEventControllerPath);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task EnterRoom_AddsEvent_WhichIsReturnedWithGetChatEvents()
    {
        // ARRANGE
        // ACT
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{EnterRoomEndpointPath}?username=userName", null);
        var response = await _httpClient.GetAsync($"{ChatEventControllerPath}?to={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}");
        var deserializedResult = await response.Content.ReadFromJsonAsync<List<ChatEventDto>>();
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deserializedResult.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task AddingDifferentEventTypes_AddsEvents_WhichAreReturnedWithGetChatEvents()
    {
        // ARRANGE
        // ACT
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{EnterRoomEndpointPath}?username=userName", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{LeaveRoomEndpointPath}?username=userName", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{HighFiveEndpointPath}?username=userName&receiverName=receiver", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{CommentEndpointPath}?username=userName&message=message", null);
        var response = await _httpClient.GetAsync($"{ChatEventControllerPath}?to={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}");
        var deserializedResult = await response.Content.ReadFromJsonAsync<List<ChatEventDto>>();
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deserializedResult.Count.Should().Be(4);
    }
    
    [Fact]
    public async Task AddingDifferentEventTypes_AddsEvents_WhichAreReturnedWithGetAggregatedChatEvents()
    {
        // ARRANGE
        // ACT
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{EnterRoomEndpointPath}?username=userName", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{LeaveRoomEndpointPath}?username=userName", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{HighFiveEndpointPath}?username=userName&receiverName=receiver", null);
        await _httpClient.PostAsync($"{ChatEventControllerPath}/{CommentEndpointPath}?username=userName&message=message", null);
        var response = await _httpClient.GetAsync($"{ChatEventControllerPath}/{AggregatedEndpointPath}?to={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}&&interval=01:00:00");
        var deserializedResult = await response.Content.ReadFromJsonAsync<List<AggregatedChatEventsGroupedByOccuredAtDto>>();
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deserializedResult.Count.Should().Be(1);
        deserializedResult.First().ChatEventsGroupedByEventType.Count.Should().Be(4);
    }
}