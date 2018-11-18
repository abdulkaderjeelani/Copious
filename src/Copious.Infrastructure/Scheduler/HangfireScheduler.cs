using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Copious.Infrastructure.Interface.Services;
using Hangfire;

namespace Copious.Infrastructure.Scheduler {
    // https://github.com/HangfireIO/Hangfire
    // http://nance.io/dotnet/core/async/2016/08/19/long-running-tasks-in-dotnet-core-with-hangfire.html
    // https://github.com/jaredcnance/hangfire-dot-net-core-example
    public class HangfireScheduler : IScheduler {
        public HangfireScheduler () { }

        public void ScheduleJob (Expression<Action> actionToDo) => BackgroundJob.Enqueue (actionToDo);

        public void ScheduleJob<T> (Expression<Action<T>> actionToDo) => BackgroundJob.Enqueue<T> (actionToDo);

        public void ScheduleAsyncJob<T> (Expression<Func<T, Task>> actionToDo) => BackgroundJob.Enqueue<T> (actionToDo);

        public void ScheduleJobAfter (Expression<Action> actionToDo, TimeSpan after) => BackgroundJob.Schedule (actionToDo, after);

        public void ScheduleRecurringJob (Expression<Action> actionToDo) => RecurringJob.AddOrUpdate (actionToDo, Cron.Hourly);
    }
}