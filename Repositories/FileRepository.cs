using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

public class FileRepository
{
    private readonly ApplicationDbContext context;

    public FileRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<FileItem> UploadFileAsync(FileItem file)
    {
        context.Files.Add(file);
        await context.SaveChangesAsync();
        return file;
    }

    public async Task<FileItem?> GetFileByIdAsync(int fileId)
    {
        return await context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
    }

    public async Task<Folder?> GetFolderByIdAsync(int folderId)
    {
        return await context.Folders.FirstOrDefaultAsync(f => f.Id == folderId);
    }

    public async Task<FileItem?> DownloadFileByIdAsync(int fileId)
    {
        return await context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
    }

    public async Task DeleteFileAsync(FileItem file)
    {
        context.Files.Remove(file);
        await context.SaveChangesAsync();
    }
}
