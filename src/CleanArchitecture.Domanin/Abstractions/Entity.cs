using CleanArchitecture.Domanin.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domanin.Abstractions
{
	public abstract class Entity
	{
		public Entity()
		{
			Id = Guid.CreateVersion7(); // CreateVersion7() => Guid'in ilk parçasını zaman'a göre yapar bu sebeple sıralanabilir.
		}
		public Guid Id { get; set; }

		#region Audit Log
		public DateTimeOffset CreateAt { get; set; }
		public Guid CreateUserId { get; set; } = default!;
		public string CreateUserName => GetCreateUserName();
		public DateTimeOffset? UpdateAt { get; set; }
		public Guid? UpdateUserId { get; set; }
		public string? UpdateUserName => GetUpdateUserName();
		public DateTimeOffset? DeleteAt { get; set; }
		public Guid? DeleteUserId { get; set; }
		public string? DeleteUserName => GetDeleteUserName();
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; } = true;

		private string GetCreateUserName()
		{
			HttpContextAccessor httpContextAccessor = new();
			var userManager = httpContextAccessor
				.HttpContext
				.RequestServices
				.GetRequiredService<UserManager<AppUser>>(); // Bu şekilde UserManager'a bu class'ta ulaşılabilir.

			AppUser appUser = userManager.Users.First(x => x.Id == CreateUserId);

			return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
		}
		private string? GetUpdateUserName()
		{
			if (UpdateUserId is null) return null;

			HttpContextAccessor httpContextAccessor = new();
			var userManager = httpContextAccessor
				.HttpContext
				.RequestServices
				.GetRequiredService<UserManager<AppUser>>();

			AppUser appUser = userManager.Users.First(x => x.Id == UpdateUserId);

			return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
		}

		private string? GetDeleteUserName()
		{
			if (DeleteUserId is null) return null;

			HttpContextAccessor httpContextAccessor = new();
			var userManager = httpContextAccessor
				.HttpContext
				.RequestServices
				.GetRequiredService<UserManager<AppUser>>();

			AppUser appUser = userManager.Users.First(x => x.Id == DeleteUserId);

			return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
		}
		#endregion
	}
}
