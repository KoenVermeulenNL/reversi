using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

// initialize screen hight and width
int SCREEN_WIDTH = 600;
int SCREEN_HEIGHT = 700;

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.ClientSize = new Size(SCREEN_WIDTH, SCREEN_HEIGHT);
scherm.MinimumSize = new Size(SCREEN_WIDTH, SCREEN_HEIGHT);

// the width of the board itself
int boardWidth = 400; 

// the three soundplayer for backgroundmusic, if you place a wrong stone and if you place a good stone
System.Media.SoundPlayer player = new System.Media.SoundPlayer();
player.SoundLocation = "../../../sounds/backgroundmusic.wav";
Debug.WriteLine(Environment.CurrentDirectory);
player.PlayLooping();

System.Media.SoundPlayer GoodplayPlayer = new System.Media.SoundPlayer();
GoodplayPlayer.SoundLocation = "../../../sounds/Good-Placement.wav";

System.Media.SoundPlayer BadplayPlayer = new System.Media.SoundPlayer();
BadplayPlayer.SoundLocation = "../../../sounds/Bad-Placement.wav";


int amountOfCells = 6; // 6 = board of 6x6
int amountOfTurns = 0;

double cellWidth = boardWidth/amountOfCells;
double stoneRadius = (boardWidth/amountOfCells)/2;

// Booleans for self made buttons
bool showViableLocation = true;
bool MultiplayerBool = true;
bool HamburgerBool = true;
bool playingSound = true;

// read the highscore from the txt file
int highscore = Convert.ToInt32(File.ReadAllText(@"../../../highscore.txt"));

int currentPlayer = 0; // even = red odd = blue

int amountRed = 0;
int amountBlue = 0;

//UI Elements
Button newGameBtn = new Button();
newGameBtn.BackColor = Color.FromArgb(200, 27, 58, 133);
newGameBtn.Location = new Point(0, 40);
newGameBtn.Width = 89;
newGameBtn.Text = "New Game";

// the labels in the game itself
Label blueStonesLabel = new Label();
blueStonesLabel.Text = $"{amountBlue}";
blueStonesLabel.Location = new Point(100, 510);
blueStonesLabel.ForeColor = Color.Blue;
blueStonesLabel.AutoSize = true;
blueStonesLabel.BackColor = Color.Transparent;
Label redStonesLabel = new Label();
redStonesLabel.Text = $"{amountRed}";
redStonesLabel.ForeColor = Color.Red;
redStonesLabel.Location = new Point(100, 540);
redStonesLabel.AutoSize = true;
redStonesLabel.BackColor = Color.Transparent;

Label PlayerStonesTextBlue = new Label();
PlayerStonesTextBlue.Text = "Blue stones";
PlayerStonesTextBlue.Location = new Point(120, 510);
PlayerStonesTextBlue.BackColor = Color.Transparent;
Label PlayerStonesTextRed = new Label();
PlayerStonesTextRed.Text = "Red stones";
PlayerStonesTextRed.Location = new Point(120, 540);
PlayerStonesTextRed.BackColor = Color.Transparent;
Label CurrentPlayer = new Label();
CurrentPlayer.BackColor = Color.Transparent;
CurrentPlayer.Text = "Current Player:";
CurrentPlayer.Location = new Point(300, 510);

Label HighscoreLabel = new Label();
HighscoreLabel.Text = $"HighScore: {highscore}";
HighscoreLabel.Location = new Point(300, 540);
HighscoreLabel.BackColor = Color.Transparent;

//FOR NOW: CHANGE IF YOU CHANGE THE amountOfCells
int[,] squares = {}; // 0 = empty, 1 = red, 2 = blue

void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum){
    var newArray = new T[newCoNum,newRoNum];
    int columnCount = original.GetLength(1);
    int columnCount2 = newRoNum;
    int columns = original.GetUpperBound(0);
    for (int co = 0; co <= columns; co++)
        Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
    original = newArray;
}
ResizeArray(ref squares, amountOfCells, amountOfCells); // Resize 2D array to be used for every board width

// populate board with middle pieces, works for (almost) every board width
squares[(amountOfCells/2-1), (amountOfCells/2-1)] = squares[(amountOfCells/2), (amountOfCells/2)] = 1; 
squares[(amountOfCells/2-1), (amountOfCells/2)] = squares[(amountOfCells/2), (amountOfCells/2 -1)] = 2;

// Create Bitmaps and graphics to draw
Bitmap ImageBoxDrawing = new Bitmap(boardWidth, boardWidth);
Bitmap CurrentPlayerBitmap = new Bitmap(30, 30);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);
Graphics CurrentPlayerDrawer = Graphics.FromImage(CurrentPlayerBitmap);

// create Label for the board
Label ImageBoxImage = new Label();
ImageBoxImage.Location = new Point(100, 100);

// create label for the current player
Label CurrentPlayerLabel = new Label();
CurrentPlayerLabel.BackColor = Color.Transparent;
CurrentPlayerLabel.Location = new Point(400, 510);

scherm.Controls.Add(ImageBoxImage);
scherm.Controls.Add(CurrentPlayerLabel);

// The self made hamburger button (in HamburgerMenu.cs)
HamburgerMenu hamburgerMenu = new HamburgerMenu();
hamburgerMenu.Location = new Point(5, 10);

// the label which the hamburger button needs to open
Label HamburgerLabel = new Label();
HamburgerLabel.Location = new Point(0, 0);
HamburgerLabel.BackColor = Color.FromArgb(120, 27, 58, 133);
HamburgerLabel.Size = new Size(90, scherm.Height);

Font LargeFont = new Font("Times New Roman", 44);
Font ButtonFont = new Font("Times new Roman", 20);;

