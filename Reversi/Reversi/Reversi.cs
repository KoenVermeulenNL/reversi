using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(700, 600);

int boardWidth = 400; 

int amountOfCells = 6; // 6 = board of 6x6

double cellWidth = boardWidth/amountOfCells;
double stoneRadius = (boardWidth/amountOfCells)/2;

int currentPlayer = 0; // even = red odd = blue

//UI Elements
Button newGameBtn = new Button();
newGameBtn.Text = "Nieuw Spel";
newGameBtn.BackColor = Color.White;

Button helpBtn = new Button();
helpBtn.Text = "Help";
helpBtn.BackColor = Color.White;

Label blueStonesLabel = new Label();
blueStonesLabel.Text = $"{0} Blue Stones";
Label redStonesLabel = new Label();
redStonesLabel.Text = $"{0} Red Stones";

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

// Add all elements to the screen
scherm.Controls.Add(newGameBtn);
scherm.Controls.Add(helpBtn);
scherm.Controls.Add(blueStonesLabel);
scherm.Controls.Add(redStonesLabel);

// Create Image Box
Bitmap ImageBoxDrawing = new Bitmap(400, 400);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);

// create Label
Label ImageBoxImage = new Label();
scherm.Controls.Add(ImageBoxImage);
ImageBoxImage.Location = new Point(100, 10);

// Image Size Variable
ImageBoxImage.Size = new Size(400, 400);

ImageBoxImage.BackColor = Color.White;
ImageBoxImage.Image = ImageBoxDrawing;

int GetPlayer(){
    return currentPlayer % 2 + 1; //1 = even - red, 2 = odd - blue, +1 to corrrect for 0 = empty
}

void createBoard(){
    int X = 0;
    int Y = 0;

    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells;j++) {
            // if (j%2 == 0) {
            //     ImageBoxDrawer.DrawRectangle(Pens.White, X, Y, (int)cellWidth, (int)cellWidth);
            // } else {
            //     
            // }
            if (i%2 == 0) {
                if (j%2 == 0) ImageBoxDrawer.FillRectangle(Brushes.White, X, Y, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.LightGray, X, Y, (int)cellWidth, (int)cellWidth);
            } else {
                if (j%2 == 0) ImageBoxDrawer.FillRectangle(Brushes.LightGray, X, Y, (int)cellWidth, (int)cellWidth);
                else ImageBoxDrawer.FillRectangle(Brushes.White, X, Y, (int)cellWidth, (int)cellWidth);

            }


            X += (int)cellWidth;
        }
        X = 0;
        Y += (int)cellWidth;
    }


}

void UpdateBoard() {
    //logic


    //Dag Hugooo
    //Niet al te veel bijzonder werk, gebruik deze functie: CreateStone(CELLX, CELLY);, MET DE PIXEL TO CELL FUNCTIE
    //en gebruik squares[,]
    //Voor de aanduiding welke moves mogelijk zijn kun je deze functie gebruiken: CheckIfViableLocation(CELLX, CELLY)
    //succes
    createBoard();

    scherm.Text = Convert.ToString(GetPlayer());

    for (int col = 0; col < squares.GetLength(1); col++) {
        for (int row = 0; row < squares.GetLength(0); row++) {
            if (squares[col, row] == 1) {
                ImageBoxDrawer.FillEllipse(Brushes.Red, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth, (int)cellWidth);
            } else if (squares[col, row] == 2) {
                ImageBoxDrawer.FillEllipse(Brushes.Blue, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth, (int)cellWidth);
            }

            if (CheckIfViableLocation(row, col)) {
                ImageBoxDrawer.DrawEllipse(Pens.Purple, (row*(int)cellWidth), (col*(int)cellWidth), (int)cellWidth, (int)cellWidth);
            }
        }
    }

    ImageBoxImage.Invalidate();
}

UpdateBoard();

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

    int currentPosition = (amountOfCells - cellY - 1); //current position in diagonal, not the coordinates
    if((amountOfCells - cellX - 1) > (amountOfCells - cellY - 1)){
        //if the horizontal offset is greater than the vertical offset, the current position is the same as the vertical offset
        //if both offsets are equal, it doesn't matter wich to pick
        currentPosition = (amountOfCells - cellX - 1);
    }

    int[] currentDiagonalLine = {};
    Array.Resize(ref currentDiagonalLine, amountOfCells);
    for (int i = 0; i < amountOfCells; i++) //Diagonal has also board widht as its length
    {
        if((amountOfCells - cellX - 1) > (amountOfCells - cellY - 1)){ //reverse the grid, so the same logic of Left to Right can be used
            //Pretty proud of this one tbh, there is probably another way, but this works ;)
            if(cellX - (currentPosition - i) < amountOfCells && cellX - (currentPosition - i) > 0){
                currentDiagonalLine[i] = squares[i, cellX - (currentPosition - i)]; //first row, then column
            }   else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
        } else {
            if(cellY - (currentPosition - i) < amountOfCells && cellX - (currentPosition - i) < amountOfCells && (cellY - (currentPosition - i) > 0 && cellX - (currentPosition - i) > 0)){
                currentDiagonalLine[i] = squares[cellY - (currentPosition - i), cellX - (currentPosition - i)]; //first row, then column
            }   else {
                currentDiagonalLine[i] = 0; //diagonal line is smaller than board width, so populate with 0
            }
   
        }
    }
    if(cellY == 4 && cellX == 4){
        Debug.WriteLine(currentPosition);
        Debug.WriteLine(String.Join(";", currentDiagonalLine));
    }

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
    //int checkedDiagonalRTL = CheckDiagonalRightToLeft(cellX, cellY);

    if(checkedHorizontal != 0){
        return true;
    }
    if(checkedVertical != 0){
        return true;
    }
    if(checkedDiagonalLTR != 0){
        return true;
    }
    /*if(checkedDiagonalRTL != 0){
        return true;
    }*/
    return false; //0 means not possible
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
    /*int diagonalRTLIndex = CheckDiagonalRightToLeft(cellX, cellY);
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
    }*/
};

int PixelToCell(int mousePixel) {
    //Determin in which cell the location of the pixel is
    //Cell 0 - amountofcells-1
    return (int)Math.Floor(mousePixel / cellWidth);
}

ImageBoxImage.MouseClick += ImageBoxImage_MouseClick;

void ImageBoxImage_MouseClick(object sender, MouseEventArgs mea) {
    //Bij Klik update board

    //Determin which cell
    int cellX = PixelToCell(mea.X);
    int cellY = PixelToCell(mea.Y);

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

Application.Run(scherm);