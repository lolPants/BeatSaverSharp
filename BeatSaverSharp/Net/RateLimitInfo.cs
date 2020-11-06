using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BeatSaverSharp
{
    /// <summary>
    /// Rate Limit Info
    /// </summary>
    public struct RateLimitInfo
    {
        /// <summary>
        /// Number of requests remaining
        /// </summary>
        public int Remaining { get; }
        /// <summary>
        /// Time at which rate limit bucket resets
        /// </summary>
        public DateTime Reset { get; }
        /// <summary>
        /// Total allowed requests for a given bucket window
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Construct a new Rate Limit Info Struct
        /// </summary>
        /// <param name="remaining"></param>
        /// <param name="reset"></param>
        /// <param name="total"></param>
        public RateLimitInfo(int remaining, DateTime? reset, int total)
        {
            Remaining = remaining;
            Reset = reset ?? throw new ArgumentNullException(nameof(reset));
            Total = total;
        }

        internal static RateLimitInfo? FromHttp(HttpResponseMessage? resp)
        {
            if (resp is null) throw new ArgumentNullException(nameof(resp));

            int _remaining;
            DateTime? _reset = null;
            int _total;

            if (resp.Headers.TryGetValues("Rate-Limit-Remaining", out IEnumerable<string> remainings))
            {
                string val = remainings.FirstOrDefault();
                if (val is not null)
                {
                    if (int.TryParse(val, out int result)) _remaining = result;
                    else return null;
                }
                else return null;
            }
            else return null;

            if (resp.Headers.TryGetValues("Rate-Limit-Total", out IEnumerable<string> totals))
            {
                string val = totals.FirstOrDefault();
                if (val is not null)
                {
                    if (int.TryParse(val, out int result)) _total = result;
                    else return null;
                }
                else return null;
            }
            else return null;

            if (resp.Headers.TryGetValues("Rate-Limit-Reset", out IEnumerable<string> resets))
            {
                string val = resets.FirstOrDefault();
                if (val is not null)
                {
                    if (ulong.TryParse(val, out ulong ts))
                    {
                        _reset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        _reset = _reset?.AddSeconds(ts).ToLocalTime();
                    }
                    else return null;
                }
            }
            else return null;

            if (_reset is null) return null;
            return new RateLimitInfo(_remaining, _reset, _total);
        }
    }
}
