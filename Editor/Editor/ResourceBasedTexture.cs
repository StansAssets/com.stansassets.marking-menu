using UnityEngine;

namespace StansAssets.MarkingMenu
{
    internal class ResourceBasedTexture
    {
        private readonly string m_path;

        public ResourceBasedTexture(string resourceRelativePath)
        {
            m_path = resourceRelativePath;
            m_cachedTexture = (Texture2D)Resources.Load(resourceRelativePath, typeof(Texture2D));
        }

        public Texture2D Texture
        {
            get
            {
                if (m_cachedTexture == null)
                {
                    m_cachedTexture = (Texture2D)Resources.Load(m_path, typeof(Texture2D));
                }
                return m_cachedTexture;
            }
        }

        private Texture2D m_cachedTexture;
    }
}
