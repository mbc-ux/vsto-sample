using System;
using System.Windows.Input;

namespace Vsto.Sample.Client.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _onExecute;
        private readonly Func<object, bool> _onCanExecute;

        public DelegateCommand(Action<object> onExecute)
            : this(onExecute, _ => true)
        {
        }

        public DelegateCommand(Action<object> onExecute, Func<object, bool> onCanExecute)
        {
            _onExecute = onExecute;
            _onCanExecute = onCanExecute;
        }

#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter)
        {
            var result = _onCanExecute(parameter);
            return result;
        }

        public void Execute(object parameter)
        {
            _onExecute(parameter);
        }
    }
}
