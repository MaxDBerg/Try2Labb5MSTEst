using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    public class Login
    {
        //Checks if user exist, returns userKey
        public static bool LogIn(out int userKey)
        {
            Utility.RemoveLines(6);
            for (int i = 0; i < 3; i++)
            {
                string username = GetUsernameFromUser();
                string password = GetPasswordFromUser();
                bool loggedIn = LoginLogic(out userKey, username, password);
                if (loggedIn)
                {
                    return loggedIn;
                }
                else
                {
                    UserInterface.CurrentMethodRed($"Fel Användarnamn eller Lösenord. Du har nu {2 - i} försök kvar, vänligen försök igen");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(8);
                }
            }
            userKey = 0;
            return false;
        }

        public static bool LoginLogic(out int userKey, string username, string password)
        {
            // loop through customer dictionary to search for username
            foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
            {
                if (username == item.Value.UserName)
                {
                    userKey = item.Key;
                    if (password == item.Value.Password)
                    {
                        return true;
                    }
                }
            }

            userKey = 0;
            return false;
        }

        public static void AdminLogIn(out bool loggedIn, out int adminKey)
        {
            adminKey = 0;
            loggedIn = false;
            Utility.RemoveLines(6);
            UserInterface.DisplayAdminMessage();

            for (int i = 3 - 1; i >= 0; i--)
            {
                string userName = UserInterface.QuestionForString("Ange ditt " +
                    "Användarnamn", "Namn").Trim();

                // loop through customer dictionary to search for userName
                foreach (Admin item in DataBase.AdminList)
                {
                    if (userName == item.UserName)
                    {
                        adminKey = DataBase.AdminList.FindIndex(item =>
                            userName == item.UserName);

                        loggedIn = CheckAdminPassWord(adminKey);

                        if (loggedIn) { return; }
                    }
                }
                if (!(i <= 0))
                {
                    UserInterface.CurrentMethodRed($"Fel Användarnamn. Du har nu {i} försök kvar, vänligen försök igen");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(8);
                }
                else
                {
                    UserInterface.CurrentMethodRed("Fel Användarnamn. Ingen användare med det namnet hittades.");
                    Utility.PressEnterToContinue();
                }
            }
        }

        public static string GetUsernameFromUser()
        {
            return UserInterface.QuestionForString("Ange ditt " +
                    "Användarnamn", "Namn").Trim();
        }
        public static string GetPasswordFromUser()
        {
            return UserInterface.QuestionForString("Ange ditt " +
                    "Lösenord", "Lösen").Trim();
        }

        public static bool CheckAdminPassWord(int adminKey)
        {
            //if the user is locked, message is displayed and user is returned to mainmenu

            for (int i = 3 - 1; i >= 0; i--)
            {

                if (UserInterface.QuestionForString("Ange ditt Lösenord", "Lösenord") == DataBase.AdminList[adminKey].Password)
                {
                    UserInterface.CurrentMethodGreen("Rätt Lösenord. Du loggas nu in");
                    return true;
                }
                else
                {
                    if (!(i == 0))
                    {
                        UserInterface.CurrentMethodRed($"Fel Lösenord. Du har nu {i} försök kvar, vänligen försök igen");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(8);
                    }
                    else
                    {
                        UserInterface.CurrentMethodRed("Fel Lösenord. Ingen användare med det lösenordet hittades.");
                        Utility.PressEnterToContinue();
                    }
                }
            }
            Program.RunProgram();
            return false;
        }

    }
}
