using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers.Provider;
using GalaSoft.MvvmLight.Command;

namespace DataPicker
{
    public sealed partial class RecordingPickerPage
    {
        private readonly FileOpenPickerUI _pickerUi;

        public RecordingPickerPage()
        {
            InitializeComponent();
        }

        public RecordingPickerPage(FileOpenPickerUI pickerUi) : this()
        {
            _pickerUi = pickerUi;
        }

        public async Task ShowRecordings()
        {
            var recordings = await RecordingStorage.GetRecordings();
            RecordingList.ItemsSource = recordings
                .Select(t =>
                    new RecordingViewModel(t.Item1, () =>
                    {
                        if (!_pickerUi.ContainsFile(t.Item1))
                        {
                            _pickerUi.AddFile(t.Item1, t.Item2);
                        }
                        else
                        {
                            _pickerUi.RemoveFile(t.Item1);
                        }
                    }))
                .ToArray();
        }
    }

    public class RecordingViewModel
    {
        public string Name { get; private set; }

        public ICommand SelectCommand { get; private set; }

        public RecordingViewModel(string name, Action chooseAction)
        {
            Name = name;
            SelectCommand = new RelayCommand(chooseAction);
        }
    }
}
