using System.ComponentModel.DataAnnotations; // for indicating property requirements

namespace SuccessfulStartup.Api.ViewModels
{
    public class BusinessPlanViewModel //  entity for use in API, originally inherited from business model, but this requires AppUser User as a navigation key, exposing the user information unnecessarily
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Exceeded 30 character maximum.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 150 character maximum.")]
        public string Description { get; set; }

        public string AuthorId { get; set; }

        
    }

}
