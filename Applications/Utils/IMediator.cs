namespace Applications.Utils;
 
public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
}
