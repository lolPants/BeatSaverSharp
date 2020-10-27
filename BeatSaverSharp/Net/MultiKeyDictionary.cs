using System.Collections.Generic;

namespace BeatSaverSharp
{
    internal class MultiKeyDictionary<TKey, TValue> : Dictionary<TKey, IEnumerable<TValue>> where TKey : notnull
    {
        public void Append(TKey key, TValue value)
        {
            Append(key, new List<TValue>() { value });
        }

        public void Append(TKey key, IEnumerable<TValue> value)
        {
            List<TValue> values = ContainsKey(key)
                ? new List<TValue>(this[key])
                : new List<TValue>();

            values.AddRange(value);
            this[key] = values;
        }
    }
}
