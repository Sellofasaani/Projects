using System;
using System.Collections.Generic;
using System.Text;

MathGame game = new MathGame();
game.Run();


public class MathGame
{

    public List<string> gameHistory = new List<string>();
    Random random = new Random();
    public LevelDifficulty level;


    public void Run()
    {
        bool gameRunning = true;
        Menu();
        while (gameRunning)
        {
            Console.WriteLine("Choose option: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1) { Addition(); }
            if (choice == 2) { Subtraction(); }
            if (choice == 3) { Multiply(); }
            if (choice == 4) { Divide(); }
            if (choice == 5) { Console.WriteLine(GameHistory()); }
            if (choice == 6) { GetLevelName(); }
            if (choice == 7) { Menu(); }
            if (choice == 8) { gameRunning = false; }


        }

    }
    public void Menu()
    {
        Console.WriteLine("Welcome to the Math Game!");
        Console.WriteLine("Please select an option:");
        Console.WriteLine(@"
    1. Addition
    2. Subtraction
    3. Multiplication
    4. Division
    5. History
    6. Difficulty level
    7. Show Menu
    8. Exit
    ");


    }

    //Tuple of numbers and change difficulty based on answer.
    public (int, int) RandomNumbers(LevelDifficulty level)
    {
        int num1, num2;

        switch (level)
        {
            case LevelDifficulty.easy:
                num1 = random.Next(0, 11);
                num2 = random.Next(0, 11);
                break;

            case LevelDifficulty.medium:
                num1 = random.Next(0, 21);
                num2 = random.Next(0, 21);
                break;

            case LevelDifficulty.hard:
                num1 = random.Next(0, 31);
                num2 = random.Next(0, 31);
                break;

            default:
                Console.WriteLine("Invalid difficulty level. Using easy by default.");
                num1 = random.Next(0, 11);
                num2 = random.Next(0, 11);
                break;

        }

        return (num1, num2);
    }

    public int Answer()
    {
        return Convert.ToInt32(Console.ReadLine());
    }

    public void Addition()
    {

        for (int i = 1; i <= 5; i++)
        {

            var (a, b) = RandomNumbers(level);
            Console.Write($"{a} + {b} = ");
            int answer = Answer();
            string result = a + b == answer ? "Correct!" : "Incorrect";
            Console.WriteLine(result);

            gameHistory.Add($"{a} + {b} = {answer} {result}");
        }
    }

    public void Subtraction()
    {
        for (int i = 1; i <= 5; i++)
        {
            var (a, b) = RandomNumbers(level);
            Console.Write($"{a} - {b} = ");
            int answer = Answer();
            string result = a - b == answer ? "Correct!" : "Incorrect";
            Console.WriteLine(result);

            gameHistory.Add($"{a} - {b} = {answer} {result}");
        }

    }

    public void Divide()
    {
        for (int i = 1; i <= 5; i++)
        {
            while (true) // Loop until a valid pair is found
            {
                var (a, b) = RandomNumbers(level);

                if (b == 0 || a % b != 0)
                {
                    continue;
                }

                Console.Write($"{a} / {b} = ");
                int answer = Answer();

                string result = a / b == answer ? "Correct!" : "Incorrect";
                Console.WriteLine(result);

                gameHistory.Add($"{a} / {b} = {answer} {result}");
                break; // Exit the WHILE loop after a valid division question
            }

        }


    }

    public void Multiply()
    {
        for (int i = 1; i <= 5; i++)
        {

            var (a, b) = RandomNumbers(level);
            Console.Write($"{a} * {b} = ");
            int answer = Answer();
            string result = a * b == answer ? "Correct!" : "Incorrect";
            Console.WriteLine(result);

            gameHistory.Add($"{a} * {b} = {answer} {result}");

        }

    }

    public string GameHistory()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("Game History: ");

        foreach (string game in gameHistory)
        {
            sb.AppendLine(game);
        }

        return sb.ToString();
    }

    public LevelDifficulty GetLevelName()
    {
        Console.WriteLine("Select difficulty: easy, medium, hard");
        string userChoice = Console.ReadLine().ToLower();

        switch (userChoice)
        {
            case "easy":
                level = LevelDifficulty.easy;
                break;
            case "medium":
                level = LevelDifficulty.medium;
                break;
            case "hard":
                level = LevelDifficulty.hard;
                break;
            default:
                Console.WriteLine("Invalid difficulty level. Please choose easy, medium, or hard.");
                // Handle invalid input (e.g., prompt again or set a default level)
                GetLevelName();
                break;
        }

        return level;
    }

    public enum LevelDifficulty { easy, medium, hard }
}


