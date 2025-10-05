using System;
using System.Linq.Expressions;
namespace Clock
{
    public partial class MainPage : ContentPage
    {
        private DateTime _lastSyncTime;
        private CancellationTokenSource? _cts;
        public MainPage()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            SyncWithSys();
            StartClock();
        }

        private void StartClock()
        {
            _cts = new CancellationTokenSource();
            _ = RunClock(_cts.Token);
        }

        private async Task RunClock(CancellationToken token)
        {
            UpdateTimeDisplay();
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(1000, token);

                    var now = DateTime.Now;
                    if (now.Hour != _lastSyncTime.Hour)
                    {
                        SyncWithSys();
                    }
                    else
                    {
                        _lastSyncTime = _lastSyncTime.AddSeconds(1);
                        UpdateTimeDisplay();
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            
        }

        private void SyncWithSys()
        {
            _lastSyncTime = DateTime.Now;
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            TimeLabel.Text = _lastSyncTime.ToString("HH:mm:ss");
        }
    }
}
