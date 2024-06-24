using AI_Training_Data_Application_Backend.Data;
using AI_Training_Data_Application_Backend.DTOs;
using AI_Training_Data_Application_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AI_Training_Data_Application_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(TextContext context) : ControllerBase
{
    /// <summary>
    /// Ruft alle Kategorien aus der Datenbank ab, einschließlich ihrer zugehörigen Texte.
    /// </summary>
    /// <returns>Eine Task, welche den asynchronen Vorgang darstellt. Das Ergebnis der Task enthält
    /// ein <see cref="ActionResult{TValue}"/> mit einer Liste von <see cref="Category"/> Objekten.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await context.Categories.Include(c => c.Texts).ToListAsync();
    }

    /// <summary>
    /// Erzeugt eine neue Kategorie in der Datenbank.
    /// </summary>
    /// <param name="categoryPostDto">Das Objekt, welches den Kategorienamen enthält.</param>
    /// <returns>
    /// Ein asynchroner Vorgang, welcher ein <see cref="ActionResult{Category}"/> zurückgibt.
    /// Das Ergebnis ist die neu erstellte Kategorie, oder ein <see cref="ConflictObjectResult"/>,
    /// wenn eine Kategorie mit demselben Namen bereits existiert.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(CategoryPostDto categoryPostDto)
    {
        var existingCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Name == categoryPostDto.Name);
        if (existingCategory != null) return Conflict("Kategorie existiert bereits.");

        var category = new Category
        {
            Name = categoryPostDto.Name
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }
}