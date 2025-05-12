//Databas-tabell för mappar
public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    // Foreign key till ApplicationUser
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;

    // En mapp kan innehålla flera filer
    public List<FileItem> Files { get; set; } = new();
}
