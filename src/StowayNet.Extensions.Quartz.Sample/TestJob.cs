using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StowayNet.Extensions.Quartz.Sample
{
    class TestJob : StowayNet.Extensions.Quartz.IQuartzJob
    {
        private readonly ILogger<TestJob> _logger;

        public string ServiceName => nameof(TestJob);

        public string QuartzCronExpression => "*/2 * * * * ?";

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"{nameof(TestJob)} start, {DateTime.Now:G}");

            return Task.CompletedTask;
        }
    }
}
