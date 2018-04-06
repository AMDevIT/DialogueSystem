using AmDevIT.Games.DialogueSystem.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem
{
    public abstract class ConversationHandlerBase
    {
        #region Methods

        /// <summary>
        /// Retrieve all the DialogDelegate flagged methods using reflection.
        /// </summary>
        /// <returns>An array of KeyValuePair ready to be imported inside a ConversationManager</returns>
        /// <remarks>Reflection is resource expensive. You can use this method for test purpose, but for production releases just test the overhead 
        /// of the method to be sure it doesn't impact the performance of the application.</remarks>
        public KeyValuePair<string, Delegate>[] GetMethodsDelegates()
        {
            List<KeyValuePair<string, Delegate>> delegatesList = new List<KeyValuePair<string, Delegate>>();
            MethodInfo[] methods = null;
            
            methods = this.GetType()
                          .GetMethods()
                          .Where(el => el.GetCustomAttributes(typeof(DialogDelegateAttribute), true).Length > 0)
                          .ToArray();

            foreach(MethodInfo currentMethodInfo in methods)
            {
                DialogDelegateAttribute dialogDelegateAttribute = null;
                KeyValuePair<string, Delegate> valuePair = default(KeyValuePair<string, Delegate>);
                Type delegateType = typeof(DialogueSystemCallbackDelegate);
                Delegate currentDelegate = null;

                dialogDelegateAttribute = currentMethodInfo.GetCustomAttribute<DialogDelegateAttribute>();
                switch (dialogDelegateAttribute.DelegateType)
                {
                    case DialogDelegatesTypes.Callback:
                        delegateType = typeof(DialogueSystemCallbackDelegate);
                        break;

                    case DialogDelegatesTypes.CanExecute:
                        delegateType = typeof(DialogueSystemCanExecuteDelegate);
                        break;
                }
                
                currentDelegate = Delegate.CreateDelegate(delegateType, this, currentMethodInfo.Name);
                valuePair = new KeyValuePair<string, Delegate>(dialogDelegateAttribute.ID, currentDelegate);
                delegatesList.Add(valuePair);
            }

            return delegatesList.ToArray();
        }

        #endregion
    }
}
