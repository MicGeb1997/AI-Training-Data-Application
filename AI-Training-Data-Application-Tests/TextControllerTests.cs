using System.Net;
using System.Text;
using System.Text.Json;
using AI_Training_Data_Application;
using AI_Training_Data_Application_Backend.DTOs;

namespace AI_Training_Data_Application_Tests;

public class TextControllerTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private const string RequestUri = "/api/Text";

    [Fact]
    public async Task GetTexts_ReturnsOkResult_WithListOfTexts()
    {
        // Act
        var response = await _client.GetAsync(RequestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var texts = JsonSerializer.Deserialize<List<TextGetDto>>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(texts);
    }

    [Fact]
    public async Task PostText_ValidRequest_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var textCreateDto = new TextPostDto
        {
            TextString = "NewText",
            CategoryIds = [1]
        };
        var content = new StringContent(JsonSerializer.Serialize(textCreateDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(RequestUri, content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnText = JsonSerializer.Deserialize<TextGetDto>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("NewText", returnText?.TextString);
    }

    [Theory]
    [InlineData(null, new[] { 1 })]
    [InlineData("NewText", new int[] { })]
    [InlineData("NewText", new[] { -1 })]
    public async Task PostText_InvalidRequest_ReturnsBadRequest(string textString, int[] categoryIds)
    {
        // Arrange
        var textCreateDto = new TextPostDto
        {
            TextString = textString,
            CategoryIds = categoryIds
        };
        var content = new StringContent(JsonSerializer.Serialize(textCreateDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(RequestUri, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostText_DuplicateTextString_ReturnsConflict()
    {
        // Arrange
        var textCreateDto = new TextPostDto
        {
            TextString = "DuplicateText",
            CategoryIds = [1]
        };
        var content = new StringContent(JsonSerializer.Serialize(textCreateDto), Encoding.UTF8, "application/json");

        // Act
        await _client.PostAsync(RequestUri, content); // First request
        var response = await _client.PostAsync(RequestUri, content); // Duplicate request

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}