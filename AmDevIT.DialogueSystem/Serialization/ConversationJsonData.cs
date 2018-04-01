using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Serialization
{
    [DataContract]
    public class ConversationJsonData
    {
        #region Properties         

        [DataMember (Name ="id")]
        public string ID
        {
            get;
            set;
        }

        #endregion
    }
}
