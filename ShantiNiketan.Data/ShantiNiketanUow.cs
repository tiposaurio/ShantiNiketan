//-----------------------------------------------------------------------
// <copyright file="ShantiNiketanUow.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data
{
    using System;
    using System.Threading.Tasks;
    using Contract;
    using Helpers;
    using Model;

    /// <summary>
    /// The Shanti Niketan "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="Contact"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in Shanti Niketan).
    /// </remarks>
    public class ShantiNiketanUow : IShantiNiketanUow, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ShantiNiketanUow" /> class.
        /// </summary>
        /// <param name="repositoryProvider">Repository provider to be used</param>
        public ShantiNiketanUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();
            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Contacto Repository
        /// </summary>
        public IRepository<Contact> Contacts
        {
            get
            {
                return GetStandardRepo<Contact>();
            }
        }

        //// public ITranslatorRepository Translators { get { return GetRepo<ITranslatorRepository>(); } }

        /// <summary>
        /// Gets or sets RepositoryProvider
        /// </summary>
        protected IRepositoryProvider RepositoryProvider { get; set; }

        /// <summary>
        /// Gets or sets Selected DB Context
        /// </summary>
        private ShantiNiketanDbContext DbContext { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves pending changes to the database
        /// </summary>
        /// <returns>Changes saved asynchronously</returns>
        public Task Commit()
        {
            return DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// The pattern for disposing an object, referred to as a dispose pattern, imposes order on the lifetime of an object. 
        /// The dispose pattern is used only for objects that access unmanaged resources, such as file and pipe handles, 
        /// registry handles, wait handles, or pointers to blocks of unmanaged memory. 
        /// This is because the garbage collector is very efficient at reclaiming unused managed objects, 
        /// but it is unable to reclaim unmanaged objects.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates selected DB context
        /// </summary>
        protected void CreateDbContext()
        {
            DbContext = new ShantiNiketanDbContext();

            //// Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            //// Loads navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            //// Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //// DbContext.Configuration.AutoDetectChangesEnabled = false;
            //// We won't use this performance tweak because we don't need 
            //// the extra performance and, when autodetect is false,
            //// we'd have to be careful. We're not being that careful.
        }

        /// <summary>
        /// The Dispose method performs all object cleanup, so the garbage collector no longer needs to call the objects' Object.Finalize override. 
        /// Therefore, the call to the SuppressFinalize method prevents the garbage collector from running the finalizer. 
        /// If the type has no finalizer, the call to GC.SuppressFinalize has no effect. 
        /// Note that the actual work of releasing unmanaged resources is performed by the second overload of the Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// The disposing parameter is a Boolean that indicates whether the method call comes from a Dispose method 
        /// (its value is true) or from a finalizer (its value is false)
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets standard repository
        /// </summary>
        /// <typeparam name="T">Type of the repository, typically a custom repository interface.</typeparam>
        /// <returns>Standard Repository</returns>
        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        /// <summary>
        /// Gets special repository
        /// </summary>
        /// <typeparam name="T">Type of the repository, typically a custom repository interface.</typeparam>
        /// <returns>Special Repository</returns>
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        #endregion
    }
}