// Labels and Buttons for settings
Label SETTINGS = new Label();
SETTINGS.ForeColor = Color.FromArgb(30, 78, 199);
SETTINGS.BackColor = Color.Transparent;
SETTINGS.Text = "SETTINGS";
SETTINGS.AutoSize = true;
SETTINGS.Font = LargeFont;
SETTINGS.Location = new Point(150, 20);

Label NewGameText = new Label();
NewGameText.ForeColor = Color.FromArgb(30, 78, 199);
NewGameText.BackColor = Color.Transparent;
NewGameText.Text = "REVERSI";
NewGameText.AutoSize = true;
NewGameText.Font = LargeFont;
NewGameText.Location = new Point(170, 20);

Label CheckViableLocationLabel = new Label();
CheckViableLocationLabel.Font = ButtonFont;
CheckViableLocationLabel.AutoSize = true;
CheckViableLocationLabel.Location = new Point(122, 100);
CheckViableLocationLabel.BackColor = Color.Transparent;
CheckViableLocationLabel.Height = 46;
CheckViableLocationLabel.Text = "Show viable Locations";

// The self made ToggleButton (made in ToggleButton.cs)
ToggleButton ViableLocation = new ToggleButton(showViableLocation);
ViableLocation.Location = new Point(380, 95);

Label MultiplayerLabel = new Label();
MultiplayerLabel.Font = ButtonFont;
MultiplayerLabel.AutoSize = true;
MultiplayerLabel.Location = new Point(122, 180);
MultiplayerLabel.BackColor = Color.Transparent;
MultiplayerLabel.Height = 46;
MultiplayerLabel.Text = "Multiplayer";

ToggleButton Multiplayer = new ToggleButton(MultiplayerBool);
Multiplayer.Location = new Point(380, 170);

// variabelen voor Difficulty:
Button DifficultyMenuButton = new Button();
DifficultyMenuButton.Font = new Font("Times New Roman", 25);
DifficultyMenuButton.BackColor = Color.FromArgb(120, 93, 135, 184);
DifficultyMenuButton.Size = new Size(182, 52);
DifficultyMenuButton.Location = new Point(209, 570);
DifficultyMenuButton.Text = "Menu";

Label DifficultyText = new Label();
DifficultyText.ForeColor = Color.FromArgb(30, 78, 199);
DifficultyText.BackColor = Color.Transparent;
DifficultyText.Text = "DIFFICULTY";
DifficultyText.AutoSize = true;
DifficultyText.Location = new Point(140, 20);
DifficultyText.Font = LargeFont;

Button Easy = new Button();
Easy.Font = ButtonFont;
Easy.BackColor = Color.Transparent;
Easy.Size = new Size(260, 52);
Easy.Location = new Point(170, 200);
Easy.Text = "Easy Game: 6x6";

Button Medium = new Button();
Medium.Font = ButtonFont;
Medium.BackColor = Color.Transparent;
Medium.Size = new Size(260, 52);
Medium.Location = new Point(170, 260);
Medium.Text = "Medium Game: 8x8";

Button Hard = new Button();
Hard.Font = ButtonFont;
Hard.BackColor = Color.Transparent;
Hard.Size = new Size(260, 52);
Hard.Location = new Point(170, 320);
Hard.Text = "Hard Game: 10x10";

// Atributes for Rules
Label RulesText = new Label();
RulesText.ForeColor = Color.FromArgb(30, 78, 199);
RulesText.BackColor = Color.Transparent;
RulesText.Text = "RULES";
RulesText.AutoSize = true;
RulesText.Location = new Point(190, 20);
RulesText.Font = LargeFont;

Label PlayingRules = new Label();
PlayingRules.BackColor = Color.Transparent;
PlayingRules.Size = new Size(400, 400);
PlayingRules.MaximumSize = new Size(400, 400);
PlayingRules.Location = new Point(100, 100);
PlayingRules.Font = new Font("Times New Roman", 14);
PlayingRules.Text = @"
Each player places a stone of their color on the board.

You may only put down the stones if it encloses an opponent's stone.

After that, all the stones that are enclosed are turned over to the player's color.

When there are no more possible moves, the game ends and the player with the most stones wins.

The purple circles are places where a player can place their stones. You can turn this of in the settings";

// Atributes for the menu
Label MENU = new Label();
MENU.ForeColor = Color.Black;
MENU.BackColor = Color.Transparent;
MENU.Text = "MENU";
MENU.Size = new Size(200,70);
MENU.Location = new Point(200, 20);
MENU.Font = LargeFont;

Label reversi = new Label();
reversi.Text = "Reversi";
reversi.ForeColor = Color.Black;
reversi.BackColor = Color.Transparent;
reversi.Location = new Point(250, 100);
reversi.Font = new Font("Times New Roman", 18);
reversi.Size = new Size(100, 50);

Button rules = new Button();
rules.Font = new Font("Times New Roman", 25);
rules.BackColor = Color.FromArgb(120, 93, 135, 184);
rules.Size = new Size(182, 52);
rules.Location = new Point(((scherm.Width-16)/2)+5, 300);
rules.Text = " Rules ";

Button difficulty = new Button();
difficulty.Font = new Font("Times New Roman", 25);
difficulty.BackColor = Color.FromArgb(120, 93, 135, 184);
difficulty.Size = new Size(182, 52);
difficulty.Location = new Point(108, 360);
difficulty.Text = "Difficulty";

Button Continue = new Button();
Continue.Font = new Font("Times New Roman", 25);
Continue.BackColor = Color.FromArgb(120, 93, 135, 184);
Continue.Size = new Size(182, 52);
Continue.Location = new Point(((scherm.Width-16)/2)-Continue.Width/2, 510);
Continue.Text = "Continue";

