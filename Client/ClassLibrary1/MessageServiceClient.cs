using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using GrpcService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public sealed class MessageServiceClient
    {
        private readonly MessageServiceApi.MessageServiceApiClient _client;

        public MessageServiceClient()
        {
            _client = new MessageServiceApi.MessageServiceApiClient(new Channel("localhost", 5152, ChannelCredentials.Insecure));
        }

        public async Task SendAsync(string content) =>
            await _client.SendAsync(new Message
            {
                Content = content,
                At = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
            });

        public IAsyncEnumerable<Message> Subscribe()
        {
            AsyncServerStreamingCall<Message> call = _client.Subscribe(new Empty());

            return call.ResponseStream
                .ToAsyncEnumerable()
                .Finally(call.Dispose);
        }
    }
}