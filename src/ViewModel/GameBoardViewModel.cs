using Cells;
using Model.Data;
using Model.MineSweeper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViewModel
{
    public class GameBoardViewModel
    {

        private readonly ICell<IGameBoard> board;
        private readonly ICell<IGame> game;

        public int MineCount { get; set;}

        public GameBoardViewModel(ICell<IGame> game)
        {
            this.game = game;
            this.board = game.Derive(g => g.Board);
        }

        public IEnumerable<RowViewModel> GetRows => Rows(board, game);

        public IEnumerable<RowViewModel> Rows(ICell<IGameBoard> board, ICell<IGame> game)
        {
            List<RowViewModel> rows = new();
            for (int i = 0; i < board.Value.Height; i++)
            {
                rows.Add(new RowViewModel(Row(i), game, i));
            }
            return rows;
        }

        public IEnumerable<SquareViewModel> Row(int row)
        {
            List<SquareViewModel> squares = new();
            for (int i = 0; i < board.Value.Width; i++)
            {
                var position = new Vector2D(row, i);
                squares.Add(new SquareViewModel(game, position));

            }
            return squares;
        }

        

    }
}
