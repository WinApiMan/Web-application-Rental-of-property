// <copyright file="AdDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Ad model.
    /// </summary>
    public class AdDTO
    {
        /// <summary>
        /// Gets or sets rental ad data model.
        /// </summary>
        public RentalAdDTO RentalAd { get; set; }

        /// <summary>
        /// Gets or sets contact person model.
        /// </summary>
        public ContactPersonDTO ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets housing photo models.
        /// </summary>
        public IEnumerable<HousingPhotoDTO> HousingPhotos { get; set; }

        /// <summary>
        /// Gets or sets aditional ad data model.
        /// </summary>
        public AditionalAdDataDTO AditionalAdData { get; set; }
    }
}
