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

    public async Task<IEnumerable<Folder>> GetAsync(string userId)
    {
        var folders = await context.Folders.Where(f => f.UserId == userId).ToListAsync();
        return folders;
    }
}
