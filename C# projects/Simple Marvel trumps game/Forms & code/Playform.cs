using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace S.Dassiemnt2
{
    public partial class Playform : Form
    {
        Card[] cards;
        Player[] players;
        ChanceCard chancecards;
        Random rnd = new Random();
        Die theDie;
        string statName;//store what stat name is being used
        //p1/2point-hand pointer,cpoint-chance card pointer, p1/2card - numbers in hand
        int p1point = 0, p2point = 0, cpoint = 0, p1cards = 5, p2cards = 5;
        //twoplayer-flag for when more then 1 person playing, turn - whos turn, cardDie-card or die, smart - how the computer will work flag
        byte twoPlayer = 0, turn = 2, carddie, smart;
        string ltext;//holds what is in the log

        public Playform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(318, 260);
            makeCard();
            makeChancecard();
            BTN_compare.Enabled = false;
            theDie = new Die(rnd);

        }
        public void makeCard()
        {
            cards = new Card[10];//create 10 cards

            //name of cards
            string[] ncard = { "Carnage", "Silver Samurai", "Thanos", "The Red Skull", "Doctor Doom", "Mephista ", "Mysterio", "Morbius", "Quicksilver", "Scarlet Witch" };
            //first stats
            int[] strength = { 8, 9, 7, 5, 8, 10, 4, 8, 6, 3 };
            //second stats
            int[] speed = { 9, 6, 3, 3, 2, 5, 2, 9, 10, 6 };
            //third stats
            int[] toughness = { 7, 8, 9, 7, 8, 2, 1, 7, 4, 2 };
            //fourth stats
            int[] power = { 5, 3, 10, 1, 9, 8, 8, 3, 3, 10 };
            //image loction
            Image[] loc = { S.Dassiemnt2.Properties.Resources.v1, S.Dassiemnt2.Properties.Resources.v2, S.Dassiemnt2.Properties.Resources.v3, S.Dassiemnt2.Properties.Resources.v4, S.Dassiemnt2.Properties.Resources.v5, S.Dassiemnt2.Properties.Resources.v6, S.Dassiemnt2.Properties.Resources.v7, S.Dassiemnt2.Properties.Resources.v8, S.Dassiemnt2.Properties.Resources.v9, S.Dassiemnt2.Properties.Resources.v10 };
            // card info
            string[] fo = { "Cletus Kasady was already a dangerous serial killer before he became the powerful psychopathic super-villain known as Carnage.", "Keniuchio Harada, also known as the Silver Samurai is the mutant son of the former Japanese crimelord Shingen Harada", "Thanos was born on Titan, one of the moons of Saturn - he was born a type of mutant and thus was much more powerful than his fellow Titans",
                              "The Red Skull's original name was Johann Schmidt and he was born in Germany.", "self-proclaimed unquestioned smartest man on Earth. Doom was born in a Romani family in the nation of Latveria", "the daughter of the archdemon Mephisto - she is not as prominent as her brother Blackheart when it comes to making trouble for mortal life",
                              "Quentin Beck, an actor and special effects artist who was turned away from a good movie gig, and decided to prove himself by becoming a phony vigilante and take down Spider-Man.", "Michael Morbius was a biogenetic scientist who was dying of a blood-destroying disease. He injected himself with radiatively altered vampire bat DNA",
                          "Quicksilver is the biological son of Magneto and the twin brother of Scarlet Witch.","The Scarlet Witch is a mutant who was born Wanda Maximoff and is the daughter of Magneto and the twin sister of Quicksilver. Scarlet witch is one of the most powerful beings in the marvel univers because of her ability to warp reality."};


            //fill cards
            for (int i = 0; i < 10; i++)
            {

                cards[i] = new Card(ncard[i], 0, strength[i], speed[i], toughness[i], power[i], loc[i], fo[i]);

            }

        }
        public void makeChancecard()
        {
            chancecards = new ChanceCard();//make chance deck
            int[] cCard = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5 };

            //fill chance cards
            for (int i = 0; i < 16; i++)
            {
                chancecards.Chancedeck[i] = cCard[i];
            }

        }
        public void shuffleDeck()
        {
            //shuffle play deck
            cards = cards.OrderBy(x => rnd.Next()).ToArray();
            //output to log
            ltext = "Playing cards Shuffled\r\n";
            MessageBoxLog(ltext);

            //shuffle chance card deck
            chancecards.Chancedeck = chancecards.Chancedeck.OrderBy(x => rnd.Next()).ToArray();
            //output to log
            ltext = "Chance cards Shuffled\r\n";
            MessageBoxLog(ltext);
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

        private void Play()
        {
            this.Size = new Size(1074, 744);

            if (RB_ccard.Checked == true)
                carddie = 0;
            else
                carddie = 1;
            if (RB_normal.Checked == true)
                smart = 0;
            else
                smart = 1;
            if (carddie == 1)
            { LBL_chanceCardlO.Text = "Chance die"; LBL_chanceCardl.Text = "Chance die"; }

            BTN_quit.Location = new Point(964, 681);

            //hide buttons and display lables
            showHideObjects();
            //shuffle both chance and play deck
            shuffleDeck();

            //make both players card hand
            players = new Player[2];
            players[0] = new Player();
            players[1] = new Player();

            //fill there hand
            for (int i = 0; i < 5; i++)
            {
                players[0].cardHand[i] = i;
                players[1].cardHand[i] = i + 5;
            }
            //using -1 to significe that there is no card there
            for (int i = 5; i < 10; i++)
            {
                players[0].cardHand[i] = -1;
                players[1].cardHand[i] = -1;
            }



            //output to log
            ltext = "Play Deck split\r\nBoth Players get 5 random cards in there hand\r\n";
            MessageBoxLog(ltext);
            //check amout of card lef in chance deck
            chanceCardCheck();
            if (carddie == 0)
                //output to log
                ltext = "To decide who goes first both, players will draw a chance card\r\n";
            else
                ltext = "To decide who goes first both, players will roll a die\r\n";
            MessageBoxLog(ltext);
            int c = 0;

            //decide who goes first
            while (turn == 2)
            {
                if (carddie == 0)
                {

                    //output to log
                    ltext = "Player 1 draws a " + chancecards.Chancedeck[c + 1] + "\r\nPlayer 2 draws a " + chancecards.Chancedeck[c] + "\r\n";
                    MessageBoxLog(ltext);
                    if (chancecards.Chancedeck[c] > chancecards.Chancedeck[c + 1])
                    {
                        //output to log
                        turn = 1;
                        ltext = "Player 2's chance card is greater\r\nPlayer 2 Goes first\r\n";

                    }
                    else if (chancecards.Chancedeck[c] == chancecards.Chancedeck[c + 1])
                    {
                        //output to log
                        ltext = "Both chance cards have the same value, both player draw another card\r\n";
                        c = c + 2;


                    }
                    else
                    {
                        //output to log
                        turn = 0;
                        ltext = "Player 1's chance card is greater\r\nPlayer 1 Goes first\r\n";

                    }

                    MessageBoxLog(ltext);
                    cpoint = cpoint + 2;
                    chanceCardCheck();
                }
                else
                {
                    int p1die = theDie.roll(), p2die = theDie.roll();
                    //output to log
                    ltext = "Player 1 rolls a " + p1die + "\r\nPlayer 2 rolls a " + p2die + "\r\n";
                    MessageBoxLog(ltext);
                    if (p2die > p1die)
                    {
                        //output to log
                        turn = 1;
                        ltext = "Player 2's die roll is greater\r\nPlayer 2 Goes first\r\n";

                    }
                    else if (p1die == p2die)

                        //output to log
                        ltext = "Both die rolls have the same value, both player roll agian\r\n";




                    else
                    {
                        //output to log
                        turn = 0;
                        ltext = "Player 1's die roll is greater\r\nPlayer 1 Goes first\r\n";


                    }
                    MessageBoxLog(ltext);
                }
                whoTurn();
                cardsInHand();


            }
        }

        public void whoTurn()
        {
            if (turn == 0)
            {//show player 1 card and hide player 2's
                player2CardHide();
                player1Display();
            }
            else
            {//show player 2 card and hide player 1's
                player1CardHide();
                player2CardDisplay();
            }
            displayCardInfo();

            //reset the compare labels
            LBL_comNumber.Text = "0";
            LBL_comNumberO.Text = "0";

            while (turn == 1 && twoPlayer == 0)//check if computers turn
                pcTurn();

        }

        private void pcTurn()
        {
            int i = rnd.Next(0, 6);
            //store them so easy 2 read and type
            int num1 = cards[players[1].cardHand[p2point]].Prop1;
            int num2 = cards[players[1].cardHand[p2point]].Prop2;
            int num3 = cards[players[1].cardHand[p2point]].Prop3;
            int num4 = cards[players[1].cardHand[p2point]].Prop4;

            //force draw image before proceeding
            Application.DoEvents();

            //output to log
            ltext = "Player 2 is thinking...\r\n";
            MessageBoxLog(ltext);
            wait(4);
            if (smart == 0)
            {

                switch (i)
                {
                    case 0:
                        //pc plays stat1
                        Pastolabe(LBL_stat1.Text, LBL_stat1O.Text);
                        statName = LBL_stat1L.Text;
                        compare();
                        break;
                    case 1:
                        //pc plays stat2
                        Pastolabe(LBL_stat2.Text, LBL_stat2O.Text);
                        statName = LBL_stat2L.Text;
                        compare();
                        break;
                    case 2:
                        //pc plays stat3
                        Pastolabe(LBL_stat3.Text, LBL_stat3O.Text);
                        statName = LBL_stat3L.Text;
                        compare();
                        break;
                    case 3:
                        //pc plays stat4
                        Pastolabe(LBL_stat4.Text, LBL_stat4O.Text);
                        statName = LBL_stat4L.Text;
                        compare();
                        break;
                    case 4://fall through to case default
                    default:
                        // pc plays chance card
                        chanceCardSelected();
                        compare();
                        break;
                }
            }
            else
            {

                if (i > 4)
                {
                    chanceCardSelected();
                    compare();
                }
                else if (num1 > num2 && num1 > num3 && num1 > num4)
                {
                    Pastolabe(LBL_stat1.Text, LBL_stat1O.Text);
                    statName = LBL_stat1L.Text;
                    compare();
                }
                else if (num2 > num1 && num2 > num3 && num2 > num4)
                {
                    //pc plays stat2
                    Pastolabe(LBL_stat2.Text, LBL_stat2O.Text);
                    statName = LBL_stat2L.Text;
                    compare();
                }
                else if (num3 > num1 && num3 > num2 && num3 > num4)
                {
                    //pc plays stat3
                    Pastolabe(LBL_stat3.Text, LBL_stat3O.Text);
                    statName = LBL_stat3L.Text;
                    compare();
                }
                else
                {
                    //pc plays stat4
                    Pastolabe(LBL_stat4.Text, LBL_stat4O.Text);
                    statName = LBL_stat4L.Text;
                    compare();
                }
            }

        }

        private void cardsInHand()
        {
            //outpout to label amount of card each player has
            LBL_playerCardsRemaining.Text = Convert.ToString(p1cards);
            LBL_playerCardsRemainingO.Text = Convert.ToString(p2cards);

            //check if player has not cards left
            if (p1cards == 0)
            {
                MessageBox.Show("Player 1 has no cards Remaining\r\n Player 2 has Won!", "Player 2 has won", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                ltext = "Player 1 has no cards Remaining\r\nPlayer 2 has Won!";
                MessageBoxLog(ltext);
                turn = 2;
                LBL_p1Take.Text = "Player 2 has won";
            }
            else if (p2cards == 0)
            {
                MessageBox.Show("Player 2 has no cards Remaining\r\n Player 1 has Won!", "Player 1 has won", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                ltext = "Player 2 has no cards Remaining\r\nPlayer 1 has Won!";
                MessageBoxLog(ltext);
                turn = 2;
                LBL_p2Take.Text = "Player 1 has won";
            }

        }
        public void displayCardInfo()
        {
            if (turn < 2)//check if game over
            {
                //load player 1's card information on to screen
                LBL_cardname.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Name);
                LBL_stat1.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Prop1);
                LBL_stat2.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Prop2);
                LBL_stat3.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Prop3);
                LBL_stat4.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Prop4);
                LBL_info.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Info);
                PB_p1.Image = cards[players[0].cardHand[p1point]].CardImageLoc;

                //load player 2's card information on to screen
                LBL_cardnameO.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Name);
                LBL_stat1O.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Prop1);
                LBL_stat2O.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Prop2);
                LBL_stat3O.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Prop3);
                LBL_stat4O.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Prop4);
                LBL_infoO.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Info);
                PB_p2.Image = cards[players[1].cardHand[p2point]].CardImageLoc;

                //check it chance card number area for player1 has a value
                if (cards[players[0].cardHand[p1point]].Ccard != 0)
                {
                    LBL_chanceCard.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Ccard);
                }
                else
                    if (carddie == 1)
                        LBL_chanceCard.Text = "Click to get a die roll";
                    else
                        LBL_chanceCard.Text = "Click to get a chance card";

                //check it chance card number area for player2 has a value
                if (cards[players[1].cardHand[p2point]].Ccard != 0)
                {
                    LBL_chanceCardO.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Ccard);
                }
                else
                    if (carddie == 1)
                        LBL_chanceCardO.Text = "Click to get a die roll";
                    else
                        LBL_chanceCardO.Text = "Click to get a chance card";
            }
            else
                endGame();

        }
        private void endGame()
        {
            //display both last played cards, disable all stat label and compare button
            player1Display();
            player2CardDisplay();
            BTN_compare.Enabled = false;
            LBL_stat1.Enabled = false;
            LBL_stat1O.Enabled = false;
            LBL_stat2.Enabled = false;
            LBL_stat2O.Enabled = false;
            LBL_stat3.Enabled = false;
            LBL_stat3O.Enabled = false;
            LBL_stat4.Enabled = false;
            LBL_stat4O.Enabled = false;
            LBL_chanceCard.Enabled = false;
            LBL_chanceCardO.Enabled = false;
        }

        private void MessageBoxLog(string nText)
        {
            //add passed text to current log
            TXTB_log.Text = TXTB_log.Text + nText;

            //auto scroll to bottom
            TXTB_log.SelectionStart = TXTB_log.Text.Length;
            TXTB_log.ScrollToCaret();
        }

        public void showHideObjects()
        {
            //show all non-card labels
            LBL_playerCardsRemainingO.Visible = true;
            LBL_playerCardsRemaininglO.Visible = true;
            LBL_playerCardsRemaining.Visible = true;
            LBL_playerCardsRemainingl.Visible = true;

            //show stat labels
            LBL_stat1L.Visible = true;
            LBL_stat1LO.Visible = true;
            LBL_stat2L.Visible = true;
            LBL_stat2LO.Visible = true;
            LBL_stat3L.Visible = true;
            LBL_stat3Lo.Visible = true;
            LBL_stat4L.Visible = true;
            LBL_stat4Lo.Visible = true;
            LBL_chanceCardl.Visible = true;
            LBL_chanceCardlO.Visible = true;

            if (carddie == 0)
            {
                LBL_chanceCardr.Visible = true;
                LBL_chanceCardrl.Visible = true;
            }

            //show text log and 2 card image while making sure they are behind labels
            TXTB_log.Visible = true;
            PB_cardbackbround.Visible = true;
            PB_cardbackbround.SendToBack();
            PB_cardbackbroundO.Visible = true;
            PB_cardbackbroundO.SendToBack();
            pictureBox1.SendToBack();

            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;

            //enable and show compare, while hiding and disabling & hiding play button
            BTN_compare.Visible = true;
            BTN_compare.Enabled = true;
            BTN_compare.BringToFront();
            BTN_1Play.Visible = false;
            BTN_1Play.Enabled = false;
            BTN_2Play.Visible = false;
            BTN_2Play.Enabled = false;
            Application.DoEvents();


        }
        private void player1Display()
        {
            //this shows all player1' card labels
            LBL_cardname.Visible = true;
            LBL_stat1.Visible = true;
            LBL_stat2.Visible = true;
            LBL_stat3.Visible = true;

            LBL_stat4.Visible = true;
            LBL_comNumber.Visible = true;
            LBL_comNumberl.Visible = true;
            LBL_chanceCard.Visible = true;

            LBL_info.Visible = true;
            PB_p1.Visible = true;

            //enables player 1's card stats label to be selected
            LBL_stat1.Enabled = true;
            LBL_stat2.Enabled = true;
            LBL_stat3.Enabled = true;
            LBL_stat4.Enabled = true;
            LBL_chanceCard.Enabled = true;
        }
        private void player2CardDisplay()
        {
            //this shows all player 2' card labels
            LBL_cardnameO.Visible = true;
            LBL_stat1O.Visible = true;
            LBL_stat2O.Visible = true;
            LBL_stat3O.Visible = true;
            LBL_stat4O.Visible = true;
            LBL_comNumberO.Visible = true;
            LBL_comNumberlO.Visible = true;
            LBL_chanceCardO.Visible = true;
            LBL_infoO.Visible = true;
            PB_p2.Visible = true;


            if (twoPlayer == 1)//check if vs comp
            {
                //enables player 2's card stats label to be selected
                LBL_stat1O.Enabled = true;
                LBL_stat2O.Enabled = true;
                LBL_stat3O.Enabled = true;
                LBL_stat4O.Enabled = true;
                LBL_chanceCardO.Enabled = true;
            }
        }
        private void player1CardHide()
        {
            //hide player 1 card labels
            LBL_cardname.Visible = false;
            LBL_stat1.Visible = false;
            //LBL_stat1L.Visible = false;
            LBL_stat2.Visible = false;
            //LBL_stat2L.Visible = false;
            LBL_stat3.Visible = false;
            //LBL_stat3L.Visible = false;
            LBL_stat4.Visible = false;
            //LBL_stat4L.Visible = false;
            LBL_comNumber.Visible = false;
            //LBL_comNumberl.Visible = false;
            LBL_chanceCard.Visible = false;
            //LBL_chanceCardl.Visible = false;
            LBL_info.Visible = false;
            PB_p1.Visible = false;

            //disable player 1's card stats label to be selected
            LBL_stat1.Enabled = false;
            LBL_stat2.Enabled = false;
            LBL_stat3.Enabled = false;
            LBL_stat4.Enabled = false;
            LBL_chanceCard.Enabled = false;
        }
        private void player2CardHide()
        {
            //hide player 2 card labels
            LBL_cardnameO.Visible = false;
            LBL_stat1O.Visible = false;
            //LBL_stat1LO.Visible = false;
            LBL_stat2O.Visible = false;
            //LBL_stat2LO.Visible = false;
            LBL_stat3O.Visible = false;
            //LBL_stat3Lo.Visible = false;
            LBL_stat4O.Visible = false;
            //LBL_stat4Lo.Visible = false;
            LBL_comNumberO.Visible = false;
            //LBL_comNumberlO.Visible = false;
            LBL_chanceCardO.Visible = false;
            //LBL_chanceCardlO.Visible = false;
            LBL_infoO.Visible = false;
            PB_p2.Visible = false;

            //disable player 2's card stats label to be selected
            LBL_stat1O.Enabled = false;
            LBL_stat2O.Enabled = false;
            LBL_stat3O.Enabled = false;
            LBL_stat4O.Enabled = false;
            LBL_chanceCardO.Enabled = false;

        }
        private void chanceCardCheck()
        {//output the number of chance cards left
            LBL_chanceCardr.Text = Convert.ToString(chancecards.Chancedeck.Length - cpoint);
        }

        private void Pastolabe(string valu, string valuO)
        {//store both value that will be compared into labels
            LBL_comNumber.Text = valu;
            LBL_comNumberO.Text = valuO;

        }
        private void BTN_compare_Click(object sender, EventArgs e)
        {//from the compare button
            compare();
        }

        private void compare()
        {
            //convert both compare label values to ints
            int player1card = Convert.ToInt16(LBL_comNumber.Text), player2card = Convert.ToInt16(LBL_comNumberO.Text);

            if (LBL_comNumber.Text == "0")//check it there is a value to compare
            {//output to log
                ltext = "There is no value to compare \r\n";
                MessageBoxLog(ltext);
            }
            else
            {
                //check whos turn
                if (turn == 0)
                {
                    //output to log
                    ltext = "Player1 Compares " + cards[players[0].cardHand[p1point]].Name + "'s " + statName + " with a value of " + player1card + " agenst player2 card " + cards[players[1].cardHand[p2point]].Name + "'s " + statName + "with a value of " + LBL_comNumberO.Text + "\r\n";
                }
                else
                {
                    //output to log
                    ltext = "Player2 Compares " + cards[players[1].cardHand[p2point]].Name + "'s " + statName + " with a value of " + player2card + " agenst player1 card " + cards[players[0].cardHand[p1point]].Name + "'s " + statName + "with a value of " + LBL_comNumber.Text + "\r\n";
                }
                MessageBoxLog(ltext);

                //check who has the greater value
                if (player1card > player2card)
                {
                    //output to log
                    ltext = "Player 1's " + statName + " has a greater value \r\nPlayer 1's takes " + cards[players[1].cardHand[p2point]].Name + " From Player 2\r\nplayer 1's turn\r\n";
                    turn = 0;//change to player 1
                    p1cards++; //increase player 1 card count
                    p2cards--; //decrease player 2 card count
                    LBL_p2Take.Text = "Player 1 takes " + cards[players[1].cardHand[p2point]].Name + " From Player 2\r\n";
                }
                else if (player1card == player2card && turn == 0)
                {
                    //output to log
                    ltext = "Player 1's " + statName + " has the same value as player 2's " + statName + " \r\nPlayer 1's takes " + cards[players[1].cardHand[p2point]].Name + " From Player 2\r\nplayer 1's turn\r\n";
                    turn = 0;
                    p1cards++;
                    p2cards--;
                    LBL_p2Take.Text = "Player 1 takes " + cards[players[1].cardHand[p2point]].Name + " From Player 2\r\n";
                }
                else if (player1card == player2card && turn == 1)
                {
                    //output to log
                    ltext = "Player 2's " + statName + " has the same value as player 1's " + statName + " \r\nPlayer 2's takes " + cards[players[0].cardHand[p1point]].Name + " From Player 1\r\nplayer 2's turn\r\n";
                    turn = 1;//change to player 2
                    p2cards++;//increase player 1 card count
                    p1cards--;//decrease player 1 card count
                    LBL_p1Take.Text = "Player 2 takes " + cards[players[0].cardHand[p1point]].Name + " From Player 1\r\n";
                }
                else
                {
                    //output to log
                    ltext = "Player 2's " + statName + " has a greater value \r\nPlayer 2's takes " + cards[players[0].cardHand[p1point]].Name + " From Player 1\r\nplayer 2's turn\r\n";
                    turn = 1;
                    p2cards++;
                    p1cards--;
                    LBL_p1Take.Text = "Player 2 takes " + cards[players[0].cardHand[p1point]].Name + " From Player 1\r\n";
                }
                MessageBoxLog(ltext);

                //make both playing cards visable
                player1Display();
                player2CardDisplay();

                //force it draw images
                Application.DoEvents();
                wait(3);
                takeCard();
                LBL_p1Take.Text = "";
                LBL_p2Take.Text = "";
                cardsInHand();

                //check it pointer's are pointing at a value that is not -1
                if (p1point > 10 || players[0].cardHand[p1point] == -1)
                    p1point = 0;
                if (p2point > 10 || players[1].cardHand[p2point] == -1)
                    p2point = 0;

                whoTurn();
            }
        }
        private void takeCard()
        {
            //since 0 is a index that will hold a value, i use -1 to signifie that there is not needed value in place(i.e free space)
            int i = 0;

            while (i < 9)//loop untill find empty space
            {
                if (turn == 0)//check whos turn
                {
                    if (players[0].cardHand[i] == -1)//check that space empty
                    {
                        //check if next played card would be the newest card, this stops new card to be played straght away
                        if (players[0].cardHand[p1point + 1] == -1)
                            p1point = p1point + 2;
                        else
                            p1point++;
                        //take player 2 current played card
                        players[0].cardHand[i] = players[1].cardHand[p2point];
                        players[1].cardHand[p2point] = -1;


                        break;
                    }
                    else
                        i++;

                }
                else
                {//all same as above but for player 2
                    if (players[1].cardHand[i] == -1)
                    {
                        if (players[1].cardHand[p2point + 1] == -1)
                            p2point = p2point + 2;
                        else
                            p2point++;

                        players[1].cardHand[i] = players[0].cardHand[p1point];
                        players[0].cardHand[p1point] = -1;
                        break;
                    }
                    else
                        i++;

                }
            }

            sortHand(0);
            sortHand(1);

        }

        private void wait(int sec)
        {
            //make the appliction wait
            DateTime start = DateTime.Now;
            while (start.AddSeconds(sec) > DateTime.Now) { };
        }
        private void sortHand(int Turn)
        {
            int i = 0;
            //this will move empty space 2 the back
            while (i < 9)
            {
                if (players[Turn].cardHand[i] == -1 && players[Turn].cardHand[i + 1] == -1)
                {
                    break;
                }
                else if (players[Turn].cardHand[i] == -1)
                {
                    players[Turn].cardHand[i] = players[Turn].cardHand[i + 1];
                    players[Turn].cardHand[i + 1] = -1;
                    i++;
                }
                else
                    i++;

            }
        }
        private void chanceCardSelected()
        {
            //check if there is chance cards left
            if (cpoint < 16)
            {
                //check if playing card needs a chance card value
                if (LBL_chanceCard.Text == "Click to get a chance card" || LBL_chanceCard.Text == "Click to get a die roll")
                {
                    if (carddie == 0)
                    {
                        //store chance card value to current in player1 current card
                        cards[players[0].cardHand[p1point]].Ccard = chancecards.Chancedeck[cpoint];
                        //change label to value
                        LBL_chanceCard.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Ccard);
                        //output to log
                        ltext = "Player1 use a Chance Card \r\n";
                        MessageBoxLog(ltext);

                        cpoint++;//increase chance card pointer

                        chanceCardCheck();
                    }
                    else
                    {
                        //store die roll value to current in player1 current card
                        cards[players[0].cardHand[p1point]].Ccard = theDie.roll();
                        //change label to value
                        LBL_chanceCard.Text = Convert.ToString(cards[players[0].cardHand[p1point]].Ccard);
                        //output to log
                        ltext = "Player1 rolled the die \r\n";
                        MessageBoxLog(ltext);
                    }

                }

                //new if
                if (LBL_chanceCardO.Text == "Click to get a chance card" ||LBL_chanceCardO.Text == "Click to get a die roll")
                {
                    if (carddie == 0)
                    {
                        //store chance card value to current in player2 current card
                        cards[players[1].cardHand[p2point]].Ccard = theDie.roll();
                        //change label to value
                        LBL_chanceCardO.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Ccard);
                        //output to log
                        ltext = "Player2 use a Chance Card \r\n";
                        MessageBoxLog(ltext);
                        cpoint++;//increase chance card pointer

                        chanceCardCheck();

                    }
                    else
                    {   //store die roll value to current in player2 current card
                        cards[players[1].cardHand[p2point]].Ccard = chancecards.Chancedeck[cpoint];
                        //change label to value
                        LBL_chanceCardO.Text = Convert.ToString(cards[players[1].cardHand[p2point]].Ccard);
                        //output to log
                        ltext = "Player2  rolled the die \r\n";
                        MessageBoxLog(ltext);
                        cpoint++;//increase chance card pointer

                        chanceCardCheck();
                    }
                }

                if (LBL_chanceCard.Text != "Click to get a chance card" && LBL_chanceCardO.Text != "Click to get a chance card" && LBL_chanceCard.Text != "Click to get a die roll" && LBL_chanceCardO.Text != "Click to get a die roll")
                    Pastolabe(LBL_chanceCard.Text, LBL_chanceCardO.Text);
                statName = LBL_chanceCardl.Text;
            }
            else
            {
                //output to log
                ltext = "There is no chance cards left to play\r\n";
                MessageBoxLog(ltext);
            }
        }



        private void BTN_1Play_Click(object sender, EventArgs e)
        {
            //set for playing agenst pc
            twoPlayer = 0;
            Play();
        }
        private void BTN_2Play_Click(object sender, EventArgs e)
        {
            //set for 2 players
            twoPlayer = 1;
            Play();
        }

        private void LBL_chanceCardO_Click(object sender, EventArgs e)
        {
            chanceCardSelected();
        }
        private void LBL_chanceCard_Click(object sender, EventArgs e)
        {
            chanceCardSelected();
        }

        //all methords below are passing the label text to the compare label and storing the stat name
        private void LBL_stat4_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat4.Text, LBL_stat4O.Text);
            statName = LBL_stat4L.Text;
        }
        private void LBL_stat3_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat3.Text, LBL_stat3O.Text);
            statName = LBL_stat3L.Text;
        }
        private void LBL_stat2_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat2.Text, LBL_stat2O.Text);
            statName = LBL_stat2L.Text;
        }
        private void LBL_stat1_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat1.Text, LBL_stat1O.Text);
            statName = LBL_stat1L.Text;
        }
        private void LBL_stat4O_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat4.Text, LBL_stat4O.Text);
            statName = LBL_stat4L.Text;
        }
        private void LBL_stat3O_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat3.Text, LBL_stat3O.Text);
            statName = LBL_stat3L.Text;
        }
        private void LBL_stat2O_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat2.Text, LBL_stat2O.Text);
            statName = LBL_stat2L.Text;
        }
        private void LBL_stat1O_Click(object sender, EventArgs e)
        {
            Pastolabe(LBL_stat1.Text, LBL_stat1O.Text);
            statName = LBL_stat1L.Text;
        }


    }
}