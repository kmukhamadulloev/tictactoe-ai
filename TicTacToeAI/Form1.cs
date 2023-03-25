using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeAI
{
    public partial class TicTacToe : Form
    {
        private string player = "X";
        private string computer = "O";
        private int count = 0;
        private int playerWin = 0;
        private int AIwin = 0;
        private Random random = new Random();
        public TicTacToe()
        {
            InitializeComponent();
            ResetGame();
        }

        private void changeText()
        {
            this.Text = $"TicTacToeAI - Player {playerWin}:{AIwin} AI";
        }

        private void button_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text == "")
            {
                button.Text = player;
                count++;
                if (IsWinner(player))
                {
                    MessageBox.Show(player + " Wins!", "Congratulations");
                    playerWin++;
                    changeText();
                    ResetGame();
                }
                else if (count == 9)
                {
                    MessageBox.Show("It's a draw!", "Game Over");
                    ResetGame();
                }
                else
                {
                    player = (player == "X") ? "O" : "X";
                    if (player == computer)
                    {
                        ComputerTurn();
                    }
                }
            }
        }

        private void ComputerTurn()
        {
            Button button;
            int index;

            // If this is the first turn, choose a random button
            if (count == 1)
            {
                if (random.Next(10) <= 3 && B2.Text == "") // 30% chance to choose center
                {
                    button = B2;
                }
                else // 70% chance to choose a random button
                {
                    index = random.Next(8);
                    button = GetEmptyButtons().ElementAt(index);
                }
            }
            else // Otherwise, use the MiniMax algorithm to choose the best move
            {
                int bestScore = int.MinValue;
                button = null;
                foreach (Button b in GetEmptyButtons())
                {
                    b.Text = "O";
                    int score = MiniMax("X", 0, false);
                    b.Text = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        button = b;
                    }
                }
            }

            button.Text = "O";
            count++;
            if (IsWinner("O"))
            {
                MessageBox.Show("Computer wins!");
                AIwin++;
                changeText();
                ResetGame();
            }
            else if (count == 9)
            {
                MessageBox.Show("Draw!");
                ResetGame();
            }
            else
            {
                player = "X";
            }
        }

        private int MiniMax(string player, int depth, bool maximizingPlayer)
        {
            if (IsWinner("X"))
            {
                return -10 + depth;
            }
            else if (IsWinner("O"))
            {
                return 10 - depth;
            }
            else if (GetEmptyButtons().Count() == 0)
            {
                return 0;
            }

            if (maximizingPlayer)
            {
                int bestScore = int.MinValue;
                foreach (Button button in GetEmptyButtons())
                {
                    button.Text = player;
                    int score = MiniMax(player == "X" ? "O" : "X", depth + 1, false);
                    button.Text = "";
                    bestScore = Math.Max(score, bestScore);
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                foreach (Button button in GetEmptyButtons())
                {
                    button.Text = player;
                    int score = MiniMax(player == "X" ? "O" : "X", depth + 1, true);
                    button.Text = "";
                    bestScore = Math.Min(score, bestScore);
                }
                return bestScore;
            }
        }



        private bool IsWinner(string player)
        {
            // Check rows
            if ((A1.Text == player && A2.Text == player && A3.Text == player) ||
                (B1.Text == player && B2.Text == player && B3.Text == player) ||
                (C1.Text == player && C2.Text == player && C3.Text == player))
            {
                return true;
            }

            // Check columns
            if ((A1.Text == player && B1.Text == player && C1.Text == player) ||
                (A2.Text == player && B2.Text == player && C2.Text == player) ||
                (A3.Text == player && B3.Text == player && C3.Text == player))
            {
                return true;
            }

            // Check diagonals
            if ((A1.Text == player && B2.Text == player && C3.Text == player) ||
                (A3.Text == player && B2.Text == player && C1.Text == player))
            {
                return true;
            }

            return false;
        }

        private IEnumerable<Button> GetEmptyButtons()
        {
            return Controls.OfType<Button>().Where(button => button.Text == "");
        }

        private void ResetGame()
        {
            foreach (Button button in Controls.OfType<Button>())
            {
                button.Text = "";
            }
            count = 0;
            player = "X";
        }
    }
}