Button settings = new Button();
settings.Font = new Font("Times New Roman", 25);
settings.BackColor = Color.FromArgb(120, 93, 135, 184);
settings.Size = new Size(182, 52);
settings.Location = new Point(((scherm.Width-16)/2)+5, 360);
settings.Text = "Settings";

Button SoundButtonOff = new Button();
SoundButtonOff.Image = Image.FromFile("../../../images/volume-off-indicator.png");
SoundButtonOff.BackColor = Color.Transparent;
SoundButtonOff.FlatStyle = FlatStyle.Flat;
SoundButtonOff.Size = new Size(42, 42);
SoundButtonOff.Location = new Point(SCREEN_WIDTH-45, SCREEN_HEIGHT-45);

Button SoundButtonOn = new Button();
SoundButtonOn.Image = Image.FromFile("../../../images/speaker-filled-audio-tool.png");
SoundButtonOn.BackColor = Color.Transparent;
SoundButtonOn.FlatStyle = FlatStyle.Flat;
SoundButtonOn.Size = new Size(42, 42);
SoundButtonOn.Location = new Point(SCREEN_WIDTH-45, SCREEN_HEIGHT-45);

Button MenuButton = new Button();
MenuButton.BackColor = Color.FromArgb(200, 27, 58, 133);
MenuButton.Width = 89;
MenuButton.Location = new Point(0, 115);
MenuButton.Text = "Menu";

Button DifficultyInGame = new Button();
DifficultyInGame.BackColor = Color.FromArgb(200, 27, 58, 133);
DifficultyInGame.Width = 89;
DifficultyInGame.Location = new Point(0, 90);
DifficultyInGame.Text = "Difficulty";

Button SettingsInGame = new Button(); 
SettingsInGame.BackColor = Color.FromArgb(200, 27, 58, 133);
SettingsInGame.Width = 89;
SettingsInGame.Location = new Point(0, 65);
SettingsInGame.Text = "Settings";

SoundButtonOn.Click += StopSound;
SoundButtonOff.Click += PlaySound;

Button newGame = new Button();
newGame.Font = new Font("Times New Roman", 25);
newGame.BackColor = Color.FromArgb(120, 93, 135, 184);
newGame.Size = new Size(182, 52);
newGame.Location = new Point(108, 300);
newGame.Text = "New game";
newGame.Click += startGameButtonClick;

void startGameButtonClick(object sender, EventArgs e) {
    startGame();
}

MenuButton.Click += BackToMenu;

// Image Size Variable
ImageBoxImage.Size = new Size(boardWidth, boardWidth);
CurrentPlayerLabel.Size = new Size(30, 30);

CurrentPlayerLabel.Image = CurrentPlayerBitmap;
ImageBoxImage.BackColor = Color.Transparent;
ImageBoxImage.Image = ImageBoxDrawing;

int GetPlayer(){
    return currentPlayer % 2 + 1; //1 = even - red, 2 = odd - blue, +1 to corrrect for 0 = empty
}

void createBoard(){
    // Nested for loop which makes a square of squares
    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells;j++) {
            if (i%2==1) {// If the rows are even
                // to oscilate between gray and white squares
                if (j%2==0) ImageBoxDrawer.FillRectangle(Brushes.LightGray, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.White, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
            } else if (i%2==0) { // If the rows are odd
                // to oscilate between gray and white squares for a chessboard like pattern
                if (j%2==0) ImageBoxDrawer.FillRectangle(Brushes.White, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.LightGray, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
            }

        }
    }

    // Makes the board more defined
    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells;j++) {
            ImageBoxDrawer.DrawRectangle(Pens.Black, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
        }
    }

    // Draws a big rectangle to make the board prettier because the edges aren't drawn
    ImageBoxDrawer.DrawRectangle(Pens.Black, 0, 0, ImageBoxImage.Width-1, ImageBoxImage.Height-1);
}

