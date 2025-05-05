# individuella-backend

Detta är den individuella uppgiften för Backend-kursen.  
Projektet är byggt med ASP.NET, PostgreSQL och Identity Core.

---

## 🔧 Databaskonfiguration

För att köra projektet behöver du skapa en egen `appsettings.json` med rätt databasinställningar.

### 1. Skapa en fil med namnet `appsettings.json`

Skapa filen i projektets rot (samma plats där `.csproj`-filen ligger).

### 2. Klistra in följande innehåll i filen:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Username=yourusername;Password=yourpassword;Database=individuella-backend"
  }
}
