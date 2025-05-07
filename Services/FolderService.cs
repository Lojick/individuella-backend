public class FolderService
{
    private readonly FolderRepository repository;

    public FolderService(FolderRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Folder> AddAsync(CreateFolderDto dto, string userId)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentException("Folder does not have a name.");
        }
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        var folder = new Folder { Name = dto.Name, UserId = userId };

        return await repository.AddAsync(folder);
    }

}
