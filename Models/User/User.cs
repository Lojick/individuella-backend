using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // En användare kan ha flera mappar och filer
    public List<Folder> Folders { get; set; } = new();
    public List<FileItem> Files { get; set; } = new();
}
