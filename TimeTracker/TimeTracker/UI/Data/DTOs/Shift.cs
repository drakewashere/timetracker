#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Data.DTOs
{
    public class Shift
    {
        [Key]
        public long ShiftId { get; set; }
        public long UserId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public long EditedByUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long DeletedByUser { get; set; }

        public virtual IEnumerable<Break> Breaks { get; set; }
    }
}
