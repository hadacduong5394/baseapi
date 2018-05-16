using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hdcontext.AdminDomain.IdentityCode
{
    [Table("ProductCode")]
    public class ProductCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string KeyCode { get; set; }
    }
}