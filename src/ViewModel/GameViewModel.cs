using Cells;
using Model.Data;
using Model.MineSweeper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    // We intend this class to contain all game related data
    public class GameViewModel
    {
        public ICell<GameBoardViewModel> Board { get; }

        private readonly ICell<IGame> game;

        public ICell<bool> GameOverCell { get; }
        public ICell<GameOutcome> GameProgressCell { get; set; }

        public GameViewModel(IGame game)
        {
            var currentGame = Cell.Create(game);
            this.game = currentGame;
            this.Board = currentGame.Derive(g => new GameBoardViewModel(currentGame));

            this.GameOverCell = currentGame.Derive(g => g.Status != GameStatus.InProgress);
            this.GameProgressCell = GameOverCell.Derive(GameProgress);
        }

        public enum GameOutcome
        {
            InProgress,
            Won,
            Lost
        }

        public GameOutcome GameProgress(bool gameOver)
        {
            if (game.Value.Status == GameStatus.Won)
            {
                return GameOutcome.Won;
            }
            if (game.Value.Status == GameStatus.Lost)
            {
                return GameOutcome.Lost;
            } else
            {
                return GameOutcome.InProgress;
            }
            
        }
    }
}
