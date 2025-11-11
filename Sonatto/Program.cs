using Microsoft.Extensions.DependencyInjection;
using Sonatto.Aplicacao;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Repositorio;
using Sonatto.Repositorio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// REGISTRAR CONNECTION STRING CORRETAMENTE
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IUsuarioRepositorio>(sp => new UsuarioRepositorio(connectionString));
builder.Services.AddScoped<IProdutoRepositorio>(sp => new ProdutoRepositorio(connectionString));


// REGISTRAR APLICAÇÕES (CAMADA DE NEGÓCIO)
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<ILoginAplicacao, LoginAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// sessão deve vir antes da autorização
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
