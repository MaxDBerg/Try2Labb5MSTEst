using System;

namespace KiwiBankomaten
{
    public class Utility
    {
        

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
            Console.WriteLine(" +-----------------------------------------------------------------------------------+");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  Klicka Enter för att fortsätta.");
            Console.ForegroundColor = ConsoleColor.White;
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

            UserInterface.DisplayMessage(" Klicka Enter för att försöka igen eller Esc för" +
                " att återgå till huvudmenyn.");
            // Gets the input from the user
            ConsoleKey userInput = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (userInput != ConsoleKey.Enter && userInput != ConsoleKey.Escape)
            {
                UserInterface.CurrentMethodRed("Felaktig inmatning, välj Enter för att försöka " +
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

        public static bool YesOrNo(string input, string input2)
        {
            do
            {
                UserInterface.CurrentMethodGreen(input);
                switch (UserInterface.QuestionForString(input2).ToUpper())
                {
                    case "J":
                        return false;
                    case "N":
                        return true;
                    default:
                        UserInterface.CurrentMethodRed("Felaktig inmatning", "Välj [J] för ja eller N för nej.");
                        PressEnterToContinue();
                        RemoveLines(11);
                        break;

                }
            } while (true);
        }
        public static bool YesOrNoAdmin(string input, string input2)
        {
            do
            {
                UserInterface.CurrentMethodGreen(input);
                switch (UserInterface.QuestionForString(input2).ToUpper())
                {
                    case "J":
                        return false;
                    case "N":
                        return true;
                    default:
                        UserInterface.CurrentMethodRed("Felaktig inmatning", "Välj [J] för ja eller N för nej.");
                        PressEnterToContinue();
                        RemoveLines(11);
                        break;

                }
            } while (true);
        }
        public static bool YesOrNo(string input)
        {

            do
            {
                switch (UserInterface.QuestionForString(input).ToUpper())
                {
                    case "J":
                        return false;
                    case "N":
                        return true;
                    default:
                        UserInterface.CurrentMethodRed("Men seriöst kan du inte följa dem mest grundläggande instruktioner?");
                        PressEnterToContinue();
                        RemoveLines(8);
                        break;

                } 
            } while (true);
        }
        public static bool YesOrNoAdmin(string input)
        {

            do
            {
                switch (UserInterface.QuestionForString(input).ToUpper())
                {
                    case "J":
                        return false;
                    case "N":
                        return true;
                    default:
                        UserInterface.CurrentMethodRed("Men seriöst kan du inte följa dem mest grundläggande instruktioner?");
                        PressEnterToContinue();
                        RemoveLines(8);
                        break;

                }
            } while (true);
        }

        public static void RemoveLines(int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop);
        }
        public static void RemoveLinesVariable(int baseLines, int variable)
        {
            for (int i = 0; i < baseLines + variable; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void MoveCursorTo(int value)
        {
            Console.SetCursorPosition(value, Console.CursorTop);
            Console.WriteLine("|");
        }
    }
}
