﻿using System;
using System.Collections.Generic;

namespace KiwiBankomaten
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Load database before starting program.
            DataSaver.LoadDataBase(); ;
            RunProgram();
        }
        public static void RunProgram()
        {
            bool loggedIn;
            int userKey = 0;
            int adminKey = -1;

            //looping menu, which will run when the program is started 
            UserInterface.DisplayLogoMessage();
            UserInterface.DisplayMenu(new string[] { "Logga in", "Avsluta" });
            do
            {
                loggedIn = false;
                string choice = UserInterface.PromptForString();
                switch (choice)
                {
                    //Logs in the user
                    case "1":

                        //Logs in the user
                        if (Login.LogIn(out userKey))
                        {
                            UserInterface.DisplayWelcomeMessageLoggedIn(userKey);

                            Utility.PressEnterToContinue();

                            //if login is successful, leads to CustomerMenu() 
                            CustomerMenu(userKey);
                        }
                        else
                        {
                            RunProgram();
                        }
                        break;

                    //Exit program
                    case "2":
                        // Write all changes to files before exiting.
                        foreach (string s in DataSaver.fileNames)
                        {
                            DataSaver.DSaver(s);
                        }
                        //closes the program 
                        Environment.Exit(0);
                        break;

                    // Hidden admin login, which leads to AdminLogIn() and AdminMenu()
                    case "admin":
                        Login.AdminLogIn(out loggedIn, out adminKey);
                        if (loggedIn)
                        {
                            UserInterface.DisplayAdminMessageLoggedIn(adminKey);

                            Utility.PressEnterToContinue();

                            Admin.AdminMenu(adminKey);
                        }
                        else
                        {
                            RunProgram();
                        }
                        break;

                    //If neither of these options are used the defaultmsg is displayed
                    default:
                        UserInterface.CurrentMethodRed("Felaktigt val, försök igen.");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(6);
                        break;
                }
            } while (loggedIn != true);
        }

        public static void LogOut()
        {
            // Makes the program go back to the log in menu
            RunProgram();
        }

        public static void CustomerMenu(int userKey)
        {
            // Looping menu  
            Utility.RemoveLines(14);
            do
            {
                // Creates an instance of the loggedIn user in database
                Customer obj = DataBase.CustomerDict[userKey];

                UserInterface.CurrentMethodMagenta($"{obj.UserName}/CustomerMenu/");

                UserInterface.DisplayMenu(new string[] {"Kontoöversikt", "Visa kontologg",
                    "Överför pengar mellan egna konton", "Öppna nytt konto",
                    "Överför pengar till annan användare", "Låna pengar", "Logga ut"});

                string choice = UserInterface.PromptForString();

                Utility.RemoveLines(13);

                switch (choice)
                {
                    case "1":
                        // Overviews the Accounts and their respective balances
                        UserInterface.CurrentMethodMagenta($"{obj.UserName}/CustomerMenu/" +
                    $"AccountOverview/");
                        obj.BankAccountOverview();
                        obj.LoanAccountOverview();
                        break;
                    case "2":
                        obj.ViewLog();
                        break;
                    case "3":
                        // Transfers a value between two accounts the user possesses
                        obj.TransferBetweenCustomerAccounts();
                        break;
                    case "4":
                        // Opens account for the specific user
                        obj.OpenAccount();
                        break;
                    case "5":
                        // Transfer money to other user in bank
                        obj.InternalMoneyTransfer();
                        break;
                    case "6":
                        obj.LoanMoney();
                        break;
                    case "7":
                        // Logout
                        LogOut();

                        break;

                    default:
                        UserInterface.CurrentMethodRed("Ogiltigt val, ange ett nummer från listan.");
                        break;
                }

                Utility.PressEnterToContinue();
                Console.Clear();
                UserInterface.DisplayLogoMessage();
            } while (true);
        }


    }
}
