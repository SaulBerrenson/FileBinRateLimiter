using System;
using System.Threading;

namespace FileRateLimiter
{
   // ported from Google.Guava RateLimiter
    public class RateLimiter
    {
        public RateLimiter()
        {
        }

        public long aquire()
        {
            return aquire(1);
        }

        public long aquire(int permits)
        {
            if (permits <= 0)
            {
                Exception exception = new Exception("RateLimiter: Must request positive amount of permits");
                throw exception;
            }

            var wait_time = claim_next(permits);
            Thread.Sleep(wait_time);

            return Convert.ToInt64(wait_time.TotalMilliseconds);
        }

        public bool try_aquire(int permits)
        {
            return try_aquire(permits, 0);
        }
        public bool try_aquire(int permits, int timeout)
        {
            var now = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;

            if (next_free_ > now + timeout)
                return false;
            else
            {
                aquire(permits);
            }

            return true;
        }

        public double get_rate()
        {
            return OneSecondMs / interval_;
        }
        public RateLimiter set_rate(double rate)
        {
            if (rate <= 0.0)
            {
                throw new Exception("RateLimiter: Rate must be greater than 0");
            }

            lock (locker)
            {
                interval_ = OneSecondMs / rate;
            }

            return this;
        }

        void sync(ulong now)
        {
            // If we're passed the next_free, then recalculate
            // stored permits, and update next_free_
            if (now > next_free_)
            {
                stored_permits_ = Math.Min(max_permits_, stored_permits_ + (now - next_free_) / interval_);
                next_free_ = now;
            }
        }
        TimeSpan claim_next(double permits)
        {
            lock (locker)
            {

                var now = (ulong)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
                // Make sure we're synced
                sync(now);

                // Since we synced before hand, this will always be >= 0.
                ulong wait = next_free_ - now;

                double stored = Math.Min(permits, stored_permits_);
                double fresh = permits - stored;

                // In the general RateLimiter, stored permits have no wait time,
                // and thus we only have to wait for however many fresh permits we consume
                ulong next_free = (ulong)(fresh * interval_);

                next_free_ += next_free;
                stored_permits_ -= stored;


                return TimeSpan.FromMilliseconds(wait);
            }
        }

        private double interval_ = 0;
        private double max_permits_ = 0;
        private double stored_permits_ = 0;
        private ulong next_free_ = 0;
        private const double OneSecondMs = 1000.0;

        private object locker = new object();

    }
}