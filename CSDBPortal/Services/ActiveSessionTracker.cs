using System.Collections.Concurrent;

namespace CSDBPortal.Services
{
    public class ActiveSessionTracker
    {
        private readonly ConcurrentDictionary<string, DateTime> _sessions = new();
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(30);

        public void Touch(string userId) =>
            _sessions[userId] = DateTime.UtcNow;

        public void Remove(string userId) =>
            _sessions.TryRemove(userId, out _);

        public int GetActiveCount() =>
            _sessions.Count(kvp => DateTime.UtcNow - kvp.Value < Timeout);
    }
}
