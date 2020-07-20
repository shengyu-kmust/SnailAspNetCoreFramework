# hangfire
## 如何使用
* 在定时任务的方法上加下如下Attribute即可
[AutomaticRetry(Attempts =3,OnAttemptsExceeded =AttemptsExceededAction.Fail)]
[JobDisplayName("测试hangfire定时任务")]
[BackgroundJob(JobType =EBackgroundJobType.Recurring,Cron ="* * * * *")]