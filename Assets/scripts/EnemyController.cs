using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    Transform player;
    Health hp;

    void Start(){
        hp = GetComponent<Health>();
        var p = GameObject.FindWithTag("Player");
        if (p) player = p.transform;
    }

    void Update(){
        if (hp != null && hp.IsDead) return;
        if (!player) return;

        // 간단한 수평 추적
        Vector2 dir = new Vector2(Mathf.Sign(player.position.x - transform.position.x), 0f);
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
