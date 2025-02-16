﻿using Collapsenav.Net.Tool.Demo.Quartz;
using Collapsenav.Net.Tool.Ext;

QuartzNode.Builder = new();
QuartzNode.Builder.AddJob<FirstJob>(3);
QuartzNode.Builder.AddJob<SecondJob>("0/5 * * * * ?");
QuartzNode.Builder.AddJob<MutJob>(new[] { "0/5 * * * * ?", "0/20 * * * * ?" });

await QuartzNode.InitFromBuilderAsync(QuartzNode.Builder);
await QuartzNode.Scheduler.Start();


// var builder = new QuartzJobBuilder();
// var scheduler = await new StdSchedulerFactory().GetScheduler();
// builder.AddJob<FirstJob>(3);
// builder.AddJob<SecondJob>("0/5 * * * * ?");
// await builder.Build(scheduler);
// await scheduler.Start();

Console.ReadKey();