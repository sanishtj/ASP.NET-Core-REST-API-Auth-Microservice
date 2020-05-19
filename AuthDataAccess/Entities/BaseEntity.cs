using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDataAccess.Entities
{
    public class BaseEntity
    {
        [Required]
        public Guid CreatedUserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
