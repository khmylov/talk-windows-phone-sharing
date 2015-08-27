using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace UriConsumerSL
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string data;
            StatusText.Text = NavigationContext.QueryString.TryGetValue("MappedData", out data) 
                ? data : "EMPTY DATA";
        }
    }
}