void UpdateBoard() {
    // Draws a whole board
    createBoard();

    int amountOfTrueLocations = 0;
    
    // loop through every square, if the square contains a 1 (for red) or a 2 (for blue) draw a circle at that posisition
    for (int col = 0; col < squares.GetLength(1); col++) {
        for (int row = 0; row < squares.GetLength(0); row++) {
            if (squares[col, row] == 1) {
                ImageBoxDrawer.FillEllipse(Brushes.Red, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            } else if (squares[col, row] == 2) {
                ImageBoxDrawer.FillEllipse(Brushes.Blue, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            }

            // every time the board gets updated it checks all the locations
            if (CheckIfViableLocation(row, col)) {
                amountOfTrueLocations++;
            }

            // draw a purple circle if the square is viable and if the user choose to draw them
            if (showViableLocation && CheckIfViableLocation(row,col)) {
                ImageBoxDrawer.DrawEllipse(Pens.Purple, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            }
        }
    }

    amountRed = CountPlayer(1);
    amountBlue = CountPlayer(2);

    redStonesLabel.Text = $"{amountRed}";
    blueStonesLabel.Text = $"{amountBlue}";

    // To show which turn it is
    switch (GetPlayer()) {
        case 1:
            CurrentPlayerDrawer.FillEllipse(Brushes.Red, 0, 0, 30, 30);
            break;
        case 2:
            CurrentPlayerDrawer.FillEllipse(Brushes.Blue, 0, 0, 30, 30);
            break;
    }

    ImageBoxImage.Invalidate();
    CurrentPlayerLabel.Invalidate();
    
    // if there are no more viable turns
    if (amountOfTrueLocations==0) {
        if (amountOfTurns < highscore) {
            File.WriteAllText(@"../../../highscore.txt", amountOfTurns.ToString());
            // write the highscore to the text file
            highscore = Convert.ToInt32(File.ReadAllText(@"../../../highscore.txt"));
        }
        // for tie
        if (amountBlue == amountRed) {
            MessageBox.Show($"Its a tie!! \nWith {amountOfTurns} turns");
            menu();
        }
        // for a win from blue
        else if (amountBlue > amountRed) {
            MessageBox.Show($"Blue wins!! \nWith {amountOfTurns} turns");
            menu();
        } 
        // for a win from red
        else {
            MessageBox.Show($"Red wins!! \nWith {amountOfTurns} turns");
            menu();
        }

        // If you win it creates a new board, populates it again, resets the currentplayer and the amount of turns
        for (int i = 0; i < squares.GetLength(0); i++){
            for (int j = 0; j < squares.GetLength(1); j++){
                squares[i,j] = 0;
            }
        }
        squares[(amountOfCells/2-1), (amountOfCells/2-1)] = squares[(amountOfCells/2), (amountOfCells/2)] = 1; //populate board with middle pieces, works for (almost) every board width
        squares[(amountOfCells/2-1), (amountOfCells/2)] = squares[(amountOfCells/2), (amountOfCells/2 -1)] = 2; //populate board with middle pieces, works for (almost) every board width
        currentPlayer = 0;
        amountOfTurns = 0;
    }
}

int CheckCaptured(int[] rowList, int columnPosition, int cellX, int cellY){
    //Expected input: array of entire row converted from any direction to horizontal. 
    
    if(rowList.Length != amountOfCells){
        MessageBox.Show("error", "rowList.Length is not the same length as the board width");
        return 0; //0 means not possible
    }
    int indexesTillEndOfRow = amountOfCells - columnPosition - 1; //-1 to correct for index starts at 0
    int checkedForwards =  CheckCapturedLine(rowList, columnPosition, indexesTillEndOfRow, cellX, cellY); //returns the index of the rowList that the next own piece is located, so all pieces in between can be captured
   
    Array.Reverse(rowList);
    int reverseIndexesTillEndOfRow = amountOfCells - indexesTillEndOfRow - 1; //get the Oposite by subtracting from board width, again - 1 to correct for index starts at 0
    columnPosition = amountOfCells - columnPosition - 1;
    
    int checkedBackwards = CheckCapturedLine(rowList, columnPosition, reverseIndexesTillEndOfRow, cellX, cellY); //returns the index of the rowList that the next own piece is located, so all pieces in between can be captured
    
    if(checkedForwards != 0){
        return checkedForwards;
    }
    if(checkedBackwards != 0){
        return -checkedBackwards; //- to indicate it was done backwards
    }
    return 0; //0 means not possible
}

int CheckCapturedLine(int[] rowList, int columnPosition, int iTER, int cellX, int cellY){
    for (int zeroIndex = 1; zeroIndex <= iTER; zeroIndex++) 
    {
        //zeroIndex = 1, because it doesn't have to check its own position
        //loops trough every square till the end of the row, because it can't go further at that point
        int squareChecking = rowList[columnPosition + zeroIndex];
        int previousSquareChecked = rowList[columnPosition + zeroIndex - 1];
        
        if(squareChecking == 0){
            //if encounters an empty square, return it is not captured
            return 0; //0 means not possible
        }
        
        if(squareChecking == GetPlayer()){
            //if encounters own square and previous square is other square
            if(previousSquareChecked != GetPlayer() && previousSquareChecked != 0){
                //succesfull capture
                return zeroIndex;
            } else {
                //encountered own square --> not possible
                return 0;
            }            
        }
    }
    return 0;
}

int CheckHorizontal(int cellX, int cellY){
    //get the row of the index
    int rowIndex = cellY; //When working with 1D array: index / amountOfCells
    int columnPosition = cellX; //When working with 1D array: index - rowIndex*amountOfCells
    
    //simplify to 1D row
    int[] currentRow = GetHorizontalRow(rowIndex);
    return CheckCaptured(currentRow, columnPosition, cellX, cellY);
}

int[] GetHorizontalRow(int rowIndex){ //rowIndex = Y of cell
    int[] currentRow = {};
    Array.Resize(ref currentRow, amountOfCells);
    for (int i = 0; i < amountOfCells; i++)
    {
        currentRow[i] = squares[rowIndex, i]; //first row, then column
    }
    return currentRow;
}

int CheckVertical(int cellX, int cellY){
    //simplify to 1D row
    int[] currentColumn = GetVerticalRow(cellX);
    return CheckCaptured(currentColumn, cellY, cellX, cellY);
}

int[] GetVerticalRow(int cellX){
    int[] currentColumn = {};
    Array.Resize(ref currentColumn, amountOfCells);
    for (int i = 0; i < amountOfCells; i++)
    {
        currentColumn[i] = squares[i, cellX]; //first row, then column
    }
    return currentColumn;
}

int CheckDiagonalLeftToRight(int cellX, int cellY){
    //simplify to 1D row

    int currentPosition = cellX; //current position in diagonal, not the coordinates
    if(cellX > cellY){
        //if the horizontal offset is greater than the vertical offset, the current position is the same as the vertical offset
        //if both offsets are equal, it doesn't matter wich to pick
        currentPosition = cellY;
    }

    int[] currentDiagonalLine = {};
    Array.Resize(ref currentDiagonalLine, amountOfCells); //populate diagonal line with board with, even though the width of a diagonal can be smaller
    for (int i = 0; i < amountOfCells; i++)
    {
        if(cellX > cellY){
            //Pretty proud of this one tbh, there is probably another way, but this works ;)
            if(cellX - (currentPosition - i) < amountOfCells){
                currentDiagonalLine[i] = squares[i, cellX - (currentPosition - i)]; //first row, then column
            } else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
        } else {
            if(cellY - (currentPosition - i) < amountOfCells){
                currentDiagonalLine[i] = squares[cellY - (currentPosition - i), i]; //first row, then column
            } else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
            
        }
    }
    return CheckCaptured(currentDiagonalLine, currentPosition, cellX, cellY); 
}

int CheckDiagonalRightToLeft(int cellX, int cellY){
    //simplify to 1D row

    int currentPosition = cellY; //current position in diagonal, not the coordinates
    int cellYInverted = amountOfCells - cellY - 1;

    int[] currentDiagonalLine = {};
    Array.Resize(ref currentDiagonalLine, amountOfCells); //populate diagonal line with board with, even though the width of a diagonal can be smaller
    for (int i = 0; i < amountOfCells; i++) //i is leading --> begins at first row, so right to left
    {
    
    if(cellX > cellYInverted){
            if(cellX + (currentPosition - i) < amountOfCells){
                currentDiagonalLine[i] = squares[i, cellX + (currentPosition - i)]; //first row, then column
            } else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
    } else {
        if(cellX + (currentPosition - i) > 0){
                currentDiagonalLine[i] = squares[i, cellX + (currentPosition - i)]; //first row, then column
            } else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
    }
    }
    /*if(cellY == 4 && cellX == 1){
        Debug.WriteLine(currentPosition);
        Debug.WriteLine(String.Join(";", currentDiagonalLine));
    }*/

    return CheckCaptured(currentDiagonalLine, currentPosition, cellX, cellY); 
}

bool CheckIfViableLocation(int cellX, int cellY){

    //Check if index is a viable location to place a stone (sluit een of meerdere andere stones in)
    if(squares[cellY, cellX] != 0){
        return false; // if stone is present
    }

    int checkedHorizontal = CheckHorizontal(cellX, cellY);
    int checkedVertical = CheckVertical(cellX, cellY);
    int checkedDiagonalLTR = CheckDiagonalLeftToRight(cellX, cellY);
    int checkedDiagonalRTL = CheckDiagonalRightToLeft(cellX, cellY);

    if(checkedHorizontal != 0){
        return true;
    }
    if(checkedVertical != 0){
        return true;
    }
    if(checkedDiagonalLTR != 0){
        return true;
    }
    if(checkedDiagonalRTL != 0){
        return true;
    }
    return false; //0 means not possible
}

int CountPlayer(int player /* 1 for red 2 for blue */) {
    int amount = 0;

    for (int i=0; i<squares.GetLength(1); i++) {
        for (int j=0; j<squares.GetLength(0); j++) {
            if (squares[i,j] == player) {
                amount++;
            }
        }
    }

    return amount;
}

void CreateStone(int cellX, int cellY){
    //plaats nieuw steen
    int currentSquare = squares[cellY, cellX]; //first row, then column

    //not possible if stone is already present --> has to be 0
    if(currentSquare == 0 && CheckIfViableLocation(cellX, cellY)){
        
        squares[cellY, cellX] = GetPlayer(); //0 = empty, so + 1
        
        CaptureStones(cellX, cellY);

        currentPlayer++;
        
    };
}

void CaptureStones(int cellX, int cellY){
    //convert capture stones to current player
    int horizontalIndex = CheckHorizontal(cellX, cellY);
    if(horizontalIndex != 0){
        //Got a hit
        if(horizontalIndex > 0){
            //done forwards
            for (int i = 1; i <= (horizontalIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY, cellX + i] = GetPlayer(); //Set stone to own player
            }
        } else {
            //done backwards
            for (int i = 1; i <= (-horizontalIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY, cellX - i] = GetPlayer(); //Set stone to own player
            }
        }
    }
    int verticalIndex = CheckVertical(cellX, cellY);
    if(verticalIndex != 0){
        //Got a hit
        if(verticalIndex > 0){
            //done forwards
            for (int i = 1; i <= (verticalIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY + i, cellX] = GetPlayer(); //Set stone to own player
            }
        } else {
            //done backwards
            for (int i = 1; i <= (-verticalIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY - i, cellX] = GetPlayer(); //Set stone to own player
            }
        }
    }
    int diagonalLTRIndex = CheckDiagonalLeftToRight(cellX, cellY);
    if(diagonalLTRIndex != 0){
        //Got a hit
        if(diagonalLTRIndex > 0){
            //done forwards
            for (int i = 1; i <= (diagonalLTRIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY + i, cellX + i] = GetPlayer(); //Set stone to own player
            }
        } else {
            //done backwards
            for (int i = 1; i <= (-diagonalLTRIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY - i, cellX - i] = GetPlayer(); //Set stone to own player
            }
        }
    }
    int diagonalRTLIndex = -CheckDiagonalRightToLeft(cellX, cellY); //invert direction
    if(diagonalRTLIndex != 0){
        //Got a hit
        if(diagonalRTLIndex > 0){
            //done forwards
            for (int i = 1; i <= (diagonalRTLIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY - i, cellX + i] = GetPlayer(); //Set stone to own player
            }
        } else {
            //done backwards
            for (int i = 1; i <= (-diagonalRTLIndex - 1); i++) //begin with 1, 0 is own stone; - 1 because last stone is also own stone
            {
                squares[cellY + i, cellX - i] = GetPlayer(); //Set stone to own player
            }
        }
    }
};

int PixelToCell(int mousePixel) {
    //Determin in which cell the location of the pixel is
    //Cell 0 - amountofcells-1
    return (int)Math.Floor(mousePixel / cellWidth);
}

// this is an event that checks if you change the size of the screen
scherm.ClientSizeChanged += change_size;
void change_size(object sender, EventArgs e) {
    // These change the location of all the attributes relative to the screen height and width
    SoundButtonOff.Location = new Point(scherm.Width-45-16, scherm.Height-45-39);
    SoundButtonOn.Location = new Point(scherm.Width-45-16, scherm.Height-45-39);
    MENU.Location = new Point(((scherm.Width-16)/2)-MENU.Width/2, 20);
    reversi.Location = new Point(((scherm.Width-16)/2)-reversi.Width/2, 100);
    newGame.Location = new Point(((scherm.Width-16)/2)-newGame.Width-5, 300);
    rules.Location = new Point(((scherm.Width-16)/2)+5, 300);
    difficulty.Location = new Point(((scherm.Width-16)/2)-difficulty.Width-5, 360);
    DifficultyText.Location = new Point(((scherm.Width-16)/2)-DifficultyText.Width/2, 20);
    Easy.Location = new Point(((scherm.Width-16)/2)-Easy.Width/2, 200);
    Medium.Location = new Point(((scherm.Width-16)/2)-Medium.Width/2, 260);
    Hard.Location = new Point(((scherm.Width-16)/2)-Hard.Width/2, 320);
    DifficultyMenuButton.Location = new Point(((scherm.Width-16)/2)-DifficultyMenuButton.Width/2, 570);
    ImageBoxImage.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, 100);
    blueStonesLabel.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, 100+ImageBoxImage.Height+15);
    redStonesLabel.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, 100+ImageBoxImage.Height+45);
    PlayerStonesTextBlue.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2+blueStonesLabel.Width+10, 100+ImageBoxImage.Height+15);
    PlayerStonesTextRed.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2+redStonesLabel.Width+10, 100+ImageBoxImage.Height+45);
    CurrentPlayerLabel.Location = new Point((scherm.Width/2)+ImageBoxImage.Width/2-CurrentPlayerLabel.Width, 100+ImageBoxImage.Height+15); 
    CurrentPlayer.Location = new Point((scherm.Width/2)+ImageBoxImage.Width/2-CurrentPlayerLabel.Width-CurrentPlayer.Width - 10, 100+ImageBoxImage.Height+15); 
    HighscoreLabel.Location = new Point((scherm.Width/2)+ImageBoxImage.Width/2-CurrentPlayerLabel.Width-CurrentPlayer.Width - 10, 100+ImageBoxImage.Height+45); 
    Continue.Location = new Point(((scherm.Width-16)/2)-Continue.Width/2, 510);
    settings.Location = new Point(((scherm.Width-16)/2)+5, 360);
    SETTINGS.Location = new Point(((scherm.Width-16)/2)-SETTINGS.Width/2, 20);
    NewGameText.Location = new Point(((scherm.Width)/2)-NewGameText.Width/2, 20);
    CheckViableLocationLabel.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2, 100);
    MultiplayerLabel.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2, 180);
    ViableLocation.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2+CheckViableLocationLabel.Width, 95);
    Multiplayer.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2+CheckViableLocationLabel.Width, 170);
    RulesText.Location = new Point(((scherm.Width-16)/2)-MENU.Width/2, 20);
    PlayingRules.Location = new Point(((scherm.Width-16)/2)-PlayingRules.Width/2, 100);
    HamburgerLabel.Size = new Size(90, scherm.Height);
}

