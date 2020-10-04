using System;
using System.Collections;
using System.Collections.Generic;

namespace ExamProject {
    class HistoryRangeTotal : IEnumerable<Action> {
        public readonly DateTime Begin;
        public readonly DateTime End;
        public readonly Currency Currency;
        public readonly List<Action> Actions;

        public HistoryRangeTotal(DateTime begin, DateTime end, Currency currency, List<Action> actions) {
            Begin = begin;
            End = end;
            Currency = currency;
            Actions = actions;
        }

        public IEnumerator<Action> GetEnumerator() {
            foreach (var item in Actions) {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
