using Quartz;
using Stock.BE.Core.DL;

namespace Stock.BE.Jobs
{
    [DisallowConcurrentExecution]
    public class BuySellBackgroundJob : IJob
    {

        private readonly IStockDL _stockDL;
        public BuySellBackgroundJob(IStockDL stockDL)
        {
            _stockDL = stockDL;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("Test 123");
        }
    }
}