// makes a list of all the viable locations so that we can choose a random position
List<Point> makeListOfViableLocations() {
    List<Point> ListOfViableLocations = new List<Point>();

    for (int col=0; col<amountOfCells; col++) {
        for (int row=0; row<amountOfCells; row++) {
            if (CheckIfViableLocation(col, row)) {
                ListOfViableLocations.Add(new Point(col, row));
            }
        }
    }

    return ListOfViableLocations;
}

// this is a function that waits a program without stopping the whole thread
void wait(int ms){
    Timer timer = new Timer();
    timer.Interval = ms;
    timer.Enabled = true;
    timer.Start();

    timer.Tick += (s, e) =>
    {
        timer.Enabled = true;
        timer.Stop();
    };

    while(timer.Enabled) Application.DoEvents();
}

// If you click on the board
ImageBoxImage.MouseClick += ImageBoxImage_MouseClick;
void ImageBoxImage_MouseClick(object sender, MouseEventArgs mea) {
    int cellX, cellY;
    Random rdn = new Random();

    // if you want to play multiplayer
    if (MultiplayerBool) {
        showViableLocation = true;
        // This checks for the mouse click, make it a cell and then plays the board.
        cellX = PixelToCell(mea.X);
        cellY = PixelToCell(mea.Y);
        if (CheckIfViableLocation(cellX, cellY)) {
            GoodplayPlayer.Play();
            amountOfTurns++;
        } else {
            BadplayPlayer.Play();
        }
    } else {
        cellX = PixelToCell(mea.X);
        cellY = PixelToCell(mea.Y);

        // only if you click on a viable location 
        if (CheckIfViableLocation(cellX, cellY)) {
            // now its the computers turn so you don't want to see the viable locations of the computer
            showViableLocation = false;
            GoodplayPlayer.Play();

            // it creates a stone for the current player
            CreateStone(cellX, cellY);
            UpdateBoard();

            // makes a list of the viable locations to choose random from
            List<Point> ViableLocations = makeListOfViableLocations();
            int Index = rdn.Next(ViableLocations.Count);
            Point randomPoint = ViableLocations[Index];
            cellX = randomPoint.X;
            cellY = randomPoint.Y;

            // one for the player one for the computer
            amountOfTurns++;
            amountOfTurns++;
            GoodplayPlayer.Play();
            // for the computer
            CreateStone(cellX, cellY);
            wait(1000);
            GoodplayPlayer.Play();
            showViableLocation = true;
            UpdateBoard();
        } else
        {
            BadplayPlayer.Play();
        }

    }


    CreateStone(cellX, cellY);
    UpdateBoard();
}

