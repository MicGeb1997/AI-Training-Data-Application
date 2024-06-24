using System.ComponentModel.DataAnnotations;

namespace AI_Training_Data_Application_Backend.DTOs;

public class CategoryPostDto
{
    [Required] [MaxLength(100)] public required string Name { get; set; }
}