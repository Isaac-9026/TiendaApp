using Microsoft.EntityFrameworkCore;
using TiendaApp.Data;
using TiendaApp.Services;
using TiendaApp.Components;

var builder = WebApplication.CreateBuilder(args);

//Actualización Proyecto TiendaApp
builder.Services.AddDbContextFactory<TiendaContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Tienda"))
    );

builder.Services.AddScoped<IProductoService, ProductoService>(); //K:V (clave : valor)
//Fin de actualización TiendaApp


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