// if you click the button it resets the squares, populates the board and updates it
newGameBtn.MouseClick += newGameBtn_MouseClick;
void newGameBtn_MouseClick(object sender, MouseEventArgs mea) {
    for (int i = 0; i < amountOfCells; i++)
    {
        for (int j = 0; j < amountOfCells; j++)
        {
            squares[i,j] = 0;
        }
    }
    squares[(amountOfCells/2-1), (amountOfCells/2-1)] = squares[(amountOfCells/2), (amountOfCells/2)] = 1; //populate board with middle pieces, works for (almost) every board width
    squares[(amountOfCells/2-1), (amountOfCells/2)] = squares[(amountOfCells/2), (amountOfCells/2 -1)] = 2; //populate board with middle pieces, works for (almost) every board width
    currentPlayer = 0;
    UpdateBoard();
}

// stops the sound and makes sure it doesnt play in other seettings or difficulty or rules
void StopSound(object sender, EventArgs e) {
    scherm.Controls.Remove(SoundButtonOn);
    scherm.Controls.Add(SoundButtonOff);
    playingSound = false;
    player.Stop();
}

// plays the sound and makes sure it doesnt start again in other seettings or difficulty or rules
void PlaySound(object sender, EventArgs e) {
    scherm.Controls.Remove(SoundButtonOff);
    scherm.Controls.Add(SoundButtonOn);
    player.PlayLooping();
    playingSound = true;
}

