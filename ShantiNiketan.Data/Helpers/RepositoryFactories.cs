//-----------------------------------------------------------------------
// <copyright file="RepositoryFactories.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Contract;

    /// <summary>
    /// A maker of Shanti Niketan Repositories.
    /// </summary>
    /// <remarks>
    /// An instance of this class contains repository factory functions for different types.
    /// Each factory function takes an EF <see cref="DbContext"/> and returns
    /// a repository bound to that DbContext.
    /// <para>
    /// Designed to be a "Singleton", configured at web application start with
    /// all of the factory functions needed to create any type of repository.
    /// Should be thread-safe to use because it is configured at app start,
    /// before any request for a factory, and should be immutable thereafter.
    /// </para>
    /// </remarks>
    public class RepositoryFactories
    {
        #region Private Members

        /// <summary>
        /// Gets the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="DbContext"/> argument and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly IDictionary<Type, Func<DbContext, object>> repositoryFactories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactories" /> class.
        /// Constructor that initializes with runtime Shanti Niketan repository factories
        /// </summary>
        public RepositoryFactories()
        {
            repositoryFactories = GetShantiNiketanFactories();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactories" /> class.
        /// Constructor that initializes with an arbitrary collection of factories
        /// </summary>
        /// <param name="factories">
        /// The repository factory functions for this instance. 
        /// </param>
        /// <remarks>
        /// This constructor is primarily useful for testing this class
        /// </remarks>
        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            repositoryFactories = factories;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        /// <remarks>
        /// The type parameter, T, is typically the repository type 
        /// but could be any type (e.g., an entity type)
        /// </remarks>
        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            repositoryFactories.TryGetValue(typeof(T), out factory);

            return factory;
        }

        /// <summary>
        /// Gets the factory for <see cref="IRepository{T}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IRepository{T}"/>, given an EF <see cref="DbContext"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="repositoryFactories"/>.
        /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="repositoryFactories"/>.
        /// </remarks>
        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Default factory for a <see cref="IRepository{T}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="T">Type of the repository's root entity</typeparam>
        /// <returns>New EF Repository for selected DB Context</returns>
        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new EFRepository<T>(dbContext);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the runtime Shanti Niketan repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        /// <remarks>
        /// MODIFY THIS METHOD TO ADD CUSTOM SHANTI NIKETAN FACTORY FUNCTIONS
        /// </remarks>
        /// <returns>Created Factory</returns>
        private IDictionary<Type, Func<DbContext, object>> GetShantiNiketanFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                //// {typeof(ITranslatorRepository), dbContext => new TranslatorRepository(dbContext)}
            };
        }

        #endregion
    }
}
