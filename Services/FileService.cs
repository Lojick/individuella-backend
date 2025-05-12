public class FileService
{
    private readonly FileRepository repository;

    public FileService(FileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<FileDto> AddAsync(CreateFileDto dto, string userId)
    {
        if (string.IsNullOrEmpty(dto.FileName))
        {
            throw new ArgumentException("File does not have a name.");
        }
        if (dto.FolderId <= 0)
        {
            throw new ArgumentException("File must belong to a folder.");
        }
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        //Kontrollera om mappen tillhör användaren
        var folder = await repository.GetFolderByIdAsync(dto.FolderId);

        if (folder == null || folder.UserId != userId)
        {
            throw new UnauthorizedAccessException("Du får inte spara filer i denna mapp.");
        }

        //Överför till FileItem objekt med DTO inputen från tidigare
        var file = new FileItem
        {
            FileName = dto.FileName,
            Content = dto.Content,
            FolderId = dto.FolderId,
            UserId = userId,
        };

        //Spara filen i databasen
        var savedFile = await repository.AddAsync(file);

        //Konvertera tillbaka till DTO och returnera den till klienten
        return new FileDto { FileName = savedFile.FileName, Id = savedFile.Id };
    }

    public async Task<FileDto> DownloadFileByIdAsync(string userId, int fileId)
    {
        //Kontrollera att userId inte är tomt
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        //Hämta filen från databasen
        var file = await repository.DownloadFileByIdAsync(fileId);

        //Kontrollera att filen inte är tom eller om den tillhör användaren.
        if (file == null || file.UserId != userId)
        {
            throw new UnauthorizedAccessException("Du får inte ladda ner denna filen.");
        }

        //Kontrollera att filens innehåll inte är tom.
        if (file.Content == null || file.Content.Length == 0)
        {
            throw new InvalidOperationException("Filen har inget innehåll.");
        }

        //Konvertera till DTO och returnera den till klienten
        return new FileDto
        {
            FileName = file.FileName,
            Id = file.Id,
            Content = file.Content,
        };
    }

    public async Task<bool> DeleteFileByIdAsync(string userId, int fileId)
    {
        //Kontrollera att userId inte är tomt
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("Userid is missing.");
        }

        //Hämta fil från databasen
        var file = await repository.GetFileByIdAsync(fileId);

        //Kontrollera att filen existerar
        if (file == null)
        {
            throw new FileNotFoundException("Filen kunde inte hittas.");
        }

        //Kontrollera att filen tillhör användaren
        if (file.UserId != userId)
        {
            throw new UnauthorizedAccessException("Du får inte radera denna fil.");
        }

        //Tar bort filen och returnerar true (Att den gick igenom)
        await repository.DeleteFileAsync(file);
        return true;
    }
}
