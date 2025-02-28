using CleanArchitecture.Infrastructure.Context;
using GenericRepository;
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
