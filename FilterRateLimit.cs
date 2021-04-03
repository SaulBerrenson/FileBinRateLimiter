using System;

namespace FileRateLimiter
{
    public class FilterRateLimit : IFilter
    {
        public FilterRateLimit(int maxBytesPerSecond)
        {
            _limiter = new RateLimiter()
                            .set_rate(maxBytesPerSecond);
        }

        private readonly RateLimiter _limiter;

        public void Process<T>(ArraySegment<T> data)
        {
            if (data == ArraySegment<T>.Empty) return;
            _limiter.aquire(data.Count);
        }
    }
}