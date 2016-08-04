using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace S.Dassiemnt2
{
    public partial class howToPlay : Form
    {
        byte pointer = 2;
        string txtbox;

        public howToPlay()
        {
            InitializeComponent();
        }

        private void BTN_quit_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Menu)
                {
                    form.Show();
                    break;
                }
            }

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {



            switch (pointer)
            {
                case 1:
                    groupBox4.Size = new Size(1, 1);
                    groupBox4.Location = new Point(896, 305);

                    groupBox1.Location = new Point(21, 0);
                    groupBox1.Size = new Size(681, 467);
                    txtbox = "This is how a card look in game, the yellow lable tell you what stat the number below is representing." +
"clicking the grey lables with a number in, will load that number into the comparer, you can click a different number to change which numer is to be compared." +
"\r\nYou can play a chance card or a roll the die, by clicking the text below the chance card(this will be called chance die if you playing with die instead). upon doing so the game will draw a chance card for this card, once you have a chance card for the current card you cannot get another, this chance card number will stay with this card until a new game is started";
                    break;
                case 2:
                    groupBox1.Size = new Size(1, 1);
                    groupBox1.Location = new Point(886, 305);

                    groupBox2.Location = new Point(21, 0);
                    groupBox2.Size = new Size(681, 467);
                    txtbox = "This is the first screen you will see from clicking play from menu.\r\n" +
"Here can set what chance methord you want to play, choosing chance card will have you using chance card to decide  who goes first and you will use a chance card for when drawing a chance card, choosing the die instead you will use the die roll stead if the chance cards for all the chance card operations.\r\n" +

"\r\nSelecting smart for the computer will make the computer choose the best choice, normal will have it play randomly." +

"You can play against another person or the computer by choosing the approprate button.";
                    break;
                case 3:
                    groupBox2.Size = new Size(1, 1);
                    groupBox2.Location = new Point(876, 305);

                    groupBox3.Location = new Point(21, 0);
                    groupBox3.Size = new Size(681, 467);
                    txtbox = "This is the set of label that you will see while in game,\r\n" +
"the top two display how many card each player has." +
"the label below them shows how many chance card remain, this will not be displayed if the user chose to player with die instead of chance cards.\r\n" +

"\r\nbelow the chance card counter labels is where the number selected by the player is shown, this number will be compared to other players number when the compare button is pressed. ";
                    break;
                case 4:
                    groupBox3.Size = new Size(1, 1);
                    groupBox3.Location = new Point(866, 305);

                    groupBox4.Location = new Point(21, 0);
                    groupBox4.Size = new Size(681, 467);
                    txtbox = "This is the game in play, only the that is having his turn can see there card, while the other side will be hiden, this is to provent cheating, when that players turn is over his card will be hiden and other players card will be shown, when a player take a card form another play both card will be shown on there respected side for a few second before they both play new card, the player that take the card will contiue to play until they lose on the compared value."+

"\r\n\r\nAt the bottom there is a log box, this log box will recored what happens in game so you dont miss anything, the player to take all the card is the winner.";
                    break;
            }

            textBox1.Text = txtbox;
            pointer++;
            if (pointer > 4)
                pointer = 1;

            //681, 467 size
            //21, 0 loc


        }
    }
}
