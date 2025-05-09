//Dto klass för att hämta mapp och samma syfte som CreateFolderDto förutom att man behöver lägga in id för att klienten ska se mapp-id.
public class FolderDto
{
    public string Name { get; set; } = "";
    public int Id { get; set; }
}
