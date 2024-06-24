using AI_Training_Data_Application_Backend.DTOs;
using Microsoft.AspNetCore.Components;

namespace AI_Training_Data_Application.Components.Pages;

public partial class Categories : ComponentBase
{
    [Inject] private HttpClient? Http { get; set; }

    [SupplyParameterFromForm]
    private CategoryPostDto NewCategory { get; set; } = new()
    {
        Name = ""
    };

    private List<CategoryGetDto>? _categories;

    protected override async Task OnInitializedAsync()
    {
        _categories = (await Http!.GetFromJsonAsync<List<CategoryGetDto>>("api/Category"))!;
    }

    /// <summary>
    /// Verarbeitet die Formularübermittlung durch Senden einer POST-Anfrage an den Endpunkt "api/Category" mit den Formular-Daten.
    /// Wenn die Anfrage erfolgreich ist, wird das Formular zurückgesetzt und die Liste der Kategorien aktualisiert.
    /// </summary>
    /// <returns>Einen Task, welcher den asynchronen Vorgang darstellt.</returns>
    private async Task AddCategory()
    {
        var response = await Http!.PostAsJsonAsync("api/Category", NewCategory);
        if (response.IsSuccessStatusCode)
        {
            _categories = await Http!.GetFromJsonAsync<List<CategoryGetDto>>("api/Category");
            NewCategory = new CategoryPostDto
            {
                Name = ""
            };
        }
    }
}