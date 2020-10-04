using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExamProject {
    [Serializable]
    class Account {
        public Currency Currency { get; set; }
        private static int IdCounter = 0;
        public int Id { get; set; }
        public Account() {}
        public string Type { get; set; }
        public Account(string type, decimal amount, string curType ) {
            Type = type;
            Currency = new Currency(curType, amount);
            Id = IdCounter++;
        }

    }
}
