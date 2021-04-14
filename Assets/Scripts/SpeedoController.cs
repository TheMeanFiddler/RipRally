using UnityEngine;
using UnityEngine.UI;



class SpeedoController:MonoBehaviour
{
    Transform Pointer;
    public Rigidbody Target;

    public void Init()
    {
        Pointer = transform.GetChild(0);
        GameObject[] P = GameObject.FindGameObjectsWithTag("Player");
        Target = P[P.Length - 1].GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Target == null) return;
        float rot = Target.velocity.magnitude * -6.7f;    // checked 10/05/2020
        Pointer.rotation = Quaternion.Euler(0,0,rot);
    }
}

