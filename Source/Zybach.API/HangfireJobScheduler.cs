using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hangfire;
using Hangfire.Storage;
using static Hangfire.JobCancellationToken;

namespace Zybach.API
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            var recurringJobIds = new List<string>();

            AddRecurringJob<GeoOptixSyncDailyJob>(GeoOptixSyncDailyJob.JobName, x => x.RunJob(Null), Cron.Daily(7, 5), recurringJobIds);
            AddRecurringJob<AgHubWellsFetchDailyJob>(AgHubWellsFetchDailyJob.JobName, x => x.RunJob(Null), Cron.Daily(7, 15), recurringJobIds);
            AddRecurringJob<FlowMeterSeriesFetchDailyJob>(FlowMeterSeriesFetchDailyJob.JobName, x => x.RunJob(Null), Cron.Daily(9, 15), recurringJobIds);
            AddRecurringJob<ContinuityMeterSeriesFetchDailyJob>(ContinuityMeterSeriesFetchDailyJob.JobName, x => x.RunJob(Null), Cron.Daily(10, 15), recurringJobIds);

            // Remove any jobs we haven't explicitly scheduled
            RemoveExtraneousJobs(recurringJobIds);
        }

        private static void AddRecurringJob<T>(string jobName, Expression<Action<T>> methodCallExpression,
            string cronExpression, ICollection<string> recurringJobIds)
        {
            RecurringJob.AddOrUpdate<T>(jobName, methodCallExpression, cronExpression);
            recurringJobIds.Add(jobName);
        }

        private static void RemoveExtraneousJobs(List<string> recurringJobIds)
        {
            using var connection = JobStorage.Current.GetConnection();
            var recurringJobs = connection.GetRecurringJobs();
            var jobsToRemove = recurringJobs.Where(x => !recurringJobIds.Contains(x.Id)).ToList();
            foreach (var job in jobsToRemove)
            {
                RecurringJob.RemoveIfExists(job.Id);
            }
        }
    }
}