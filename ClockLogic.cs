using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    public class ClockLogic
    {
        private DateTime _lastSyncTime;
        private CancellationTokenSource? _cts;
        private Action<DateTime> _updateDisplayAction;

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

        public Dictionary<int, bool[]> GetDigitSegments()
        {
            return _digitSegment;
        }

        public ClockLogic(Action<DateTime> updateDisplayAction)
        {
            _updateDisplayAction = updateDisplayAction;
        }

        public void InitializeClock()
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
            _updateDisplayAction(_lastSyncTime);
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
                        _updateDisplayAction(_lastSyncTime);
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
            _updateDisplayAction(time);
        }
    }
}
