using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SDSYSTEM.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public TicketStatus? Status { get; set; }

        public int? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public User? AssignedTo { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public required User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        [Required]
        public required string Department { get; set; }

        [Required]
        public required string FullName { get; set; }
    }
}
