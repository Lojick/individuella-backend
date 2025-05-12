public class FolderWithFilesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<FileDto> Files { get; set; } = new();
}
