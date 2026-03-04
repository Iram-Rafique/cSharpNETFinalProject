var builder = WebApplication.CreateBuilder(args);
// add services to the container
builder.Services.AddRazorPages();
var app = builder.Build();
app.UseStaticFiles();   // serve CSS, JS, wwwroot files
app.MapRazorPages();    // Map Razor Pages routes
app.Run();