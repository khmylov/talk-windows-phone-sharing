using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace UriProducer
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void OnSendClick(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("sampledata:Share?Data=" + DataText.Text));
        }
    }
}
