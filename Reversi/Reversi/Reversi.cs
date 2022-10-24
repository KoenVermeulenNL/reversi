using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

int SCREEN_WIDTH = 600;
int SCREEN_HEIGHT = 700;

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.ClientSize = new Size(SCREEN_WIDTH, SCREEN_HEIGHT);

int boardWidth = 400; 
System.Media.SoundPlayer player = new System.Media.SoundPlayer();
player.SoundLocation = "backgroundmusic.wav";
player.PlayLooping();

System.Media.SoundPlayer GoodplayPlayer = new System.Media.SoundPlayer();
GoodplayPlayer.SoundLocation = "Good-Placement.wav";

System.Media.SoundPlayer BadplayPlayer = new System.Media.SoundPlayer();
BadplayPlayer.SoundLocation = "Bad-Placement.wav";


int amountOfCells = 6; // 6 = board of 6x6

double cellWidth = boardWidth/amountOfCells;
double stoneRadius = (boardWidth/amountOfCells)/2;
bool showViableLocation = true;
bool playingSound = true;

int currentPlayer = 0; // even = red odd = blue

//UI Elements
Button newGameBtn = new Button();
newGameBtn.BackColor = Color.Transparent;
newGameBtn.Text = "New Game";


int amountRed = 0;
int amountBlue = 0;

Label blueStonesLabel = new Label();
blueStonesLabel.Text = $"{amountBlue}";
blueStonesLabel.Location = new Point(100, 410);
// blueStonesLabel.Size = new Size(15, 20);
blueStonesLabel.AutoSize = true;
blueStonesLabel.BackColor = Color.Transparent;
Label redStonesLabel = new Label();
redStonesLabel.Text = $"{amountRed}";
redStonesLabel.Location = new Point(100, 440);
redStonesLabel.AutoSize = true;
// redStonesLabel.Size = new Size(20, 20);
redStonesLabel.BackColor = Color.Transparent;

Label PlayerStonesTextBlue = new Label();
PlayerStonesTextBlue.Text = "Blue stones";
PlayerStonesTextBlue.Location = new Point(120, 410);
PlayerStonesTextBlue.BackColor = Color.Transparent;
Label PlayerStonesTextRed = new Label();
PlayerStonesTextRed.Text = "Red stones";
PlayerStonesTextRed.Location = new Point(120, 440);
PlayerStonesTextRed.BackColor = Color.Transparent;
Label CurrentPlayer = new Label();
CurrentPlayer.BackColor = Color.Transparent;
CurrentPlayer.Text = "Current Player:";
CurrentPlayer.Location = new Point(300, 410);

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
ResizeArray(ref squares, amountOfCells, amountOfCells); //Resize 2D array to be used for every board width

squares[(amountOfCells/2-1), (amountOfCells/2-1)] = squares[(amountOfCells/2), (amountOfCells/2)] = 1; //populate board with middle pieces, works for (almost) every board width
squares[(amountOfCells/2-1), (amountOfCells/2)] = squares[(amountOfCells/2), (amountOfCells/2 -1)] = 2; //populate board with middle pieces, works for (almost) every board width

// Create Image Box
Bitmap ImageBoxDrawing = new Bitmap(boardWidth, boardWidth);
Bitmap CurrentPlayerBitmap = new Bitmap(30, 30);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);
Graphics CurrentPlayerDrawer = Graphics.FromImage(CurrentPlayerBitmap);

// create Label
Label ImageBoxImage = new Label();
ImageBoxImage.Location = new Point(100, 10);

Label CurrentPlayerLabel = new Label();
CurrentPlayerLabel.BackColor = Color.Transparent;
CurrentPlayerLabel.Location = new Point(400, 410);

scherm.Controls.Add(ImageBoxImage);
scherm.Controls.Add(CurrentPlayerLabel);

