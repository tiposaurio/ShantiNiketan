//-----------------------------------------------------------------------
// <copyright file="IRepositoryProvider.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data.Helpers
{
    using System;
    using System.Data.Entity;
    using Contract;

    /// <summary>
    /// Interface for a class that can provide repositories by type.
    /// The class may create the repositories dynamically if it is unable
    /// to find one in its cache of repositories.
    /// </summary>
    /// <remarks>
    /// Repositories created by this provider tend to require a <see cref="DbContext"/>
    /// to retrieve data.
    /// </remarks>
    public interface IRepositoryProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="DbContext"/> with which to initialize a repository
        /// if one must be created.
        /// </summary>
        DbContext DbContext { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an <see cref="IRepository{T}"/> for entity type, T.
        /// </summary>
        /// <typeparam name="T">
        /// Root entity type of the <see cref="IRepository{T}"/>.
        /// </typeparam>
        /// <returns>Repository for selected Entity</returns>
        IRepository<T> GetRepositoryForEntityType<T>() where T : class;

        /// <summary>
        /// Gets a repository of type T.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the repository, typically a custom repository interface.
        /// </typeparam>
        /// <param name="factory">
        /// An optional repository creation function that takes a <see cref="DbContext"/>
        /// and returns a repository of T. Used if the repository must be created.
        /// </param>
        /// <remarks>
        /// Looks for the requested repository in its cache, returning if found.
        /// If not found, tries to make one with the factory, falling back to 
        /// a default factory if the factory parameter is null.
        /// </remarks>
        /// <returns>Repository for selected Entity</returns>
        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;

        /// <summary>
        /// Sets the repository to return from this provider.
        /// </summary>
        /// <param name="repository">Repository to be set</param>
        /// <typeparam name="T">
        /// Type of the repository, typically a custom repository interface.
        /// </typeparam>
        /// <remarks>
        /// Sets a repository if you don't want this provider to create one.
        /// Useful in testing and when developing without a backend
        /// implementation of the object returned by a repository of type T.
        /// </remarks>
        void SetRepository<T>(T repository);

        #endregion
    }
}
