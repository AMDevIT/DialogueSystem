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

        [DataMember(Name = "onStartConversation", IsRequired = false)]
        public string OnStartConversation
        {
            get;
            set;
        }

        [DataMember(Name = "didEnterNode", IsRequired = false)]
        public string DidEnterNode
        {
            get;
            set;
        }

        [DataMember(Name = "didExitNode", IsRequired = false)]
        public string DidExitNode
        {
            get;
            set;
        }


        [DataMember(Name = "defaultOnSelected", IsRequired = false)]
        public string DefaultOnSelected
        {
            get;
            set;
        }

        [DataMember(Name = "defaultCanShow", IsRequired = false)]
        public string DefaultCanShow
        {
            get;
            set;
        }

        [DataMember(Name = "comment", IsRequired = false)]
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
