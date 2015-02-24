//-----------------------------------------------------------------------
// <copyright file="ShantiNiketanDbContext.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Model;

    /// <summary>
    /// Shanti Niketan Entity Framework Database Context
    /// </summary>
    public class ShantiNiketanDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShantiNiketanDbContext" /> class.
        /// </summary>
        public ShantiNiketanDbContext()
            : base("ShantiNiketanDbContext")
        {
        }

        #endregion

        #region Native Properties

        /// <summary>
        /// Gets or sets Contacts
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but before the model has been locked down and used to initialize the context. 
        /// The default implementation of this method does nothing, but it can be overridden in a derived class such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">DbModelBuilder is used to map CLR classes to a database schema. This code centric approach to building an Entity Data Model (EDM) model is known as 'Code First'.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// Uses singular table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //// Disable proxy creation and lazy loading; not wanted in this service context.
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            //// Adds configurations
            //// modelBuilder.Configurations.Add(new CourseConfiguration());
        }

        #endregion
    }
}