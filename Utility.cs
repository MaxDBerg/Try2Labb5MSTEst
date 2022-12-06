﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KiwiBankomaten
{
    internal class Utility
    {
        public static bool CheckPassWord(int userKey, int tries)
        {
            if (tries == 3) //Checks to see if user failed login trice, before requesting Password again
            {
                DataBase.CustomerDict[userKey].Locked = true;//locks user if 3 fails occur
            }
            //if the user is locked, message is displayed and user is returned to mainmenu
            if (DataBase.CustomerDict[userKey].Locked == true)
            {
                UserInterface.DisplayMessage("Du har angett fel lösenord 3 gånger.\nDitt konto är låst\nKontakta admin");
                PressEnterToContinue();
                Console.Clear();
                Program.RunProgram();
                return false; //Returns to Login
            }
            
            string userPassWord = UserInterface.PromptForString("Enter your password"); // lets user enter password

            //if the user is locked, message is displayed and user is returned to mainmenu
            if (userPassWord == DataBase.CustomerDict[userKey].Password)
            {
                Console.WriteLine("Password is correct");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password"); //if wrong password is entered 
                tries++;  //"tries"adds with one, and user is returned CheckPassWord again
                CheckPassWord(userKey, tries);
                return false;
            }
        }
        public static bool AdminCheckPassWord(int adminKey)
        {
            Console.WriteLine("Enter your password");
            string userPassWord = (Console.ReadLine());

            if (userPassWord == DataBase.AdminList[adminKey].Password)
            {
                Console.WriteLine("Password is correct");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password");
                return false;
            }
        }

        //Method to separete commas in amount
        public static string AmountDecimal(decimal valueDec)
        {
            //the 0 is a placeholder, which shows even if the value is 0 
            string stringDec = valueDec.ToString("#,##0.00");
            return stringDec;
        }

        // Stops the program until the user presses "Enter"
        public static void PressEnterToContinue()
        {
            UserInterface.DisplayMessage("Klicka Enter för att fortsätta.");
            // Gets the input from the user
            ConsoleKey enterPressed = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (!Console.KeyAvailable && enterPressed != ConsoleKey.Enter)
            {
                enterPressed = Console.ReadKey(true).Key;
            }
        }

        // Choose to continue or go back to main menu.
        public static bool ContinueOrAbort()
        {

            UserInterface.DisplayMessage("Klicka Enter för att försöka igen eller Esc för" +
                " att återgå till huvudmenyn.");
            // Gets the input from the user
            ConsoleKey userInput = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (userInput != ConsoleKey.Enter && userInput != ConsoleKey.Escape)
            {
                Console.WriteLine("Felaktig inmatning, välj Enter för att försöka " +
                    "igen eller Esc för att återgå till huvudmenyn.");
                userInput = Console.ReadKey(true).Key;
            }
            if (userInput == ConsoleKey.Escape)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void RemoveLastThreeLines()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
