using System;
namespace Clock
{
    public partial class MainPage : ContentPage
    {
        private DateTime _currTime;
        public MainPage()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            _currTime = DateTime.Now;
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            TimeLabel.Text = _currTime.ToString("HH:mm:ss");
        }
    }
}
