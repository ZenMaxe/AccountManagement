using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace AccountManagement.Providers;

public class NumericTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
{
    private readonly Random _random = new Random();

    readonly IMemoryCache _cache;

    readonly TimeSpan _tokenLifeSpan;



    public NumericTokenProvider(IMemoryCache cache)
    {
        _cache = cache;
        _tokenLifeSpan = new TimeSpan(0, 8, 0, 0);
    }

    public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        if (manager is null)
        {
            throw new ArgumentException(nameof(manager));
        }

        var token = _random.Next(10000, 99999).ToString();

        var cacheKey = GetCacheKey(purpose, await manager.GetUserIdAsync(user));

        _cache.Set(cacheKey, token, _tokenLifeSpan);

        return token;
    }


    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (token.Length != 5)
        {
            return false;
        }

        var cacheKey = GetCacheKey(purpose, await manager.GetUserIdAsync(user));

        if (_cache.TryGetValue(cacheKey, out string? cachedToken) && token == cachedToken)
        {
            _cache.Remove(cacheKey);

            return true;
        }

        return false;
    }



    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        => Task.FromResult(false);

    public string GetCacheKey(string purpose, string userId)
        => $"{userId}:{purpose}";
}