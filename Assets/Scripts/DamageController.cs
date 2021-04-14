using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void CollisionExitHandler();

public abstract class DamageController : MonoBehaviour
{
    protected GameObject car;
    protected Rigidbody _rb;
    protected bool StationaryCollision;
    public byte VId;
    protected Dictionary<int, string> colldrMap;
    protected Dictionary<string, CarPart> partsMap;
    protected PhysicMaterial StickyCarBodyPhysicsMaterial;
    protected PhysicMaterial CarBodyPhysicsMaterial;
    protected bool _replayer = false;
    protected Material _dirtyBodywork;
    protected Material ClearGlass;
    protected Material ReflectiveGlass;
    public GPS Gps { get; set; }
    protected AudioSource Crash1AudioSource;
    protected AudioSource Crash2AudioSource;
    protected AudioSource Crash3AudioSource;
    protected bool Damg;

    public event CollisionExitHandler OnCollisionExitEvent;

    public void SetColor(string VehAndCol, bool Gloss = false)
    {
        Material Mat;
        if (Gloss)
            Mat = (Material)Resources.Load("Prefabs/Materials/" + VehAndCol + "_Gloss", typeof(Material));
        else
            Mat = (Material)Resources.Load("Prefabs/Materials/" + VehAndCol, typeof(Material));
        _dirtyBodywork = (Material)Resources.Load("Prefabs/Materials/" + VehAndCol + "_Scratch", typeof(Material));
        //Change the colour
        foreach (Transform child in car.transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {

                Material[] CarMats = child.gameObject.GetComponent<Renderer>().sharedMaterials;
                for (int idx = 0; idx < CarMats.Length; idx++)
                {
                    if (CarMats[idx].name.StartsWith("Bodywork") || CarMats[idx].name.StartsWith("Car_") || CarMats[idx].name.EndsWith("_Red"))
                    {
                        CarMats[idx] = Mat;
                    }
                }
                child.gameObject.GetComponent<Renderer>().sharedMaterials = CarMats;
            }   //end of Change the colour
        }
    }

    void Awake()
    {
        car = this.transform.Find("car").gameObject;
        _rb = GetComponent<Rigidbody>();
        StickyCarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/StickyCarBodyPhysicsMaterial");
        CarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/CarBodyPhysicsMaterial");
        partsMap = new Dictionary<string, CarPart>();
        //Add all parts to the parts map
        foreach (Transform child in car.transform)
        {
            if(!child.gameObject.name.Contains(".")) {
            CarPart p = new CarPart();
            p.Transform = child;
            p.MR = child.GetComponent<MeshRenderer>();
            p.MF = child.GetComponent<MeshFilter>();
            p.StartPos = child.localPosition;
            p.StartRot = child.localRotation;
            partsMap.Add(child.gameObject.name, p);
            }
        }
        foreach (Transform child in car.transform)
        {
            string gameObjectName = child.gameObject.name;
            MeshRenderer meshrend = child.GetComponent<MeshRenderer>();
            Mesh msh = child.GetComponent<MeshFilter>().sharedMesh;
            if (gameObjectName.Contains("."))
            {
                //partsMap[gameObjectName.Substring(0, gameObjectName.IndexOf("."))].MeshRenderers.Add(gameObjectName.Substring(gameObjectName.IndexOf(".") + 1), meshrend);
                partsMap[gameObjectName.Substring(0, gameObjectName.IndexOf("."))].Meshes.Add(gameObjectName.Substring(gameObjectName.IndexOf(".") + 1), msh);
                //child.GetComponent<MeshRenderer>().enabled = false;
                Destroy(child.gameObject);
            }
            else
            {
                //partsMap[gameObjectName].MeshRend = meshrend;
                partsMap[gameObjectName].Meshes.Add("Intact", msh);
            }

        } //end of carparts loop

        CreateColliderMap();

    } //end of Awake

    public virtual void CreateColliderMap() { }


    void OnCollisionExit(Collision coll)
    {
        foreach (ContactPoint cont in coll.contacts)
        {
            try
            {
                if (cont.thisCollider.sharedMaterial.name == "StickyCarBodyPhysicsMaterial")
                {
                    cont.thisCollider.sharedMaterial = CarBodyPhysicsMaterial;
                    //SwapMaterial back to the slippery one
                }
            }
            catch { }
        }

        if (coll.transform.name.EndsWith("le0") && coll.collider.name.StartsWith("Colldr") && StationaryCollision==false)
        {
            //This tells the racer to start timing 3 seconds and then award a hog bonus
            if (OnCollisionExitEvent != null)
                OnCollisionExitEvent();
        }
        StationaryCollision = false;
    }

