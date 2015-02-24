//-----------------------------------------------------------------------
// <copyright file="NinjectDependencyScope.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Http.Dependencies;
    using Ninject;
    using Ninject.Syntax;

    /// <summary>
    /// Ninject Dependency Scope class
    /// </summary>
    public class NinjectDependencyScope : IDependencyScope
    {
        #region Private Members

        /// <summary>
        /// Ninject resolver
        /// </summary>
        private IResolutionRoot resolver;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyScope" /> class.
        /// </summary>
        /// <param name="resolver">Resolver to be used</param>
        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);

            this.resolver = resolver;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disposes current resolver
        /// </summary>
        public void Dispose()
        {
            var disposable = resolver as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }

            resolver = null;
        }

        /// <summary>
        /// Gets service for a given type
        /// </summary>
        /// <param name="serviceType">Selected service type</param>
        /// <returns>Service for given type</returns>
        public object GetService(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return resolver.TryGet(serviceType);
        }

        /// <summary>
        /// Gets all services for a given type
        /// </summary>
        /// <param name="serviceType">Selected service type</param>
        /// <returns>Services for given type</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return resolver.GetAll(serviceType);
        }

        #endregion
    }
}