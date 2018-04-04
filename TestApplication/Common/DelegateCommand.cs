using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApplication.Common
{
    public class DelegateCommand
        : ICommand
    {
        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Fields

        private Func<object, bool> canExecuteFunction = null;
        private Action<object> executeAction = null;
        private bool canExecutePreviousValue = false;

        #endregion

        #region .ctor

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null)
        {

        }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunction)
        {
            if (executeAction == null)
                throw new ArgumentNullException(nameof(executeAction));

            this.executeAction = executeAction;
            this.canExecuteFunction = canExecuteFunction;
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            bool functionResult = true;

            if (this.canExecuteFunction == null)
                return true;
            else
            {
                try
                {
                    functionResult = this.canExecuteFunction(parameter);
                    if (this.canExecutePreviousValue != functionResult)
                        this.OnCanExecuteChanged(functionResult);                    
                }
                catch(Exception)
                {
                    this.OnCanExecuteChanged(true);
                    return true;
                }
            }
            return functionResult;
        }

        public void Execute(object parameter)
        {
            this.executeAction?.Invoke(parameter);
        }

        protected void OnCanExecuteChanged(bool newValue)
        {
            this.canExecutePreviousValue = newValue;
            this.OnCanExecuteChanged();
        }

        protected void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        #endregion
    }
}
