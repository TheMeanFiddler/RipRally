using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


class CarMenuItem : IMenuItem
{

    public int Id { get; set; }
    public int ScrollIdx { get; set; }
    public bool Permanent { get; set; }
    public IMenuListView ContainingMenu { get; set; }
    public IMenuDataItem Data { get; set; }
    public GameObject OuterGameObject { get; set; }

    public bool Deleted { get; set; }

    iInputManager InputManager;

    string _type;
    string _name;

    public void Populate (IMenuDataItem Data) { }

    public void CreateGameObjects(string N, string Typ)
    {
        _name = N;
        _type = Typ;
        Object prefab = Resources.Load("Prefabs/" + N + "Player");
        OuterGameObject = (GameObject)GameObject.Instantiate(prefab);
        DamageController dm;
        if (N == "ModelT")
            dm = (ModelTDamageController)OuterGameObject.AddComponent<ModelTDamageController>();
        else if (N == "Car")
            dm = (CarDamageController)OuterGameObject.AddComponent<CarDamageController>();
        else if (N == "Anglia")
            dm = (AngliaDamageController)OuterGameObject.AddComponent<AngliaDamageController>();
        else if (N == "Porsche")
            dm = (PorscheDamageController)OuterGameObject.AddComponent<PorscheDamageController>();
        else if (N == "Hotrod")
            dm = (HotrodDamageController)OuterGameObject.AddComponent<HotrodDamageController>();
        else
            dm = null;
        if (dm != null)
        {
            dm.SetColor(N + "_" + Typ, true);
            dm.RepairAll();
        }
        if (N == "ModelT")
            OuterGameObject.AddComponent<CarMenuModelTController>();
        else if (N == "Car")
            OuterGameObject.AddComponent<CarMenuCarController>();
        else if (N == "Anglia")
            OuterGameObject.AddComponent<CarMenuAngliaController>();
        else if (N == "Porsche")
            OuterGameObject.AddComponent<CarMenuPorscheController>();
        else
            OuterGameObject.AddComponent<CarMenuHotrodController>();
    }

    public void SetType(string color)
    {
        Main.Instance.Color = color;
        Material Mat = (Material)Resources.Load("Prefabs/Materials/" + _name + "_" + color + "_Gloss", typeof(Material));
        _type = color;
        Transform trCar = OuterGameObject.transform.Find("car");
        foreach (Transform child in trCar)
        {
            //Change the colour
            if (child.gameObject.GetComponent<Renderer>() != null)
            {

                Material[] CarMats = child.gameObject.GetComponent<Renderer>().sharedMaterials;
                for (int idx = 0; idx < CarMats.Length; idx++)
                {
                    if (CarMats[idx].name.StartsWith("Bodywork") || CarMats[idx].name.StartsWith(_name))
                    {
                        CarMats[idx] = Mat;
                        child.gameObject.GetComponent<Renderer>().sharedMaterials = CarMats;
                        idx = CarMats.Length;
                    }
                }
            }
        }
    }

    public string Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public void Initialize()
    {

    }

    public void BumpInto(CarMenuItem bumpee, bool fwd)
    {
        OuterGameObject.GetComponent<CarMenuCarInput>().BumpInto(bumpee.OuterGameObject.GetComponent<CarMenuCarInput>(), fwd);
    }

    public void Select()
    {
        Selected = true;
        Main.Instance.Vehicle = this._name;
        Main.Instance.Color = this._type;
    }

    public void Deselect()
    {

    }

    public bool Selected { get; set; }
}

public class CarMenuCarInput : MonoBehaviour, iInputManager
{
    public float Accel { get; set; }
    public float Steer { get; set; }
    public bool BrakeOn { get; set; }
    public bool BumpSequenceRunning = false;
    public bool ExitSequenceRunning = false;
    public bool CentreStageSequenceRunning = false;
    ApproachType _approach;
    Transform trCam;
    public void BumpInto(CarMenuCarInput bumpee, bool fwd)
    {
        if (ExitSequenceRunning) ExitSequenceRunning = false;
        trCam = GameObject.Find("CameraSpinner").transform;
        float Angle = Vector3.Angle(transform.position - trCam.position, Vector3.forward);
        if (Angle < 35) { _approach = ApproachType.Direct; }
        else
        {
            float rnd = Random.value;
            transform.localPosition = new Vector3(fwd ? 10 : -10, 0, bumpee.transform.localPosition.z * 2);
        }

        StartCoroutine(BumpSequence(bumpee, fwd));
    }

    IEnumerator BumpSequence(CarMenuCarInput bumpee, bool fwd)
    {
        BumpSequenceRunning = true;
        BrakeOn = false;
        bumpee.BrakeOn = false;
        do
        {
            if (!BumpSequenceRunning) break;
            if ((transform.position.x < 1.5 && transform.position.x > -1.5))// && Mathf.Abs(transform.position.z) < 1)
            { BumpSequenceRunning = false; break; }
            SteerTowards(bumpee.transform.position, 3);
            yield return null;
        } while (true);
        Accel = 0;
        //BrakeOn = true;
        bumpee.Exit(fwd);
        CentreStage();
        BumpSequenceRunning = false;
        yield break;
    }

    public void Exit(bool fwd)
    {
        StartCoroutine(ExitSequence(fwd));
    }

    public void CentreStage()
    {
        StartCoroutine(CentreStageSequence());
    }

    IEnumerator ExitSequence(bool fwd)
    {
        ExitSequenceRunning = true;
        do
        {
            if (ExitSequenceRunning == false)
            { ExitSequenceRunning = false; break; }
            if (fwd && transform.position.x < -15)
            { ExitSequenceRunning = false; break; }
            if (!fwd && transform.position.x > 15)
            { ExitSequenceRunning = false; break; }
            SteerTowards(new Vector3(fwd ? -20 : 20, 500, 0), 2.5f);
            yield return null;
        } while (true);
        Accel = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        ExitSequenceRunning = false;
        yield break;
    }

    IEnumerator CentreStageSequence()
    {
        CentreStageSequenceRunning = true;
        do
        {
            if (CentreStageSequenceRunning == false)
            { CentreStageSequenceRunning = false; break; }
            if (Vector3.Distance(transform.position, new Vector3(0, 500, 0)) < 5)
            { CentreStageSequenceRunning = false; break; }
            SteerTowards(new Vector3(0, 500, 0), 1.5f);
            yield return null;
        } while (true);
        Accel = 0;
        BrakeOn = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        CentreStageSequenceRunning = false;
        yield break;
    }

    public void SteerTowards(Vector3 Dest, float accel)
    {
        Vector3 AimDirection = Dest - transform.position;
        float DesiredHeadingAngle = Vector3.Angle(AimDirection, transform.forward);
        Vector3 crossDes = Vector3.Cross(AimDirection, transform.forward);
        if (crossDes.y > 0) DesiredHeadingAngle = -DesiredHeadingAngle;
        if (DesiredHeadingAngle > 90)  //we pointing away from the target. So reverse
        {
            DesiredHeadingAngle = 180 - DesiredHeadingAngle;
            accel = -accel;
        }
        else if (DesiredHeadingAngle < -90)
        {
            DesiredHeadingAngle += 180;
            accel = -accel;
        }
        Steer = DesiredHeadingAngle / 25;
        Accel = accel;
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        return Steer;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        return Accel;
    }

    public bool Brake()
    {
        return BrakeOn;
    }

    public float BrakeForce { get { if (BrakeOn) return 1; else return 0; } }

    public void Dispose() { }

    enum ApproachType { Direct, Drift, Spin }
}

