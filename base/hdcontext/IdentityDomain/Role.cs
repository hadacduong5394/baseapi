using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hdcontext.IdentityDomain
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        [Required]
        public string Name { get; set; }

        [MaxLength(128)]
        [Required]
        public string Descreption { get; set; }

        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool Status { get; set; }
    }
}