Font LargeFont = new Font("Times New Roman", 44);
// variabelen voor settings
Label SETTINGS = new Label();
SETTINGS.ForeColor = Color.FromArgb(30, 78, 199);
SETTINGS.BackColor = Color.Transparent;
SETTINGS.Text = "SETTINGS";
SETTINGS.AutoSize = true;
SETTINGS.Font = LargeFont;
SETTINGS.Location = new Point(150, 20);

Label CheckViableLocationLabel = new Label();
CheckViableLocationLabel.Font = new Font("Times New Roman", 20);
CheckViableLocationLabel.AutoSize = true;
CheckViableLocationLabel.Location = new Point(122, 100);
CheckViableLocationLabel.BackColor = Color.Transparent;
CheckViableLocationLabel.Height = 46;
CheckViableLocationLabel.Text = "Show viable Locations";

ToggleButton ViableLocation = new ToggleButton(showViableLocation);
ViableLocation.Location = new Point(380, 95);

// variabelen voor Difficulty:
Button DifficultyMenuButton = new Button();
DifficultyMenuButton.Font = new Font("Times New Roman", 25);
DifficultyMenuButton.BackColor = Color.Transparent;
DifficultyMenuButton.Size = new Size(182, 52);
DifficultyMenuButton.Location = new Point(209, 570);
DifficultyMenuButton.Text = "Menu";

Label DifficultyText = new Label();
DifficultyText.ForeColor = Color.FromArgb(30, 78, 199);
DifficultyText.BackColor = Color.Transparent;
DifficultyText.Text = "Difficulty";
DifficultyText.AutoSize = true;
DifficultyText.Location = new Point(170, 20);
DifficultyText.Font = LargeFont;

Button Easy = new Button();
Easy.Font = new Font("Times New Roman", 20);
Easy.BackColor = Color.Transparent;
Easy.Size = new Size(260, 52);
Easy.Location = new Point(170, 200);
Easy.Text = "Easy Game: 6x6";

Button Medium = new Button();
Medium.Font = new Font("Times New Roman", 20);
Medium.BackColor = Color.Transparent;
Medium.Size = new Size(260, 52);
Medium.Location = new Point(170, 260);
Medium.Text = "Medium Game: 8x8";

Button Hard = new Button();
Hard.Font = new Font("Times New Roman", 20);
Hard.BackColor = Color.Transparent;
Hard.Size = new Size(260, 52);
Hard.Location = new Point(170, 320);
Hard.Text = "Hard Game: 10x10";

// Atributes for the menu
Label MENU = new Label();
MENU.ForeColor = Color.FromArgb(30, 78, 199);
MENU.BackColor = Color.Transparent;
MENU.Text = "MENU";
MENU.Size = new Size(200,70);
MENU.Location = new Point(200, 20);
MENU.Font = LargeFont;
Font mediumFont = new Font("Times New Roman", 18);
Label reversi = new Label();
reversi.Text = "Reversi";
reversi.ForeColor = Color.FromArgb(30, 78, 199);
reversi.BackColor = Color.Transparent;
reversi.Location = new Point(250, 100);
reversi.Font = mediumFont;
reversi.Size = new Size(100, 50);

Button rules = new Button();
rules.Font = new Font("Times New Roman", 25);
rules.BackColor = Color.Transparent;
rules.Size = new Size(182, 52);
rules.Location = new Point(((scherm.Width-16)/2)+5, 300);
rules.Text = " Rules ";

Button difficulty = new Button();
difficulty.Font = new Font("Times New Roman", 25);
difficulty.BackColor = Color.Transparent;
difficulty.Size = new Size(182, 52);
difficulty.Location = new Point(108, 360);
difficulty.Text = "Difficulty";

Button Continue = new Button();
Continue.Font = new Font("Times New Roman", 25);
Continue.BackColor = Color.Transparent;
Continue.Size = new Size(182, 52);
Continue.Location = new Point(((scherm.Width-16)/2)-Continue.Width/2, 510);
Continue.Text = "Continue";

