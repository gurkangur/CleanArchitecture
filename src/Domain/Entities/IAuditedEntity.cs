using System;

namespace Domain.Entities
{
    public interface IAuditedEntity
    {
        DateTime CreationTime { get; set; }
        string CreatorUser { get; set; }
        DateTime? LastModificationTime { get; set; }
        string LastModifierUser { get; set; }
    }
}