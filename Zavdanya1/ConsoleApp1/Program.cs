using System;
using System.Collections.Generic;

class BankAccount
{
    public string Name { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public List<string> Transactions { get; set; } = new List<string>();
    public int Pin { get; set; }

    public BankAccount(string name, string accountNumber, decimal initialBalance, int pin)
    {
        Name = name;
        AccountNumber = accountNumber;
        Balance = initialBalance;
        Pin = pin;
        Transactions.Add($"Створено рахунок з балансом {initialBalance}");
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
        Transactions.Add($"Поповнено на {amount}");
    }

    public void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Transactions.Add($"Знято {amount}");
        }
        else
        {
            Console.WriteLine("Недостатньо коштів.");
        }
    }

    public void Transfer(BankAccount receiver, decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            receiver.Balance += amount;
            Transactions.Add($"Переведено {amount} на рахунок {receiver.AccountNumber}");
            receiver.Transactions.Add($"Отримано {amount} з рахунку {AccountNumber}");
        }
        else
        {
            Console.WriteLine("Недостатньо коштів.");
        }
    }

    public void ViewBalance()
    {
        Console.WriteLine($"Поточний баланс: {Balance}");
    }

    public void ViewTransactions()
    {
        Console.WriteLine("Історія транзакцій:");
        foreach (var transaction in Transactions)
        {
            Console.WriteLine(transaction);
        }
    }
}

class Bank
{
    private Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();

    public void CreateAccount(string name, string accountNumber, decimal initialBalance, int pin)
    {
        if (!accounts.ContainsKey(accountNumber))
        {
            var account = new BankAccount(name, accountNumber, initialBalance, pin);
            accounts.Add(accountNumber, account);
            Console.WriteLine("Рахунок успішно створено.");
        }
        else
        {
            Console.WriteLine("Такий номер рахунку вже існує.");
        }
    }

    public BankAccount GetAccount(string accountNumber, int pin)
    {
        if (accounts.ContainsKey(accountNumber))
        {
            var account = accounts[accountNumber];
            if (account.Pin == pin)
            {
                return account;
            }
            else
            {
                Console.WriteLine("Неправильний PIN.");
            }
        }
        else
        {
            Console.WriteLine("Рахунок не знайдено.");
        }
        return null;
    }

    public void ShowMenu()
    {
        Console.WriteLine("\n1. Створити новий рахунок");
        Console.WriteLine("2. Поповнити рахунок");
        Console.WriteLine("3. Зняти кошти");
        Console.WriteLine("4. Переказати кошти");
        Console.WriteLine("5. Переглянути баланс");
        Console.WriteLine("6. Переглянути історію транзакцій");
        Console.WriteLine("7. Вийти");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Bank bank = new Bank();
        bool running = true;

        while (running)
        {
            bank.ShowMenu();
            Console.Write("\nОберіть дію: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Введіть ваше ім'я: ");
                    string name = Console.ReadLine();
                    Console.Write("Введіть номер рахунку: ");
                    string accountNumber = Console.ReadLine();
                    Console.Write("Введіть початковий баланс: ");
                    decimal initialBalance = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("Введіть PIN: ");
                    int pin = Convert.ToInt32(Console.ReadLine());
                    bank.CreateAccount(name, accountNumber, initialBalance, pin);
                    break;

                case "2":
                    Console.Write("Введіть номер рахунку: ");
                    accountNumber = Console.ReadLine();
                    Console.Write("Введіть PIN: ");
                    pin = Convert.ToInt32(Console.ReadLine());
                    var account = bank.GetAccount(accountNumber, pin);
                    if (account != null)
                    {
                        Console.Write("Введіть суму для поповнення: ");
                        decimal amount = Convert.ToDecimal(Console.ReadLine());
                        account.Deposit(amount);
                        Console.WriteLine("Рахунок успішно поповнено.");
                    }
                    break;

                case "3":
                    Console.Write("Введіть номер рахунку: ");
                    accountNumber = Console.ReadLine();
                    Console.Write("Введіть PIN: ");
                    pin = Convert.ToInt32(Console.ReadLine());
                    account = bank.GetAccount(accountNumber, pin);
                    if (account != null)
                    {
                        Console.Write("Введіть суму для зняття: ");
                        decimal amount = Convert.ToDecimal(Console.ReadLine());
                        account.Withdraw(amount);
                    }
                    break;

                case "4":
                    Console.Write("Введіть номер вашого рахунку: ");
                    string senderAccountNumber = Console.ReadLine();
                    Console.Write("Введіть ваш PIN: ");
                    pin = Convert.ToInt32(Console.ReadLine());
                    var senderAccount = bank.GetAccount(senderAccountNumber, pin);
                    if (senderAccount != null)
                    {
                        Console.Write("Введіть номер рахунку отримувача: ");
                        string receiverAccountNumber = Console.ReadLine();
                        var receiverAccount = bank.GetAccount(receiverAccountNumber, 0);
                        if (receiverAccount != null)
                        {
                            Console.Write("Введіть суму для переказу: ");
                            decimal amount = Convert.ToDecimal(Console.ReadLine());
                            senderAccount.Transfer(receiverAccount, amount);
                            Console.WriteLine("Переказ коштів успішно виконано.");
                        }
                    }
                    break;

                case "5":
                    Console.Write("Введіть номер рахунку: ");
                    accountNumber = Console.ReadLine();
                    Console.Write("Введіть PIN: ");
                    pin = Convert.ToInt32(Console.ReadLine());
                    account = bank.GetAccount(accountNumber, pin);
                    if (account != null)
                    {
                        account.ViewBalance();
                    }
                    break;

                case "6":
                    Console.Write("Введіть номер рахунку: ");
                    accountNumber = Console.ReadLine();
                    Console.Write("Введіть PIN: ");
                    pin = Convert.ToInt32(Console.ReadLine());
                    account = bank.GetAccount(accountNumber, pin);
                    if (account != null)
                    {
                        account.ViewTransactions();
                    }
                    break;

                case "7":
                    running = false;
                    Console.WriteLine("До побачення!");
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}
