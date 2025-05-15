public class FileService
{
    private readonly FileRepository repository;

    public FileService(FileRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Laddar upp fil till en specifik mapp som tillhör en användare.
    /// </summary>
    /// <param name="file">Filen som ska laddas upp.</param>
    /// <param name="folderId">ID för mappen där filen ska sparas.</param>
    /// <param name="userId">ID för den inloggade användaren som äger mappen.</param>
    /// <returns>En DTO som innehåller information om den sparade filen, inklusive namn, ID och innehåll.</returns>
    /// <exception cref="ArgumentException">Kastas om filen saknas, om filen är tom, om filnamn saknas, eller om inget giltigt mapp-ID anges.</exception>
    /// <exception cref="UnauthorizedAccessException">Kastas om användaren försöker ladda upp en fil till en mapp som inte tillhör dem.</exception>
    public async Task<FileDto> UploadFileByIdAsync(IFormFile file, int folderId, string userId)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is missing or empty.");
        }
        if (string.IsNullOrEmpty(file.FileName))
        {
            throw new ArgumentException("File must have a name.");
        }
        if (folderId <= 0)
        {
            throw new ArgumentException("File must belong to a folder.");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var content = memoryStream.ToArray();

        //Hämta mapp från databas för kontroll
        var folder = await repository.GetFolderByIdAsync(folderId);

        //Kontrollera att filen inte är tom eller om den tillhör användaren.
        if (folder == null || folder.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You are not allowed to upload files to this folder."
            );
        }

        //Överför till FileItem objekt med DTO inputen från tidigare
        var newFile = new FileItem
        {
            FileName = file.FileName,
            Content = content,
            FolderId = folderId,
            UserId = userId,
        };

        //Spara filen i databasen
        var savedFile = await repository.UploadFileAsync(newFile);

        //Konvertera tillbaka till DTO och returnera den till klienten
        return new FileDto
        {
            FileName = savedFile.FileName,
            Id = savedFile.Id,
            Content = savedFile.Content,
        };
    }

    /// <summary>
    /// Laddar ner fil som tillhör en användare.
    /// </summary>
    /// <param name="userId">ID för den inloggade användaren som äger filen.</param>
    /// <param name="fileId">ID för filen som ska laddas ner. </param>
    /// <returns>En DTO som innehåller information om den nedladdade filen, inklusive namn, ID och innehåll.</returns>
    /// <exception cref="ArgumentException">Kastas om filen har ogiltigt file ID..</exception>
    /// <exception cref="FileNotFoundException">Kastas om filen inte existerar.</exception>
    /// <exception cref="UnauthorizedAccessException">Kastas om användaren försöker ladda ner en fil som inte tillhör dem.</exception>
    /// <exception cref="InvalidOperationException">Kastas om filen saknar innehåll.</exception>
    public async Task<FileDto> DownloadFileByIdAsync(string userId, int fileId)
    {
        //Kontrollera att fileId är giltigt
        if (fileId <= 0)
        {
            throw new ArgumentException("Invalid file ID.");
        }

        //Hämta filen från databas för kontroll.
        var file = await repository.DownloadFileByIdAsync(fileId);

        //Kontrollera att filen existerar
        if (file == null)
        {
            throw new FileNotFoundException("File could not be found.");
        }
        //Kontrollera att filen tillhör användaren
        if (file.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to download this file.");
        }

        //Kontrollera att filens innehåll inte är tom.
        if (file.Content == null || file.Content.Length == 0)
        {
            throw new InvalidOperationException("File is empty.");
        }

        //Konvertera till DTO och returnera den till klienten
        return new FileDto
        {
            FileName = file.FileName,
            Id = file.Id,
            Content = file.Content,
        };
    }

    /// <summary>
    /// Raderar en fil som tillhör en specifik användare.
    /// </summary>
    /// <param name="userId">ID för den inloggade användaren som äger filen.</param>
    /// <param name="fileId">ID för filen som ska raderas.</param>
    /// <returns>True om raderingen lyckades.</returns>
    /// <exception cref="ArgumentException">Kastas om filen har ogiltigt file ID.</exception>
    /// <exception cref="FileNotFoundException">Kastas om filen inte kunde hittas i databasen.</exception>
    /// <exception cref="UnauthorizedAccessException">Kastas om användaren försöker radera en fil som inte tillhör dem.</exception>
    public async Task<bool> DeleteFileByIdAsync(string userId, int fileId)
    {
        //Kontrollera att fileId är giltigt
        if (fileId <= 0)
        {
            throw new ArgumentException("Invalid file ID.");
        }

        //Hämta fil från databasen för kontroll.
        var file = await repository.GetFileByIdAsync(fileId);

        //Kontrollera att filen existerar
        if (file == null)
        {
            throw new FileNotFoundException("File could not be found.");
        }

        //Kontrollera att filen tillhör användaren
        if (file.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to delete this file.");
        }

        //Tar bort filen och returnerar true (Att den gick igenom)
        await repository.DeleteFileAsync(file);
        return true;
    }
}
