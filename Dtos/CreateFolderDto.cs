//DTO klass för att skapa mapp. Man är endast intresserad av att ge den ett namn, därför finns bara den propertyn.
//Här behövs inget ID, eftersom den sätts automatiskt av databasen när mappen skapas.
public class CreateFolderDto
{
    public string Name { get; set; } = "";
}
