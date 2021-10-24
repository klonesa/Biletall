using Autofac;
using Biletall.Business.Abstract;
using Biletall.Business.Concrete;
using Biletall.DataAccess.EntityFramework.Context;
using Biletall.DataAccess.EntityFramework.UnityOfWork;
using Biletall.DataAccess.Services.Abstraction;
using Biletall.DataAccess.Services.Concrete;

namespace Biletall.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModuleWeb : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region DATA ACCESS

            builder.RegisterType<BiletallContext>()
               .AsSelf()
               .InstancePerLifetimeScope();

            builder.RegisterType<UoWBiletall>().As<IUoWBiletall>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<BiletallService>().As<IBiletallService>()
                .AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region SERVICES

            builder.RegisterType<BusinessService>().As<IBusinessService>();

            #endregion
        }
    }
}
