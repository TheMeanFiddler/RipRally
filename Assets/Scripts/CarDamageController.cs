using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarDamageController : DamageController
{

    HingeJoint BonnetHinge;
    HingeJoint LDoorHinge;
    ParticleSystem psDivot;

    void Start()
    {
        Crash1AudioSource = transform.Find("Engine/Crash1").GetComponent<AudioSource>();
        Crash2AudioSource = transform.Find("Engine/Crash2").GetComponent<AudioSource>();
        Crash3AudioSource = transform.Find("Engine/Crash3").GetComponent<AudioSource>();
        if (GetComponent<CarReplayerController>()) _replayer = true;
        psDivot = transform.Find("Divot").GetComponent<ParticleSystem>();
    }

    public override void CreateColliderMap()
    {
        //put all the colliders in a dictionary and give them names
        //  FR  FSR RSR RR
        //  F           R
        //  FL  FSL RSL RL
        colldrMap = new Dictionary<int, string>();
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            string strname;
            //Front and Rear
            if (c.bounds.center.z - transform.position.z > 0)
            {
                strname = "ColldrF";
            }
            else
            {
                strname = "ColldrR";
            }
            //T=Top  S=Side   Nothing = Bottom
            if (c.bounds.center.y - transform.position.y > 1)
            {
                strname += "T";
            }
            else
            {
                if (c.bounds.center.y - transform.position.y > 0.57)
                {
                    strname += "S";
                }
                else
                {
                    if (c.bounds.center.y - transform.position.y < 0.4)
                    {
                        strname = "Bottom Collider";
                    }
                }
            }
            //L and R
            if (c.bounds.center.x - transform.position.x < -0.1)
            {
                strname += "L";
            }
            if (c.bounds.center.x - transform.position.x > 0.1)
            {
                strname += "R";
            }
            colldrMap.Add(c.GetInstanceID(), strname);
        }
    }

    public override void AttachHingesAndJoints()
    {

        //Create all the hinges and joints
        HingeJoint BH;
        FixedJoint BF;
        GameObject Bonnet = car.transform.Find("Bonnet.FCrunch").gameObject;
        BonnetHinge = Bonnet.AddComponent<HingeJoint>();
        BonnetHinge.connectedBody = GetComponent<Rigidbody>();
        BonnetHinge.anchor = new Vector3(0, -0.12f, 0.47f);
        BonnetHinge.axis = new Vector3(1, 0, 0);
        BonnetHinge.useLimits = true;
        JointLimits JL = new JointLimits();
        JL.max = 50;
        JL.min = 0;
        BonnetHinge.limits = JL;
        BonnetHinge.breakForce = 1500;
        BonnetHinge.breakTorque = 1500;
        GameObject BonnetFRCrunch = car.transform.Find("Bonnet.FRCrunch").gameObject;
        BH = BonnetFRCrunch.AddComponent<HingeJoint>();
        BH.connectedBody = GetComponent<Rigidbody>();
        BH.anchor = new Vector3(0, 0.12f, 0.47f);
        BH.axis = new Vector3(1, 0, 0);
        BH.useLimits = true;
        BH.limits = JL;
        BH.breakForce = 1500;
        BH.breakTorque = 1500;
        GameObject Trunk = car.transform.Find("Trunk").gameObject;
        BH = Trunk.AddComponent<HingeJoint>();
        BH.connectedBody = GetComponent<Rigidbody>();
        BH.anchor = new Vector3(0, 0.26f, 0.59f);   //Depends where you put the origin in Blender
        BH.axis = new Vector3(1, 0, 0);
        BH.useLimits = true;
        JointLimits TLim = new JointLimits();
        TLim.min = 0;
        TLim.max = 60;
        BH.limits = TLim;

        //BH.breakForce = 300f ;
        //BH.breakTorque = 300f;

        HingeParams HP = new HingeParams
        {
            Anchor = new Vector3(0, 0f, 0.59f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 60, min = -10 }
        };
        partsMap["FLDoor"].Hinges.Add("FLDoor", HP);

        GameObject FRLight = car.transform.Find("FRLight").gameObject;
        BF = FRLight.AddComponent<FixedJoint>();
        BF.connectedBody = GetComponent<Rigidbody>();
        BF.breakForce = 400;
        GameObject FLLight = car.transform.Find("FLLight").gameObject;
        BF = FLLight.AddComponent<FixedJoint>();
        BF.connectedBody = GetComponent<Rigidbody>();
        BF.breakForce = 400;

        base.AttachHingesAndJoints();
    }



    void OnCollisionEnter(Collision coll)
    {
        if (_replayer) return;
        if (Race.Current == null) return;
        if (!Race.Current.Started) return;
        if (Crash3AudioSource == null) return;
        Damg = false;
        if (name.StartsWith("Menu")) return;
        if (_rb.velocity.sqrMagnitude < 8) { StationaryCollision = true;} else StationaryCollision = false;
        float sqrMag = coll.relativeVelocity.sqrMagnitude;
        foreach (ContactPoint cont in coll.contacts)
        {
            Collider colldr = cont.thisCollider;

            try
            {
                if (colldr.sharedMaterial.name == "CarBodyPhysicsMaterial" && cont.normal.y > 0.5f)
                {
                    colldr.sharedMaterial = StickyCarBodyPhysicsMaterial;
                }
            }
            catch { }
            if (colldr.name.StartsWith("V") || colldr.name.StartsWith("Coll"))
            {
                string colldrName = colldrMap[colldr.GetInstanceID()];
                Vector3 Impulse = transform.InverseTransformVector(coll.impulse);

                if (colldrName == "ColldrF")
                {
                    Vector3 localPoint = transform.InverseTransformPoint(cont.point);
                    if (cont.otherCollider.name.EndsWith("rain"))
                    {
                        psDivot.Stop();
                        psDivot.Play();
                    }
                    if (localPoint.y > 0.5f && sqrMag > 200)    //so you dont get kerb collisions
                    {
                        if (localPoint.x < -0.5f)
                        {
                            if (Random.value > 0.5f) BreakOffPart(partsMap["FLLight"]);
                            BreakOffPart(partsMap["FLWing"]);
                        }
                        else if (localPoint.x < 0.5f)
                        {
                            //Front hit square on
                            
                            if (sqrMag < 800)
                                Damage(partsMap["Bonnet"], "FCrunch");
                            else
                                BreakOffPart(partsMap["Bonnet"]);
                            if (sqrMag < 600)
                                Damage(partsMap["FrontPanel"], "FCrunch");
                            else
                                BreakOffPart(partsMap["FrontPanel"]);
                        }
                        else
                        {
                            if(sqrMag < 800)
                            {Damage(partsMap["FRWing"], "FRCrunch");}
                            else
                            { BreakOffPart(partsMap["FRWing"]); }
                            if (sqrMag > 400)
                            {
                                if(Random.value>0.5f) BreakOffPart(partsMap["FRLight"]);
                                Damage(partsMap["Bonnet"], "FRCrunch");
                                Damage(partsMap["FrontPanel"], "FRCrunch");
                            }
                        }
                    }
                }

                if (colldrName == "ColldrFSR")
                {
                    Damage(partsMap["FRDoor"], "Scrape");
                }
                if (colldrName == "ColldrFSL")
                {
                    if (Mathf.Abs(Impulse.x) > 600)
                    { Damage(partsMap["FLDoor"], "Crunch"); AddHinge(partsMap["FLDoor"], "FLDoor");}
                    if (Mathf.Abs(Impulse.x) > 3000)
                    {
                        BreakOffPart(partsMap["FLDoor"]);
                    }
                }
                if (colldrName == "ColldrRSL" && coll.relativeVelocity.magnitude > 15)
                {
                    Damage(partsMap["LSidePanel"],"Crunch");
                }
                if (colldrName == "ColldrR" && coll.relativeVelocity.z > 10) //was 12
                {
                    Damage(partsMap["Rear"],"RCrunch");
                }
                if (colldrName == "ColldrFTL")
                {
                    if (sqrMag > 600)
                    {
                        Damage(partsMap["FLTop"], "Mangle");
                    }
                    else if (sqrMag > 100)
                    {
                        Damage(partsMap["FLTop"], "Crunch");
                    }
                }
                if (colldrName == "ColldrFTR" && sqrMag > 100)
                {
                    Damage(partsMap["FRTop"],"Crunch");
                }
                if (Damg == false && !Crash3AudioSource.isPlaying) { Crash3AudioSource.pitch = Random.Range(0.17f, 0.26f); Crash3AudioSource.volume = Mathf.Clamp01(coll.relativeVelocity.magnitude / 20); Crash3AudioSource.Play(); }
            }
        }
    }

    void PlaceMarker(Vector3 pos, Vector3 LookDirection)
    {
        GameObject rtn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rtn.GetComponent<Collider>().enabled = false;
        rtn.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        rtn.transform.LookAt(LookDirection);
        rtn.transform.position = pos + LookDirection * 0.5f;
    }
} //end of DamageController class




