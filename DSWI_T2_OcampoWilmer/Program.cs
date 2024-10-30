using DSWI_T2_OcampoWilmer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Añadiendo mis Services
builder.Services.AddSingleton<IProductoService, ProductoService>();
builder.Services.AddSingleton<IProveedorService, ProveedorService>();
builder.Services.AddSingleton<ICategoriaService, CategoriaService>();
builder.Services.AddSingleton<PaginationHelper>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
