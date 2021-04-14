using UnityEngine;
public delegate void PheasantBounceHandler();

public class PheasantController:MonoBehaviour
{
    public event PheasantBounceHandler BounceEvent;
    public Transform trGroundDatum;
    private Animator anim;
    private int TakeoffHash = Animator.StringToHash("Base Layer.TakeOff");
    private int FlyHash = Animator.StringToHash("Base Layer.Fly");
    private int PeckHash = Animator.StringToHash("Base Layer.Peck");
    private int LandHash = Animator.StringToHash("Base Layer.Land");

    private void Awake()
    {
        trGroundDatum = new GameObject().transform;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(trGroundDatum.GetComponent<BoxCollider>());
        trGroundDatum.position = transform.position;
    }
    public void Bounce()
    {
        if (BounceEvent != null)
            BounceEvent();
    }


    RaycastHit hit;
    float OldDatumY;


    void OnAnimatorMove()
    {
        //This allows you to move the gameobject and the datum will follow it
        //if(trGroundDatum.position.x != transform.position.x || trGroundDatum.position.z != transform.position.z)
        //{
            //trGroundDatum.position = transform.position;
        //}
        //if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == TakeoffHash) { return; }
        OldDatumY = trGroundDatum.position.y;
        transform.forward = anim.deltaRotation * transform.forward;
        int Hash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;

        //This allows it to walk up a hill using a GroundDatum transform that follows the pheasant round and sticks to the ground
        {
            //do a small raycast
            if (Physics.Raycast(trGroundDatum.position + Vector3.up * 3, Vector3.down, out hit, 5))
            {
                trGroundDatum.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                if (Hash == TakeoffHash || Hash == FlyHash || Hash == LandHash)
                { transform.position += anim.deltaPosition; }
                else
                {
                    if (Hash == PeckHash)
                        transform.position = trGroundDatum.position;
                    else
                        transform.position += anim.deltaPosition + Vector3.up * (hit.point.y - OldDatumY);
                }
            }
            else
            //if the small raycast missed, do a big raycast
            {
                if (Physics.Raycast(transform.position + Vector3.up * 3, Vector3.down, out hit))
                {
                    trGroundDatum.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                    if (Hash == TakeoffHash || Hash == FlyHash)
                    { transform.position += anim.deltaPosition;}
                    else
                        transform.position += anim.deltaPosition + Vector3.up * (hit.point.y - OldDatumY);
                }
            }
        }
    }

    public bool IsTakingOff
    {
        get { return anim.GetCurrentAnimatorStateInfo(0).fullPathHash == TakeoffHash; }
    }

    public bool IsFlying
    {
        get { return anim.GetCurrentAnimatorStateInfo(0).fullPathHash == FlyHash; }
    }

}

