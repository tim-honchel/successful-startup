using System.ComponentModel.DataAnnotations; // for key

namespace SuccessfulStartup.Data.Entities
{
    public class User
    {
        [Key]
        public string AuthorId { get; set; }
        public string SecurityStamp {get; set;}
    }
}
