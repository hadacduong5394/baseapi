using System;

namespace hdcontext.AdminDomain.Abtracttions
{
    public interface IAuditable
    {
        string CreateBy { get; set; }

        string ModifyBy { get; set; }

        DateTime CreateDate { get; set; }

        DateTime? ModifyDate { get; set; }

        bool Status { get; set; }
    }
}