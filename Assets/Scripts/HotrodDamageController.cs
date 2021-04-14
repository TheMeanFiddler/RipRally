using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HotrodDamageController : DamageController
{
    HingeJoint LHoodHinge;
    HingeJoint RHoodHinge;
    FixedJoint LHoodSideHinge;
    HingeJoint LDoorHinge;

    void Start()
    {
        Crash1AudioSource = transform.Find("Sounds/Crash1").GetComponent<AudioSource>();
        Crash2AudioSource = transform.Find("Sounds/Crash2").GetComponent<AudioSource>();
        Crash3AudioSource = transform.Find("Sounds/Crash3").GetComponent<AudioSource>();
        if (GetComponent<HotrodReplayerController>()) _replayer = true;
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
            Vector3 BoundsCenter = transform.InverseTransformPoint(c.bounds.center);
            string strname = "";
            if (!c.name.StartsWith("V"))
            {
                strname = c.name;
            }
            else if (BoundsCenter.z > 0.9f)
            {
                strname = "ColldrHood";
            }
            else if (BoundsCenter.y > 0.9f)
            {
                strname = "ColldrRoof";
            }
            else if (BoundsCenter.z > -0.02f)
            {
                if(BoundsCenter.x > 0)
                {
                    strname = "ColldrRFootplate";
                }
                else
                {
                    strname = "ColldrLFootplate";
                }
            }
            else if (c.bounds.size.z > 2.3f)
            {
                strname = "ColldrBottom";
            }
            else if (BoundsCenter.z < -1f)
            {
                if (BoundsCenter.x > 0)
                {
                    strname = "ColldrRRWheelarch";
                }
                else
                {
                    strname = "ColldrLRWheelarch";
                }

            }


            colldrMap.Add(c.GetInstanceID(), strname);
        }
    }

    public override void AttachHingesAndJoints()
    {


        //Create all the hinges and joints
        HingeJoint BH;
        FixedJoint BF;
        GameObject LHood = car.transform.Find("LHood").gameObject;
        LHoodHinge = LHood.AddComponent<HingeJoint>();
        LHoodHinge.connectedBody = GetComponent<Rigidbody>();
        LHoodHinge.anchor = new Vector3(0.32f, 0.1f, 0f);
        LHoodHinge.axis = new Vector3(0, -0.08f, 1);
        LHoodHinge.useLimits = true;
        JointLimits JL = new JointLimits();
        JL.max = -90;
        JL.min = 0;
        LHoodHinge.limits = JL;
        LHoodHinge.breakForce = 32000;
        LHoodHinge.breakTorque = 32000;
        LHoodHinge.GetComponent<Rigidbody>().mass = 20;

        GameObject RHood = car.transform.Find("RHood").gameObject;
        RHoodHinge = RHood.AddComponent<HingeJoint>();
        RHoodHinge.connectedBody = GetComponent<Rigidbody>();
        RHoodHinge.anchor = new Vector3(-0.34f, 0.23f, 0f);
        RHoodHinge.axis = new Vector3(0, -0.08f, -1);
        RHoodHinge.useLimits = true;
        JointLimits JLRH = new JointLimits();
        JLRH.max = -90;
        JLRH.min = 0;
        RHoodHinge.limits = JLRH;
        RHoodHinge.breakForce = 1600;
        RHoodHinge.breakTorque = 1600;

        HingeParams HP = new HingeParams
        {
            Anchor = new Vector3(0, 0f, -0.48f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 10, min = -60 }
        };
        partsMap["FLDoor"].Hinges.Add("FLDoor", HP);

        HP = new HingeParams
        {
            Anchor = new Vector3(0, 0f, 0.48f),
            Axis = new Vector3(0.4f, 1, 0),
            Limits = new JointLimits { max = -40, min = 10 }
        };
        partsMap["RFootplate"].Hinges.Add("RFootplate", HP);

        base.AttachHingesAndJoints();
    }


    void OnCollisionEnter(Collision coll)
    {
        if (_replayer) return;
        if (Race.Current==null) return;
        if (!Race.Current.Started) return;
        if (Crash3AudioSource == null) return;
        Damg = false;
        if (name.StartsWith("Menu")) return;

        if (_rb.velocity.sqrMagnitude < 8) StationaryCollision = true; else StationaryCollision = false;

        foreach (ContactPoint cont in coll.contacts)
        {
            Collider colldr = cont.thisCollider;
            try
            {
                if (cont.thisCollider.sharedMaterial.name == "CarBodyPhysicsMaterial" && cont.normal.y > 0.5f)
                {
                    cont.thisCollider.sharedMaterial = StickyCarBodyPhysicsMaterial;
                }
            }
            catch { }

            if (colldr.name.StartsWith("V") || colldr.name.StartsWith("Coll"))
            {
                string colldrName = colldrMap[colldr.GetInstanceID()];
                Vector3 Impulse = transform.InverseTransformVector(coll.impulse);
                Vector3 relVel = transform.InverseTransformVector(coll.relativeVelocity);
                float relVelSqrMag = relVel.sqrMagnitude;
                Vector3 localPoint = transform.InverseTransformPoint(cont.point);

                if (colldrName == "ColldrF" && localPoint.y > 0.5f)    //so you dont get kerb collisions && relVel.sqrMagnitude > 200)
                {
                    if (localPoint.x < -0.5f)
                    {
                        Scratch(partsMap["FLWing"]);
                        if (relVelSqrMag > 200 && relVelSqrMag < 800)
                        { Damage(partsMap["FLWing"], "Crunch"); }
                        if (relVelSqrMag > 800)
                        { BreakOffPart(partsMap["FLWing"]); }
                    }
                    else if (localPoint.x < 0.5f)
                    {
                        Scratch(partsMap["Grille"]);
                        if(relVelSqrMag > 600)
                        BreakOffPart(partsMap["Grille"]);
                    }
                    else
                    {
                        Scratch(partsMap["FRWing"]);
                        if (relVelSqrMag > 200 && relVelSqrMag < 800)
                        { Damage(partsMap["FRWing"], "Crunch"); }
                        if (relVelSqrMag > 800)
                        { BreakOffPart(partsMap["FRWing"]); }
                    }
                }

                if (colldrName == "ColldrLRWheelarch")
                {
                    Scratch(partsMap["RLWing"]);
                    if(relVel.x > 600)
                    BreakOffPart(partsMap["RLWing"]);
                }

                if (colldrName == "ColldrRRWheelarch")
                {
                    Scratch(partsMap["RRWing"]);
                    if (relVel.x < -600)
                    BreakOffPart(partsMap["RRWing"]);
                }

                if (colldrName == "ColldrRFootplate")
                {
                    Scratch(partsMap["RFootplate"]);
                    Scratch(partsMap["FRDoor"]);
                    Scratch(partsMap["RHood"]);
                    Scratch(partsMap["CarBody"]);
                    if (relVelSqrMag > 400 && relVelSqrMag < 1000)
                        AddHinge(partsMap["RFootplate"], "RFootplate");
                    if (relVelSqrMag > 1000)
                    BreakOffPart(partsMap["RFootplate"]);
                }

                if (colldrName == "ColldrLFootplate")
                {
                    Scratch(partsMap["LFootplate"]);
                    Scratch(partsMap["FLDoor"]);
                    Scratch(partsMap["LHood"]);
                    if (relVelSqrMag > 1000)
                        BreakOffPart(partsMap["LFootplate"]);
                    else
                    {
                        AddHinge(partsMap["FLDoor"], "FLDoor");
                    }
                }


                if (colldrName == "ColldrLSide")
                {
                    Scratch(partsMap["CarBody"]);
                    if (Mathf.Abs(Impulse.x) > 600)
                    { Damage(partsMap["FLDoor"], "Crunch"); AddHinge(partsMap["FLDoor"], "FLDoor"); }
                    if (Mathf.Abs(Impulse.z) > 500)
                    {
                        BreakOffPart(partsMap["FLDoor"]);
                    }
                }

                if (colldrName == "ColldrRoof")
                {
                    Damage(partsMap["Roof"], "Crunch");
                }

                if (colldrName == "ColldrTrunk" && Mathf.Abs(Impulse.z) > 1000)
                {
                    Damage(partsMap["Tail"], "Crunch");
                    Scratch(partsMap["Tail"]);
                    Scratch(partsMap["Trunk"]);
                    Scratch(partsMap["RLWing"]);
                    Scratch(partsMap["RRWing"]);
                }

                if (Damg == false && !Crash3AudioSource.isPlaying) { Crash3AudioSource.pitch = Random.Range(0.17f, 0.26f); Crash3AudioSource.volume = Mathf.Clamp01(coll.relativeVelocity.magnitude / 20); Crash3AudioSource.Play(); }
            }
        }
        //Tell the GPS that we've hit a fence so the drift fails
        if (coll.collider.name.StartsWith("RoadSec") && Gps!=null) {Gps.CollideRoadSection(); }
    }


    void PlaceMarker(Vector3 pos, Vector3 LookDirection)
    {
        GameObject rtn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rtn.GetComponent<Collider>().enabled = false;
        rtn.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        rtn.transform.LookAt(LookDirection.normalized);
        rtn.transform.position = pos + LookDirection.normalized * 0.5f;
        rtn.name = LookDirection.ToString();
    }

} //end of DamageController class




