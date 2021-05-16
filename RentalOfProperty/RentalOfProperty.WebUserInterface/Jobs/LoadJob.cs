// <copyright file="LoadJob.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Jobs
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Quartz;
    using RentalOfProperty.BusinessLogicLayer.Enums;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;

    /// <summary>
    /// Load ads job.
    /// </summary>
    [DisallowConcurrentExecution]
    public class LoadJob : IJob
    {
        private readonly IAdsManager _adsManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadJob"/> class.
        /// </summary>
        /// <param name="adsManager">Ads manager.</param>
        public LoadJob(IAdsManager adsManager)
        {
            _adsManager = adsManager;
        }

        /// <summary>
        /// Call loads.
        /// </summary>
        /// <param name="context">Job context.</param>
        /// <returns>Task result.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await _adsManager.LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu.GoHomeByDailyAds);
            await _adsManager.LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu.GoHomeByLongTermAds);
            await _adsManager.LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu.RealtByDailyAds);
            await _adsManager.LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu.RealtByLongTermAds);
        }
    }
}
