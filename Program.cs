using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;


// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit)
{
    CloseTerminal();
    if (FoodConsumed())
    {
        ChangePlayer();
        ShowFood();
    }
    else if (PlayerIsFreezed())
    {
        FreezePlayer();
    }
    else
        Move();

    if (FoodConsumed())
    {
        ChangePlayer();
        ShowFood();
    }
}

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

//create a method that closes terminal
void CloseTerminal()
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        shouldExit = true;
    }
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

//write a method that detects when food is eaten
bool FoodConsumed()
{
    return foodX == playerX && foodY == playerY;
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Method that checks if a player should be freezed
bool PlayerIsFreezed()
{
    return player == states[2];
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Method to add speed to player

// Reads directional input from the Console and moves the player
void Move(bool nonDirectional = false)
{
    int lastX = playerX;
    int lastY = playerY;
    bool anotherButton = false;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX--;
            break;
        case ConsoleKey.RightArrow:
            playerX++;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            anotherButton = true;
            break;
    }
    if (nonDirectional && anotherButton)
    {
        shouldExit = true;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame()
{
    if (TerminalResized())
        return;
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
    ChangePlayer();
}