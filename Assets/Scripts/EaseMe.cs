using UnityEngine;
class EaseMe: MonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 EndPos;
    public Quaternion StartRot;
    public Quaternion EndRot;
    public float Duration;
    float t;
    void Start()
    {
        t = Time.time;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(StartPos, EndPos, (Time.time-t)/Duration);
        transform.rotation = Quaternion.Lerp(StartRot, EndRot, (Time.time - t) / Duration);
        if (Time.time - t > Duration) Destroy(this);
    }
}

