using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class IaidoAttack : MonoBehaviour
{
    public AttackData data;
    public Collider2D hitbox;           // child, isTrigger, layer=Hitbox
    public Animator anim;
    public TrailRenderer trail;         // 선택
    public CameraShaker shaker;         // 선택

    PlayerMotor motor;
    float bufferT;
    bool inAttack;

    void Awake(){
        motor = GetComponent<PlayerMotor>();
        if(!anim) anim = GetComponent<Animator>();
        if(hitbox) hitbox.enabled = false;
        if(trail) trail.emitting = false;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.J)) BufferAttack();
        if (bufferT > 0) bufferT -= Time.unscaledDeltaTime;

        if (!inAttack && bufferT > 0 && CanAttackNow()){
            bufferT = 0;
            StartCoroutine(CoAttack());
        }
    }

    public void BufferAttack(){ bufferT = data.bufferTime; }
    bool CanAttackNow(){ return motor.state != PlayerState.Dash && motor.state != PlayerState.Slash && motor.state != PlayerState.Draw; }

    System.Collections.IEnumerator CoAttack(){
        inAttack = true;

        // Draw
        motor.LockMovement(PlayerState.Draw, data.drawTime + data.activeTime + data.sheathTime);
        anim?.Play("Draw");
        yield return Wait(data.drawTime);

        // Slash (Active frames)
        anim?.Play("Slash");
        if(trail) trail.emitting = true;
        hitbox.enabled = true;
        yield return Wait(data.activeTime);
        hitbox.enabled = false;
        if(trail) trail.emitting = false;

        // Sheath
        anim?.Play("Sheath");
        yield return Wait(data.sheathTime);

        inAttack = false;
    }

    System.Collections.IEnumerator Wait(float t){
        float timer = t;
        while (timer>0){ timer -= Time.deltaTime; yield return null; }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (!hitbox.enabled) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Hurtbox")) return;

        var h = other.GetComponentInParent<Health>();
        if (h && !h.IsDead){
            h.TakeDamage(data.damage);
            StartCoroutine(HitStop());
            shaker?.Shake(data.shakeDur, data.shakeMag);
        }
    }

    System.Collections.IEnumerator HitStop(){
        float prev = Time.timeScale;
        Time.timeScale = data.slowScale;
        yield return new WaitForSecondsRealtime(data.slowDuration);
        Time.timeScale = prev;
    }
}
