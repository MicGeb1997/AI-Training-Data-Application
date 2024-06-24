namespace AI_Training_Data_Application_Backend.DTOs;

public class TextGetDto
{
    public int Id { get; set; }
    public string TextString { get; set; }
    public List<CategoryGetDto> Categories { get; set; }
}