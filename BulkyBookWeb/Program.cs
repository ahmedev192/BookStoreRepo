using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BulkyBookWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection")
				));

			builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddSingleton<IEmailSender, EmailSender>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();
			app.MapRazorPages();
			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}