using hdcontext.IdentityDomain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdidentity.Interface
{
    public interface IErrorService: IBaseService<Error, int>
    {
        void Log(Exception ex);
    }
}
