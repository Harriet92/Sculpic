using System;

namespace Assets.Sources.Scripts.GameRoom
{
    public class DrawingTimer
    {
        private static readonly TimeSpan RoundLength = new TimeSpan(0, 0, 5, 0);
        private static readonly TimeSpan TimePointInterval = new TimeSpan(0, 0, 1, 0);
        private float _secondsLeft = (float)RoundLength.TotalSeconds;

        private TimeSpan TimeLeft
        {
            get
            {
                var minutes = (int)(_secondsLeft / 60);
                var seconds = (int)(_secondsLeft % 60);
                return new TimeSpan(0, 0, minutes, seconds);
            }
        }

        public bool IsOn { get; set; }

        private int IntervalsNumber
        {
            get
            {
                var intervalsNumber = 1;
                var timeLeft = RoundLength;
                while ((timeLeft = timeLeft.Subtract(TimePointInterval)) > TimeSpan.Zero)
                    intervalsNumber++;
                return intervalsNumber;
            }
        }

        private int IntervalsLeft
        {
            get
            {
                var intervalsNumber = 1;
                var timeLeft = TimeLeft;
                while ((timeLeft = timeLeft.Subtract(TimePointInterval)) > TimeSpan.Zero)
                    intervalsNumber++;
                return intervalsNumber;
            }
        }

        public float PointsPart { get { return (float)IntervalsLeft / IntervalsNumber; } }

        public bool HasFinished { get { return IsOn && _secondsLeft <= 0; } }

        public void UpdateTime(float deltaTime)
        {
            if (IsOn)
            {
                _secondsLeft -= deltaTime;
                if (_secondsLeft < 0)
                    _secondsLeft = 0;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", TimeLeft.Minutes.ToString("00"), TimeLeft.Seconds.ToString("00"));
        }
    }
}
