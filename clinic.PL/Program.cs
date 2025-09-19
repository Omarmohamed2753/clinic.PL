using Microsoft.EntityFrameworkCore;
using clinic.DAL.DataBase;
using clinic.BLL.mapper;
using clinic.DAL.Repo.abstraction;
using clinic.DAL.Repo.implementation;
using clinic.BLL.Service.abstraction;
using clinic.BLL.Service.implementation;
using Microsoft.AspNetCore.Identity;
using clinic.DAL.Entities;
using clinic.PL.Models;

namespace clinic.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("clinicConnection");


            builder.Services.AddDbContext<ClinicDbcontext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(connectionString));


            builder.Services.Configure<StripeSettings>(
            builder.Configuration.GetSection("StripeSettings"));


            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<ClinicDbcontext>()
            .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor();


            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IDoctorRepo, DoctorRepo>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientRepo, PatientRepo>();
            builder.Services.AddScoped<IPatientServicecs, PatientService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IAppointmentRepo, AppointmentRepo>();



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                accountService.SeedSuperAdmin().GetAwaiter().GetResult();

            }


                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();
                app.UseSession();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            }
        }
    
} 