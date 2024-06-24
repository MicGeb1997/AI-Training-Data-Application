using AI_Training_Data_Application_Backend.DTOs;
using Microsoft.AspNetCore.Components;

namespace AI_Training_Data_Application.Components.Pages;

public partial class Text : ComponentBase
{
    [Inject] private HttpClient? Http { get; set; }

    private List<CategoryGetDto>? Categories { get; set; }

    [SupplyParameterFromForm]
    private TextPostDto TextObject { get; set; } = new()
    {
        TextString = "",
        CategoryIds = []
    };

    protected override async Task OnInitializedAsync()
    {
        Categories = await Http!.GetFromJsonAsync<List<CategoryGetDto>>("/api/Category");
    }

    /// <summary>
    /// Verarbeitet die Formularübermittlung durch Senden einer POST-Anfrage an den Endpunkt "api/Text" mit den Formular-Daten.
    /// Wenn die Anfrage erfolgreich ist, wird das Formular zurückgesetzt.
    /// </summary>
    /// <returns>Eine Task, welche den asynchronen Vorgang darstellt.</returns>
    private async Task HandleSubmit()
    {
        var response = await Http!.PostAsJsonAsync("/api/Text", TextObject);
        if (response.IsSuccessStatusCode)
        {
            TextObject = new TextPostDto
            {
                TextString = "",
                CategoryIds = []
            };
        }
    }
}