Button settings = new Button();
settings.Font = new Font("Times New Roman", 25);
settings.BackColor = Color.Transparent;
settings.Size = new Size(182, 52);
settings.Location = new Point(((scherm.Width-16)/2)+5, 360);
settings.Text = "Settings";

Button SoundButtonOff = new Button();
SoundButtonOff.Image = Image.FromFile("volume-off-indicator.png");
SoundButtonOff.BackColor = Color.Transparent;
SoundButtonOff.FlatStyle = FlatStyle.Flat;
SoundButtonOff.Size = new Size(42, 42);
SoundButtonOff.Location = new Point(SCREEN_WIDTH-45, SCREEN_HEIGHT-45);

Button SoundButtonOn = new Button();
SoundButtonOn.Image = Image.FromFile("speaker-filled-audio-tool.png");
SoundButtonOn.BackColor = Color.Transparent;
SoundButtonOn.FlatStyle = FlatStyle.Flat;
SoundButtonOn.Size = new Size(42, 42);
SoundButtonOn.Location = new Point(SCREEN_WIDTH-45, SCREEN_HEIGHT-45);

Button MenuButton = new Button();
MenuButton.BackColor = Color.Transparent;
MenuButton.Location = new Point(0, 20);
MenuButton.Text = "Menu";


SoundButtonOn.Click += StopSound;
SoundButtonOff.Click += PlaySound;


Button newGame = new Button();
newGame.Font = new Font("Times New Roman", 25);
newGame.BackColor = Color.Transparent;
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
ImageBoxImage.BackColor = Color.White;
ImageBoxImage.Image = ImageBoxDrawing;

int GetPlayer(){
    return currentPlayer % 2 + 1; //1 = even - red, 2 = odd - blue, +1 to corrrect for 0 = empty
}

void createBoard(){

    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells;j++) {
            if (i%2==1) {
                if (j%2==0) ImageBoxDrawer.FillRectangle(Brushes.LightGray, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.White, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
            } else if (i%2==0) {
                if (j%2==0) ImageBoxDrawer.FillRectangle(Brushes.White, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.LightGray, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
            }

        }
    }

    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells;j++) {
            ImageBoxDrawer.DrawRectangle(Pens.Black, i*(int)cellWidth, j*(int)cellWidth, (int)cellWidth, (int)cellWidth);
        }
    }

    ImageBoxDrawer.DrawRectangle(Pens.Black, 0, 0, ImageBoxImage.Width-1, ImageBoxImage.Height-1);
}

