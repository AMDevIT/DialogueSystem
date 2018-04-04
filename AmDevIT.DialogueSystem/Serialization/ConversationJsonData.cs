using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Serialization
{
    [DataContract]
    [KnownType(typeof(ConversationNodeJsonData))]
    internal class ConversationJsonData
    {
        #region Properties         

        [DataMember(Name ="id")]
        public string ID
        {
            get;
            set;
        }

        [DataMember(Name = "defaultRootNodeId")]
        public string DefaultRootNodeID
        {
            get;
            set;
        }

        [DataMember(Name = "initConversationScriptId")]
        public string InitConversationScriptId
        {
            get;
            set;
        }

        [DataMember(Name = "defaultOnSelected")]
        public string DefaultOnSelected
        {
            get;
            set;
        }

        [DataMember(Name = "defaultCanShow")]
        public string DefaultCanShow
        {
            get;
            set;
        }

        [DataMember(Name = "comment")]
        public string Comment
        {
            get;
            set;
        }

        [DataMember(Name ="nodes")]
        public ConversationNodeJsonData[] Nodes
        {
            get;
            set;
        }   

        #endregion
    }
}
