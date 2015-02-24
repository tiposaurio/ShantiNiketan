//-----------------------------------------------------------------------
// <copyright file="EFRepository.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using Contract;

    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class EFRepository<T> : IRepository<T> where T : class
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="{T}" /> class.
        /// </summary>
        /// <param name="dbContext">DB Context to be set</param>
        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets DbContext
        /// </summary>
        protected DbContext DbContext { get; set; }

        /// <summary>
        /// Gets or sets DbSet
        /// </summary>
        protected DbSet<T> DbSet { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all records of selected Entity
        /// </summary>
        /// <returns>All records of selected Entity</returns>
        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Gets selected Entity synchronously
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        /// <returns>Selected Entity synchronously</returns>
        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Gets selected Entity asynchronously
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        /// <returns>Selected Entity asynchronously</returns>
        public virtual Task<T> GetByIdAsync(int id)
        {
            return DbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds selected Entity
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        /// <summary>
        /// Updates selected Entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes selected Entity
        /// </summary>
        /// <param name="entity">Selected Entity</param>
        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Deletes selected Entity
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        public virtual void Delete(int id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                return; //// not found; assumes already deleted.
            }

            Delete(entity);
        }

        #endregion
    }
}