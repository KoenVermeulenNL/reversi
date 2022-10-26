using System;
using System.Drawing;
using System.Windows.Forms;

class HamburgerMenu : Button {
    bool Closed = true;
    
    public HamburgerMenu() {
        // sets initail variables
        this.MouseClick += changeLayout;
        this.Size = new Size(20, 20);
        this.Image = Image.FromFile("../../../HamburgerClosed.png");
        this.BackColor = Color.Transparent;
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.FlatAppearance.MouseOverBackColor = Color.Transparent;
        this.FlatAppearance.MouseDownBackColor = Color.Transparent;

        this.checkLayout();
    }

    public void ForceClose() {
        Closed = true;
        checkLayout();
    }

    private void changeLayout(object sender, EventArgs e) {
        checkLayout();
    }

    private void checkLayout() {
        // if the menu is closed show three stripes
        if (Closed) {
            this.Image = Image.FromFile("../../../HamburgerClosed.png");
            this.BackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Closed = false;
        } else { // if its open show a cross
            this.Image = Image.FromFile("../../../HamurgerOpen.png");
            this.BackColor = Color.FromArgb(120, 27, 58, 133);
            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 27, 58, 133);
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(120, 27, 58, 133); 
            Closed = true;
        }
    }
}