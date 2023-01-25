using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Entities;
using System.ComponentModel.DataAnnotations; // for indicating property requirements
using System.ComponentModel.DataAnnotations.Schema; // for indicating foreign key

namespace SuccessfulStartup.Api.ViewModels
{
    public class BusinessPlanViewModel : BusinessPlan // model for Entity Framework
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Exceeded 30 character maximum.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 150 character maximum.")]
        public string Description { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual AppUser User { get; set; }

        
    }

}
