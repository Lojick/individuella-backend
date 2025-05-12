using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

        //  Här aktiveras både Identity och inbyggda endpoints (/register, /login)
        builder
            .Services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        //Här aktiveras alla services och repositories för Folder och File tabellerna.
        builder.Services.AddScoped<FolderService>();
        builder.Services.AddScoped<FolderRepository>();
        builder.Services.AddScoped<FileService>();
        builder.Services.AddScoped<FileRepository>();
        // builder
        //     .Services.AddControllers()
        //     .AddJsonOptions(options =>
        //     {
        //         options.JsonSerializerOptions.Converters.Add(new ByteArrayConverter());
        //     });

        var app = builder.Build();
        // Aktiverar /register, /login, /logout osv.
        app.MapIdentityApi<ApplicationUser>();

        app.UseAuthentication();
        app.UseAuthorization();

        // Aktiverar dina egna controllers, t.ex. /api/admin/users
        app.MapControllers();

        app.Run();
    }
}
