//-----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data.Contract
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Repository pattern
    /// </summary>
    /// <typeparam name="T">Selected Entity</typeparam>
    public interface IRepository<T> where T : class
    {
        #region Exposed Methods

        /// <summary>
        /// Gets all records of selected Entity
        /// </summary>
        /// <returns>All records of selected Entity</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets selected Entity synchronously
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        /// <returns>Selected Entity synchronously</returns>
        T GetById(int id);

        /// <summary>
        /// Gets selected Entity Asynchronously
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        /// <returns>Selected Entity asynchronously</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds selected Entity
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        void Add(T entity);

        /// <summary>
        /// Updates selected Entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        void Update(T entity);

        /// <summary>
        /// Deletes selected Entity
        /// </summary>
        /// <param name="entity">Selected Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes selected Entity
        /// </summary>
        /// <param name="id">Id of selected Entity</param>
        void Delete(int id);

        #endregion
    }
}
