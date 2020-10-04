using System;
using System.Reflection.Metadata;

namespace ExamProject {
    class Action {
        public Action() {}

        public Action(ActionType type, Currency currency, string note, string category) {
            Type = type;
            Currency = currency;
            Note = note;
            Category = category;
            Time = DateTime.Now;
        }
        public override string ToString() {
            return $"\t\nAction type: {Type} \nCUrrency: {Currency}\nNote: {Note} \nCategory: {Category} \nTime: {Time}\n";
        }
        public ActionType Type { get; set; }
        public DateTime Time { get; set; }
        public Currency Currency { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
    }
}
