using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hdcontext.AdminDomain.Domain
{
    [Table("CompanyInfo")]
    public class CompanyInfo : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [MaxLength(50)]
        public string ShortTittle { get; set; }

        public string SrefShort { get; set; }

        [MaxLength(500)]
        public string LongTittle { get; set; }

        public string SrefLong { get; set; }

        public string ImageIcon { get; set; }
    }
}