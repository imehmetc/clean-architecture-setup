namespace CleanArchitecture.Domanin.Abstractions
{
	public abstract class EntityDto
	{
		public Guid Id { get; set; }
		public DateTimeOffset CreateAt { get; set; }
		public Guid CreateUserId { get; set; }
		public string CreateUserName { get; set; } = default!;
		public DateTimeOffset? UpdateAt { get; set; }
		public Guid? UpdateUserId { get; set; }
		public string? UpdateUserName { get; set; } = default!;
		public DateTimeOffset? DeleteAt { get; set; }
		public Guid? DeleteUserId { get; set; }
		public string? DeleteUserName { get; set; } = default!;
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
	}
}
