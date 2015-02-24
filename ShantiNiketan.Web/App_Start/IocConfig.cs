//-----------------------------------------------------------------------
// <copyright file="IocConfig.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System.Web.Http;
    using Data;
    using Data.Contract;
    using Data.Helpers;
    using Ninject;

    /// <summary>
    /// Inversion of Control configuration class
    /// </summary>
    public class IocConfig
    {
        #region Public Methods

        /// <summary>
        /// Registers IoC configuration
        /// </summary>
        /// <param name="config">Represents a configuration of HttpServer instances.</param>
        public static void RegisterIoc(HttpConfiguration config)
        {
            //// Ninject IoC
            var kernel = new StandardKernel();

            //// These registrations are "per instance request".
            //// See http://blog.bobcravens.com/2010/03/ninject-life-cycle-management-or-scoping/
            kernel.Bind<RepositoryFactories>().To<RepositoryFactories>().InSingletonScope();
            kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            kernel.Bind<IShantiNiketanUow>().To<ShantiNiketanUow>();
            //// Tells WebApi how to use our Ninject IoC
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }

        #endregion
    }
}