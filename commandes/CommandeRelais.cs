using System;
using System.Windows.Input;

namespace RefugeAnimauxWPF.commandes
{
    public class CommandeRelais : ICommand
    {
        private readonly Action action;
        private readonly Func<bool>? peutExecuter;

        public CommandeRelais(Action action, Func<bool>? peutExecuter = null)
        {
            this.action = action;
            this.peutExecuter = peutExecuter;
        }

        public bool CanExecute(object? parameter) => peutExecuter == null || peutExecuter();

        public void Execute(object? parameter) => action();

        public event EventHandler? CanExecuteChanged;

        public void Rafraichir() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
