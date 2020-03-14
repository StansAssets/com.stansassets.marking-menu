using UnityEngine;

namespace StansAssets.MarkingMenu
{
    internal class SceneViewMarkingMenu : MarkingMenu
	{
        private const float k_CloseAngleThreshold = 25;
        private const float k_CloseDistanceThreshold = 20;

		public SceneViewMarkingMenu() : base() { }

        /// <summary>
        /// Overrides base method. Find closest menu item depending on mouse position & relative angle.
        /// </summary>
        public override IMarkingMenuItem GetClosestMarkingMenuItem(Vector2 menuPosition, Vector2 currentMousePos, float relativeMouseAngle) {

            //Dead zone
            if (Vector2.Distance(menuPosition, currentMousePos) < k_CloseDistanceThreshold)
            {
                return null;
            }

            var baseItemSelection = base.GetClosestMarkingMenuItem(menuPosition, currentMousePos, relativeMouseAngle);
            if (baseItemSelection != null)
                return baseItemSelection;

            for (int i = 0; i < m_MenuPositions.Count; i++)
            {
                float menuRelativeAngle = Mathf.Atan2(m_MenuPositions[i].y, m_MenuPositions[i].x) * Mathf.Rad2Deg;
                if (Mathf.Abs(relativeMouseAngle - menuRelativeAngle) <= k_CloseAngleThreshold)
                    return m_MenuItems[i];
            }

            return null;
        }
	}
}
