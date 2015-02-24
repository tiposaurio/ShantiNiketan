//-----------------------------------------------------------------------
// <copyright file="RepositoryProvider.cs" company="Shanti Niketan">
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
    /// Provides an <see cref="IRepository{T}"/> for a client request.
    /// </summary>
    /// <remarks>
    /// Caches repositories of a given type so that repositories are only created once per provider.
    /// Shanti Niketan creates a new provider per client request.
    /// </remarks>
    public class RepositoryProvider : IRepositoryProvider
    {
        #region Private Members

        /// <summary>
        /// The <see cref="RepositoryFactories"/> with which to create a new repository.
        /// </summary>
        /// <remarks>
        /// Should be initialized by constructor injection
        /// </remarks>
        private readonly RepositoryFactories repositoryFactories;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryProvider" /> class.
        /// </summary>
        /// <param name="repositoryFactories">Repository Factory to be set and used</param>
        public RepositoryProvider(RepositoryFactories repositoryFactories)
        {
            this.repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="DbContext"/> with which to initialize a repository
        /// if one must be created.
        /// </summary>
        public DbContext DbContext { get; set; }

        /// <summary>
        /// Gets the dictionary of repository objects, keyed by repository type.
        /// </summary>
        /// <remarks>
        /// Caller must know how to cast the repository object to a useful type.
        /// <p>This is an extension point. You can register fully made repositories here
        /// and they will be used instead of the ones this provider would otherwise create.</p>
        /// </remarks>
        protected Dictionary<Type, object> Repositories { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets or create-and-cache the default <see cref="IRepository{T}"/> for an entity of type T.
        /// </summary>
        /// <typeparam name="T">
        /// Root entity type of the <see cref="IRepository{T}"/>.
        /// </typeparam>
        /// <remarks>
        /// If can't find repository in cache, use a factory to create one.
        /// </remarks>
        /// <returns>Repository for selected Entity</returns>
        public IRepository<T> GetRepositoryForEntityType<T>() where T : class
        {
            return GetRepository<IRepository<T>>(repositoryFactories.GetRepositoryFactoryForEntityType<T>());
        }

        /// <summary>
        /// Gets or create-and-cache a repository of type T.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the repository, typically a custom repository interface.
        /// </typeparam>
        /// <param name="factory">
        /// An optional repository creation function that takes a DbContext argument
        /// and returns a repository of T. Used if the repository must be created and
        /// caller wants to specify the specific factory to use rather than one
        /// of the injected <see cref="RepositoryFactories"/>.
        /// </param>
        /// <remarks>
        /// Looks for the requested repository in its cache, returning if found.
        /// If not found, tries to make one using <see cref="MakeRepository{T}"/>.
        /// </remarks>
        /// <returns>Repository created by selected factory</returns>
        public virtual T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
        {
            //// Looks for T dictionary cache under typeof(T).
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);

            if (repoObj != null)
            {
                return (T)repoObj;
            }

            //// Not found or null; make one, add to dictionary cache, and return it.
            return MakeRepository<T>(factory, DbContext);
        }

        /// <summary>
        /// Sets the repository for type T that this provider should return.
        /// </summary>
        /// <param name="repository">Selected repository</param>
        /// <typeparam name="T">Type of repository to make.</typeparam>
        /// <remarks>
        /// Plug in a custom repository if you don't want this provider to create one.
        /// Useful in testing and when developing without a backend
        /// implementation of the object returned by a repository of type T.
        /// </remarks>
        public void SetRepository<T>(T repository)
        {
            Repositories[typeof(T)] = repository;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Makes a repository of type T.
        /// </summary>
        /// <typeparam name="T">Type of repository to make.</typeparam>
        /// <param name="factory">Factory with <see cref="DbContext"/> argument. Used to make the repository.
        /// If null, gets factory from <see cref="repositoryFactories"/>.</param>
        /// <param name="dbContext">The <see cref="DbContext"/> with which to initialize the repository.</param>
        /// <returns>Created repository for selected Entity</returns>
        protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
        {
            var f = factory ?? repositoryFactories.GetRepositoryFactory<T>();

            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }

            var repo = (T)f(dbContext);
            Repositories[typeof(T)] = repo;

            return repo;
        }

        #endregion
    }
}
