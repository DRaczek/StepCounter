using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Plugin.Maui.Pedometer;
using System;

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
        private IPedometer? _pedometer;

        public override void OnCreate()
        {
            base.OnCreate();
            CreateNotificationChannel();

            var notification = BuildNotification();
            StartForeground(NotificationId, notification);

            // Inicjalizacja pedometru
            _pedometer = Pedometer.Default;
            _pedometer.ReadingChanged += OnPedometerReadingChanged;
            _pedometer.Start();

            // Timer opcjonalny, jeśli chcesz wykonywać dodatkowe operacje cyklicznie
            _timer = new Timer(
                callback: state => { /* tu możesz np. zapisywać kroki do bazy */ },
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromSeconds(1)
            );
        }

        private void OnPedometerReadingChanged(object? sender, PedometerData reading)
        {
            // Tutaj masz dostęp do reading.NumberOfSteps
            // Możesz zapisać do bazy, wysłać broadcast, itp.
            Console.WriteLine("StepCounter", $"Kroki: {reading.NumberOfSteps}");
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
            if (_pedometer != null)
            {
                _pedometer.ReadingChanged -= OnPedometerReadingChanged;
                _pedometer.Stop();
            }
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    ChannelId,
                    "Pedometer Service",
                    NotificationImportance.Max
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

        private Notification BuildNotification()
        {
            var builder = new NotificationCompat.Builder(this, ChannelId)
                .SetContentTitle("Krokomierz działa")
                .SetContentText("Zliczanie kroków w tle")
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetCategory(NotificationCompat.CategoryService);

            return builder.Build();
            }
    }
}

