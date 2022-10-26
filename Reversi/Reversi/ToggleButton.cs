using System;
using System.Drawing;
using System.Windows.Forms;
class ToggleButton : Button{
    bool onOff = true;

    public ToggleButton(bool onOff) {
        // sets initial variables
        this.MouseClick += changeLayout;
        this.AutoSize = true;
        this.Image = Image.FromFile("../../../toggle.png");
        this.BackColor = Color.Transparent;
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.FlatAppearance.MouseOverBackColor = Color.Transparent;
        this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        this.onOff = onOff;

        this.checkLayout();
    }

    private void changeLayout(object sender, EventArgs e) {
        checkLayout();
    }
    private void checkLayout() {
        // if the button is on show a full toggle
        if (onOff) {
            this.Image = Image.FromFile("../../../toggle.png");
            onOff = false;
        } else { // if its off show only it's outline
            this.Image = Image.FromFile("../../../toggle-outline.png");
            onOff = true;
        }
    }
}