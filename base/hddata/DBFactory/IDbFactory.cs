using hdcontext;
using System;

namespace hddata.DBFactory
{
    public interface IDbFactory : IDisposable
    {
        ContextConnection Init();
    }
}