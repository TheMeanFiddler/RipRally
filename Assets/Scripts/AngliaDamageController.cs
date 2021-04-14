using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AngliaDamageController : DamageController
{
    HingeJoint BonnetHinge;
    FixedJoint LHoodSideHinge;
    HingeJoint LDoorHinge;

    void Start()
    {
        Crash1AudioSource = transform.Find("Sounds/Crash1").GetComponent<AudioSource>();
        Crash2AudioSource = transform.Find("Sounds/Crash2").GetComponent<AudioSource>();
        Crash3AudioSource = transform.Find("Sounds/Crash3").GetComponent<AudioSource>();
        if (GetComponent<AngliaReplayerController>()) _replayer = true;
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
            else if (BoundsCenter.y > 0.9f)
            {
                strname = "ColldrRoof";
            }
            else if (BoundsCenter.z>0.5f)
            {
                strname = "ColldrBonnet";
            }
            else if (BoundsCenter.z >= -0.1f)
            {
                if(BoundsCenter.x > 0.1f)
                {
                    strname = "ColldrRSide";
                }
                else
                {
                    strname = "ColldrLSide";
                }
            }
            else if (c.bounds.size.z > 2.3f)
            {
                strname = "ColldrBottom";
            }
            else if (BoundsCenter.z < -1f)
            {
                if (BoundsCenter.x > 0.1f)
                {
                    strname = "ColldrRRWing";
                }
                else
                {
                    strname = "ColldrRLWing";
                }
            }


            colldrMap.Add(c.GetInstanceID(), strname);
        }
    }

    public override void AttachHingesAndJoints()
    {


        //Create all the hinges and joints
        GameObject LHood = car.transform.Find("Bonnet").gameObject;
        BonnetHinge = LHood.AddComponent<HingeJoint>();
        BonnetHinge.connectedBody = GetComponent<Rigidbody>();
        BonnetHinge.anchor = new Vector3(0.32f, 0.05f, -0.47f);
        BonnetHinge.axis = new Vector3(1, 0, 0);
        BonnetHinge.useLimits = true;
        JointLimits JL = new JointLimits();
        JL.max = -50;
        JL.min = 0;
        BonnetHinge.limits = JL;
        BonnetHinge.breakForce = 32000;
        BonnetHinge.breakTorque = 32000;
        BonnetHinge.GetComponent<Rigidbody>().mass = 20;

        HingeParams HP = new HingeParams
        {
            Anchor = new Vector3(0, 0f, 0.59f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 60, min = -10 }
        };
        partsMap["FLDoor"].Hinges.Add("FLDoor", HP);

        HP = new HingeParams
        {
            Anchor = new Vector3(0, 0f, 0.59f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 10, min = -60 }
        };
        partsMap["FRDoor"].Hinges.Add("FRDoor", HP);

        HP = new HingeParams
        {
            Anchor = new Vector3(0.69f, 0f, 0f),
            Axis = new Vector3(0, 1, -0.3f),
            Limits = new JointLimits { max = 0, min = -100 }
        };
        partsMap["RBumper"].Hinges.Add("RBumper", HP);

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
                Vector3 relVel = transform.InverseTransformVector(coll.relativeVelocity);
                float relVelSqrMag = relVel.sqrMagnitude;
                Vector3 localPoint = transform.InverseTransformPoint(cont.point);
                Vector3 Impulse = transform.InverseTransformVector(coll.impulse);
                if (colldrName == "ColldrF" && localPoint.y > 0.5f)    //so you dont get kerb collisions && relVel.sqrMagnitude > 200)
                {
                    if (localPoint.x < -0.5f)
                    {
                        Scratch(partsMap["FLWing"]);
                        if (relVelSqrMag > 200 && relVelSqrMag < 800)
                        { Damage(partsMap["FLWing"], "Crunch"); }
                        if (relVelSqrMag > 800)
                        { }
                    }
                    else if (localPoint.x < 0.5f)
                    {
                        Scratch(partsMap["Grille"]);
                        if (relVelSqrMag > 800)
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

                if (colldrName == "ColldrLSide")
                {
                    if (Mathf.Abs(Impulse.x) > 600)
                    { Damage(partsMap["FLDoor"], "Crunch"); AddHinge(partsMap["FLDoor"], "FLDoor"); }
                    if (Mathf.Abs(Impulse.x) > 2500)
                    {
                        BreakOffPart(partsMap["FLDoor"]);
                    }
                }

                if (colldrName == "ColldrRSide")
                {
                    if (Mathf.Abs(Impulse.x) > 600)
                    { Damage(partsMap["FRDoor"], "Crunch"); AddHinge(partsMap["FRDoor"], "FRDoor"); }
                    if (Mathf.Abs(Impulse.x) > 2500)
                    {
                        BreakOffPart(partsMap["FRDoor"]);
                    }
                }

                if (colldrName == "ColldrRRWing")
                {
                    if (Mathf.Abs(Impulse.x) > 600)
                    { Damage(partsMap["RRWing"], "Crunch"); }
                }

                if (colldrName == "ColldrTrunk")
                {
                    if (Mathf.Abs(Impulse.z) > 8000)
                        AddHinge(partsMap["RBumper"], "RBumper");
                    if (Mathf.Abs(Impulse.z) > 5000)
                        { Damage(partsMap["Trunk"], "Crunch"); Damage(partsMap["Rear"], "Crunch"); }
                }

                if (colldrName == "ColldrRLWing" && Mathf.Abs(Impulse.x) > 600)
                {
                    { Damage(partsMap["RLWing"], "Crunch"); }
                    //MeshDeform(partsMap["RLWing"], coll.contacts[0].point, Impulse);
                    //MeshDeform(partsMap["Trunk"], coll.contacts[0].point, Impulse);
                }

                if (colldrName == "ColldrRoof")
                {
                    if (Mathf.Abs(Impulse.y) > 10000)
                        Damage(partsMap["Roof"], "Mangle");
                    else if (Mathf.Abs(Impulse.y) > 2000)
                        Damage(partsMap["Roof"], "Crunch");

                }

                    //MeshDeform(partsMap["Roof"], coll.contacts[0].point, Impulse);

                if (Damg == false && !Crash3AudioSource.isPlaying) { Crash3AudioSource.pitch = Random.Range(0.17f, 0.26f); Crash3AudioSource.volume = Mathf.Clamp01(coll.relativeVelocity.magnitude / 20); Crash3AudioSource.Play(); }
            }
        }
        //Tell the GPS that we've hit a fence
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




