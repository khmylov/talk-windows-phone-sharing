using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Utils
{
    public class UriQuery
    {
        private readonly IDictionary<string, string> _values;

        public string Protocol { get; private set; }

        public UriQuery(string url)
        {
            var protocolIndex = url.IndexOf(":");
            Protocol = protocolIndex > 0 ? url.Substring(0, protocolIndex) : "";

            var queryIndex = url.IndexOf("?");
            if (queryIndex >= 0)
            {
                var query = url.Substring(queryIndex + 1);
                _values = query
                    .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split('='))
                    .Where(pair => pair.Length == 2)
                    .ToDictionary(
                        pair => WebUtility.UrlDecode(pair[0]),
                        x => WebUtility.UrlDecode(x[1]),
                        StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public string GetValue(string key)
        {
            string value;
            return _values.TryGetValue(key, out value)
                ? value
                : null;
        }
    }
}
