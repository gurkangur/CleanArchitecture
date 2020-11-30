using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    {
        private readonly string policyName;

        public AuthorizationBehavior()
        {

        }
        public AuthorizationBehavior(string policyName)
        {
            this.policyName = policyName;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            if (!string.IsNullOrWhiteSpace(policyName))
            {
 
            }
            return response;
        }
    }
}