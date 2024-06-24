using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AI_Training_Data_Application_Backend.Models;

public class Text
{
    [Key] public int Id { get; set; }
    [Required] public required string TextString { get; set; }
    [Required] [JsonIgnore] public required ICollection<Category> Categories { get; set; }
}