    void PlaceMarker(Vector3 pos, Vector3 LookDirection)
    {
        GameObject rtn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rtn.GetComponent<Collider>().enabled = false;
        rtn.transform.localScale = new Vector3(0.05f, 0.05f, LookDirection.magnitude / 1000);
        rtn.transform.LookAt(LookDirection + pos);
        rtn.transform.position = pos + LookDirection * LookDirection.magnitude / 500;
    }

    public virtual void AttachHingesAndJoints()
    {
        foreach (KeyValuePair<string, CarPart> Part in partsMap)
        {
            if (!Part.Key.Contains(".") && !Part.Key.Contains("Steer"))
            {
                Part.Value.ShowMesh("Intact");
                Part.Value.Transform.localPosition = Part.Value.StartPos;
                Part.Value.Transform.localRotation = Part.Value.StartRot;
                try
                {
                    Part.Value.Transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    Part.Value.Transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                catch { }
            }
        }
    }

    //This one used by the Live player
    protected void AddHinge(CarPart part, string Hinge)
    {
        if (part.Transform.parent != null && !part.HasHinge)
        {
            part.AddHinge(Hinge);
            Recrdng.Current.RecordDamage(VId, RecEventType.AddHinge, part.Transform.name, Hinge);
            Damg = true;
        }
    }

    public void RepairAll()
    {
        RemoveHingesAndJoints();
        AttachHingesAndJoints();
    }

    void RemoveHingesAndJoints()
    {
        foreach (HingeJoint h in car.GetComponentsInChildren<HingeJoint>())
        {
            Destroy(h);
        }
        foreach (FixedJoint f in car.GetComponentsInChildren<FixedJoint>())
        {
            Destroy(f);
        }
    }
    public void Scratch(CarPart part)
    {
        if (part.ScratchMaterial(_dirtyBodywork))
        Recrdng.Current.RecordDamage(VId, RecEventType.Scratch, part.Transform.name, null);
    }

    public void ScratchPart(string partName) //this one used by the replayer
    {
        bool rtn = partsMap[partName].ScratchMaterial(_dirtyBodywork);
    }

    public void Damage(CarPart part, string Mesh)
    {
        if (part.Transform.parent != null)
        {
            part.ShowMesh(Mesh);
            part.Transform.GetComponent<MeshRenderer>().sharedMaterial = _dirtyBodywork;
            Recrdng.Current.RecordDamage(VId, RecEventType.Crunch, part.Transform.name, Mesh);
        }
        Crash2AudioSource.Play();
        Damg = true;
    }

    //used by player
    /*
     //It worked but not very realistic. Cars are not made of plasticene
    public void MeshDeform(CarPart part, Vector3 point, Vector3 force)
    {
        Debug.Log("MeshDeform");
        Vector3 lclpt = part.Transform.InverseTransformPoint(point);
        if (part.Transform.parent != null)
        {
            Vector3[] verts = part.Transform.GetComponent<MeshFilter>().sharedMesh.vertices;
            Vector2[] uv = part.Meshes["Intact"].uv;
            int[] tris = part.Meshes["Intact"].triangles;
            int vlen = verts.Length;
            Vector3[] newVerts = new Vector3[verts.Length];
            for (int i = 0; i < verts.Length; i++)
            {
                newVerts[i] = verts[i];
            }
            //int rand = UnityEngine.Random.Range(0, verts.Length);
            for(int v = 0; v<vlen; v++)
            {
                //Debug.Log((lclpt - newVerts[v]).sqrMagnitude);
                float distsq = (lclpt - newVerts[v]).sqrMagnitude;
                if(distsq<0.16f)
                newVerts[v] += force / (UnityEngine.Random.Range(40000, 50000) * (1f+distsq*6));
                    //Debug.Log("Bash");
            }
            //newVerts[vertid] += force/20000;
            Mesh newMesh = new Mesh();
            newMesh.vertices = newVerts;
            newMesh.triangles = tris;
            newMesh.RecalculateNormals();
            newMesh.uv = uv;
            part.ShowMesh(newMesh);
        }
        part.Transform.GetComponent<MeshRenderer>().sharedMaterial = _dirtyBodywork;
        Recrdng.Current.RecordDamage(VId, RecEventType.MeshDef, part.Transform.name, lclpt);
        Crash2AudioSource.Play();
        Damg = true;
    }   */

    //used by replayer
    //This ones used by the replayer
    public void CrunchPart(string partname, string Crunchname)
    {
        if (partsMap[partname].Transform.parent != null)
        {
            partsMap[partname].ShowMesh(Crunchname);
            partsMap[partname].ScratchMaterial(_dirtyBodywork);
        }

        }
    //This one used by the Replayer
    public void AddHingeToPart(string partname, string hinge)
    {
        partsMap[partname].AddHinge(hinge);
    }
    //This one used by the Live Player
    public void BreakOffPart(CarPart part)
    {
        Transform trPart = part.Transform;
        if (part.HasHinge) { part.RemoveHinge(); }
        if (trPart.parent != null)
        {
            trPart.SetParent(null);
            if(trPart.gameObject.GetComponent<Rigidbody>() == null) trPart.gameObject.AddComponent<Rigidbody>();
            if (trPart.gameObject.GetComponent<BoxCollider>() == null)
                trPart.gameObject.AddComponent<BoxCollider>().sharedMaterial = StickyCarBodyPhysicsMaterial;
            else
                trPart.gameObject.GetComponent<BoxCollider>().sharedMaterial = StickyCarBodyPhysicsMaterial;
            Recrdng.Current.RecordDamage(VId, RecEventType.JointBreak, part.Transform.name, null);
            Damg = true;
            Crash1AudioSource.Play();
        }
    }
    //This one used by the Replayer
    public void BreakOffPart(string PartName)
    {
        CarPart part = partsMap[PartName];
        Transform trPart = part.Transform;
        if (part.HasHinge) { part.RemoveHinge(); }
        if (trPart.parent != null)
        {
            trPart.SetParent(null);
            if (trPart.gameObject.GetComponent<Rigidbody>() == null) trPart.gameObject.AddComponent<Rigidbody>();
            trPart.gameObject.AddComponent<BoxCollider>().sharedMaterial = StickyCarBodyPhysicsMaterial;
        }
    }

    public bool IsStillAttached(CarPart part)
    {
        return part.Transform.parent != null;
    }

    public void FreezeDetatchedParts()
    {
        foreach(KeyValuePair<string, CarPart> de in partsMap)
        {
            CarPart cp = de.Value;
            if (!IsStillAttached(cp))
            {
                Rigidbody rb = cp.Transform.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                cp.StartPos = rb.velocity;
            }
        }
        foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Pause();
        }
        foreach (WheelController wc in GetComponentsInChildren<WheelController>()) wc.enabled = false;
    }

