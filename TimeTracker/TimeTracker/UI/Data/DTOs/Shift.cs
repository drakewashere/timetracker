#nullable disable

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Data.DTOs
{
    public class Shift
    {
        [Key]
        public long ShiftId { get; set; }
        public string UserId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? EditedDate { get; set; }
        public string EditedByUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedByUser { get; set; }

        public virtual IEnumerable<Break> Breaks { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
    }
}
