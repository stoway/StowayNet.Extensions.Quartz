# StowayNet.Extensions.Quartz
StowayNet.Extensions.Quartz is an extension of Quartz components, which can support injection dependencies and simple implementation job interface.

## Get Started
### NuGet 

You can run the following command to install the `StowayNet.Extensions.Quartz` in your project.

```
PM> Install-Package StowayNet.Extensions.Quartz
```

### Configuration

First,You need to config `StowayNet.Extensions.Quartz` in your `Startup.cs`:
```c#
......
using StowayNet;
......

public void ConfigureServices(IServiceCollection services)
{
    ......

    services.AddStowayNet();

    ......
}

```

### Sample

#### Implement `StowayNet.Extensions.Quartz.IQuartzJob` Interface

```c#
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
```
