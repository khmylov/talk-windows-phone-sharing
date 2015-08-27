using System;
using System.Linq;
using System.Net;
using Windows.UI.Xaml.Navigation;

namespace UriConsumer
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        public void SetProtocolData(Uri protocolUri)
        {
            var builder = new UriBuilder(protocolUri);
            var args = builder.Query
                .Substring(1)
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('='))
                .Where(pair => pair.Length == 2)
                .ToDictionary(
                    pair => WebUtility.UrlDecode(pair[0]), 
                    x => WebUtility.UrlDecode(x[1]), 
                    StringComparer.OrdinalIgnoreCase);

            string data;
            StatusText.Text = args.TryGetValue("data", out data) ? data : "Empty data";
        }
    }
}
