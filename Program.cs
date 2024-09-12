using Microsoft.EntityFrameworkCore;
using StudentGradesApp.Data;
using StudentGradesApp.Models;
using StudentGradesApp.Repositories;
using StudentGradesApp.Services;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IGradeTypeRepository, GradeTypeRepository>();
builder.Services.AddScoped<IFinalResultRepository, FinalResultRepository>();
builder.Services.AddSingleton<IPassingGradeService>(PassingGradeService.Instance);
builder.Services.AddSingleton<IGradeTypeService, GradeTypeService>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
