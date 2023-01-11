using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccessfulStartup.Data.Entities
{
    public class BusinessPlan : BusinessPlanAbstract // model for Entity Framework
    {
        public override int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Exceeded 30 character maximum.")]
        public override string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 150 character maximum.")]
        public override string? Description { get; set; }

        public override string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual AppUser User { get; set; }

        
    }

}
