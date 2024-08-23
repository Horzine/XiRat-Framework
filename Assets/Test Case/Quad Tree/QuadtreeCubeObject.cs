using UnityEngine;
using Xi.Extend.Collection;

namespace Xi.TestCase
{
    public class QuadtreeCubeObject : MonoBehaviour, IQuadtreeObject
    {
        Rect IQuadtreeObject.GetBounds()
        {
            var position = new Vector2(transform.position.x, transform.position.z);
            var size = new Vector2(transform.localScale.x, transform.localScale.z);
            return new Rect(position - (size / 2), size);
        }
    }
}
