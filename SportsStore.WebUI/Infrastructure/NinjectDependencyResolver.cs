using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver: IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        object IDependencyResolver.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    }
}