using System;

namespace FileRateLimiter
{
    public interface IFilter
    {
        void Process<T>(ArraySegment<T> data);
    }


}