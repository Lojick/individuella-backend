# individuella-backend

Detta är den individuella uppgiften för Backend-kursen.  
Projektet är byggt med ASP.NET Core, PostgreSQL och Identity Core.

---

## 🧪 Testning och login/register

API:et har testats med Bruno.

Login och register hanteras via Identity Core. Vid lyckad inloggning returneras en JWT-token som ska användas i `Authorization`-headern för skyddade endpoints:

Exempel i Bruno:

- **URL:** `http://localhost:5235/login` eller `http://localhost:5235/register`
- **Method:** `POST`
- **Body (JSON):**
```json
{
  "email": "test@example.com",
  "password": "Pass123!"
}
```
---

## 📁 Folder-endpoints

- `POST /api/folder/addfolder` – Skapar mapp (kräver inloggning)
- `GET /api/folder/getfolderswithfiles` – Hämtar mappar med tillhörande filer (kräver inloggning)

---

## 📄 File-endpoints

- `POST /api/file/uploadfile` – Ladda upp fil till mapp (kräver inloggning, FormData: file + folderId).  
  I Bruno:  
  **- Method:** `POST`  
  **- Välj:** `Body > multipart/form-data`  
  **- Klicka på:** **+ Add Param**, skriv `folderId`, sätt typen till **Text** och fyll i ID:t på en befintlig mapp  
  **- Klicka på:** **+ Add File**, skriv `file`, sätt typen till **File**, klicka på **Select Files** och välj en fil från datorn  
  **- Lägg till header:** `Authorization: Bearer <din-token>`

- `GET /api/file/download/{fileid}` – Ladda ner fil (kräver inloggning).  
  I Bruno: använd `GET` och skriv in filens ID i URL:en, t.ex. `/api/file/download/1`. Lägg till header `Authorization: Bearer <din-token>`. När svaret visas, klicka på **nedladdningsikonen längst upp till höger** i Bruno för att spara filen.

- `DELETE /api/file/delete/{fileid}` – Radera fil (kräver inloggning)

---

## 🔧 Databaskonfiguration

Skapa en `appsettings.json` i projektets rot:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Username=yourusername;Password=yourpassword;Database=individuella-backend"
  }
}
