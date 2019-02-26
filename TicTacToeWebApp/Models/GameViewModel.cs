namespace TicTacToeWebApp.Models
{
    public class GameViewModel
    {
        public PlayerViewModel Player1 { get; set; }
        public PlayerViewModel Player2 { get; set; }
        public string Board { get; set; }
        public int Attempts { get; set; }

        public GameViewModel(PlayerViewModel player1, PlayerViewModel player2, string board, int attempts)
        {
            Player1 = player1;
            Player2 = player2;
            Board = board;
            Attempts = attempts;
        }

        private void SetXO(int idx, char val)
        {
            if (Board[idx] != 'X' && Board[idx] != 'O')
                Board = Board.Remove(idx, 1).Insert(idx, val.ToString());
        }

        public bool IsWinner(char ch)
        {
            bool result = false;
            int i = 0;

            if ((Board[i] == ch) && (Board[i + 1] == ch) && (Board[i + 2] == ch))
            {
                result = true;
            }
            else if ((Board[i + 3] == ch) && (Board[i + 4] == ch) && (Board[i + 5] == ch))
            {
                result = true;
            }
            else if ((Board[i + 6] == ch) && (Board[i + 7] == ch) && (Board[i + 8] == ch))
            {
                result = true;
            }
            else if ((Board[i] == ch) && (Board[i + 3] == ch) && (Board[i + 6] == ch))
            {
                result = true;
            }
            else if ((Board[i + 1] == ch) && (Board[i + 4] == ch) && (Board[i + 7] == ch))
            {
                result = true;
            }
            else if ((Board[i + 2] == ch) && (Board[i + 5] == ch) && (Board[i + 8] == ch))
            {
                result = true;
            }
            else if ((Board[i] == ch) && (Board[i + 4] == ch) && (Board[i + 8] == ch))
            {
                result = true;
            }
            else if ((Board[i + 2] == ch) && (Board[i + 4] == ch) && (Board[i + 6] == ch))
            {
                result = true;
            }
            return result;
        }
        public void Update(int? id)
        {
            if (Board[id.Value] != 'X' && Board[id.Value] != 'O')
            {
                if (Attempts % 2 == 0)
                {
                    SetXO(id.Value, 'X');
                    ++Attempts;
                }
                else
                {
                    SetXO(id.Value, 'O');
                    ++Attempts;
                }
            }
        }
        public void Reset()
        {
            Board = "_________";
            Attempts = 0;
            Player1.NumOfWins = Player2.NumOfWins = 0;
        }
    }
}