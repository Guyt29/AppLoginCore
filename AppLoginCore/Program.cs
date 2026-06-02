using AppLoginCore.Libraries.Login;
using AppLoginCore.Repository;
using AppLoginCore.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adicionando para manipular a view
builder.Services.AddHttpContextAccessor();

//Adicionar interface como serviço
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<AppLoginCore.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();

// Corrigir problema com TEMPDATA
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //definir tempo para duração.
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;

    //Mostrar para o navegador que o cookie e essencial
    options.Cookie.IsEssential = true;

});

builder.Services.AddMvc().AddSessionStateTempDataProvider();

builder.Services.AddScoped<AppLoginCore.Libraries.Sessao.Sessao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.MapStaticAssets();
app.UseCookiePolicy();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
