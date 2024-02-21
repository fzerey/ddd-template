using MediatR;

namespace Fzerey.DDDStarter.Application
{
    public interface IApplicationService
    {
        Task<T> SendRequest<T>(IRequest<T> request);
        Task SendRequest(IRequest request);
        Task SendNotification(INotification notification);
    }
}