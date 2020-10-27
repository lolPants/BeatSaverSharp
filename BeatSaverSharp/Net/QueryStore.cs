using System.Text;

namespace BeatSaverSharp
{
    internal sealed class QueryStore : MultiKeyDictionary<string, string>
    {
        public string? ToQueryString()
        {
            if (Count == 0)
            {
                return null;
            }

            StringBuilder sb = new();
            foreach (var entry in this)
            {
                foreach (var value in entry.Value)
                {
                    if (value is null) continue;

                    sb.Append(sb.Length == 0 ? '?' : '&');
                    sb.Append(entry.Key);
                    sb.Append('=');
                    sb.Append(value);
                }
            }

            return sb.ToString();
        }
    }
}
