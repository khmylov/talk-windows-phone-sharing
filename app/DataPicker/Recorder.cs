using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace DataPicker
{
    public class RecordingStorage
    {
        public static async Task<List<Tuple<string, IStorageFile>>> GetRecordings()
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            return files
                .Where(x => x.Name.EndsWith(".wav"))
                .Select(x => Tuple.Create(Path.GetFileNameWithoutExtension(x.Name), (IStorageFile) x))
                .ToList();
        }
    }

    public class Recorder
    {
        public async Task<RecordingToken> StartAsync()
        {
            var capture = new MediaCapture();

            var initSettings = new MediaCaptureInitializationSettings();
            initSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;

            await capture.InitializeAsync(initSettings);

            var fileName = DateTimeOffset.Now.TimeOfDay.ToString().Replace(':', '_') + ".wav";
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName);

            var profile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Medium);
            await capture.StartRecordToStorageFileAsync(profile, file);

            return new RecordingToken(file.Path, async () =>
            {
                await capture.StopRecordAsync();
                // It's important to dispose the capture device here to avoid application crash when using FileSavePicker afterwards
                capture.Dispose();
            });
        }
    }

    public class RecordingToken
    {
        private readonly Func<Task> _stopRecording;
        private readonly string _path;

        public RecordingToken(string path, Func<Task> stopRecording)
        {
            _stopRecording = stopRecording;
            _path = path;
        }

        public async Task<RecordingResult> StopAsync()
        {
            await _stopRecording();
            return new RecordingResult(_path);
        }
    }

    public class RecordingResult
    {
        public string FilePath { get; private set; }

        public RecordingResult(string filePath)
        {
            FilePath = filePath;
        }
    }
}
