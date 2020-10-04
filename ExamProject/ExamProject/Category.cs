using System;

namespace ExamProject {
    class Category {
        public Category() {}

        public Category(string type, Currency currency) {
            Type = type;
            Currency = currency;
        }
        public string Type { get; set; }
        public Currency Currency { get; set; }
    }

}
