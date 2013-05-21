using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CarData.ViewModels;

namespace CarData.Commands
{
    public class GoToCommand : ICommand
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public GoToCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object paramter)
        {
            mainWindowViewModel.GoTo();
        }
    }
}
