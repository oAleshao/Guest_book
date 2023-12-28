using Azure.Messaging;
using Guest_book.Models;
using Guest_book.Repository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<BookGuestContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(10); // ������������ ������ (����-��� ���������� ������)
	options.Cookie.Name = "Session"; // ������ ������ ����� ���� �������������, ������� ����������� � �����.

});
builder.Services.AddControllersWithViews();
// ��������� ������� MVC
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository, GuestBookRepository>();

var app = builder.Build();
app.UseSession();
app.UseStaticFiles(); // ������������ ������� � ������ � ����� wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Messages}/{action=Index}/{id?}");

app.Run();
