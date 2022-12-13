﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBankomaten
{
    internal class Admin : User
    {
        // Used for creating test admins.
        public Admin(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
        }
        // Use this when creating admins in program.
        public Admin(string username, string password)
        {
            if (DataBase.AdminList == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.AdminList.Count;
                Id = newId;
            }
            UserName = username;
            Password = password;
        }
        // Admin method for creating new users.
        public static void CreateNewUser() 
        {
            // Used in the do-while loop to repeat if any input errors are detected.
            bool error; 
            do
            {
                int userType;
                string userName = "";
                string passWord = "";
                error = false;
                Console.Clear();

                UserInterface.DisplayMessage("Vilken sorts användare vill du skapa?");
                UserInterface.DisplayMenu(new string[] {"Customer", "Admin"});

                while (UserInterface.PromptForInt(out userType) != 1 && userType != 2)
                {
                    UserInterface.DisplayMessage("Fel input, skriv in ett korrekt värde");
                    Utility.PressEnterToContinue();
                }

                userName = UserInterface.QuestionForString("Vilket användarnamn ska den nya användaren ha?");

                passWord = UserInterface.QuestionForString("Vilket lösenord ska den nya användaren ha?");

                Console.Clear();

                switch (userType)
                {
                    // Adds customer account to CustomerDict with name and
                    // password set from user input.
                    case 1: 
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, 
                            new Customer(userName, passWord));
                        UserInterface.DisplayMessage($"Customer {userName} har " +
                            $"skapats med nyckeln {DataBase.CustomerDict.Last().Key}.");
                        break;
                    // Adds admin account to AdminList with name and password set from user input.
                    case 2: 
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        UserInterface.DisplayMessage($"Admin {userName} har skapats.");
                        break;
                    // Loop repeats and switch is run again if none of the correct values are chosen.
                    default:
                        error = true;
                        break;
                }
            } while (error == true);
        }
        // Menu where admin can select different functions.
        public static void AdminMenu(int adminKey) 
        {
            // Used to log admin out if set to false.
            bool loggedIn = true;

            Utility.RemoveLines(16);

            // Loop that runs so long as the admin has not chosen to log out.
            while (loggedIn == true) 
            {
                UserInterface.CurrentMethod($"{DataBase.AdminList[adminKey].UserName}/AdminMenu/");
                UserInterface.DisplayMenu(new string[] { "Skapa ny användare", 
                    "Uppdatera växlingskurs","Visa alla användare", 
                    "Redigera ett användarkonto",
                    "Uppdatera bankkontotyper","Logga ut" });
                switch (UserInterface.PromptForString())
                {
                    case "1":
                        CreateNewUser();
                        break;
                    // Shows list of exchange rates with their values and asks
                    // if admin wants to change them.
                    case "2": 
                        UpdateExchangeRate();
                        break;
                    case "3": 
                        Console.Clear(); 
                        ViewAllUsers();
                        Utility.PressEnterToContinue();
                        break;
                    case "4": 
                        Console.Clear();
                        EditUserAccount();
                        break;
                    case "5": 
                        UpdateAccountTypes();
                        Utility.PressEnterToContinue();
                        break;
                    case "6": 
                        loggedIn = false;
                        Console.Clear();
                        break;
                    // Loop repeats and switch is run again if none of the
                    // correct values are chosen.
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(5);
                        break;
                }
            }
            // Program.LogOut is called outside the loop and switch because
            // of possible bugs if it were to be called inside it.
            Program.LogOut(); 
        }
        // Method for printing out all currencies and their exchange rates.
        public static void ListExchangeRates()
        {
            foreach (KeyValuePair<string, decimal> exchangeRates in DataBase.ExchangeRates)
            {
                Console.WriteLine($"{exchangeRates.Key} {exchangeRates.Value}");
            }
        }
        // Method for updating a currency's exchange rate.
        public static void UpdateExchangeRate()
        {
            while (true)
            {
                // Is used to ensure the new value of the exchange rate is valid.
                bool noError;
                // Is used to make sure user answers with J/N when confirming options.
                string answer;
                // The new value of the exchange rate.
                decimal newValue;
                // Used as key to get currency from database.
                string currency;
                // Loops until admin answers whether they want to update a value or not.
                do
                {
                    Console.Clear();
                    ListExchangeRates();
                    Console.WriteLine("Vill du ändra växlingskursen på någon " +
                        "valuta? J/N");
                    answer = Console.ReadLine().ToUpper();
                } while (answer != "J" && answer != "N");
                if (answer == "J")
                {
                    // If admin does want to update a value, loops until they
                    // input a valid currency.
                    do
                    {
                        Console.Clear();
                        ListExchangeRates();
                        Console.WriteLine("Var vänlig skriv in den valuta du " +
                            "vill ändra. SEK, USD, EUR, etc ");
                        currency = Console.ReadLine().ToUpper();
                        if (!DataBase.ExchangeRates.ContainsKey(currency))
                        {
                            Console.WriteLine("Ogiltigt värde, skriv in en valuta");
                            Utility.PressEnterToContinue();
                        }
                    } while (!DataBase.ExchangeRates.ContainsKey(currency));
                    // Loops until admin inputs a valid positive number´as the new exchange rate.
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Växlingskursen för {currency} - " +
                            $"{DataBase.ExchangeRates[currency]}");
                        Console.WriteLine("Var vänlig skriv in den nya " +
                            "växlingskursen för valutan");
                        noError = Decimal.TryParse(Console.ReadLine(), out newValue);
                        if (noError == false || newValue < 0)
                        {
                            Console.WriteLine("Ogiltigt värde, mata in en positiv siffra");
                            Utility.PressEnterToContinue();
                        }
                    } while (noError == false || newValue < 0);
                    // Loops until admin confirms whether or not they want to
                    // apply the changes to the exchange rate.
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Växlingskursen för {currency} " +
                            $"kommer ändras till {newValue}. Godkänner du detta? J/N");
                        answer = Console.ReadLine().ToUpper();
                    } while (answer != "J" && answer != "N");
                    // If admin answers yes, exchange rate is updated.
                    if (answer == "J")
                    {
                        DataBase.ExchangeRates[currency] = newValue;
                        Console.WriteLine($"Växlingskursen för {currency} " +
                            $"har ändrats till {newValue}");
                        Utility.PressEnterToContinue();
                    }
                }
                else
                {
                    return;
                }
            }

        }
        // Method for printing out all customer accounts with their ID,
        // Username, Password and IsLocked status.
        public static void ViewAllUsers()
        {
            Console.WriteLine("---Alla användarkonton i Kiwibank---");
            foreach (KeyValuePair<int, Customer> customer in DataBase.CustomerDict)
            {
                Console.WriteLine($"ID:{customer.Value.Id} - " +
                    $"Användarnamn:{customer.Value.UserName} - " +
                    $"Lösenord:{customer.Value.Password} - " +
                    $"Spärrat:{customer.Value.Locked}");
            }
        }
        // Method for selecting one specific user account and then giving
        // the admin a choice of what to do with the account.
        public static void EditUserAccount()
        {
            bool noError;
            do
            {
                Console.Clear();
                // Prints all user accounts so admin can see which accounts
                // they can select.
                ViewAllUsers();
                Console.WriteLine("Vilken användare vill du välja? Välj " +
                    "genom att skriva in ID");
                // Admin inputs user ID here to select which account
                // they want to edit.
                noError = Int32.TryParse(Console.ReadLine(), out int userID);
                // Checks if admin input is a number and if that number exists
                // as a key in the customer dictionary.
                if (noError && DataBase.CustomerDict.ContainsKey(userID))
                {
                    // Chosen customer account is saved for access to non-static
                    // methods and easier property access.
                    Customer selectedUser = DataBase.CustomerDict[userID];
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Du har valt användaren ID:" +
                            $"{selectedUser.Id} - Användarnamn:" +
                            $"{selectedUser.UserName}" +
                            $" - Lösenord:{selectedUser.Password}" +
                            $" - Spärrad:{selectedUser.Locked}");
                        Console.WriteLine("Vad vill du göra med användaren?\n-1 " +
                            "Visa bankkonton\n-2 Spärra/Avspärra" +
                            "\n-3 Ändra lösenord\n-4 Återvänd till adminmenyn");
                        switch (Console.ReadLine())
                        {
                            // Prints all user's bank accounts.
                            case "1":
                                selectedUser.BankAccountOverview();
                                selectedUser.LoanAccountOverview();
                                Utility.PressEnterToContinue();
                                break;
                            // Lets admin lock or unlock user's account.
                            case "2":
                                LockOrUnlockAccount(selectedUser);
                                break;
                            // Lets admin change password of user's account.
                            case "3":
                                ChangeUserPassword(selectedUser);
                                break;
                            // Returns admin to the main admin menu.
                            case "4":
                                return;
                            default:
                                Console.WriteLine("Fel input, skriv in ett korrekt värde");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig användare vald, skriv in ett giltigt ID");
                    noError = false;
                }
            } while (noError == false);
        }
        // Method for locking user account if it is not locked, or unlocking
        // user account if it is locked.
        public static void LockOrUnlockAccount(Customer selectedUser)
        {
            string answer;
            // Checks if user account is locked or not, asks different question
            // depending on status.
            if (selectedUser.Locked)
            {
                Console.WriteLine("Kontot är spärrat");
                Console.WriteLine("Vill du avspärra kontot? J/N");
            }
            else
            {
                Console.WriteLine("Kontot är inte spärrat");
                Console.WriteLine("Vill du spärra kontot? J/N");
            }
            do
            {
                answer = Console.ReadLine().ToUpper();
                // If admin confirms that they want to lock/unlock user then we do so.
                switch (answer)
                {
                    case "J":
                        if (selectedUser.Locked)
                        {
                            selectedUser.Locked = false;
                        }
                        else
                        {
                            selectedUser.Locked = true;
                        }
                        break;
                    case "N":
                        break;
                    default:
                        Console.WriteLine("Fel input, välj antingen J/N");
                        break;
                }
            } while (answer != "J" && answer != "N");

        }
        // Method for changing user account's password.
        public static void ChangeUserPassword(Customer selectedUser)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Vill du ändra lösenordet till användaren " +
                    $"{selectedUser.Id} - {selectedUser.UserName}? J/N");
                if (Console.ReadLine().ToUpper() == "J")
                {
                    Console.Clear();
                    Console.WriteLine($"Skriv in det nya lösenordet till " +
                        $"användaren {selectedUser.UserName}");
                    string newPassWord = Console.ReadLine();
                    Console.WriteLine("Konfirmera användarens lösenord genom " +
                        "att skriva in det igen");
                    // Makes user input the password again to confirm that it
                    // is what they want.
                    if (Console.ReadLine() == newPassWord)
                    {
                        selectedUser.Password = newPassWord;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Du konfirmerade inte lösenordet");
                        Utility.PressEnterToContinue();
                    }
                }
                else { return; }
            }

        }
        public static void ViewAccountTypes(int selection)
        {
            int i = 1;
            switch (selection)
            {
                case 1: 
                    foreach (KeyValuePair<string, decimal> type in DataBase.BankAccountTypes)
                    {
                        Console.WriteLine($"-{i} {type.Key} - {type.Value}");
                        i++;
                    }
                    break;
                case 2:
                    foreach (KeyValuePair<string, decimal> type in DataBase.LoanAccountTypes)
                    {
                        Console.WriteLine($"-{i} {type.Key} - {type.Value}");
                        i++;
                    }
                    break;
                default:
                    Console.WriteLine("Ogiltigt värde, det här borde inte kunna hända. Kontakta en admin.");
                    break;
            }
        }
        public static void UpdateAccountTypes()
        {
            string answer;
            do
            {
                Console.Clear();
                Console.WriteLine("Olika typer av bankkonto, namn och ränta:");
                ViewAccountTypes(1);
                Console.WriteLine("---------------------------");
                Console.WriteLine("Olika typer av lånekonto, namn och ränta:");
                ViewAccountTypes(2);
                Console.WriteLine("---------------------------");
                Console.WriteLine("Vilket typ av konto vill du ändra?\n-1 Bankkonto" +
                    "\n-2 Lånekonto\n-3 Återvänd till adminmenyn");
                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        UpdateBankAccountTypes();
                        break;
                }
            } while (answer != "1" && answer != "2" && answer != "3");
        }
        public static void UpdateBankAccountTypes()
        {

        }
    }
}
