using System;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.IO;

namespace ExamProject {
    class Application {


        private static Worker<Account> accountWorker = new Worker<Account>("Accounts.json");
        private static Worker<Action> actionWorker = null;
        private static Account currentAcc;
        private static bool flag = true;
        private static bool hasCurrentAccount()=>currentAcc != null;
        private static string CreateAccountWorkerTemplateFileName(int id) => $"Acc_{id}";

        private static void DrawMainMenu() {
            Console.Clear();
            Console.WriteLine("1.Create account");
            Console.WriteLine("2.Exit");
            var choose = Console.ReadKey(true).Key;
            switch (choose) {
                case ConsoleKey.D1: {
                        Console.Clear();
                        AddAccount();
                        Console.Clear();
                        SwitchAccount();
                        break;
                    }
                case ConsoleKey.D2: {
                        flag = false;
                        return;
                    }
            }
        }
        private static void DrawFullMenu() {
            Console.Clear();

            Console.WriteLine("1.Income");
            Console.WriteLine("2.Expense");
            Console.WriteLine("3.Options");
            Console.WriteLine("4.Exit");
            var choose = Console.ReadKey(true).Key;
            switch (choose) {
                case ConsoleKey.D1: {
                        Console.Clear();
                        Console.WriteLine("Enter Amount: ");
                        decimal accAmount = 0;
                        try {
                             accAmount = decimal.Parse(Console.ReadLine());
                        } catch {
                            Console.WriteLine("Invalid format!");
                            break;
                        }

                        Console.WriteLine("Note: ");
                        var accNote = Console.ReadLine();
                        Console.WriteLine("Category type:  ");
                        var accCategoryType = Console.ReadLine();
                        Console.WriteLine("Currency type:  ");
                        var accCurType = Console.ReadLine().ToUpper();
                        var resultAmount = accAmount;
                        if (accCurType == currentAcc.Currency.Type) {
                            currentAcc.Currency.Amount += accAmount;
                        } else {
                        resultAmount = Currency.Convert(accCurType, currentAcc.Currency.Type, accAmount).Amount;
                            currentAcc.Currency.Amount += resultAmount;
                            
                        }

                        
                        actionWorker.Data.Add(new Action(ActionType.Income, new Currency(currentAcc.Currency.Type, resultAmount), accNote, accCategoryType));
                        accountWorker.Save();
                        actionWorker.Save();
                        break;
                    }
                case ConsoleKey.D2: {
                        Console.Clear();
                        Console.WriteLine("Enter Amount: ");
                        decimal accAmount = 0;
                        try {
                            accAmount = decimal.Parse(Console.ReadLine());
                        } catch {
                            Console.WriteLine("Invalid format!");
                            break;
                        }

                        Console.WriteLine("Note: ");
                        var accNote = Console.ReadLine();
                        Console.WriteLine("Category type:  ");
                        var accCategoryType = Console.ReadLine();
                        Console.WriteLine("Currency type:  ");
                        var accCurType = Console.ReadLine().ToUpper();
                        var resultAmount = accAmount;
                        if (accCurType == currentAcc.Currency.Type) {
                            currentAcc.Currency.Amount -= accAmount;
                        } else {
                            resultAmount = Currency.Convert(accCurType, currentAcc.Currency.Type, accAmount).Amount;
                            currentAcc.Currency.Amount -= resultAmount;

                        }
                        actionWorker.Data.Add(new Action(ActionType.Expense, new Currency(currentAcc.Currency.Type, resultAmount), accNote, accCategoryType));
                        accountWorker.Save();
                        actionWorker.Save();
                        break;
                    }
                case ConsoleKey.D3: {
                        Console.Clear();
                        Console.WriteLine("1.Add account");
                        Console.WriteLine("2.Remove account");
                        Console.WriteLine("3.Switch account");
                        Console.WriteLine("4.Show account History");
                        Console.WriteLine("5.Show account statistics");

                        var opChoose = Console.ReadKey(true).Key;
                        switch (opChoose) {
                            case ConsoleKey.D1: {
                                    Console.Clear();
                                    AddAccount();
                                    break;
                                }
                            case ConsoleKey.D2: {
                                    Console.Clear();
                                    RemoveAccount();
                                    break;
                                }
                            case ConsoleKey.D3: {
                                    Console.Clear();
                                    SwitchAccount();
                                    break;
                                }
                            case ConsoleKey.D4: {
                                    Console.Clear();
                                    Console.WriteLine("1.Show all story");
                                    Console.WriteLine("2.Show story filtered by category");
                                    var ch = Console.ReadKey(true).Key;
                                    switch (ch) {
                                        case ConsoleKey.D1: {
                                                Console.Clear();
                                                ShowHistory();
                                                break;
                                            }
                                        case ConsoleKey.D2: {
                                                Console.Clear();
                                                ShowHistoryByCategory();
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case ConsoleKey.D5: {
                                    Console.Clear();
                                    ShowAccountStatistics();
                                    Console.ReadKey(true);
                                    break;
                                }
                            case ConsoleKey.Escape: {
                                    return;
                                }
                        }
                        break;
                    }
                case ConsoleKey.D4: {
                        flag = false;
                        return;
                    }
                case ConsoleKey.Escape: {
                        flag = false;
                        return;
                    }
                default:
                    break;
            }
        }

        public static void Run() {
            accountWorker.Load();
            if (accountWorker.Data.Count == 0) {
                currentAcc = null;
            } else {
                currentAcc = accountWorker.Data[0];
                actionWorker = new Worker<Action>(CreateAccountWorkerTemplateFileName(currentAcc.Id));
                actionWorker.Load();
            }
            do {
                if (hasCurrentAccount()) {
                    DrawFullMenu();
                } else {
                    DrawMainMenu();

                }
            } while (flag);


        }


        private static void SwitchAccount() {
            
            Console.WriteLine("Account list: ");
            foreach (var item in accountWorker.Data) {
                Console.WriteLine($"{item.Type} {item.Currency.Type} {item.Currency.Amount} ");
            }
            Console.WriteLine("Select account Type: ");
            var type = Console.ReadLine();
            var result = accountWorker.Data.First(x => x.Type == type);
            if (result == null) {
                Console.WriteLine("Account does not exist!");
                return;
            }
            currentAcc = result;
            accountWorker = new Worker<Account>(CreateAccountWorkerTemplateFileName(currentAcc.Id));
            accountWorker.Load();


        }
        private static void AddAccount() {
            Console.WriteLine("Enter account type: ");
            var accType = Console.ReadLine();
            if (accountWorker.Data.Exists(x => x.Type == accType)) {
                Console.WriteLine("This type is already exists!");
                return;
            }
            Console.WriteLine("Enter Amount: ");
            var accAmount = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter currency type: ");
            var accCurType = Console.ReadLine();
            accountWorker.Data.Add(new Account(accType, accAmount, accCurType.ToUpper()));
            accountWorker.Save();
        }
        private static void RemoveAccount() {
            Console.WriteLine("Enter account type: ");
            var accType = Console.ReadLine();
            var result = accountWorker.Data.FirstOrDefault(x => x.Type == accType);
            if (result == null) {
                Console.WriteLine("This type does not exist!");
                return;
            }
            accountWorker.Data.Remove(result);
            if (currentAcc.Type == accType) {
                if (accountWorker.Data.Count >= 1) {
                    currentAcc = accountWorker.Data[0];
                    actionWorker.Load();
                } else {
                    currentAcc = null;
                }
            } 

            accountWorker.Save();
            File.Delete(CreateAccountWorkerTemplateFileName(result.Id));
        }
        private static void ShowAccountStatistics() {

            Console.WriteLine("\t\tStart date: ");
            Console.Write("Enter year --> ");
            var beginYear = int.Parse(Console.ReadLine());
            Console.Write("Enter month --> ");
            var beginMonth = int.Parse(Console.ReadLine());
            Console.Write("Enter day --> ");
            var beginDay = int.Parse(Console.ReadLine());

            var begin = new DateTime(beginYear, beginMonth, beginDay);
            Console.WriteLine("\t\t\n\nEnd date: \n");
            Console.Write("Enter year --> ");
            var endYear = int.Parse(Console.ReadLine());
            Console.Write("\nEnter month --> ");
            var endMonth = int.Parse(Console.ReadLine());
            Console.Write("\nEnter day --> ");
            var endDay = int.Parse(Console.ReadLine());

            var end = new DateTime(endYear, endMonth, endDay);

            var historyRange = new HistoryRange(actionWorker);
            
            var group = historyRange.Interval(begin, end);
            var filter = group.Where(x => x.Type == ActionType.Expense);
            var sumResult = filter.Sum(x => x.Currency.Amount);


            var linqResult = filter.GroupBy(x => x.Category);
            foreach (var item in linqResult) {
                var sum = item.Sum(x => x.Currency.Amount);
                var res = (sum / sumResult) * 100;
                Console.WriteLine($"\n\t{item.Key} {decimal.Round(res,3)}%");
            }

        }
        private static void ShowHistory() {
            Console.Clear();
            Console.WriteLine("1.For Day");
            Console.WriteLine("2.For Week");
            Console.WriteLine("3.For Month");
            Console.WriteLine("4.For Year");
            HistoryRange range = new HistoryRange(actionWorker);
            var chooseHistory = Console.ReadKey(true).Key;
            switch (chooseHistory) {
                case ConsoleKey.D1: {
                        foreach (var item in range.Day()) {
                            Console.WriteLine(item);
                        }
                        break;
                    }
                case ConsoleKey.D2: {
                        foreach (var item in range.Week()) {
                            Console.WriteLine(item);
                        }
                        break;
                    }
                case ConsoleKey.D3: {
                        foreach (var item in range.Month()) {
                            Console.WriteLine(item);
                        }
                        break;
                    }
                case ConsoleKey.D4: {
                        foreach (var item in range.Year()) {
                            Console.WriteLine(item);
                        }
                        break;
                    }
            }
        }
        private static void ShowHistoryByCategory() {
            Console.WriteLine("\t\tStart date: ");
            Console.Write("Enter year --> ");
            var beginYear = int.Parse(Console.ReadLine());
            Console.Write("Enter month --> ");
            var beginMonth = int.Parse(Console.ReadLine());
            Console.Write("Enter day --> ");
            var beginDay = int.Parse(Console.ReadLine());

            var begin = new DateTime(beginYear, beginMonth, beginDay);
            Console.WriteLine("\t\t\n\nEnd date: \n");
            Console.Write("Enter year --> ");
            var endYear = int.Parse(Console.ReadLine());
            Console.Write("\nEnter month --> ");
            var endMonth = int.Parse(Console.ReadLine());
            Console.Write("\nEnter day --> ");
            var endDay = int.Parse(Console.ReadLine());

            var end = new DateTime(endYear, endMonth, endDay);

            var historyRange = new HistoryRange(actionWorker);

            var group = historyRange.Interval(begin, end);

            var linqResult = group.GroupBy(x => x.Category);
            foreach (var item in linqResult) {
                var sum = item.Sum(x => x.Currency.Amount);
                Console.WriteLine(item.Key);
                foreach (var i in item) {
                    Console.WriteLine(i);
                }
            }
        }

    }
}
