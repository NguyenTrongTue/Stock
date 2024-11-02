using Microsoft.Extensions.Options;
using Quartz;

namespace Stock.BE.Jobs
{
    public class BuySellBackgroundJobSetup: IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(BuySellBackgroundJob));
            options
                .AddJob<BuySellBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10).RepeatForever()));
        }
    }
}
