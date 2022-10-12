using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(700, 600);

int boardWidth = 400;

int amountOfCells = 4; // 6 = board of 6x6
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

int[] squares = {}; // 0 = empty, 1 = red, 2 = blue
Array.Resize(ref squares, (amountOfCells * amountOfCells));

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
    return currentPlayer % 2 + 1; //0 = even - red, 1 = odd - blue, +1 to corrrect for 0 = empty
}

void UpdateBoard() {
    //logic


    //HUGOOOOOOOOOOOOOOOOOOOOOOOOO Gebruik squares én GetViableLocations(), kijk er ff naar

    
    scherm.Invalidate();
    int x = 0;
    int y = 0;
    for (int i = 0; i < amountOfCells; i++) {
        for (int j = 0; j< amountOfCells; j++) {
            j+=i;
            if (j%2 == 0) {
                ImageBoxDrawer.FillRectangle(Brushes.Black, x, y, (int)(x+cellWidth), (int)(y+cellWidth));
            } else if (j%2 != 0) {
                ImageBoxDrawer.FillRectangle(Brushes.White, x, y, (int)(x+cellWidth), (int)(y+cellWidth));
            }

            x += (int)cellWidth;
        }
        x=0;
        y+= (int)cellWidth;
    }
}

UpdateBoard();

void CheckCaptured(int index){
    //check between, end on end of row
    int rowIndex = index/ amountOfCells;
    int indexesTillEndOfRow = amountOfCells - rowIndex;
    for (int zeroIndex = 1; zeroIndex < indexesTillEndOfRow; zeroIndex++)
    {
        int squareChecked = squares[index + zeroIndex];
        int previousSquareChecked = squares[index + zeroIndex - 1];
        if(squareChecked == GetPlayer() && (previousSquareChecked != GetPlayer() && previousSquareChecked != 0)){
            break; //gotten to own stone
        }
    }
}

CheckCaptured(7);

void CheckHorizontal(){
    
}

void CheckVertical(int index){
    int nextCell = index + amountOfCells;
    //nextCell - amountOfCells;
}

void CheckDiagonal(int index){
    int nextCell = index + (amountOfCells -1); //next (or previous) row - 1 --> checks diagonal
    //Check
    nextCell = index + (amountOfCells + 1);
    //Check
}



bool CheckIfViableLocation(int index){
    //Check if index is a viable location to place a stone (sluit een of meerdere andere stones in)
    if(squares[index] != 0){
        return false; // if stone is present
    }


    return true;
}

void CreateStone(int cellX, int cellY){
    //plaats nieuw steen
    int index = ConvertXYToListIndex(cellX, cellY);
    int currentSquare = squares[index];

    //not possible if stone is already present --> has to be 0
    if(currentSquare == 0 && CheckIfViableLocation(index)){
        squares[index] = GetPlayer(currentPlayer); //0 = empty, so + 1
    };
}

int ConvertXYToListIndex(int x, int y){
    //Hier is ervan uitgegaan dat x en y starten op 0
    return y*amountOfCells + x; //rijen * aantal cells in de rij + overgen: x
}

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
    
}

Application.Run(scherm);
