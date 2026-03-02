var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // Serve CSS, JS, wwwroot files

app.UseRouting();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.MapRazorPages();    // Map Razor Pages routes

app.Run();