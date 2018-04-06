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
            List<KeyValuePair<string, ConversationNode>> nodesList = null;

            nodesList = new List<KeyValuePair<string, ConversationNode>>();

            foreach(ConversationNodeJsonData currentNodeData in source.Nodes)
            {
                ConversationNode currentConversationNode = null;
                KeyValuePair<string, ConversationNode> valuePair = default(KeyValuePair<string, ConversationNode>);

                currentConversationNode = currentNodeData.AsConversationNode();
                valuePair = new KeyValuePair<string, ConversationNode>(currentConversationNode.ID, 
                                                                       currentConversationNode);
                nodesList.Add(valuePair);
            }

            currentConversation = new Conversation(manager, 
                                                   source.ID, 
                                                   source.DefaultRootNodeID,
                                                   source.InitConversationScriptId,
                                                   source.DefaultOnSelected,
                                                   source.DefaultCanShow,
                                                   nodesList.ToArray());
            return currentConversation;
        }

        internal static ConversationNode AsConversationNode(this ConversationNodeJsonData source)
        {
            ConversationNode conversationNode = null;

            conversationNode = new ConversationNode();

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
