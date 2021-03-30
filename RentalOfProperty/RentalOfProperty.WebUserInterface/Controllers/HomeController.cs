// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.Models;
    using RentalOfProperty.WebUserInterface.Models.Ad;

    /// <summary>
    /// Main.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAdsManager _adsManager;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">Logger object.</param>
        /// <param name="adsManager">Ads manager.</param>
        /// <param name="mapper">Automapper object.</param>
        public HomeController(ILogger<HomeController> logger, IAdsManager adsManager, IMapper mapper)
        {
            _logger = logger;
            _adsManager = adsManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Index.
        /// </summary>
        /// <returns>Result object.</returns>
        public async Task<IActionResult> Index()
        {
            return View(new AdsPageView
            {
                RentalAdView = new List<RentalAdView>(),
                IsSuccess = false,
            });
        }

        /// <summary>
        /// Privacy.
        /// </summary>
        /// <returns>Result object.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <returns>Result object.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
