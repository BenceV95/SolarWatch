using System.Collections.Concurrent;

namespace SolarWatch.Services.Authentication
{
    public interface ITokenBlacklistService
    {
        void AddTokenToBlacklist(string token);
        bool IsTokenBlacklisted(string token);
    }

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, bool> _blacklistedTokens = new();

        public void AddTokenToBlacklist(string token)
        {
            _blacklistedTokens[token] = true;
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.ContainsKey(token);
        }
    }
}
