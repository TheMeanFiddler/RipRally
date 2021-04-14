using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PorscheDamageController : DamageController
{


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
            else if (BoundsCenter.y > 0.8f)
            {
                strname = "ColldrRoof";
            }

            else if (c.bounds.size.z > 2.2f)
            {
                strname = "ColldrBottom";
            }
            


            colldrMap.Add(c.GetInstanceID(), strname);
        }
    }

    public override void AttachHingesAndJoints()
    {

        //Create all the hinges and joints

        HingeParams HP = new HingeParams
        {
            Anchor = new Vector3(-0.08f, 0f, 0.7f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 60, min = -3 }
        };
        partsMap["FLDoor"].Hinges.Add("FLDoor", HP);

        HP = new HingeParams
        {
            Anchor = new Vector3(-0.08f, 0f, 0.7f),
            Axis = new Vector3(0, 1, 0),
            Limits = new JointLimits { max = 60, min = -0 }
        };
        partsMap["FRDoor"].Hinges.Add("FRDoor", HP);

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
                    if (Impulse.y>800)
                    {
                        
                        BreakOffPart(partsMap["FrontPanel"]);
                    }
                    if (Impulse.x>700)
                    {
                       BreakOffPart(partsMap["FLWing"]);
                    }
                }

                /*
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
                */

                if (colldrName == "ColldrLSide")
                {
                    //Scratch(partsMap["CarBody"]);
                    if (Mathf.Abs(Impulse.x) > 1000)
                    {
                        AddHinge(partsMap["FLDoor"], "FLDoor");
                        //Damage(partsMap["FLDoor"], "Crunch");
                    }
                    if (Mathf.Abs(Impulse.z) > 500)
                    {
                        BreakOffPart(partsMap["FLDoor"]);
                    }
                }
                if (colldrName == "ColldrRSide")
                {
                    //Scratch(partsMap["CarBody"]);
                    if (Mathf.Abs(Impulse.x) > 1000)
                    {
                        AddHinge(partsMap["FRDoor"], "FRDoor");
                        //Damage(partsMap["FLDoor"], "Crunch");
                    }
                    if (Mathf.Abs(Impulse.z) > 500)
                    {
                        BreakOffPart(partsMap["FRDoor"]);
                    }
                }
                /*
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
*/
                if (Damg == false && !Crash3AudioSource.isPlaying) { Crash3AudioSource.pitch = Random.Range(0.17f, 0.26f); Crash3AudioSource.volume = Mathf.Clamp01(coll.relativeVelocity.magnitude / 20); Crash3AudioSource.Play(); }
            }
        }
        
        //Tell the GPS that we've hit a fence so the drift fails
        if (coll.collider.name.StartsWith("RoadSec") && Gps!=null) {Gps.CollideRoadSection(); }
    }


} //end of DamageController class