    internal void UnfreezeDetatchedParts()
    {
        foreach (KeyValuePair<string, CarPart> de in partsMap)
        {
            CarPart cp = de.Value;
            if (!IsStillAttached(cp))
            {
                Rigidbody rb = cp.Transform.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.velocity = cp.StartPos;
            }
        }
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
        foreach (WheelController wc in GetComponentsInChildren<WheelController>()) wc.enabled = true;
    }
} //end of DamageController class

public class CarPart
{
    //public GameObject GameObject { get; set; } // Todo Remove this
    public Transform Transform { get; set; }
    public Vector3 StartPos { get; set; }
    public Quaternion StartRot { get; set; }
    public Dictionary<string, Mesh> Meshes;
    public Dictionary<string, HingeParams> Hinges;
    public bool HasHinge = false;
    public MeshRenderer MR;
    public MeshFilter MF;

    public CarPart()
    {
        Meshes = new Dictionary<string, Mesh>();
        Hinges = new Dictionary<string, HingeParams>();
    }

    public bool ShowMesh(string meshName)
    {
        Mesh m = Meshes[meshName];
        if (MF.sharedMesh!= m)
        {
            MF.sharedMesh = m;
            return true;
        }
        else return false;
    }

    public bool ShowMesh2(string meshName)
    {
        Mesh m;
        bool rtn = false;
        if (Meshes.TryGetValue(meshName, out m))
        {
            if (Transform.GetComponent<MeshFilter>().sharedMesh != m)
            {
                rtn = true;
                Transform.GetComponent<MeshFilter>().sharedMesh = m;
            }
        }
        return rtn;
    }

    public void ShowMesh(Mesh m)
    {
        Transform.GetComponent<MeshFilter>().mesh = m;
    }

    public bool ScratchMaterial(Material m)
    {
        if (MR.sharedMaterials[0] != m)
        {
            Material[] Ms = MR.sharedMaterials;
            Ms[0] = m;
            MR.sharedMaterials = Ms;

            return true;
        }
        else return false;
    }

    //This one used during play and replay
    public void AddHinge(string HingeName)
    {
        if (Transform.parent == null) return;
        HingeParams hp = Hinges[HingeName];
        HingeJoint hj = Transform.gameObject.AddComponent<HingeJoint>();
        hj.connectedBody = Transform.parent.parent.GetComponent<Rigidbody>();
        hj.anchor = hp.Anchor;
        hj.axis = hp.Axis;
        hj.useLimits = true;
        hj.limits = hp.Limits;
        HasHinge = true;
        if(Transform.gameObject.GetComponent<BoxCollider>()==null)
        Transform.gameObject.AddComponent<BoxCollider>();//.sharedMaterial = StickyCarBodyPhysicsMaterial;
    }

    public void RemoveHinge()
    {
        GameObject.Destroy(Transform.GetComponent<HingeJoint>());
        HasHinge = false;
    }

}

public class HingeParams
{
    public Vector3 Anchor { get; set; }
    public Vector3 Axis { get; set; }
    public JointLimits Limits { get; set; }
}