void UpdateBoard() {
    //logic


    //Dag Hugooo
    //Niet al te veel bijzonder werk, gebruik deze functie: CreateStone(CELLX, CELLY);, MET DE PIXEL TO CELL FUNCTIE
    //en gebruik squares[,]
    //Voor de aanduiding welke moves mogelijk zijn kun je deze functie gebruiken: CheckIfViableLocation(CELLX, CELLY)
    //succes

    createBoard();

    int amountOfTrueLocations = 0;

    for (int col = 0; col < squares.GetLength(1); col++) {
        for (int row = 0; row < squares.GetLength(0); row++) {
            if (squares[col, row] == 1) {
                ImageBoxDrawer.FillEllipse(Brushes.Red, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            } else if (squares[col, row] == 2) {
                ImageBoxDrawer.FillEllipse(Brushes.Blue, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            }

            if (CheckIfViableLocation(row, col)) {
                amountOfTrueLocations++;
            }
            if (showViableLocation && CheckIfViableLocation(row,col)) {
                ImageBoxDrawer.DrawEllipse(Pens.Purple, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth-1, (int)cellWidth-1);
            }
        }
    }


    amountRed = CountPlayer(1);
    amountBlue = CountPlayer(2);

    redStonesLabel.Text = $"{amountRed}";
    blueStonesLabel.Text = $"{amountBlue}";

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
    
    if (amountOfTrueLocations==0) {
        if (amountBlue == amountRed) {
            MessageBox.Show("Its a tie!! ");
            menu();
        }
        else if (amountBlue > amountRed) {
            MessageBox.Show("Blue wins!! ");
            menu();
        } else {
            MessageBox.Show("Red wins!!");
            menu();
        }
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
    /*if(cellX == 1 && cellY == 4){
    Debug.WriteLine(String.Join(";", rowList));
    Debug.WriteLine(squares[cellY, cellX]);
    Debug.WriteLine(squares[cellY, cellX + 1]);
    Debug.WriteLine(squares[cellY, cellX + 2]);
    }*/

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

int CheckDiagonal(int cellX, int cellY){
    //4 directions
    int diagonalLTR = CheckDiagonalLeftToRight(cellX, cellY);
    //int diagonalRTL = CheckDiagonalRightToLeft(cellX, cellY);
    
    if(diagonalLTR != 0){
        return diagonalLTR;
    }
    /*if(diagonalRTL != 0){
        return diagonalRTL;
    }*/
    return 0; //0 means not possible
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

scherm.ClientSizeChanged += change_size;
void change_size(object sender, EventArgs e) {
    Debug.WriteLine(scherm.Width);
    Debug.WriteLine(scherm.Height);
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
    ImageBoxImage.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, 10);
    blueStonesLabel.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, ImageBoxImage.Height+15);
    redStonesLabel.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2, ImageBoxImage.Height+blueStonesLabel.Height+15);
    PlayerStonesTextBlue.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2+blueStonesLabel.Width+10, ImageBoxImage.Height+15);
    PlayerStonesTextRed.Location = new Point((scherm.Width/2)-ImageBoxImage.Width/2+redStonesLabel.Width+10, ImageBoxImage.Height+blueStonesLabel.Height+15);
    CurrentPlayerLabel.Location = new Point((scherm.Width/2)+ImageBoxImage.Width/2-CurrentPlayerLabel.Width, ImageBoxImage.Height+15); 
    CurrentPlayer.Location = new Point((scherm.Width/2)+ImageBoxImage.Width/2-CurrentPlayerLabel.Width-CurrentPlayer.Width - 10, ImageBoxImage.Height+15); 
    Continue.Location = new Point(((scherm.Width-16)/2)-Continue.Width/2, 510);
    settings.Location = new Point(((scherm.Width-16)/2)+5, 360);
    SETTINGS.Location = new Point(((scherm.Width-16)/2)-SETTINGS.Width/2, 20);
    CheckViableLocationLabel.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2, 100);
    ViableLocation.Location = new Point(((scherm.Width-16)/2)-(CheckViableLocationLabel.Width+ViableLocation.Width)/2+CheckViableLocationLabel.Width, 95);
}


ImageBoxImage.MouseClick += ImageBoxImage_MouseClick;
void ImageBoxImage_MouseClick(object sender, MouseEventArgs mea) {
    //Bij Klik update board

    //Determin which cell
    int cellX = PixelToCell(mea.X);
    int cellY = PixelToCell(mea.Y);
    
    if (CheckIfViableLocation(cellX, cellY)) {
        GoodplayPlayer.Play();
    } else {
        BadplayPlayer.Play();
    }

    CreateStone(cellX, cellY);

    
    UpdateBoard();
}

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

void StopSound(object sender, EventArgs e) {
    scherm.Controls.Remove(SoundButtonOn);
    scherm.Controls.Add(SoundButtonOff);
    player.Stop();
}

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
    try {
        scherm.Controls.Remove(ImageBoxImage);
        scherm.Controls.Remove(newGameBtn);
        scherm.Controls.Remove(blueStonesLabel);
        scherm.Controls.Remove(redStonesLabel);
        scherm.Controls.Remove(PlayerStonesTextBlue);
        scherm.Controls.Remove(PlayerStonesTextRed);
        scherm.Controls.Remove(MenuButton);
        scherm.Controls.Remove(CurrentPlayer);
        scherm.Controls.Remove(CurrentPlayerLabel);
        scherm.Controls.Remove(DifficultyMenuButton);
        scherm.Controls.Remove(DifficultyText);
        scherm.Controls.Remove(Easy);
        scherm.Controls.Remove(Medium);
        scherm.Controls.Remove(Hard);
        scherm.Controls.Remove(ViableLocation);
        scherm.Controls.Remove(SETTINGS);
        scherm.Controls.Remove(CheckViableLocationLabel);

    } catch {
        // pass
    }

    // scherm.BackColor = Color.FromArgb(32, 32, 32);
    scherm.BackgroundImage = Image.FromFile("background.png");
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

