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

        //Överför tillbaka till DTO och returnera den vidare till klienten
        return new FileDto { FileName = savedFile.FileName, Id = savedFile.Id };
    }
}
