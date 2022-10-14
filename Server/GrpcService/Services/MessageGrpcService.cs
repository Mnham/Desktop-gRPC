using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace GrpcService.Services
{
    public sealed class MessageGrpcService : MessageServiceApi.MessageServiceApiBase
    {
        private static readonly Empty _empty = new();
        private readonly MessageService _messageService;

        public MessageGrpcService(MessageService messageService)
        {
            _messageService = messageService;
        }

        public override Task<Empty> Send(Message request, ServerCallContext context)
        {
            _messageService.AddMessage(request);

            return Task.FromResult(_empty);
        }

        public override async Task Subscribe(Empty request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            try
            {
                await _messageService.GetMessagesAsObservable()
                    .ToAsyncEnumerable()
                    .ForEachAwaitAsync(async message => await responseStream.WriteAsync(message), context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}