using System;

namespace Domain.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        string DeleterUser { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}