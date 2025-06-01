//DTO-hjälpklass när man ska göra .Include på både mapp och filer.
public class FolderWithFilesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<FileWithoutContentDto> Files { get; set; } = new();
}
