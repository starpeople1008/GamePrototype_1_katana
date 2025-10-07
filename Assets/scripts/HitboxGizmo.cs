using UnityEngine;

[ExecuteAlways]
public class HitboxGizmo : MonoBehaviour {
    void OnDrawGizmos(){
        var col = GetComponent<Collider2D>();
        if (col == null) return;
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        if (col is BoxCollider2D b)
            Gizmos.DrawWireCube(b.offset, b.size);
        else if (col is CircleCollider2D c)
            Gizmos.DrawWireSphere(c.offset, c.radius);
    }
}
