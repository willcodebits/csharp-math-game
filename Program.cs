
/*
    Try to implement levels of difficulty.
    Add a timer to track how long the user takes to finish the game.
    Add a function that let's the user pick the number of questions.
    Create a 'Random Game' option where the players will be presented with questions from random operations
*/


using willCodeBits_MathGame;
using System.Diagnostics;

MathGame mathGame = new MathGame();
Random random = new Random();

int firstNumber;
int secondNumber;
int userMenuSelection;
int score = 0;
bool gameOver = false;

DifficultyLevel difficulty = DifficultyLevel.Easy;


while (!gameOver)
{
    userMenuSelection = GetUserMenuSelection(mathGame);

    firstNumber = random.Next(1, 101);
    secondNumber = random.Next(1, 101);
    switch (userMenuSelection)
    {
        case 1:
            score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '+');
            break;
        case 2:
            score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '-');
            break;
        case 3:

            score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '*');
            break;
        case 4:
            while (firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(1, 101);
                secondNumber = random.Next(1, 101);
            }
            score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '/');
            break;
        case 5:
            int numberOfQuestions = 99;
            Console.WriteLine("Please enter the number of questions you want to attempt.");
            while (!int.TryParse(Console.ReadLine(), out numberOfQuestions))
            {
                Console.WriteLine("Please enter the number of questions you want to attempt as an integer number.");
            }
            while (numberOfQuestions > 0)
            {
                // Generate random numbers
                int randomOperation = random.Next(1, 4);

                if (randomOperation == 1)
                {
                    firstNumber = random.Next(1, 101);
                    secondNumber = random.Next(1, 101);
                    score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '+');
                }
                else if (randomOperation == 2)
                {
                    firstNumber = random.Next(1, 101);
                    secondNumber = random.Next(1, 101);
                    score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '-');
                }
                else if (randomOperation == 3)
                {
                    firstNumber = random.Next(1, 101);
                    secondNumber = random.Next(1, 101);
                    score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '*');
                }
                else
                {
                    firstNumber = random.Next(1, 101);
                    secondNumber = random.Next(1, 101);
                    while (firstNumber % secondNumber != 0)
                    {
                        firstNumber = random.Next(1, 101);
                        secondNumber = random.Next(1, 101);
                    }
                    score += await PerformOperation(mathGame, firstNumber, secondNumber, score, difficulty, '/');
                }
                numberOfQuestions--;
            }
            break;
        case 6:
            Console.WriteLine("This is the Game  History");
            foreach (var operation in mathGame.GameHistory)
            {
                Console.WriteLine($"{operation}");
            }
            break;
        case 7:
            difficulty = ChangeDifficulty();
            DifficultyLevel difficultyEnum = (DifficultyLevel)difficulty;
            Enum.IsDefined(typeof(DifficultyLevel), difficultyEnum);
            Console.WriteLine($"Your new difficulty level is {difficulty}.");
            break;
        case 8:
            gameOver = true;
            Console.WriteLine($"Your final score is: {score}");
            break;

    }
}

static DifficultyLevel ChangeDifficulty()
{
    int response = 0;

    Console.WriteLine("Your current level of Difficulty is easy");
    Console.WriteLine("Please enter a difficulty level");
    Console.WriteLine("1 Easy");
    Console.WriteLine("2 Medium");
    Console.WriteLine("3 Hard");

    while (!int.TryParse(Console.ReadLine(), out response) || (response < 1 || response > 3))
    {
        Console.WriteLine("Please enter a valid option 1-3");
    }

    switch (response)
    {
        case 1:
            return DifficultyLevel.Easy;
        case 2:
            return DifficultyLevel.Medium;
        case 3:
            return DifficultyLevel.Hard;
    }
    return DifficultyLevel.Easy;
}

static void DisplayMathGameQuestion(int firstNumber, int secondNumber, char operation)
{
    Console.WriteLine($"{firstNumber} {operation} {secondNumber} == ??");
}


static int GetUserMenuSelection(MathGame mathGame)
{
    int selection = -1;
    mathGame.ShowMenu();
    while (!(selection >= 1 && selection <= 8))
    {
        while (!int.TryParse(Console.ReadLine(), out selection))
        {
            Console.WriteLine("Please enter a valid Selection from 1-8");
        }

        if (!(selection >= 1 && selection <= 8))
        {
            Console.WriteLine("Please enter a valid Selection from 1-8");
        }
    }
    return selection;
}

// static int? GetUserResponse(DifficultyLevel difficulty)
// {
//     int response = 0;
//     var startTime = DateTime.Now;
//     var timeout = (int)difficulty;
//     Stopwatch stopwatch = new Stopwatch();
//     stopwatch.Start();
//     Console.WriteLine("Please enter your response.");
//     while ((DateTime.Now - startTime).TotalSeconds < timeout)
//     {
//         while (!int.TryParse(Console.ReadLine(), out response))
//         {
//             Console.WriteLine("Please enter a valid Integer Value");
//         }
//         stopwatch.Stop();
//         TimeSpan timeTaken = stopwatch.Elapsed;
//         Console.WriteLine("Time taken to answer: " + timeTaken.ToString(@"m\:ss\.fff"));
//         return response;
//     }
//     stopwatch.Stop();
//     Console.WriteLine("Time's up!");
//     return null;

// }


static async Task<int?> GetUserResponse(DifficultyLevel difficulty)
{
    int response = 0;
    int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> task = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(task, Task.Delay(timeout * 1000)) == task ? task.Result : null;
        stopwatch.Stop();

        if (result != null && int.TryParse(result, out response))
        {
            Console.WriteLine("Time taken to answer: " + stopwatch.Elapsed.ToString(@"m\:ss\.fff"));
            return response;
        }
        else
        {
            throw new OperationCanceledException();
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Time's up!");
        return null;
    }
}



static int validateResult(int result, int? userResponse, int score)
{
    if (userResponse == result)
    {
        Console.WriteLine("You answer correctly; You earned 5 pts.");
        score += 5;
    }
    else
    {
        Console.WriteLine($"Try again");
        Console.WriteLine($"Correct result: {result}");
        score += 0;
    }

    return score;
}

static async Task<int> PerformOperation(MathGame mathGame, int firstNumber, int secondNumber, int score, DifficultyLevel difficulty, char operation)
{
    int result;
    int? userResponse;
    DisplayMathGameQuestion(firstNumber, secondNumber, operation);
    result = mathGame.mathOperation(firstNumber, secondNumber, operation);
    userResponse = await GetUserResponse(difficulty);
    score += validateResult(result, userResponse, score);
    return score;
}

public enum DifficultyLevel
{
    Easy = 60,
    Medium = 45,
    Hard = 30
}

