using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace DataPicker
{
    public sealed partial class MainPage
    {
        private RecordingToken _token;

        private bool _isRecording;

        private bool IsRecording
        {
            get { return _isRecording; }
            set
            {
                _isRecording = value;
                ToggleRecordingButton.Content = value ? "stop recording" : "start recording";
            }
        }

        private bool _isBusy;

        private bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                ToggleRecordingButton.IsEnabled = !_isBusy;
            }
        }

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void OnToggleRecording(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            if (IsRecording)
            {
                var result = await _token.StopAsync();
                IsRecording = false;

                TrySaveAudio(result.FilePath);
            }
            else
            {
                _token = await new Recorder().StartAsync();
                IsRecording = true;
            }

            IsBusy = false;
        }

        private void TrySaveAudio(string audioFileName)
        {
            var picker = new FileSavePicker();
            picker.ContinuationData.Add("OriginalFileName", audioFileName);
            picker.FileTypeChoices["Audio files"] = new List<string> { ".wav" };
            picker.SuggestedFileName = Path.GetFileNameWithoutExtension(audioFileName);
            picker.DefaultFileExtension = ".wav";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            picker.PickSaveFileAndContinue();
        }

        public async Task SaveToFileAsync(ValueSet continuationData, IStorageFile target)
        {
            var audioFileName = continuationData["OriginalFileName"].ToString();
            var source = await StorageFile.GetFileFromPathAsync(audioFileName);
            await source.CopyAndReplaceAsync(target);
        }
    }
}
