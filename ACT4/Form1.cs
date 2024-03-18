using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
// Kyle A. Canonigo | BSCS 3 F1 | ACT 4 | Using Local Beam
namespace ACT4
{
    public partial class Form1 : Form
    {
        private Random randomGenerator = new Random();

        int side;
        int n = 6;
        SixState startState;
        
        SixState currentState;
        SixState currentState2;
        SixState currentState3;
        SixState currentState4;

        int moveCounter;
        int moveCounter2;
        int moveCounter3;
        int moveCounter4;

        //bool stepMove = true;

        int[,] hTable;
        int[,] hTable2;
        int[,] hTable3;
        int[,] hTable4;
        ArrayList bMoves;
        ArrayList bMoves2;
        ArrayList bMoves3;
        ArrayList bMoves4;

        Object chosenMove;
        Object chosenMove2;
        Object chosenMove3;
        Object chosenMove4;

        public Form1()
        {
            InitializeComponent();

            side = pictureBox1.Width / n;

            startState = randomSixState();
            currentState = new SixState(startState);
            currentState2 = new SixState(startState);
            currentState3 = new SixState(startState);
            currentState4 = new SixState(startState);


            updateUI();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label9.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label13.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label17.Text = "Attacking pairs: " + getAttackingPairs(startState);

            this.Width = 1050;
            this.Height = 550;
        }

        private void updateUI()
        {
            //pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
            pictureBox5.Refresh();

            //label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label3.Text = "Attacking pairs: " + getAttackingPairs(currentState);
            label9.Text = "Attacking pairs: " + getAttackingPairs(currentState2);
            label13.Text = "Attacking pairs: " + getAttackingPairs(currentState3);
            label17.Text = "Attacking pairs: " + getAttackingPairs(currentState4);

            label4.Text = "Moves: " + moveCounter;
            label8.Text = "Moves: " + moveCounter2;
            label12.Text = "Moves: " + moveCounter3;
            label16.Text = "Moves: " + moveCounter4;
            hTable = getHeuristicTableForPossibleMoves(currentState);
            hTable2 = getHeuristicTableForPossibleMoves(currentState2);
            hTable3 = getHeuristicTableForPossibleMoves(currentState3);
            hTable4 = getHeuristicTableForPossibleMoves(currentState4);

            bMoves = getBestMoves(hTable);
            bMoves2 = getBestMoves(hTable2);
            bMoves3 = getBestMoves(hTable3);
            bMoves4 = getBestMoves(hTable4);

            listBox1.Items.Clear();
            foreach (Point move in bMoves)
            {
                listBox1.Items.Add(move);
            }

            listBox2.Items.Clear();
            foreach (Point move in bMoves2)
            {
                listBox2.Items.Add(move);
            }

            listBox3.Items.Clear();
            foreach (Point move in bMoves3)
            {
                listBox3.Items.Add(move);
            }

            listBox4.Items.Clear();
            foreach (Point move in bMoves4)
            {
                listBox4.Items.Add(move);
            }

            if (bMoves.Count > 0)
                chosenMove = chooseMove(bMoves);

            if (bMoves2.Count > 0)
                chosenMove2 = chooseMove(bMoves2);

            if (bMoves3.Count > 0)
                chosenMove3 = chooseMove(bMoves3);

            if (bMoves4.Count > 0)
                chosenMove4 = chooseMove(bMoves4);

            label2.Text = "Chosen move: " + chosenMove;
            label7.Text = "Chosen move: " + chosenMove2;
            label11.Text = "Chosen move: " + chosenMove3;
            label15.Text = "Chosen move: " + chosenMove4;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Blue, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == startState.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private SixState randomSixState()
        {
            //Random r = new Random();
            SixState random = new SixState(randomGenerator.Next(n),
                                             randomGenerator.Next(n),
                                             randomGenerator.Next(n),
                                             randomGenerator.Next(n),
                                             randomGenerator.Next(n),
                                             randomGenerator.Next(n));

            return random;
        }

        private int getAttackingPairs(SixState f)
        {
            int attackers = 0;

            for (int rf = 0; rf < n; rf++)
            {
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get horizontal attackers
                    if (f.Y[rf] == f.Y[tar])
                        attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get diagonal down attackers
                    if (f.Y[tar] == f.Y[rf] + tar - rf)
                        attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get diagonal up attackers
                    if (f.Y[rf] == f.Y[tar] + tar - rf)
                        attackers++;
                }
            }

            return attackers;
        }

        private int[,] getHeuristicTableForPossibleMoves(SixState thisState)
        {
            int[,] hStates = new int[n, n];

            for (int i = 0; i < n; i++) // go through the indices
            {
                for (int j = 0; j < n; j++) // replace them with a new value
                {
                    SixState possible = new SixState(thisState);
                    possible.Y[i] = j;
                    hStates[i, j] = getAttackingPairs(possible);
                }
            }

            return hStates;
        }

        private ArrayList getBestMoves(int[,] heuristicTable)
        {
            ArrayList bestMoves = new ArrayList();
            int bestHeuristicValue = heuristicTable[0, 0];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (bestHeuristicValue > heuristicTable[i, j])
                    {
                        bestHeuristicValue = heuristicTable[i, j];
                        bestMoves.Clear();
                        if (currentState.Y[i] != j)
                            bestMoves.Add(new Point(i, j));
                    }
                    else if (bestHeuristicValue == heuristicTable[i, j])
                    {
                        if (currentState.Y[i] != j)
                        {
                            bestMoves.Add(new Point(i, j));
                        }
                    }
                }
            }
            label5.Text = "Possible Moves (H=" + bestHeuristicValue + ")";
            label6.Text = "Possible Moves (H=" + bestHeuristicValue + ")";
            label10.Text = "Possible Moves (H=" + bestHeuristicValue + ")";
            label14.Text = "Possible Moves (H=" + bestHeuristicValue + ")";
            return bestMoves;
        }

        private Object chooseMove(ArrayList possibleMoves)
        {
            int arrayLength = possibleMoves.Count;
            Random r = new Random();
            int randomMove = r.Next(arrayLength);

            return possibleMoves[randomMove];
        }

        private void executeMove(Point move)
        {
            for (int i = 0; i < n; i++)
            {
                startState.Y[i] = currentState.Y[i];
            }
            currentState.Y[move.X] = move.Y;
            currentState2.Y[move.X] = randomSixState().Y[move.X];
            currentState3.Y[move.X] = randomSixState().Y[move.X];
            currentState4.Y[move.X] = randomSixState().Y[move.X];

            moveCounter++;
            moveCounter2++;
            moveCounter3++;
            moveCounter4++;

            chosenMove = null;
            chosenMove2 = null;
            chosenMove3 = null;
            chosenMove4 = null;
            updateUI();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (getAttackingPairs(currentState) > 0)
                executeMove((Point)chosenMove);
            else if (getAttackingPairs(currentState2) > 0)
                executeMove((Point)chosenMove2);
            else if (getAttackingPairs(currentState3) > 0)
                executeMove((Point)chosenMove3);
            else if (getAttackingPairs(currentState4) > 0)
                executeMove((Point)chosenMove4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            startState = randomSixState();
            currentState = new SixState(startState);
            currentState2 = new SixState(startState);
            currentState3 = new SixState(startState);
            currentState4 = new SixState(startState);

            moveCounter = 0;
            moveCounter2 = 0;
            moveCounter3 = 0;
            moveCounter4 = 0;

            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (getAttackingPairs(currentState) > 0)
            {
                executeMove((Point)chosenMove);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState2.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState3.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState4.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }
    }
}
