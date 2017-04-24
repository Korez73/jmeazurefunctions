using System;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"Every Minute Timer trigger function executed at: {DateTime.Now}");    
}