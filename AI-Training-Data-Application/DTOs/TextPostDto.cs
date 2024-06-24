using System.ComponentModel.DataAnnotations;

namespace AI_Training_Data_Application_Backend.DTOs;

public class TextPostDto
{
    [Required] public required string TextString { get; set; }
    [Required, MinLength(1)] public required int[] CategoryIds { get; set; }
}