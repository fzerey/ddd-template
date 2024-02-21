using MediatR;

namespace Fzerey.DDDStarter.Application
{
    public class ApplicationService(IMediator mediator) : IApplicationService
    {
        public async Task SendNotification(INotification notification)
        {
            await mediator.Publish(notification);
        }

        public async Task<T> SendRequest<T>(IRequest<T> request)
        {
            return await mediator.Send(request);
        }

        public async Task SendRequest(IRequest request)
        {
            await mediator.Send(request);
        }
    }
}
