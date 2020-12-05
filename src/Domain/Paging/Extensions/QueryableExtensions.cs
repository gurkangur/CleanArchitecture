using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Paging.Requests;
using Domain.Paging.Responses;
using Microsoft.EntityFrameworkCore;

namespace Domain.Paging.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedResponse<T> ToPage<T>(this IQueryable<T> source, PagedRequest request)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    $"You cannot pageinate on a null object reference. The parameter source should be initialized.",
                    nameof(source));
            }

            if (request == null)
            {
                throw new ArgumentNullException(
                    $"You need to initialize a paging request before paging on a list. The parameter request should be initialized.",
                    nameof(request));
            }

            if (request.Page == 0)
            {
                return new PagedResponse<T>(source, 0, 0, 0);
            }
            int totalItemCount = source.Count();
            int skip = (request.Page - 1) * request.PageSize;
            int take = request.PageSize;

            return new PagedResponse<T>(source.Skip(skip).Take(take), request.Page, request.PageSize, totalItemCount);
        }

        public static async Task<PagedResponse<T>> ToPageAsync<T>(this IQueryable<T> source, PagedRequest request)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    $"You cannot pageinate on a null object reference. The parameter source should be initialized.",
                    nameof(source));
            }

            if (request == null)
            {
                throw new ArgumentNullException(
                    $"You need to initialize a paging request before paging on a list. The parameter request should be initialized.",
                    nameof(request));
            }

            if (request.Page == 0)
            {


                return new PagedResponse<T>(await source.ToListAsync(), 0, 0, 0);
            }


            int skip = (request.Page - 1) * request.PageSize;
            int take = request.PageSize;
            int totalItemCount = await source.CountAsync();

            return new PagedResponse<T>(await source.Skip(skip).Take(take).ToListAsync(), request.Page, request.PageSize, totalItemCount);
        }

    }
}