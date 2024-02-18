using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("MeetingEvents")]
    public class MeetingEvent
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(225)]
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
