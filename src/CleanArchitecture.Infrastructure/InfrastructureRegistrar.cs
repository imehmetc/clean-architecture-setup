using CleanArchitecture.Domanin.Users;
using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Options;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CleanArchitecture.Infrastructure
{
	public static class InfrastructureRegistrar
	{
		public static IServiceCollection AddInfrastrusture(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(opt =>
			{
				string connectionString = configuration.GetConnectionString("SqlServer")!;
				opt.UseSqlServer(connectionString);
			});

			services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

			// UserManager kullanabilmek için yazılır
			services
				.AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
				{
					opt.Password.RequiredLength = 1;
					opt.Password.RequireNonAlphanumeric = false;
					opt.Password.RequireDigit = false;
					opt.Password.RequireLowercase = false;
					opt.Password.RequireUppercase = false;
					opt.Lockout.MaxFailedAccessAttempts = 5; // kullanıcı şifreyi 5 denemeden sonra da hatalı girmişse
					opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // 5 dakika kitler
					opt.SignIn.RequireConfirmedEmail = true; // kitlendikten sonra email onayı gerektirsin
				}) // Identity kütüphanesini tanımlar.
				.AddEntityFrameworkStores<ApplicationDbContext>() // ApplicationDbContext ile bağlantısını yapar.
				.AddDefaultTokenProviders(); // Şifremi unuttum, şifremi yenile gibi işlemler için UserManager class'ının token üretme metodunun çalışabilmesini sağlar.


			services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
			services.ConfigureOptions<JwtOptionsSetup>();

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer();
			services.AddAuthorization();

			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			// Scrutor kütüphanesi bu dependency injection işlemini otomatik yapıyor.
			services.Scan(opt => opt
				.FromAssemblies(typeof(InfrastructureRegistrar).Assembly) // mevcut katmanı tanımlar
				.AddClasses(publicOnly: false) // class'ı public olmayanlar dahil listele
				.UsingRegistrationStrategy(RegistrationStrategy.Skip) // Daha önce dependency injection yapılmışsa ikinciyi yapmadan atla.
				.AsImplementedInterfaces()
				.WithScopedLifetime()
			);

			return services;
		}
	}
}
