//Databas-tabell för filer
public class FileItem
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
    public byte[] Content { get; set; } = [];

    // Foreign key till mappen den tillhör
    public int FolderId { get; set; }
    public Folder Folder { get; set; } = null!;

    // Foreign key till användaren
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
}
