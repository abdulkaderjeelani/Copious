using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Copious.Infrastructure.Interface.Services
{
    public interface IScheduler
    {
        void ScheduleJob(Expression<Action> actionToDo);

        void ScheduleJob<T>(Expression<Action<T>> actionToDo);

        void ScheduleJobAfter(Expression<Action> actionToDo, TimeSpan after);

        void ScheduleRecurringJob(Expression<Action> actionToDo);

        void ScheduleAsyncJob<T>(Expression<Func<T, Task>> actionToDo);
    }
}