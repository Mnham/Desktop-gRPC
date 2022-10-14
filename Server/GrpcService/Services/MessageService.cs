using System.Reactive.Linq;

namespace GrpcService.Services
{
    public sealed class MessageService
    {
        private readonly MessageRepository _messageRepository;

        private event Action<Message> Added;

        public MessageService(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void AddMessage(Message message)
        {
            _messageRepository.AddMessage(message);
            Added?.Invoke(message);
        }

        public IObservable<Message> GetMessagesAsObservable()
        {
            IObservable<Message> oldMessages = _messageRepository.GetAll().ToObservable();
            IObservable<Message> newMessages = Observable.FromEvent<Message>(
                message => Added += message,
                message => Added -= message);

            return oldMessages.Concat(newMessages);
        }
    }
}