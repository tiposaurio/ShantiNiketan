//-----------------------------------------------------------------------
// <copyright file="NinjectDependencyResolver.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System.Web.Http.Dependencies;
    using Ninject;

    /// <summary>
    /// Ninject Dependency Resolver class
    /// </summary>
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        #region Private Members

        /// <summary>
        /// Ninject kernel
        /// </summary>
        private readonly IKernel kernel;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver" /> class.
        /// </summary>
        /// <param name="kernel">Kernel to be used</param>
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Begins Ninject Scope
        /// </summary>
        /// <returns>New kernel scope</returns>
        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }

        #endregion
    }
}