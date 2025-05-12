public class FolderService
{
    private readonly FolderRepository repository;

    public FolderService(FolderRepository repository)
    {
        this.repository = repository;
    }

    public async Task<FolderDto> AddAsync(CreateFolderDto dto, string userId)
    {
        //Kontrollerar om det angavs ett namn för mappen.
        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentException("Folder does not have a name.");
        }
        //Kontrollerar att userId inte är tomt.
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        //Omvandla till en Folder objekt med den datan som angavs  i DTO-objektet.
        var folder = new Folder { Name = dto.Name, UserId = userId };

        //Skicka sedan in objektet till databasen
        var savedFolder = await repository.AddAsync(folder);

        //Överför tillbaka till DTO och returnera till klienten
        return new FolderDto { Id = savedFolder.Id, Name = savedFolder.Name };
    }

    public async Task<IEnumerable<FolderWithFilesDto>> GetFoldersWithFilesAsync(string userId)
    {
        //Kontrollerar att userId inte är tomt.
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        var folders = await repository.GetFoldersWithFilesAsync(userId);

        //Om inga mappar hittas eller om det inte finns några element i listan, returnera en tom lista.
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
