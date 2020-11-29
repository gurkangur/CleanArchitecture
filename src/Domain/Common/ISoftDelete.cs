using System;

namespace Domain.Common
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        string DeleterUser { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}