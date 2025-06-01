//DTO-hjälpklass för att utesluta filinnehåll i mapp-respons
public class FileWithoutContentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
}
