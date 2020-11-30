using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;

namespace Application.Common.Behaviours.Transaction
{
    public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await next();
                transactionScope.Complete();
                return response;
            }
        }
    }
}