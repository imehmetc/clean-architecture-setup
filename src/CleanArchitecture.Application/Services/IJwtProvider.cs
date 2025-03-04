using CleanArchitecture.Domanin.Users;

namespace CleanArchitecture.Application.Services;

public interface IJwtProvider
{
	public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
}
