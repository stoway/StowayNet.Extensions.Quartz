using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace StowayNet.Extensions.Quartz
{
    public interface IQuartzJob : IJob, IStowayDependency
    {
        string ServiceName { get; }
        string QuartzCronExpression { get; }
    }
}
