using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
 
    public partial class MainWindow : Window
    {
        private bool isCrossTurn;
        private char[,] board;
        private bool isGameOver;

        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }

        private void NewGame()
        {
            isCrossTurn = true;
            board = new char[3, 3];
            isGameOver = false;

            foreach (Button button in GameBoard.Children)
            {
                button.Content = "";
                button.IsEnabled = true;
            }

            Result.Text = "";
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (isGameOver)
            {
                MessageBox.Show("Игра окончена. Начните новую игру.", "Крестики-нолики", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (isCrossTurn)
            {
                RobotMove();
            }
            else
            {
                PlayerMove();
            }

            CheckGameState();
        }

        private void PlayerMove()
        {
            foreach (Button button in GameBoard.Children)
            {
                if (button.IsEnabled && button.Content == "")
                {
                    button.Content = "X";
                    button.IsEnabled = false;

                    isCrossTurn = false;
                    break;
                }
            }
        }

        private void RobotMove()
        {
            int[] move = GetRobotMove();

            Button button = (Button)GameBoard.Children[move[0] * 3 + move[1]];
            button.Content = "O";
            button.IsEnabled = false;

            isCrossTurn = true;
        }

        private int[] GetRobotMove()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '\0')
                    {
                        board[i, j] = 'O';
                        if (IsWin('O'))
                        {
                            board[i, j] = '\0';
                            return new int[] { i, j };
                        }
                        board[i, j] = '\0';
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '\0')
                    {
                        board[i, j] = 'X';
                        if (IsWin('X'))
                        {
                            board[i, j] = '\0';
                            return new int[] { i, j };
                        }
                        board[i, j] = '\0';
                    }
                }
            }

            // Выбрать случайную свободную клетку
            while (true)
            {
                int i = new Random().Next(0, 3);
                int j = new Random().Next(0, 3);

                if (board[i, j] == '\0')
                {
                    return new int[] { i, j };
                }
            }
        }

        private void CheckGameState()
        {
            if (IsWin('X'))
            {
                Result.Text = "Победили крестики!";
                isGameOver = true;
                DisableGameBoard();
            }
            else if (IsWin('O'))
            {
                Result.Text = "Победили нолики!";
                isGameOver = true;
                DisableGameBoard();
            }
            else if (IsDraw())
            {
                Result.Text = "Ничья!";
                isGameOver = true;
                DisableGameBoard();
            }
        }

        private bool IsWin(char player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                {
                    return true;
                }
            }

            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] == player && board[1, j] == player && board[2, j] == player)
                {
                    return true;
                }
            }

            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            {
                return true;
            }
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                return true;
            }

            return false;
        }

        private bool IsDraw()
        {
            foreach (char c in board)
            {
                if (c == '\0')
                {
                    return false;
                }
            }

            return true;
        }

        private void DisableGameBoard()
        {
            foreach (Button button in GameBoard.Children)
            {
                button.IsEnabled = false;
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = int.Parse(button.Tag.ToString()[0].ToString());
            int column = int.Parse(button.Tag.ToString()[1].ToString());

            if (board[row, column] == '\0')
            {
                if (isCrossTurn)
                {
                    button.Content = "X";
                    board[row, column] = 'X';
                }
                else
                {
                    button.Content = "O";
                    board[row, column] = 'O';
                }

                button.IsEnabled = false;

                CheckGameState();

                if (!isGameOver)
                {
                    RobotMove();
                    CheckGameState();
                }
            }
        }
    }
}