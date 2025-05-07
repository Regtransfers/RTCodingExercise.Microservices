using MassTransit;

namespace WebMVC.Services
{
    public interface IMessageService
    {
        Task SendMessage<T>(T message, string queue) where T : class;
    }

    public class MessageService : IMessageService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessageService(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendMessage<T>(T message, string queue) where T : class
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queue}"));
            await endpoint.Send(message);
        }
    }
}