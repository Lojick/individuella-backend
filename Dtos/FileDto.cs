//DTO-hjälpklass när man ska skicka tillbaka objektet som HTTP-respons
public class FileDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";

    public byte[] Content { get; set; } = [];
}
