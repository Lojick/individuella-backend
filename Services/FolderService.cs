public class FolderService
{
    private readonly FolderRepository repository;

    public FolderService(FolderRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Skapar en ny mapp för en specifik användare.
    /// </summary>
    /// <param name="dto">Objekt som innehåller information om mappen, t.ex. namn.</param>
    /// <param name="userId">ID för den inloggade användaren som äger mappen.</param>
    /// <returns>En DTO som representerar den skapade mappen, inklusive ID och namn.</returns>
    public async Task<FolderDto> AddFolderAsync(CreateFolderDto dto, string userId)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentException("Folder must have a name.");
        }

        //Omvandla till en Folder objekt med den datan som angavs i DTO-objektet.
        var folder = new Folder { Name = dto.Name, UserId = userId };

        //Skicka sedan in objektet till databasen
        var savedFolder = await repository.AddFolderAsync(folder);

        return new FolderDto { Id = savedFolder.Id, Name = savedFolder.Name };
    }

    /// <summary>
    /// Hämtar mappar med filer för en specifik användare.
    /// </summary>
    /// <param name="userId">ID för den inloggade användaren som äger mapparna.</param>
    /// <returns>En lista med mappar och deras filer, eller en tom lista om inga hittas.</returns>
    public async Task<IEnumerable<FolderWithFilesDto>> GetFoldersWithFilesAsync(string userId)
    {
        var folders = await repository.GetFoldersWithFilesAsync(userId);

        if (folders == null || !folders.Any())
        {
            return Enumerable.Empty<FolderWithFilesDto>();
        }

        //Omvandlar till DTO och skapar nya objekter genom att välja rad för rad, som blir en lista av objekter. Eftersom det finns en lista inuti dto-mappen, måste man även göra .Select på den.
        return folders.Select(f => new FolderWithFilesDto
        {
            Id = f.Id,
            Name = f.Name,
            Files = f
                .Files.Select(file => new FileDto { Id = file.Id, FileName = file.FileName })
                .ToList(),
        });
    }
}
