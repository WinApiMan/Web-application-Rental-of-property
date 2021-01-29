// <copyright file="IdentityResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Identity result in business logic.
    /// </summary>
    public class IdentityResult
    {
        /// <summary>
        /// Gets or sets identity errors list.
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is operation successed.
        /// </summary>
        public bool Succeeded { get; set; }
    }
}
