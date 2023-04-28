
using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ecommerceapp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			// Db context

			//session
			builder.Services.AddSingleton<IHttpContextAccessor , HttpContextAccessor>();


			//session
			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
                options.IdleTimeout = TimeSpan.FromMinutes(120);//You can set Time   
                options.Cookie.IsEssential = true;
			});
			//


			builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession();
            // other configurations

            

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            // Register UserManager service
            builder.Services.AddScoped<UserManager<IdentityUser>>();

            // Add this to your ConfigureServices method
            //builder.Services.AddSendGridEmailSender(Configuration.GetSection("EmailSettings"));
            //builder.Services.AddSingleton<IEmailSender, SendGridEmailSender>();
            //builder.Services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            
			builder.Services.AddTransient<IEmailSender, EmailSender>();




            var con = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			 options.UseMySql(con, new MySqlServerVersion(new Version(8, 0, 28))));




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
			//session
            app.UseSession();

            app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=View}/{action=Index}/{id?}");

			app.Run();
		}
	}
}