void menu() {
    if (!playingSound) {
        playingSound = true;
        player.PlayLooping();
    }

    DeleteAllWidgets();

    // scherm.BackColor = Color.FromArgb(32, 32, 32);
    scherm.BackgroundImage = Image.FromFile("../../../images/background.png");
    scherm.BackgroundImageLayout = ImageLayout.Stretch;
    scherm.Controls.Add(MENU);
    scherm.Controls.Add(reversi);

    scherm.Controls.Add(newGame);
    scherm.Controls.Add(rules);
    scherm.Controls.Add(SoundButtonOn);
    scherm.Controls.Add(difficulty);
    scherm.Controls.Add(Continue);
    scherm.Controls.Add(settings);
}

DifficultyMenuButton.MouseClick += BackToMenu;
void BackToMenu(object sender, EventArgs e) {
    menu();
}

void DifficultyClickButton(object sender, EventArgs e) {
    // checks which button is pressed
    if (sender == Easy) {
        amountOfCells = 6;
    } else if (sender == Medium) {
        amountOfCells = 8;
    } else if (sender == Hard) {
        amountOfCells = 10;
    }

    // reset the squares with a width that is just chosen
    squares = new int[amountOfCells, amountOfCells];

    cellWidth = boardWidth/amountOfCells;
    stoneRadius = (boardWidth/amountOfCells)/2;

    startGame();
}

Continue.MouseClick += continueGame;
difficulty.MouseClick += Difficulty;
DifficultyInGame.MouseClick += Difficulty;
void Difficulty(object sender, EventArgs e) {
    showDifficulty();
}

void showDifficulty() {
    if (!playingSound) {
        playingSound = true;
        player.PlayLooping();
    }
    DeleteAllWidgets();

    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(DifficultyText);
    scherm.Controls.Add(Easy);
    scherm.Controls.Add(Medium);
    scherm.Controls.Add(Hard);
    scherm.Controls.Add(Continue);
    scherm.Controls.Add(SoundButtonOn);

    Easy.MouseClick += DifficultyClickButton;
    Medium.MouseClick += DifficultyClickButton;
    Hard.MouseClick += DifficultyClickButton;
}

ViableLocation.MouseClick += changeViableLocationSettings;
void changeViableLocationSettings(object sender, EventArgs e) {
    if (showViableLocation) {
        showViableLocation = false;
    } else {
        showViableLocation = true;
    }
}

Multiplayer.MouseClick += changeMultiplayer;
void changeMultiplayer(object sender, EventArgs e) {
    if (MultiplayerBool) {
        MultiplayerBool = false;
    } else {
        MultiplayerBool = true;
    }
}

hamburgerMenu.MouseClick += showHamburgerMenu;
void showHamburgerMenu(object sender, EventArgs e) {
    // if you click on the hamburger menu it adds the label and the buttons and changes the location of the icon
    if (HamburgerBool) {
        HamburgerBool = false;
        hamburgerMenu.Location = new Point(50, 10);
        scherm.Controls.Add(newGameBtn);
        scherm.Controls.Add(MenuButton);
        scherm.Controls.Add(DifficultyInGame);
        scherm.Controls.Add(SettingsInGame);
        scherm.Controls.Add(HamburgerLabel);
    } else {
        // removes the label and buttons and changes the location of the icon
        HamburgerBool = true;
        hamburgerMenu.Location = new Point(10, 10);
        scherm.Controls.Remove(newGameBtn);
        scherm.Controls.Remove(MenuButton);
        scherm.Controls.Remove(DifficultyInGame);
        scherm.Controls.Remove(SettingsInGame);
        scherm.Controls.Remove(HamburgerLabel);
    }
}

