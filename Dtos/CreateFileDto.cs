//DTO-hjälpklass när man laddar upp fil till en mapp (innan den överförs till en riktig FileItem-objekt och sedan lagras i databas)
public class CreateFileDto
{
    public string FileName { get; set; } = "";
    public byte[] Content { get; set; } = [];
    public int FolderId { get; set; }
}
