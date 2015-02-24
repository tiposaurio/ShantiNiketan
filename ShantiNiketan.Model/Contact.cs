//-----------------------------------------------------------------------
// <copyright file="Contact.cs" company="Shanti Niketan">
//     Copyright (c) /shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// POCO (Plain Old CLR Object) Contact Entity
    /// </summary>
    public class Contact
    {
        #region Native Properties

        /// <summary>
        /// Gets or sets ContactId
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        public int ContactId { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        [Required]
        [StringLength(100)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Subject
        /// </summary>
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets Message
        /// </summary>
        [Required]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        #endregion
    }
}
