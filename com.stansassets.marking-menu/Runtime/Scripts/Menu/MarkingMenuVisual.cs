using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public partial class MarkingMenu : VisualElement
    {
        const int k_TextureSize = 16;
        Texture2D m_Texture;
        Vector2 m_MousePosition;
        bool m_SkipFirstGenerateVisualContext;

        MarkingMenuItem AngleSelectionItem { get; set; }

        // TODO: is model parameter needed?
        internal void InitVisual(MarkingMenuModel model)
        {
            m_Texture = new Texture2D(k_TextureSize, k_TextureSize)
            {
                filterMode = FilterMode.Point,
                hideFlags = HideFlags.HideAndDontSave,
            };
            generateVisualContent += OnGenerateVisualContent;
        }

        //TODO: check redundant parameters
        internal void OpenVisual(VisualElement root, Vector2 center)
        {
            RegisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            RegisterCallback<MouseUpEvent>(MouseUpEventHandler, TrickleDown.TrickleDown);
            // Not working
            RegisterCallback<PointerMoveEvent>(PointerMoveEventHandler, TrickleDown.TrickleDown);
            RegisterCallback<MouseMoveEvent>(MouseMoveEventHandler, TrickleDown.TrickleDown);

            m_SkipFirstGenerateVisualContext = true;
        }

        internal void CloseVisual()
        {
            UnregisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            UnregisterCallback<MouseUpEvent>(MouseUpEventHandler, TrickleDown.TrickleDown);
            // Not working
            UnregisterCallback<PointerMoveEvent>(PointerMoveEventHandler, TrickleDown.TrickleDown);
            UnregisterCallback<MouseMoveEvent>(MouseMoveEventHandler, TrickleDown.TrickleDown);
        }

        void PointerMoveEventHandler(PointerMoveEvent e)
        {
            // Debug.Log("PointerMoveEvent");
        }

        void MouseMoveEventHandler(MouseMoveEvent e)
        {
            // Debug.Log("MouseMoveEvent");
        }

        Vector2 ConvertPosition(Vector2 pos)
        {
            return new Vector2(pos.x, pos.y + 21);
        }

        // Used to render Marking Menu Line and center pivot
        void OnGenerateVisualContent(MeshGenerationContext context)
        {
            if (Active)
            {
                if (m_SkipFirstGenerateVisualContext)
                {
                    m_SkipFirstGenerateVisualContext = false;
                    return;
                }

                m_Texture = new Texture2D(k_TextureSize, k_TextureSize)
                {
                    filterMode = FilterMode.Point,
                    hideFlags = HideFlags.HideAndDontSave
                };
                RenderUtility.Quad(ConvertPosition(Center), Vector2.one * 6, Color.grey, m_Texture, context);
                RenderUtility.Quad(ConvertPosition(m_MousePosition), Vector2.one * 4, Color.black, m_Texture, context);
                RenderUtility.Line(ConvertPosition(Center), ConvertPosition(m_MousePosition), 2, Color.black, m_Texture,
                    context);
            }
        }

        void PointerUpEventHandler(PointerUpEvent evt)
        {
            evt.PreventDefault();
            HandleEvent(evt);
        }

        void MouseUpEventHandler(MouseUpEvent evt)
        {
            evt.PreventDefault();
        }

        void HandleEvent(PointerUpEvent e)
        {
            // Item Hover
            bool itemExecuted = false;
            for (var i = 0; i < m_Items.Count; ++i)
            {
                itemExecuted = HandleItemInput(m_Items[i], e);
                if (itemExecuted)
                {
                    break;
                }
            }

            // Check by angle
            if (itemExecuted == false)
            {
                AngleSelectionItem?.Execute();
            }

            Close();
        }

        bool HandleItemInput(MarkingMenuItem item, PointerUpEvent e)
        {
            if (item.MouseOver)
            {
                item.Execute();
                return true;
            }

            return false;
        }

        void HandleInput()
        {
            if (Active)
            {
                // Check angle
                Vector2 mousePos = m_MousePosition;
                MarkingMenuItem nearestItem = null;
                float nearestAngle = 360f;

                if ((mousePos - Center).magnitude > m_Model.AngleSelectionDeadZone)
                {
                    Vector2 mousePosVector = mousePos - Center;
                    for (var i = 0; i < m_Items.Count; ++i)
                    {
                        var item = m_Items[i];
                        var itemPos = item.Model.RelativePosition;
                        float angle = Vector2.SignedAngle(mousePosVector, itemPos);
                        float absAngle = Mathf.Abs(angle);
                        if (absAngle <= m_Model.MaxSelectableAngle && absAngle < Mathf.Abs(nearestAngle))
                        {
                            nearestAngle = angle;
                            nearestItem = item;
                        }
                    }
                }

                if (AngleSelectionItem != nearestItem)
                {
                    AngleSelectionItem?.SetHighlight(false);
                    AngleSelectionItem = nearestItem;
                }

                AngleSelectionItem?.SetHighlight(true);
            }
        }
    }
}