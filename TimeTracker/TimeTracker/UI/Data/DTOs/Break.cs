#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UI.Data.DTOs
{
    public class Break
    {
        [Key]
        public long BreakId { get; set; }
        public long ShiftId { get; set; }
        public BreakTypeId BreakTypeId { get; set; }
        [NotNull]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? EditedDate { get; set; }
        public string EditedByUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedByUser { get; set; }

        public virtual Shift Shift { get; set; }
        [ForeignKey("BreakTypeId")]
        public virtual BreakType BreakType { get; set; }
    }
}
