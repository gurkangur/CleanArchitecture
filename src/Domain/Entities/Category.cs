using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : IEntity<int>, IAuditedEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUser { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LastModifierUser { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleterUser { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}