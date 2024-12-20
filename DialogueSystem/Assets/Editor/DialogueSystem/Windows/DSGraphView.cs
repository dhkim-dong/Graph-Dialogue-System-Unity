using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace DS.Windows
{
    using Elements;
    using Enumerations;
    using Utilities;

    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackGround();

            AddStyles();
        }

        #region Overrided Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if(startPort == port)
                {
                    return;
                }

                if(startPort.node == port.node)
                {
                    return;
                }

                if(startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Chocie)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Chocie)", DSDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", actionEvent.eventInfo.localMousePosition)))
                );

            return contextualMenuManipulator;
        }


        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
                );

            return contextualMenuManipulator;
        }
        #endregion

        #region Elements Creation
        private GraphElement CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }


        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            // GraphView Inheritance Method
            return node; 
        }
        #endregion

        #region Element Addition
        private void AddGridBackGround()
        {
            GridBackground gridBackGround = new GridBackground();

            gridBackGround.StretchToParentSize();

            Insert(0, gridBackGround);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
                );
        }
        #endregion
    }
}
