using UnityEngine;


public class SceneryPheasant: MonoBehaviour
{
    public bool Walking;
    public float Speed;
    Animator Anim;
    int IdleHash;
    Transform leftFoot;
    Transform rightFoot;
    public Transform leftIKTarget;
    public Transform rightIKTarget;
    public float IKWeight = 1;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Debug.Log(Anim.name);
        IdleHash = Anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        //Anim.SetInteger("AnimType", AnimType);
        Anim.SetBool("Walking", false);
        //Anim.speed = 0f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            Anim.SetBool("Walking", true);

        }
        else
        {
            Anim.SetBool("Walking", false);
        }
        //Anim.SetFloat("Speed", Speed);
        //transform.Translate(transform.forward * Speed);
        //Anim.speed = Speed * 10;

    }

    /*void OnAnimatorIK()
    {
        Anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        Anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);

        Anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftIKTarget.position);
        Anim.SetIKPosition(AvatarIKGoal.RightFoot, rightIKTarget.position);
    }*/
}
