//DTO-hjälpklass när man ska skicka tillbaka objektet som HTTP-respons
public class FileWithoutContentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
}
