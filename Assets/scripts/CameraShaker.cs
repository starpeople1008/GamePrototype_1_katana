using UnityEngine;

public class CameraShaker : MonoBehaviour {
    Vector3 origin;
    void Awake(){ origin = transform.localPosition; }
    public void Shake(float dur, float mag){
        StopAllCoroutines();
        StartCoroutine(Co(dur, mag));
    }
    System.Collections.IEnumerator Co(float t, float m){
        float timer=0;
        while(timer<t){
            timer += Time.deltaTime;
            transform.localPosition = origin + (Vector3)Random.insideUnitCircle * m;
            yield return null;
        }
        transform.localPosition = origin;
    }
}
