using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static System.Convert;

namespace WPFClinica.Scripts {

    class TimedEvent {

        DispatcherTimer T;
        Action Event;

        public readonly int TotalCount;
        public int Count {
            get;
            private set;
        }
        readonly bool StopA;

        public TimedEvent(int update, int timer, bool stop, Action action) {
            TotalCount = Count = timer;
            StopA = stop;
            Event = action ?? throw new ArgumentNullException();
            T = new DispatcherTimer {
                Interval = new TimeSpan(0, 0, 0, 0, update * 750)
            };
            T.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (--Count <= 0) {
                Event();
                Count = TotalCount;
                if (StopA) T.Stop();
            }
        }

        public void TryTigger () {
            Count = TotalCount;
            T.Start();
        }

        public void TriggerNow () => Event();

        public void Stop () => T.Stop();

        public void Start() {
            Event();
            T.Start();
        }
    }
}