using System;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace DataPicker
{
    public sealed partial class App
    {
        private TransitionCollection transitions;

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            var page = new RecordingPickerPage(args.FileOpenPickerUI);
            Window.Current.Content = page;
            Window.Current.Activate();
            await page.ShowRecordings();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            Debug.WriteLine("Activated with " + args.Kind);
            if (args.Kind == ActivationKind.PickSaveFileContinuation)
            {
                var continuationArgs = (FileSavePickerContinuationEventArgs) args;
                var frame = GetOrCreateRootFrame();
                var page = (MainPage) frame.Content;
                await page.SaveToFileAsync(continuationArgs.ContinuationData, continuationArgs.File);
                //await new MessageDialog("Saved").ShowAsync();
            } 
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = GetOrCreateRootFrame();

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                if (!rootFrame.Navigate(typeof (MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

        private static Frame GetOrCreateRootFrame()
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame { CacheSize = 1 };
                Window.Current.Content = rootFrame;
            }
            return rootFrame;
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = transitions ?? new TransitionCollection { new NavigationThemeTransition() };
            rootFrame.Navigated -= RootFrame_FirstNavigated;
        }
    }
}
