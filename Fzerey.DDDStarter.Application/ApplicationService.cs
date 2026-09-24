using MediatR;

namespace Fzerey.DDDStarter.Application
{
    public class ApplicationService(IMediator mediator) : IApplicationService
    {
        public Task SendNotification(INotification notification, CancellationToken cancellationToken = default)
        {
            return mediator.Publish(notification, cancellationToken);
        }

        public Task<T> SendRequest<T>(IRequest<T> request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }

        public Task SendRequest(IRequest request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }
    }
}
