using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hdcontext.AdminDomain.Domain
{
    [Table("TopMenu")]
    public class TopMenu : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string UI_SREF { get; set; }

        [MaxLength(200)]
        public string Icon { get; set; }

        public int OrderNumber { get; set; }

        [MaxLength(1024)]
        public string URL { get; set; }
    }
}