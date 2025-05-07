public class CreateFileDto
{
    public string FileName { get; set; } = "";
    public byte[] Content { get; set; } = [];
    public int FolderId { get; set; }
}
