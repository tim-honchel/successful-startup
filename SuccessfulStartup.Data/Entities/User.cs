using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessfulStartup.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string SecurityStamp {get; set;}
    }
}
