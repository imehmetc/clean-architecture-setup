﻿using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domanin.Users;

public sealed class AppUser : IdentityUser<Guid>
{
	public AppUser()
	{
		Id = Guid.CreateVersion7();
	}
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string FullName => $"{FirstName} {LastName}"; // computed property
	
	#region Audit Log
	public DateTimeOffset CreateAt { get; set; }
	public Guid CreateUserId { get; set; } = default!;
	public DateTimeOffset? UpdateAt { get; set; }
	public Guid? UpdateUserId { get; set; }
	public DateTimeOffset? DeleteAt { get; set; }
	public Guid? DeleteUserId { get; set; }
	public bool IsDeleted { get; set; }
	#endregion

}
