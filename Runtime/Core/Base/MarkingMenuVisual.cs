using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public partial class MarkingMenu
    {
        const int k_TextureSize = 16;
        Texture2D m_Texture;

        internal void InitVisual()
        {
            // m_Texture = new Texture2D(k_TextureSize, k_TextureSize)
            // {
            //     filterMode = FilterMode.Point,
            //     hideFlags = HideFlags.HideAndDontSave,
            // };
            // generateVisualContent += OnGenerateVisualContent;
        }

        // Used to render Marking Menu Line and center pivot
        void OnGenerateVisualContent(MeshGenerationContext context)
        {
            if (Active)
            {
                m_Texture = new Texture2D(k_TextureSize, k_TextureSize)
                {
                    filterMode = FilterMode.Point,
                    hideFlags = HideFlags.HideAndDontSave
                };
                var mesh = context.Allocate(4, 6, m_Texture);
            }
        }

        internal void OpenVisual()
        {

        }

        internal void CloseVisual()
        {

        }
    }
}
