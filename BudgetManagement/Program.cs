using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IOperationTypeRepository, OperationTypeRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IDealRepository, DealRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IAppUserRepository, AppUserRepository>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IUserStore<AppUser>, AppUserStore>();
builder.Services.AddIdentityCore<AppUser>();

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
    pattern: "{controller=Deal}/{action=Index}/{id?}");

app.Run();
