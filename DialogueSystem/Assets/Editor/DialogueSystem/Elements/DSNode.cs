using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;


    public class DSNode : Node
    {
        // Avoid name duplication because Node already uses name as a property.
        public string DialogueName { get; set; } 
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text.";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* Title CONTAINER*/
            TextField dialogueNameTextFied = DSElementUtility.CreateTextField(DialogueName);

            dialogueNameTextFied.AddClasses(
               "ds-node__text-field",
               "ds-node__text-field__hidden",
               "ds-node__filename-text-field"
             );
 

            titleContainer.Insert(0, dialogueNameTextFied);

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputPort.portName = "Dialogue Connection";

            inputContainer.Add(inputPort);

            /* EXTENSIONS CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldOut = DSElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = DSElementUtility.CreateTextField(Text);

            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
                );

        
            textFoldOut.Add(textTextField);

            customDataContainer.Add(textFoldOut);

            extensionContainer.Add(customDataContainer);
        }
    }
}
