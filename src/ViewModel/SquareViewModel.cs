using Cells;
using Model.Data;
using Model.MineSweeper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class SquareViewModel
    {
        public ICommand Uncover { get; }

        public ICell<Square> Square { get; }

        public ICommand Flag { get; }

        public bool ContainsMine { get; }

        public ICell<IGame> Game { get; }

        public bool LastClick { get; }

        public SquareViewModel(ICell<IGame> game, Vector2D position)
        {
            this.Game = game;
            this.Uncover = new UncoverSquareCommand(game, position);
            this.Flag = new FlagSquareCommand(game, position);
            this.Square = game.Derive(g => g.Board[position]);
            this.ContainsMine = CheckIfContainsMine(game, position);
        }

        private bool CheckIfContainsMine(ICell<IGame> game, Vector2D position)
        {
            if(game.Value.Status == GameStatus.Lost)
            {
                return game.Value.Mines.Contains(position);
            }
            return false;
        }
    }

    public class UncoverSquareCommand : ICommand
    {
        private readonly ICell<IGame> game;
        private readonly Vector2D position;

        public UncoverSquareCommand(ICell<IGame> game, Vector2D position)
        {
            this.game = game;
            this.position = position;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return game.Value.Status == GameStatus.InProgress &&
                (game.Value.Board[position].Status == SquareStatus.Covered ||
                game.Value.Board[position].Status == SquareStatus.Flagged);

        }

        public void Execute(object? parameter)
        {
            game.Value = game.Value.UncoverSquare(position);
        }
    }

    public class FlagSquareCommand : ICommand
    {

        private readonly ICell<IGame> game;
        private readonly Vector2D position;

        public FlagSquareCommand(ICell<IGame> game, Vector2D position)
        {
            this.game = game;
            this.position = position;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return game.Value.Status == GameStatus.InProgress;
        }

        public void Execute(object? parameter)
        {
            game.Value = game.Value.ToggleFlag(position);
        }
    }

}
