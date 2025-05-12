using Microsoft.EntityFrameworkCore;

public class FolderRepository
{
    private readonly ApplicationDbContext context;

    public FolderRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Folder> AddAsync(Folder folder)
    {
        context.Folders.Add(folder);
        await context.SaveChangesAsync();
        return folder;
    }

    public async Task<IEnumerable<Folder>> GetFoldersWithFilesAsync(string userId)
    {
        //Slår ihop både Folder och FileItem tabellerna tillsamans.
        var folders = await context
            .Folders.Include(f => f.Files)
            .Where(f => f.UserId == userId)
            .ToListAsync();
        return folders;
    }
}
