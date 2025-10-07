using UnityEngine;

public class Health : MonoBehaviour {
    public int maxHP = 1;
    public Animator anim;
    public bool IsDead { get; private set; }
    int hp;

    void Awake(){ hp = maxHP; if(!anim) anim = GetComponent<Animator>(); }

    public void TakeDamage(int dmg){
        if (IsDead) return;
        hp -= dmg;
        if (hp <= 0) Die();
    }

    void Die(){
        IsDead = true;
        anim?.Play("Die");
        Destroy(gameObject, 0.3f);
    }
}
