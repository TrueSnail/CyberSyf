using E_Book_Store.Data;
using E_Book_Store.Models;
using E_Book_Store.Services;
using E_Book_Store.Validation;
using E_Book_Store.ViewModels.EBooks;
using FluentValidation;
using FormHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("secrets.json");
builder.Services.AddControllersWithViews().AddFormHelper();
builder.Services.AddValidatorsFromAssemblyContaining<EBookValidator>();
builder.Services.AddDbContext<EBookDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EBookDbContext>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<EBooksCreateViewModel, EBook>();
    cfg.CreateMap<EBooksDeleteViewModel, EBook>();
    cfg.CreateMap<EBook, EBooksDeleteViewModel>();
    cfg.CreateMap<EBook, EBooksDetailsViewModel>();
    cfg.CreateMap<EBooksDetailsViewModel, EBook>();
    cfg.CreateMap<EBook, EBooksEditViewModel>();
    cfg.CreateMap<EBooksEditViewModel, EBook>();
    cfg.CreateMap<EBook, EBooksIndexItemViewModel>();
    cfg.CreateMap<IEnumerable<EBook>, EBooksIndexViewModel>().ForMember(dest => dest.EBooks, opt => opt.MapFrom(src => src));
});
builder.Services.AddTransient<IRepository<EBook>, EntityFrameworkRepository<EBook>>();
builder.Services.AddTransient<IRepository<EBookPurchase>, EntityFrameworkRepository<EBookPurchase>>();
builder.Services.AddTransient<IEBooksService, EBooksService>();
builder.Services.AddTransient<IEBooksContentService, EBooksContentService>();

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
app.MapRazorPages();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
