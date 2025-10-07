using UnityEngine;

public enum PlayerState { Idle, Run, Dash, Draw, Slash, Sheath, Hit, Dead }

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {
    [Header("Move")]
    public float moveSpeed = 6f;
    public float dashSpeed = 14f;
    public float dashTime = 0.15f;
    public float dashCooldown = 0.4f;

    [Header("Refs")]
    public Animator anim;

    Rigidbody2D rb;
    public PlayerState state { get; private set; } = PlayerState.Idle;
    float x, dashT, dashCD;
    bool facingRight = true;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        if(!anim) anim = GetComponent<Animator>();
    }

    void Update(){
        x = Input.GetAxisRaw("Horizontal");
        bool dash = Input.GetKeyDown(KeyCode.LeftShift);

        // 방향 전환
        if (x != 0) {
            bool right = x > 0;
            if (right != facingRight) Flip(right);
        }

        // 대시 입력
        if (dash && dashCD <= 0 && state != PlayerState.Slash && state != PlayerState.Draw){
            state = PlayerState.Dash;
            dashT = dashTime;
            dashCD = dashCooldown + dashTime;
            if(anim) anim.Play("Dash");
        }

        if (dashCD > 0) dashCD -= Time.deltaTime;
    }

    void FixedUpdate(){
        switch(state){
            case PlayerState.Idle:
            case PlayerState.Run:
                rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);
                if (Mathf.Abs(x) > 0.01f) { state = PlayerState.Run; anim?.Play("Run"); }
                else { state = PlayerState.Idle; anim?.Play("Idle"); }
            break;

            case PlayerState.Dash:
                rb.velocity = new Vector2((facingRight?1:-1)*dashSpeed, 0);
                dashT -= Time.fixedDeltaTime;
                if (dashT <= 0) state = PlayerState.Idle;
            break;

            case PlayerState.Draw:
            case PlayerState.Slash:
            case PlayerState.Sheath:
            case PlayerState.Hit:
            case PlayerState.Dead:
                // 이동 잠금
            break;
        }
    }

    public void LockMovement(PlayerState lockAs, float duration){
        StopAllCoroutines();
        StartCoroutine(LockRoutine(lockAs, duration));
    }
    System.Collections.IEnumerator LockRoutine(PlayerState s, float t){
        var prev = state;
        state = s;
        float timer = t;
        while(timer>0){ timer -= Time.deltaTime; yield return null; }
        if (state != PlayerState.Dead) state = PlayerState.Idle;
    }

    void Flip(bool right){
        facingRight = right;
        var s = transform.localScale; s.x = Mathf.Abs(s.x)*(right?1:-1); transform.localScale = s;
    }

    public bool FacingRight => facingRight;
}
