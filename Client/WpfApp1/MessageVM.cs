using CommunityToolkit.Mvvm.ComponentModel;

using GrpcService;

using System;

namespace WpfApp
{
    public sealed class MessageVM : ObservableObject
    {
        public DateTime At { get; }

        public string Content { get; }

        public MessageVM(Message message)
        {
            At = message.At.ToDateTime();
            Content = message.Content;
        }
    }
}