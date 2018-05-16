using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace hdcore
{
    public class ConfigAutofac
    {
        public static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public static void RegisterAssemblyTypes<T>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(T).Assembly).AsImplementedInterfaces().InstancePerRequest();
        }
    }
}