void BackToMenu(object sender, EventArgs e) {
    menu();
}

void DifficultyClickButton(object sender, EventArgs e) {
    if (sender == Easy) {
        amountOfCells = 6;
    } else if (sender == Medium) {
        amountOfCells = 8;
    } else if (sender == Hard) {
        amountOfCells = 10;
    }

    squares = new int[amountOfCells, amountOfCells];

    cellWidth = boardWidth/amountOfCells;
    stoneRadius = (boardWidth/amountOfCells)/2;

    startGame();
}

Continue.MouseClick += continueGame;
difficulty.MouseClick += Difficulty;
void Difficulty(object sender, EventArgs e) {
    try{
        scherm.Controls.Remove(MENU);
        scherm.Controls.Remove(reversi);
        scherm.Controls.Remove(newGame);    
        scherm.Controls.Remove(rules);
        scherm.Controls.Remove(SoundButtonOn);
        scherm.Controls.Remove(difficulty);
        scherm.Controls.Remove(settings);
        scherm.Controls.Remove(ViableLocation);
        scherm.Controls.Remove(SETTINGS);
        scherm.Controls.Remove(CheckViableLocationLabel);
    } catch{}

    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(DifficultyText);
    scherm.Controls.Add(Easy);
    scherm.Controls.Add(Medium);
    scherm.Controls.Add(Hard);
    scherm.Controls.Add(Continue);

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

    Debug.WriteLine(showViableLocation);
}

void DeleteAllWidgets() {
    try {
        scherm.Controls.Remove(MENU);
        scherm.Controls.Remove(reversi);
        scherm.Controls.Remove(newGame);    
        scherm.Controls.Remove(rules);
        scherm.Controls.Remove(SoundButtonOn);
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
    } catch{}
}

void Rules(object sender, EventArgs e) {
    DeleteAllWidgets();
    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(Continue);
}

rules.MouseClick += Rules;
DifficultyMenuButton.MouseClick += BackToMenu;
settings.MouseClick += Settings;
void Settings(object sender, EventArgs e) {
    DeleteAllWidgets();
    scherm.Controls.Add(DifficultyMenuButton);
    scherm.Controls.Add(Continue);
    scherm.Controls.Add(ViableLocation);
    scherm.Controls.Add(SETTINGS);
    scherm.Controls.Add(CheckViableLocationLabel);
}

void continueGame(object sender, EventArgs e) {
    DeleteAllWidgets();

    scherm.Controls.Add(ImageBoxImage);
    scherm.Controls.Add(CurrentPlayerLabel);
    scherm.Controls.Add(newGameBtn);
    scherm.Controls.Add(blueStonesLabel);
    scherm.Controls.Add(redStonesLabel);
    scherm.Controls.Add(PlayerStonesTextBlue);
    scherm.Controls.Add(PlayerStonesTextRed);
    scherm.Controls.Add(MenuButton);
    scherm.Controls.Add(CurrentPlayer);

    UpdateBoard();    
}

void startGame() {
    player.Stop();
    playingSound = false;

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
    scherm.Controls.Add(newGameBtn);
    scherm.Controls.Add(blueStonesLabel);
    scherm.Controls.Add(redStonesLabel);
    scherm.Controls.Add(PlayerStonesTextBlue);
    scherm.Controls.Add(PlayerStonesTextRed);
    scherm.Controls.Add(MenuButton);
    scherm.Controls.Add(CurrentPlayer);

    UpdateBoard();
}

menu();

Application.Run(scherm);