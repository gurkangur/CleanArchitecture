using System.Collections.Generic;

namespace Domain.Paging.Responses
{
    public interface IPage<T>
    {
        List<T> Items { get; }
        int Index { get; }
        int Size { get; }
        int TotalCount { get; }
        int TotalPages { get; }
    }
}