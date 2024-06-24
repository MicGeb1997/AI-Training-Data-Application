using System.Net;
using System.Text;
using System.Text.Json;
using AI_Training_Data_Application;
using AI_Training_Data_Application_Backend.DTOs;

namespace AI_Training_Data_Application_Tests;

public class CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private const string RequestUri = "/api/Category";

    [Fact]
    public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
    {
        // Act
        var response = await _client.GetAsync(RequestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryGetDto>>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(categories);
    }

    [Fact]
    public async Task PostCategory_ValidRequest_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var categoryDto = new CategoryPostDto
        {
            Name = "NewCategory"
        };
        var content = new StringContent(JsonSerializer.Serialize(categoryDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(RequestUri, content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnCategory = JsonSerializer.Deserialize<CategoryGetDto>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Equal("NewCategory", returnCategory?.Name);
    }

    [Fact]
    public async Task PostCategory_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var content = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(RequestUri, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostCategory_DuplicateName_ReturnsConflict()
    {
        // Arrange
        var categoryDto = new CategoryPostDto()
        {
            Name = "DuplicateCategory"
        };
        var content = new StringContent(JsonSerializer.Serialize(categoryDto), Encoding.UTF8, "application/json");

        // Act
        await _client.PostAsync(RequestUri, content); // First request
        var response = await _client.PostAsync(RequestUri, content); // Duplicate request

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}