// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Quartz;
    using RentalOfProperty.WebUserInterface.Jobs;

    /// <summary>
    /// Start class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method for start program.
        /// </summary>
        /// <param name="args">Arguments for start.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create host builder object method.
        /// </summary>
        /// <param name="args">Artuments for create host builder.</param>
        /// <returns>Host builder object.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>().UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")))
                .ConfigureServices((hostContext, services) =>
                {
                    // Add the required Quartz.NET services
                    services.AddQuartz(q =>
                    {
                        // Use a Scoped container to create jobs. I'll touch on this later
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key" for the job
                        var jobKey = new JobKey("LoadJob");

                        // Register the job with the DI container
                        q.AddJob<LoadJob>(opts => opts.WithIdentity(jobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey) // link to the HelloWorldJob
                            .WithIdentity("LoadAdsJob-trigger") // give the trigger a unique name
                            .WithCronSchedule("0 00 23 ? * MON,TUE,WED,THU,FRI,SAT,SUN")); // run every 5 seconds
                    });

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);

                    // other config
                });
    }
}
