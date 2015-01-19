using System;

namespace Assets.Sources.Scripts.GameRoom
{
    public class DrawingTimer
    {
        private static readonly TimeSpan RoundLength = new TimeSpan(0, 0, 3, 0);
        private static readonly TimeSpan TimePointInterval = new TimeSpan(0, 0, 1, 0);
        private TimeSpan _timeLeft = RoundLength;

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
                var timeLeft = _timeLeft;
                while ((timeLeft = timeLeft.Subtract(TimePointInterval)) > TimeSpan.Zero)
                    intervalsNumber++;
                return intervalsNumber;
            }
        }

        public float PointsPart { get { return (float)IntervalsLeft / IntervalsNumber; } }

        public bool HasFinished { get { return IsOn && _timeLeft <= TimeSpan.Zero; } }

        public void Tick()
        {
            if (IsOn)
            {
                _timeLeft = _timeLeft.Subtract(new TimeSpan(0, 0, 0, 1));
                if (_timeLeft < TimeSpan.Zero)
                    _timeLeft = TimeSpan.Zero;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", _timeLeft.Minutes, _timeLeft.Seconds);
        }
    }
}
