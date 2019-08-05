using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace Workflow
{
    internal class DependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel kernel;
 
        public DependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }
 
        public IDependencyScope BeginScope()
        {
            return new DependencyScope(kernel);
        }
 
        public object GetService(Type type)
        {
            return kernel.HasComponent(type) ? kernel.Resolve(type) : null;
        }
 
        public IEnumerable<object> GetServices(Type type)
        {
            return kernel.ResolveAll(type).Cast<object>();
        }
 
        public void Dispose()
        {
        }
    }
    
    public class DependencyScope : IDependencyScope
    {
        private readonly IKernel kernel;
 
        private readonly IDisposable disposable;
 
        public DependencyScope(IKernel kernel)
        {
            this.kernel = kernel;
            disposable = kernel.BeginScope();
        }
 
        public object GetService(Type type)
        {
            return kernel.HasComponent(type) ? kernel.Resolve(type) : null;
        }
 
        public IEnumerable<object> GetServices(Type type)
        {
            return kernel.ResolveAll(type).Cast<object>();
        }
 
        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}