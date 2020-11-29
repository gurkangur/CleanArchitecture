using System;

namespace Domain.Common
{
    public interface IAuditedEntity
    {
        DateTime CreationTime { get; set; }
        string CreatorUser { get; set; }
        DateTime? LastModificationTime { get; set; }
        string LastModifierUser { get; set; }
    }
}