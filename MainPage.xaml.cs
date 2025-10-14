using System;
using System.Linq.Expressions;
namespace Clock
{
    public partial class MainPage : ContentPage
    {
        private DateTime _lastSyncTime;
        private CancellationTokenSource? _cts;

        private readonly Dictionary<int, bool[]> _digitSegment = new()
        {
            {0, new bool[] {true, true, true, true, true, true, false} },
            {1, new bool[] {false, true, true, false, false, false, false} },
            {2, new bool[] {true, true, false, true, true, false, true } },
            {3, new bool[] {true, true, true, true, false, false, true} },
            {4, new bool[] {false, true, true, false, false, true, true} },
            {5, new bool[] {true, false, true, true, false, true, true} },
            {6, new bool[] {true, false, true, true, true, true, true} },
            {7, new bool[] {true, true, true, false, false, false, false} },
            {8, new bool[] {true, true, true, true, true, true, true} },
            {9, new bool[] {true, true, true, true, false, true, true} }
        };
        public MainPage()
        {
            InitializeComponent();
            InitializeDigitGrids();
            InitializeClock();
        }
        private void InitializeDigitGrids()
        {
            InitializeSingleDigit(HourTensDigit);
            InitializeSingleDigit(MinuteTensDigit);
            InitializeSingleDigit(SecondTensDigit);
            InitializeSingleDigit(HourOnesDigit);
            InitializeSingleDigit(MinuteOnesDigit);
            InitializeSingleDigit(SecondOnesDigit);

        }

        private void InitializeSingleDigit(Grid digitGrid)
        {
            digitGrid.Children.Clear();

            var segA = new BoxView { HeightRequest=6};
            Grid.SetRow(segA, 0);
            Grid.SetColumn(segA, 0);
            Grid.SetColumnSpan(segA, 3);

            var segB = new BoxView { WidthRequest = 8};
            Grid.SetRow(segB, 1);
            Grid.SetRowSpan(segB, 1);
            Grid.SetColumn(segB, 2);

            var segC = new BoxView { WidthRequest = 8};
            Grid.SetRow(segC, 3);
            Grid.SetRowSpan(segC, 1);
            Grid.SetColumn(segC, 2);

            var segD = new BoxView{ HeightRequest = 6};
            Grid.SetRow(segD, 4);
            Grid.SetColumn(segD, 0);
            Grid.SetColumnSpan(segD, 3);

            var segE = new BoxView{ WidthRequest = 8 };
            Grid.SetRow(segE, 3);
            Grid.SetRowSpan(segE, 1);
            Grid.SetColumn(segE, 0);

            var segF = new BoxView{ WidthRequest = 8 };
            Grid.SetRow(segF, 1);
            Grid.SetRowSpan(segF, 1);
            Grid.SetColumn(segF, 0);

            var segG = new BoxView{ HeightRequest =6 };
            Grid.SetRow(segG, 2);
            Grid.SetColumn(segG, 0);
            Grid.SetColumnSpan(segG,3);

            digitGrid.Children.Add(segA);
            digitGrid.Children.Add(segB);
            digitGrid.Children.Add(segC);
            digitGrid.Children.Add(segD);
            digitGrid.Children.Add(segE);
            digitGrid.Children.Add(segF);
            digitGrid.Children.Add(segG);
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
            UpdateTimeDisplay(_lastSyncTime);
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
                        UpdateTimeDisplay(_lastSyncTime);
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
            var time = DateTime.Now;
            UpdateTimeDisplay(time);
        }

        private void UpdateTimeDisplay(DateTime time)
        {
            UpdateDigit(HourTensDigit, time.Hour / 10);
            UpdateDigit(HourOnesDigit, time.Hour % 10);

            UpdateDigit(MinuteTensDigit, time.Minute / 10);
            UpdateDigit(MinuteOnesDigit, time.Minute % 10);

            UpdateDigit(SecondTensDigit, time.Second / 10);
            UpdateDigit(SecondOnesDigit, time.Second % 10);
        }

        private void UpdateDigit(Grid digitGrid, int digit)
        {
            if (!_digitSegment.ContainsKey(digit)) return;
            var segments = digitGrid.Children;
            var activeSegments = _digitSegment[digit];
            int index = 0;
            foreach (var segment in segments)
            {
                var boxView = segment as BoxView;
                if (boxView != null)
                {
                    boxView.Color = activeSegments[index] ? Colors.LimeGreen : Color.FromArgb("#003300");
                }
                index++;
            }
        }
    }
}
