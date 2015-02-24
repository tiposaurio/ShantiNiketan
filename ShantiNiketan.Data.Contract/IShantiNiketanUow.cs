//-----------------------------------------------------------------------
// <copyright file="IShantiNiketanUow.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Data.Contract
{
    using System.Threading.Tasks;
    using ShantiNiketan.Model;

    /// <summary>
    /// Interface for the Shanti Niketan "Unit of Work"
    /// </summary>
    public interface IShantiNiketanUow
    {
        #region Repositories

        /// <summary>
        /// Gets Contact Repository
        /// </summary>
        IRepository<Contact> Contacts { get; }

        #endregion

        #region Operations

        /// <summary>
        /// Saves pending changes to the data store
        /// </summary>
        /// <returns>Changes saved asynchronously</returns>
        Task Commit();

        #endregion
    }
}
