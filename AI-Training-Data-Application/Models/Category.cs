using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AI_Training_Data_Application_Backend.Models;

public class Category
{
    [Key] public int Id { get; set; }
    [Required] [MaxLength(100)] public required string Name { get; set; }
    [JsonIgnore] public ICollection<Text>? Texts { get; set; } = new List<Text>();
}