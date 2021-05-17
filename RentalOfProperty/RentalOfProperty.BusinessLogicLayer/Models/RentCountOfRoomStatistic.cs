// <copyright file="RentCountOfRoomStatistic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Rent count of room statistic.
    /// </summary>
    public class RentCountOfRoomStatistic
    {
        /// <summary>
        /// Gets or sets count of room.
        /// </summary>
        public int RentCountOfRoom { get; set; }

        /// <summary>
        /// Gets or sets dailt ads average price.
        /// </summary>
        public int DailyAdsAveragePrice { get; set; }

        /// <summary>
        /// Gets or sets long term ads average price.
        /// </summary>
        public int LongTermAdsAveragePrice { get; set; }
    }
}
