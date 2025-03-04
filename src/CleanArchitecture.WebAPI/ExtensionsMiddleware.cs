using CleanArchitecture.Domanin.Users;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.WebAPI;

public static class ExtensionsMiddleware
{
	public static void CreateFirstUser(WebApplication app)
	{
		using (var scoped = app.Services.CreateScope())
		{
			var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

			if (!userManager.Users.Any(p => p.UserName == "admin"))
			{
				AppUser user = new()
				{
					UserName = "admin",
					Email = "admin@admin.com",
					FirstName = "Mehmet Can",
					LastName = "Şimşek",
					EmailConfirmed = true,
					CreateAt = DateTime.Now,
				};
				
				user.CreateUserId = user.Id;

				userManager.CreateAsync(user, "1").Wait();
			}
		}
	}
}
