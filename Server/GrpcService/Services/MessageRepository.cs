namespace GrpcService.Services
{
    public sealed class MessageRepository
    {
        private readonly List<Message> _storage = new();

        public void AddMessage(Message message) => _storage.Add(message);

        public IEnumerable<Message> GetAll() => _storage.AsReadOnly();
    }
}