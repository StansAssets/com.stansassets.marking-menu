using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu {
    static class RenderUtility
    {
        /// <summary>
        /// Render quad
        /// </summary>
        /// <param name="center">Center point</param>
        /// <param name="size">Size of a quad</param>
        /// <param name="color">Color of a quad</param>
        /// <param name="texture2D">Quad texture</param>
        /// <param name="context">MeshGenerationContext</param>
        internal static void Quad(Vector2 center, Vector2 size, Color color, Texture2D texture2D, MeshGenerationContext context)
        {
            var mesh = context.Allocate(4, 6, texture2D);
            int halfX = (int)(size.x / 2f);
            int halfY = (int)(size.y / 2f);

            var x0 = center.x - halfX;
            var y0 = center.y - halfY;

            var x1 = center.x + halfX;
            var y1 = center.y + halfY;

            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(x0, y0, Vertex.nearZ),
                tint = color,
                uv = new Vector2(0,0)
            });
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(x1, y0, Vertex.nearZ),
                tint = color,
                uv =  new Vector2(1,0)
            });
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(x0, y1, Vertex.nearZ),
                tint = color,
                uv =  new Vector2(0,1)
            });

            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(x1, y1, Vertex.nearZ),
                tint = color,
                uv = new Vector2(1,1)
            });

            mesh.SetNextIndex(0);
            mesh.SetNextIndex(1);
            mesh.SetNextIndex(2);

            mesh.SetNextIndex(1);
            mesh.SetNextIndex(3);
            mesh.SetNextIndex(2);
        }

        /// <summary>
        /// Rander line from point to point
        /// </summary>
        /// <param name="from">Start point position</param>
        /// <param name="to">End point position</param>
        /// <param name="width">Width of a line</param>
        /// <param name="color">Color of a line</param>
        /// <param name="texture2D">Texture of a line</param>
        /// <param name="context">MeshGenerationContext</param>
        internal static void Line(Vector2 from, Vector2 to, int width, Color color, Texture2D texture2D, MeshGenerationContext context)
        {
            var mesh = context.Allocate(4, 6, texture2D);

            int halfWidth = width / 2;
            Vector2 lineVector = to - from;
            Vector2 p1 = (Vector2)(Quaternion.Euler(0f, 0f, -90) * lineVector.normalized * halfWidth) + from;
            Vector2 p3 = (Vector2)(Quaternion.Euler(0f, 0f, 90) * lineVector.normalized * halfWidth) + from;
            Vector2 p2 = p1 + lineVector;
            Vector2 p4 = p3 + lineVector;

            //1
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(p1.x, p1.y, Vertex.nearZ),
                tint = color,
                uv = new Vector2(0,0)
            });
            //2
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(p2.x, p2.y, Vertex.nearZ),
                tint = color,
                uv =  new Vector2(1,0)
            });
            //3
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(p3.x, p3.y, Vertex.nearZ),
                tint = color,
                uv =  new Vector2(0,1)
            });
            //4
            mesh.SetNextVertex(new Vertex()
            {
                position = new Vector3(p4.x, p4.y, Vertex.nearZ),
                tint = color,
                uv = new Vector2(1,1)
            });

            mesh.SetNextIndex(0);
            mesh.SetNextIndex(1);
            mesh.SetNextIndex(2);

            mesh.SetNextIndex(1);
            mesh.SetNextIndex(3);
            mesh.SetNextIndex(2);
        }
    }
}
