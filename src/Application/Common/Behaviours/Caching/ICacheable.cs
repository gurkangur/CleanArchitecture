using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Behaviours.Caching
{
    public interface ICacheable
    {
        string Key { get; set; }
        TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    }
}
