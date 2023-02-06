using SuccessfulStartup.Domain.Entities;
using System.ComponentModel.DataAnnotations; // for indicating property requirements

namespace SuccessfulStartup.Data.Entities
{
    public class BusinessPlan : BusinessPlanDomain // model for Entity Framework
    {
        public int Id { get; set; } // TODO : change to GUID?

        [Required]
        [MaxLength(30, ErrorMessage = "Exceeded 30 character maximum.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 150 character maximum.")]
        public string Description { get; set; }

        public string AuthorId { get; set; }

        //[ForeignKey("AuthorId")]
        //public virtual AppUser User { get; set; }

        
    }

}

/* generates the following SQL script when adding migration
 
  Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand(2ms) [Parameters=[], CommandType = 'Text', CommandTimeout = '30']
      CREATE TABLE[BusinessPlans] (
          [Id] int NOT NULL IDENTITY,
          [Name] nvarchar(30) NOT NULL,
          [Description] nvarchar(150) NOT NULL,
          [AuthorId] nvarchar(450) NOT NULL,
          CONSTRAINT[PK_BusinessPlans] PRIMARY KEY ([Id]),
          CONSTRAINT[FK_BusinessPlans_AspNetUsers_AuthorId] FOREIGN KEY([AuthorId]) REFERENCES[AspNetUsers]([Id]) ON DELETE CASCADE
*/
