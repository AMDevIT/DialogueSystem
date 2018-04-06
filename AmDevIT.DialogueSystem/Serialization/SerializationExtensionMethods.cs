using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Serialization
{
    internal static class SerializationExtensionMethods
    {
        #region Methods

        /// <summary>
        /// Converts the conversation json data class to a conversation model.
        /// </summary>
        /// <param name="source">Current json data class</param>
        /// <param name="manager">The manager that is linked to current conversation that is parsing the data.</param>
        /// <returns></returns>
        internal static Conversation AsConversation(this ConversationJsonData source, ConversationsManager manager)
        {
            Conversation currentConversation = null;
            List<ConversationNode> nodesList = null;
            
            currentConversation = new Conversation(manager, 
                                                   source.ID, 
                                                   source.DefaultRootNodeID,
                                                   source.InitConversationScriptId,
                                                   source.DefaultOnSelected,
                                                   source.DefaultCanShow);

            nodesList = new List<ConversationNode>();

            foreach (ConversationNodeJsonData currentNodeData in source.Nodes)
            {
                ConversationNode currentConversationNode = null;

                currentConversationNode = currentNodeData.AsConversationNode(currentConversation);
                nodesList.Add(currentConversationNode);
            }
            currentConversation.AddConversationNodes(nodesList.ToArray());
            return currentConversation;
        }

        internal static ConversationNode AsConversationNode(this ConversationNodeJsonData source, Conversation parent)
        {
            ConversationNode conversationNode = null;

            conversationNode = new ConversationNode(parent, source.ID, source.CharacterID, source.TextID);
            return conversationNode;
        }

        internal static ConversationChoice AsConversationChoice(this ConversationChoiceJsonData source)
        {
            ConversationChoice conversationChoice = null;

            conversationChoice = new ConversationChoice();

            return conversationChoice;
        }

        #endregion
    }
}
