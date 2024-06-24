using AI_Training_Data_Application_Backend.DTOs;
using Microsoft.AspNetCore.Components;

namespace AI_Training_Data_Application.Components.Pages;

public partial class TextList : ComponentBase
{
    [Inject] private HttpClient? Http { get; set; }
    
    private List<TextGetDto>? _textItems;

    protected override async Task OnInitializedAsync()
    {
        _textItems = await Http!.GetFromJsonAsync<List<TextGetDto>>("api/Text");
    }
}