using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using StepCounter.Services;
using Android.Content.PM;

namespace StepCounter.Platforms.Android
{
    [Service(
        Enabled = true,
        Exported = false,
        ForegroundServiceType = ForegroundService.TypeDataSync
    )]
    public class PedometerForegroundService : Service
    {
        private Timer? _timer;
        private const string ChannelId = "pedometer_service_channel";
        private const int NotificationId = 1001;
        private StepCounterService? _stepCounterService;

        public override void OnCreate()
        {
            base.OnCreate();
            CreateNotificationChannel();

            _stepCounterService = MauiApplication.Current.Services.GetService<StepCounterService>();
            _ = _stepCounterService?.UpdateDailyStepsFromDatabase();

            var notification = BuildNotification(_stepCounterService?.DailySteps ?? 0);
            StartForeground(NotificationId, notification);

            // policz kiedy jest kolejna pełna minuta
            var now = DateTime.Now;
            var nextFullMinute = now.AddMinutes(1).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
            var initialDelay = nextFullMinute - now;

            _timer = new Timer(
                callback: async state =>
                {
                    var steps = _stepCounterService?.DailySteps ?? 0;
                    var updatedNotification = BuildNotification(steps);
                    var notificationManager = NotificationManagerCompat.From(this);
                    await _stepCounterService!.SaveDailyStepsToDbAsync();
                    notificationManager!.Notify(NotificationId, updatedNotification);
                },
                state: null,
                dueTime: initialDelay,
                period: TimeSpan.FromMinutes(1)
            );
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent) => null!;

        public override void OnDestroy()
        {
            base.OnDestroy();
            _timer?.Dispose();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    ChannelId,
                    "Pedometer Service",
                    NotificationImportance.Low
                )
                {
                    Description = "Foreground service for step counting"
                };
                channel.SetShowBadge(true);
                channel.SetBypassDnd(true);
                var notificationManager = (NotificationManager)GetSystemService(NotificationService)!;
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        private Notification BuildNotification(int steps)
        {
            var builder = new NotificationCompat.Builder(this, ChannelId)
                .SetContentTitle("Krokomierz działa")
                .SetContentText($"Dzisiejsze kroki: {steps}")
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                .SetOngoing(true)
                .SetSilent(true)
                .SetPriority((int)NotificationPriority.Low)
                .SetCategory(NotificationCompat.CategoryService);

            return builder.Build();
        }
    }
}

