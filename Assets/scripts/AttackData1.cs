using UnityEngine;

[CreateAssetMenu(fileName="IaidoAttackData", menuName="Combat/Iaido Attack Data")]
public class AttackData : ScriptableObject {
    [Header("Timing (seconds)")]
    public float bufferTime = 0.12f;   // 입력 버퍼
    public float drawTime   = 0.07f;   // 발도 준비
    public float activeTime = 0.05f;   // 히트박스 활성 프레임
    public float sheathTime = 0.14f;   // 검집

    [Header("HitStop/SlowMo")]
    [Range(0.02f, 1f)] public float slowScale = 0.15f;
    public float slowDuration = 0.08f;

    [Header("Damage/FX")]
    public int damage = 1;
    public float shakeDur = 0.1f;
    public float shakeMag = 0.2f;
}
