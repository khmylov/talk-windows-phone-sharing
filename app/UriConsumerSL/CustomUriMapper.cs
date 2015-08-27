using System;
using System.Windows.Navigation;
using Utils;

namespace UriConsumerSL
{
    public class CustomUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            var urlText = uri.ToString();
            var protocol = "/Protocol";
            if (urlText.StartsWith(protocol))
            {
                var query = new UriQuery(urlText.Substring(protocol.Length));
                var launchUri = query.GetValue("encodedLaunchUri");
                var launchQuery = new UriQuery(launchUri);
                if (launchQuery.Protocol == "sampledata")
                {
                    var data = launchQuery.GetValue("data") ?? "EMPTY DATA";
                    return new Uri("/MainPage.xaml?MappedData=" + data, UriKind.Relative);
                }
            }

            return uri;
        }
    }
}
