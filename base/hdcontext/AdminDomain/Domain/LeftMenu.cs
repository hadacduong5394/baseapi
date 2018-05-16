using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hdcontext.AdminDomain.Domain
{
    [Table("LeftMenu")]
    public class LeftMenu : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        [MaxLength(1024)]
        public string UI_SREF { get; set; }

        public int OrderNumber { get; set; }

        [MaxLength(1024)]
        public string URL { get; set; }
    }
}