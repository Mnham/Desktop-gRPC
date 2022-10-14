using Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp
{
    public sealed class MainWindowVM : ObservableObject
    {
        private readonly MessageServiceClient _messageServiceClient = new();
        private string _messageContent;

        public string MessageContent
        {
            get => _messageContent;
            set => SetProperty(ref _messageContent, value);
        }

        public ObservableCollection<MessageVM> Messages { get; } = new();

        public AsyncRelayCommand SendCommand { get; }

        public MainWindowVM()
        {
            SendCommand = new AsyncRelayCommand(Send);
        }

        public void Init()
        {
            object lockObj = new();
            _messageServiceClient.Subscribe()
                .ForEachAsync(message =>
                {
                    lock (lockObj)
                    {
                        App.Current?.Dispatcher?.Invoke(() => Messages.Add(new MessageVM(message)));
                    }
                });
        }

        public async Task Send()
        {
            if (string.IsNullOrWhiteSpace(MessageContent))
            {
                return;
            }

            await _messageServiceClient.SendAsync(MessageContent);
            MessageContent = string.Empty;
        }
    }
}