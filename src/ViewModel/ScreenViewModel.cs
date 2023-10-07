using Cells;
using Model.MineSweeper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public abstract class ScreenViewModel
    {
        // override to switch
        protected ICell<ScreenViewModel> CurrentScreen { get; }

        protected ScreenViewModel(ICell<ScreenViewModel> currentScreen)
        {
            CurrentScreen = currentScreen;
        }
    }

    // home
    public class HomeViewModel : ScreenViewModel
    {
        public ICommand SwitchToEasyPlay { get; }

        public ICommand SwitchToMediumPlay { get; }

        public ICommand SwitchToHardPlay { get; }

        public HomeViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            SwitchToEasyPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, IGame.MinimumBoardSize, 0.1, true));
            SwitchToMediumPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, 10, 0.2, true));
            SwitchToHardPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, IGame.MaximumBoardSize, 0.3, false));
        }
    }

    // play
    public class PlayViewModel : ScreenViewModel
    {
        public ICell<GameViewModel> Game { get; }

        public ICommand SwitchToSettings { get; }

        public ICommand NewGame { get; }

        public ICell<int> Counter { get; }

        public PlayViewModel(ICell<ScreenViewModel> currentScreen, int boardSize, double mineProbability, bool flooding) : base(currentScreen)
        {
            var game = IGame.CreateRandom(boardSize, mineProbability, flooding, null);
            Game = Cell.Create(new GameViewModel(game));

            SwitchToSettings = new ActionCommand(() => CurrentScreen.Value = new SettingsViewModel(CurrentScreen));
            NewGame = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, boardSize, mineProbability, flooding));

            Counter = Cell.Create(0);
            var second = new System.Timers.Timer();
            second.Interval = 1000;
            second.Elapsed += (sender, e) =>
            {
                if(Game.Value.GameProgressCell.Value == GameViewModel.GameOutcome.InProgress)
                { 
                Counter.Value++;
                }
            };
            second.Start();
        }
    }

    // settings
    public class SettingsViewModel : ScreenViewModel
    {
        public ICommand SwitchToPlay { get; }

        public ICommand SwitchToEasyPlay { get; }

        public ICommand SwitchToMediumPlay { get; }

        public ICommand SwitchToHardPlay { get; }

        public int MinimumSize { get; }

        public int MaximumSize { get; }

        public int BoardSize { get; set; }

        public bool Flooding { get; set; }

        public double MinimumMineProbability { get; }

        public double MaximumMineProbability { get; }

        public double MineProbability { get; set; }
        public SettingsViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            MinimumSize = IGame.MinimumBoardSize;
            MaximumSize = IGame.MaximumBoardSize;
            BoardSize = IGame.MinimumBoardSize;

            Flooding = true;

            MinimumMineProbability = 0.1;
            MaximumMineProbability = 0.9;
            MineProbability = Math.Round(MinimumMineProbability, 2);

            SwitchToPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, BoardSize, MineProbability, Flooding));
            SwitchToEasyPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, MinimumSize, MinimumMineProbability, true));
            SwitchToMediumPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, 10, 0.2, true));
            SwitchToHardPlay = new ActionCommand(() => CurrentScreen.Value = new PlayViewModel(CurrentScreen, MaximumSize, 0.3, false));
        }
    }

    // action
    public class ActionCommand : ICommand
    {
        private readonly Action action;

        public ActionCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
    }
}

