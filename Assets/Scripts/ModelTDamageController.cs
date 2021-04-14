using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class ModelTDamageController : DamageController {


        public override void CreateColliderMap() {
            //put all the colliders in a dictionary and give them names
            //  FR  FSR RSR RR
            //  F           R
            //  FL  FSL RSL RL
             colldrMap = new Dictionary<int, string>();
             foreach (Collider c in GetComponents<Collider>())
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
             AttachHingesAndJoints();

        } //end of Awake

        public override void AttachHingesAndJoints()
        {
            return;
            //Create all the hinges and joints
            HingeJoint BH;
            FixedJoint BF;
            BoxCollider BC;
            GameObject Bonnet = car.transform.Find("Bonnet.FCrunch").gameObject;
            BH = Bonnet.AddComponent<HingeJoint>();
            BH.connectedBody = GetComponent<Rigidbody>();
            BH.anchor = new Vector3(0, -0.48f, 0.08f);
            BH.axis = new Vector3(1, 0, 0);
            BH.useLimits = true;
            JointLimits JL = new JointLimits();
            JL.max = 50;
            BH.limits = JL;
            BH.breakForce = 100;
            BH.breakTorque = 100;
            GameObject BonnetFRCrunch = car.transform.Find("Bonnet.FRCrunch").gameObject;
            BH = BonnetFRCrunch.AddComponent<HingeJoint>();
            BH.connectedBody = GetComponent<Rigidbody>();
            BH.anchor = new Vector3(0, -0.51f, 0.12f);
            BH.axis = new Vector3(1, 0, 0);
            BH.useLimits = true;
            BH.limits = JL;
            BH.breakForce = 100;
            BH.breakTorque = 100;
            GameObject Trunk = car.transform.Find("Trunk").gameObject;
            BH = Trunk.AddComponent<HingeJoint>();
            BH.connectedBody = GetComponent<Rigidbody>();
            BH.anchor = new Vector3(0, 0.94f, 0.46f);
            BH.axis = new Vector3(1, 0, 0);
            BH.useLimits = true;
            JointLimits TLim = new JointLimits();
            TLim.max = -60;
            BH.limits = TLim;
            //BH.breakForce = 300f ;
            //BH.breakTorque = 300f;
            GameObject FLDoor = car.transform.Find("FLDoor").gameObject;
            BH = FLDoor.AddComponent<HingeJoint>();
            BH.connectedBody = GetComponent<Rigidbody>();
            BH.anchor = new Vector3(0, 0.29f, 0f);
            BH.axis = new Vector3(0, 0, 1);
            BH.useLimits = true;
            JointLimits DLim = new JointLimits();
            DLim.max = 60;
            BH.limits = DLim;
            BH.breakForce = 50f;
            BH.breakTorque = 50f;

            BF = FLDoor.AddComponent<FixedJoint>();
            BF.connectedBody = GetComponent<Rigidbody>();
            BF.breakForce = 10;
            BF.breakTorque = 10;

            BC = FLDoor.AddComponent<BoxCollider>();
            BC.center = new Vector3(0, 0, -0.6f);
            BC.size = new Vector3(-0.16f, 0.45f, 1.7f);
            //FLDoor.AddComponent<JointBreakDetector>();

            GameObject FLWing = car.transform.Find("FLWing").gameObject;
            FLWing.transform.localPosition = new Vector3(-0.76449f, 0.531734943f, 1.68743348f);
            BF = FLWing.AddComponent<FixedJoint>();
            FixedJoint FJ = FLWing.GetComponent<FixedJoint>();
            BF.connectedBody = GetComponent<Rigidbody>();
            BF.breakForce = 30;
            BF.breakTorque = 30;
            //FLWing.AddComponent<JointBreakDetector>();

            GameObject FRLight = car.transform.Find("FRLight").gameObject;
            BF = FRLight.AddComponent<FixedJoint>();
            BF.connectedBody = GetComponent<Rigidbody>();
            BF.breakForce = 25;
            BF.breakTorque = 25;
            //FRLight.AddComponent<JointBreakDetector>();

            GameObject FLLight = car.transform.Find("FLLight").gameObject;
            BF = FLLight.AddComponent<FixedJoint>();
            BF.connectedBody = GetComponent<Rigidbody>();
            BF.breakForce = 20;
            BF.breakTorque = 20;
            //FLLight.AddComponent<JointBreakDetector>();

        }

        void RemoveHingesAndJoints()
        {

        }

        void OnCollisionEnter(Collision coll)
        {
            return;
            foreach (ContactPoint cont in coll.contacts)
            {
                Collider colldr = cont.thisCollider;
                if (colldr.name.Contains("Car"))
                {
                    string colldrName = colldrMap[colldr.GetInstanceID()];

                    if (colldrName == "ColldrFR" && coll.relativeVelocity.magnitude > 15)
                    {
                        partsMap["FRWing"].ShowMesh("FRCrunch");
                        partsMap["Bonnet"].ShowMesh("FRCrunch");
                        partsMap["FrontPanel"].ShowMesh("FRCrunch");
                        
                    }
                    if (colldrName == "ColldrF" && coll.relativeVelocity.magnitude > 15)
                    {
                        partsMap["Bonnet"].ShowMesh("FCrunch");
                        partsMap["FrontPanel"].ShowMesh("FCrunch");
                    }
                    if (colldrName == "ColldrFSR")
                    {
                        partsMap["FRDoor"].ShowMesh("Scrape");
                    }
                    if (colldrName == "ColldrRSL" && coll.relativeVelocity.magnitude > 15)
                    {
                        partsMap["LSidePanel"].ShowMesh("Crunch");
                    }
                    if (colldrName == "ColldrR" && coll.relativeVelocity.magnitude > 15)
                    {
                        partsMap["Rear"].ShowMesh("RCrunch");
                    }
                    if (colldrName == "ColldrFTL" && coll.relativeVelocity.magnitude > 10)
                    {
                        partsMap["FLTop"].ShowMesh("Crunch");
                    }
                }
            }
        }

        public void RepairAll()
        {
            foreach (KeyValuePair<string, CarPart> Part in partsMap)
            {
                if (!Part.Key.Contains(".") && !Part.Key.Contains("Steer"))
                    Part.Value.ShowMesh("Intact");
            }
            AttachHingesAndJoints();
        }

    } //end of DamageController class




