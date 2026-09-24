using MediatR;

namespace Fzerey.DDDStarter.Application
{
    public interface IApplicationService
    {
        Task<T> SendRequest<T>(IRequest<T> request, CancellationToken cancellationToken = default);
        Task SendRequest(IRequest request, CancellationToken cancellationToken = default);
        Task SendNotification(INotification notification, CancellationToken cancellationToken = default);
    }
}
