using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace ExamProject {

    class Currency {
        public Currency() {

        }
        public Currency(string type) : this(type, 0) { }
        public Currency(string type, decimal amount) {
            Amount = amount;
            Type = type;
        }
        public override string ToString() {
            return $"{Amount} {Type}";
        }


        public string Type { get; set; }
        public decimal Amount { get; set; }

        internal static Currency Convert(string from, string to, decimal amount = default) {

            return Convert(new Currency(from), new Currency(to), amount);
        }

        private const string path = "currencies.csv";

        public static Currency Convert(Currency from, Currency to, decimal? amount = null) {
            var content = File.ReadAllLines(path);
            var fromCurrencies = content[0].Split(',');
            int FromIndx = -1;
            int ToIndx = -1;
            // From Currency Search(first row search)
            for (int i = 0; i < fromCurrencies.Length; i++) {
                if (fromCurrencies[i] == from.Type) {
                    FromIndx = i;
                    break;
                }
            }
            // To Currency Search(all rows search)
            for (int i = 1; i < content.Length; i++) {
                var toCurrencies = content[i].Split(',');
                if (toCurrencies[0] == to.Type) {
                    ToIndx = i;
                }
            }
            if (FromIndx == -1 || ToIndx == -1) {
                throw new Exception("Currency does not exist!");
            }

            var exchangeRate = System.Convert.ToDecimal(content[ToIndx].Split(',')[FromIndx]);
            if (amount == null) {
                return new Currency(to.Type, exchangeRate * from.Amount);
            }
            return new Currency(to.Type, exchangeRate * (decimal)amount);
        }
    }
}
