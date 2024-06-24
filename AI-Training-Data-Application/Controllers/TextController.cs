using AI_Training_Data_Application_Backend.Data;
using AI_Training_Data_Application_Backend.DTOs;
using AI_Training_Data_Application_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AI_Training_Data_Application_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextController(TextContext context) : ControllerBase
{
    /// <summary>
    /// Ruft alle Texte aus der Datenbank ab, einschließlich ihren zugehörigen Kategorien.
    /// </summary>
    /// <returns>Eine Task, welche den asynchronen Vorgang darstellt. Das Ergebnis der Task enthält
    /// ein <see cref="ActionResult{TValue}"/> mit einer Liste von <see cref="TextGetDto"/> Objekten.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TextGetDto>>> GetTexts()
    {
        var texts = await context.Texts
            .Select(t => new TextGetDto
            {
                Id = t.Id,
                TextString = t.TextString,
                Categories = t.Categories.Select(c => new CategoryGetDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            }).ToListAsync();
        return Ok(texts);
    }
    
    /// <summary>
    /// Erzeugt einen neuen Text in der Datenbank und verknüpfet ihn mit den angegebenen Kategorien.
    /// </summary>
    /// <param name="textPostDto">Das Objekt, welches den Textinhalt und die IDs der zugehörigen Kategorien enthält.</param>
    /// <returns>
    /// Ein asynchroner Vorgang, welcher ein <see cref="ActionResult{TextGetDto}"/> zurückgibt.
    /// Das Ergebnis ist die neu erstellte Text, oder ein <see cref="ConflictObjectResult"/>,
    /// wenn ein Text mit demselben Textinhalt bereits existiert.
    /// Existiert mindestens eine angegebene Kategorie nicht, wird ein <see cref="BadRequestObjectResult"/> zurückgegeben.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<TextGetDto>> PostText(TextPostDto textPostDto)
    {
        // Prüft, ob ein Text mit demselben Textinhalt bereits existiert
        var existingText = await context.Texts
            .FirstOrDefaultAsync(t => t.TextString == textPostDto.TextString);
        if (existingText != null) return Conflict("Text existiert bereits.");

        // Prüft, ob alle angegebenen Kategorien existieren
        var categories = await context.Categories
            .Where(c => textPostDto.CategoryIds.Contains(c.Id))
            .ToListAsync();
        if (categories.Count != textPostDto.CategoryIds.Length)
            return BadRequest("Mindestens eine Kategorie existiert nicht.");

        var text = new Text
        {
            TextString = textPostDto.TextString,
            Categories = categories
        };

        context.Texts.Add(text);
        await context.SaveChangesAsync();

        var textReadDto = new TextGetDto
        {
            Id = text.Id,
            TextString = text.TextString,
            Categories = text.Categories.Select(c => new CategoryGetDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        };

        return CreatedAtAction(nameof(GetTexts), new { id = text.Id }, textReadDto);
    }
}