using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

public class FileRepository
{
    private readonly ApplicationDbContext context;

    public FileRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<FileItem> AddAsync(FileItem file)
    {
        context.Files.Add(file);
        await context.SaveChangesAsync();
        return file;
    }

    public async Task<Folder?> GetFolderByIdAsync(int folderId)
    {
        return await context.Folders.FirstOrDefaultAsync(f => f.Id == folderId);
    }
}
