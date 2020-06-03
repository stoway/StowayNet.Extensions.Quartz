using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StowayNet.Extensions.Quartz
{
    [StowayDependency(StowayDependencyType.Singleton)]
    internal class QuartzHostedService : IHostedService
    {
        private readonly ILogger<QuartzHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public QuartzHostedService(ILogger<QuartzHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var scheduleServices = _serviceProvider.GetServices<IQuartzJob>();

            ISchedulerFactory factory = new StdSchedulerFactory();

            var scheduler = await factory.GetScheduler();
            scheduler.JobFactory = new QuartzJobFactory(_serviceProvider);
            foreach (var service in scheduleServices)
            {
                _logger.LogTrace($"register job:{service.ServiceName}.");
                await CreateJobAsync(service, scheduler);
            }
            await scheduler.Start();
            _logger.LogDebug($"job services started!");
        }

        async Task CreateJobAsync(IQuartzJob service, IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create(service.GetType()).WithIdentity(service.ServiceName, "ScheduleServiceGroup").Build();

            var builder = TriggerBuilder.Create()
                .WithIdentity(service.ServiceName, "ScheduleServiceGroup")
                .WithCronSchedule(service.QuartzCronExpression);
            ITrigger trigger = null;
            trigger = builder.Build();
            await scheduler.ScheduleJob(job, trigger);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
