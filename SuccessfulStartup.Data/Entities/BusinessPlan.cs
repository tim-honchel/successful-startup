using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccessfulStartup.Data.Entities
{
    internal class BusinessPlan : BusinessPlanAbstract
    {
        protected override int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Exceeded 30 character maximum.")]
        protected override string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 150 character maximum.")]
        protected override string? Description { get; set; }

        protected override int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        internal virtual AppUser User { get; set; }

        
    }

}
