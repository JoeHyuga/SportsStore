using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

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
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(new List<Product> {
                new Product { Name="Football",Price=25},
                new Product { Name="Surf board",Price=179},
                new Product { Name="Running shone",Price=95}
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }
    }
}