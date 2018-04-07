using AmDevIT.Games.DialogueSystem.Resources;
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
                                                   source.OnStartConversation,
                                                   source.DidEnterNode,
                                                   source.DidExitNode,
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
            ConversationChoice defaultOk = null;
            bool useDefaultOk = true;

            conversationNode = new ConversationNode(parent, source.ID, source.CharacterID, source.TextID);

            if (source.Choices != null)
            {
                if (source.Choices.Length > 0)
                {
                    useDefaultOk = false;
                    foreach (ConversationChoiceJsonData conversationChoiceData in source.Choices)
                    {
                        ConversationChoice currentConversationChoice = conversationChoiceData.AsConversationChoice(conversationNode);
                        conversationNode.AddChoice(currentConversationChoice);
                    }
                }
            }

            if (useDefaultOk)
            {
                // Must be optimized. This is really ugly.
                defaultOk = new ConversationChoice(conversationNode, 
                                                   ReservedIdentifiers.DefaultContinueConversationChoiceID, 
                                                   ReservedIdentifiers.DefaultContinueConversationStringID, 
                                                   null, 
                                                   null, 
                                                   null);
                conversationNode.AddChoice(defaultOk);
            }
            return conversationNode;
        }

        internal static ConversationChoice AsConversationChoice(this ConversationChoiceJsonData source, ConversationNode parent)
        {
            ConversationChoice conversationChoice = null;


            if (source.ID == ReservedIdentifiers.DefaultContinueConversationChoiceID)
                conversationChoice = new ConversationChoice(parent,
                                                            source.ID,
                                                            ReservedIdentifiers.DefaultContinueConversationStringID,
                                                            null,
                                                            null,
                                                            null);
            else
            {
                conversationChoice = new ConversationChoice(parent,
                                                            source.ID,
                                                            source.TextID,
                                                            source.CanShowID,
                                                            source.OnSelectedID,
                                                            source.NavigateTo);
            }
            return conversationChoice;
        }

        #endregion
    }
}