void DeleteAllWidgets() {
    try {
        // deletes all widgets and makes sure it doesn't crash
        scherm.Controls.Remove(ImageBoxImage);
        scherm.Controls.Remove(newGameBtn);
        scherm.Controls.Remove(blueStonesLabel);
        scherm.Controls.Remove(redStonesLabel);
        scherm.Controls.Remove(PlayerStonesTextBlue);
        scherm.Controls.Remove(PlayerStonesTextRed); 
        scherm.Controls.Remove(CurrentPlayer);
        scherm.Controls.Remove(CurrentPlayerLabel);
        scherm.Controls.Remove(MENU);
        scherm.Controls.Remove(reversi);
        scherm.Controls.Remove(newGame);    
        scherm.Controls.Remove(rules);
        scherm.Controls.Remove(SoundButtonOn);
        scherm.Controls.Remove(SoundButtonOff);
        scherm.Controls.Remove(difficulty);
        scherm.Controls.Remove(DifficultyMenuButton);
        scherm.Controls.Remove(DifficultyText);
        scherm.Controls.Remove(Easy);
        scherm.Controls.Remove(Medium);
        scherm.Controls.Remove(Hard);
        scherm.Controls.Remove(Continue);
        scherm.Controls.Remove(settings);
        scherm.Controls.Remove(ViableLocation);
        scherm.Controls.Remove(SETTINGS);
        scherm.Controls.Remove(CheckViableLocationLabel);
        scherm.Controls.Remove(RulesText);
        scherm.Controls.Remove(PlayingRules);
        scherm.Controls.Remove(NewGameText);
        scherm.Controls.Remove(hamburgerMenu);
        scherm.Controls.Remove(HamburgerLabel);
        scherm.Controls.Remove(newGameBtn);
        scherm.Controls.Remove(MenuButton);
        scherm.Controls.Remove(DifficultyInGame);
        scherm.Controls.Remove(HamburgerLabel);
        scherm.Controls.Remove(SettingsInGame);
        scherm.Controls.Remove(HighscoreLabel);
        scherm.Controls.Remove(Multiplayer);
        scherm.Controls.Remove(MultiplayerLabel);
    } catch{}
}

rules.MouseClick += Rules;
void Rules(object sender, EventArgs e) {
    if (!playingSound) {
        playingSound = true;
        player.PlayLooping();
    }
    DeleteAllWidgets();
    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(Continue);
    scherm.Controls.Add(RulesText);
    scherm.Controls.Add(PlayingRules);
    scherm.Controls.Add(SoundButtonOn);
}

settings.MouseClick += Settings;
SettingsInGame.MouseClick += Settings;
void Settings(object sender, EventArgs e) {
    if (!playingSound) {
        playingSound = true;
        player.PlayLooping();
    }
    DeleteAllWidgets();
    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(Continue);
    scherm.Controls.Add(ViableLocation);
    scherm.Controls.Add(SETTINGS);
    scherm.Controls.Add(CheckViableLocationLabel);
    scherm.Controls.Add(Multiplayer);
    scherm.Controls.Add(MultiplayerLabel);
    scherm.Controls.Add(SoundButtonOn);
}

void continueGame(object sender, EventArgs e) {
    DeleteAllWidgets();
    player.Stop();
    // makes sure the sound stops playing and the hamburger menu is not extended
    playingSound = false;
    HamburgerBool = true;
    hamburgerMenu.Location = new Point(10, 10);
    hamburgerMenu.ForceClose();

    scherm.Controls.Add(ImageBoxImage);
    scherm.Controls.Add(CurrentPlayerLabel);
    scherm.Controls.Add(blueStonesLabel);
    scherm.Controls.Add(redStonesLabel);
    scherm.Controls.Add(PlayerStonesTextBlue);
    scherm.Controls.Add(PlayerStonesTextRed);
    scherm.Controls.Add(CurrentPlayer);
    scherm.Controls.Add(NewGameText);
    scherm.Controls.Add(hamburgerMenu);
    scherm.Controls.Add(HighscoreLabel);

    UpdateBoard();    
}

void startGame() {
    amountOfTurns = 0;
    player.Stop();
    // makes sure the sound stops playing and the hamburger menu is not extended
    playingSound = false;
    HamburgerBool = true;
    hamburgerMenu.Location = new Point(10, 10);
    hamburgerMenu.ForceClose();

    // makes a new board and populates it
    for (int i = 0; i < squares.GetLength(0); i++)
    {
        for (int j = 0; j < squares.GetLength(1); j++)
        {
            squares[i,j] = 0;
        }
    }
    squares[(amountOfCells/2-1), (amountOfCells/2-1)] = squares[(amountOfCells/2), (amountOfCells/2)] = 1; //populate board with middle pieces, works for (almost) every board width
    squares[(amountOfCells/2-1), (amountOfCells/2)] = squares[(amountOfCells/2), (amountOfCells/2 -1)] = 2; //populate board with middle pieces, works for (almost) every board width
    currentPlayer = 0;

    DeleteAllWidgets();

    scherm.Controls.Add(ImageBoxImage);
    scherm.Controls.Add(CurrentPlayerLabel);
    
    scherm.Controls.Add(blueStonesLabel);
    scherm.Controls.Add(redStonesLabel);
    scherm.Controls.Add(PlayerStonesTextBlue);
    scherm.Controls.Add(PlayerStonesTextRed);
    
    scherm.Controls.Add(CurrentPlayer);
    scherm.Controls.Add(NewGameText);
    scherm.Controls.Add(hamburgerMenu);
    scherm.Controls.Add(HighscoreLabel);

    UpdateBoard();
}

menu();
Application.Run(scherm);