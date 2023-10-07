using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainViewModel
    {
        public ICell<ScreenViewModel> CurrentScreen { get; }

        public MainViewModel()
        {
            CurrentScreen = Cell.Create<ScreenViewModel>(null);

            // homeview
            var firstScreen = new HomeViewModel(CurrentScreen);

            CurrentScreen.Value = firstScreen;
        }
    }
}
