using JetBrains.Annotations;
using StansAssets.Foundation.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu {
    public class MarkingMenuVisualElement : VisualElement {

        /// <exclude/>
        [UsedImplicitly]
        public new class UxmlFactory : UxmlFactory<MarkingMenuVisualElement, UxmlTraits> { }

        /// <exclude/>
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        /// <summary>
        /// VisualElement Uss class name.
        /// </summary>
        private const string USSClassName = "stansassets-markingmenu-visualelement";

        /// <summary>
        /// Creates new MarkingMenuVisualElement.
        /// </summary>
        public MarkingMenuVisualElement() {
            AddToClassList(USSClassName);
        }
        
        /// <summary>
        /// Overriden method to extend area for OnMouseOver event. 
        /// </summary>
        /// <param name="point">Pointer position</param>
        /// <returns>'true' if selection condition is met. 'false' otherwise</returns>
        public override bool ContainsPoint(Vector2 point) {
            if (base.ContainsPoint(point)) {
                return true;
            }

            return this[0].GetPseudoState() == PseudoStates.Hover;
        }
    }
}