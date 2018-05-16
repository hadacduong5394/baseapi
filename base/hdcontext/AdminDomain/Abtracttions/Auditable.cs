using System;

namespace hdcontext.AdminDomain.Abtracttions
{
    public abstract class Auditable : IAuditable
    {
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; } = true;
    }
}