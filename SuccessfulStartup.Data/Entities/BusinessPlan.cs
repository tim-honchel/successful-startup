using Microsoft.AspNetCore.Authorization;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Xml.Linq;

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
