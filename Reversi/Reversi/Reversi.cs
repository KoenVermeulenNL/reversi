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



void UpdateBoard() {
    //logic




    
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

void CreateStone(int cellX, int cellY, int ownerId){
    //plaats nieuw steen
    ConvertXYToListIndex(cellX, cellY);
}
int ConvertXYToListIndex(int x, int y){
    return 5;
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

    
}

Application.Run(scherm);
