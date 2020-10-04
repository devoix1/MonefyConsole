using System;
using System.Collections.Generic;

namespace ExamProject {
    class HistoryRange {
        private readonly Worker<Action> worker;

        public HistoryRange(Worker<Action> worker) {
            this.worker = worker;
        }

        public HistoryRangeTotal Interval(DateTime begin, DateTime end) {
            List<Action> actions = new List<Action>();
            Currency currency = new Currency(worker.Data[0].Currency.Type);
            foreach (var item in worker.Data) {
                if (begin <= item.Time && end >= item.Time) {
                    actions.Add(item);
                    if (item.Type == ActionType.Expense) {
                        currency.Amount -= item.Currency.Amount;
                    } else {
                        currency.Amount += item.Currency.Amount;
                    }
                }
            }
            return new HistoryRangeTotal(begin, end, currency, actions);
        }

        public HistoryRangeTotal Day() {
            DateTime now = DateTime.Now;
            var begin = now.Subtract(now.TimeOfDay);
            return Interval(begin, now);
        }

        public HistoryRangeTotal Week() {
            int temp = default;
            switch (DateTime.Today.DayOfWeek) {
                case DayOfWeek.Sunday:
                    temp = 6;
                    break;
                case DayOfWeek.Monday:
                    temp = 0;
                    break;
                case DayOfWeek.Tuesday:
                    temp = 1;
                    break;
                case DayOfWeek.Wednesday:
                    temp = 2;
                    break;
                case DayOfWeek.Thursday:
                    temp = 3;
                    break;
                case DayOfWeek.Friday:
                    temp = 4;
                    break;
                case DayOfWeek.Saturday:
                    temp = 5;
                    break;
            }
            DateTime now = DateTime.Now;


            var begin = now.Subtract(new TimeSpan(temp, now.Hour, now.Minute, now.Second));
            return Interval(begin, now);
        }
        public HistoryRangeTotal Month() {
            DateTime now = DateTime.Now;
            var begin = new DateTime(now.Year, now.Month, 1);
            return Interval(begin, now);
        }
        public HistoryRangeTotal Year() {
            var now = DateTime.Now;
            DateTime begin = new DateTime(now.Year, 1, 1);
            return Interval(begin, now);


        }

    }
}
