using UnityEngine;
using System.Collections.Generic;


public class SeeSaw: aSceneryComponent
{
    Transform _frontRayOrigin;
    Transform _backRayOrigin;
    float _bckTipAngle;
    float _fwdTipAngle;
    float _radius;
    Ray _ray;
    RaycastHit hit;
    float _angularVelocity = 0;
    List<GameObject> Markers = new List<GameObject>();

    void Start()
    {
        Init();
    }
    public override void Init(PlaceableObject p)
    {
        _placeableObj = p;
        Init();
    }
    private void Init()
    {
        _frontRayOrigin = transform.Find("FrontRayOrigin");
        _backRayOrigin = transform.Find("BackRayOrigin");

        foreach(GameObject m in Markers)
        {
            Destroy(m);
        }
        Markers.Clear();
        
        float Adj = Vector3.Distance(_frontRayOrigin.position, transform.position);
        float AngleIncr = Mathf.Atan(0.4f / Adj) * Mathf.Rad2Deg;
        bool foundGround = false;
        transform.localEulerAngles = new Vector3(270, 0, 0);
        transform.parent.parent.Find("ClickCollider").GetComponent<Collider>().enabled = false;
        do
        {
            //PlaceMarker(_frontRayOrigin.position);
            if (Physics.Raycast(_frontRayOrigin.position, -transform.up, out hit, 0.8f))
            {
                float opp = Vector3.Distance(_frontRayOrigin.position, hit.point);
                transform.Rotate(Vector3.right, Mathf.Atan(opp / Adj) * Mathf.Rad2Deg, Space.Self);
                _fwdTipAngle = transform.localEulerAngles.x;
                foundGround = true;
            }
            else
            {
                transform.Rotate(Vector3.right, AngleIncr, Space.Self);
            }
            PlaceMarker(_frontRayOrigin.position);
        } while (foundGround == false && (transform.localEulerAngles.x > 270 || transform.localEulerAngles.x < 85));

        transform.localEulerAngles = new Vector3(90, 0, 0);
        foundGround = false;
        do
        {
            if (Physics.Raycast(_backRayOrigin.position, -transform.up, out hit, 0.8f))
            {
                float opp = Vector3.Distance(_backRayOrigin.position, hit.point);
                PlaceMarker(hit.point);
                transform.Rotate(Vector3.right, -Mathf.Atan(opp / Adj) * Mathf.Rad2Deg, Space.Self);
                _bckTipAngle = transform.localEulerAngles.x;
                foundGround = true;
            }
            else
            {
                transform.Rotate(Vector3.right, -AngleIncr, Space.Self);
            }
            PlaceMarker(_backRayOrigin.position);
        } while (foundGround == false && (transform.localEulerAngles.x > 275 || transform.localEulerAngles.x < 90));

        _angularVelocity = -0.1f;
        transform.localEulerAngles = new Vector3(0, 0, 0);

        Debug.Log("BacktipAngle=" + _bckTipAngle);
        Debug.Log("FwdtipAngle=" + _fwdTipAngle);
        if (PlayerManager.Type == "BuilderPlayer") transform.parent.parent.Find("ClickCollider").GetComponent<Collider>().enabled = true;
    }

    void OnCollisionStay(Collision coll)
    {
        //If the platform hits a car it bounces away from it
        float _accel = 0;
        foreach (ContactPoint cp in coll.contacts)
        {
            if (cp.otherCollider.name.StartsWith("Colldr") || cp.otherCollider.name.Contains("Car"))
            {
                Vector3 _localPoint = transform.InverseTransformPoint(cp.point);
                Vector3 _nrm = transform.InverseTransformPoint(cp.normal);
                if (Mathf.Sign(_localPoint.z) == Mathf.Sign(_localPoint.y))
                {
                    if (transform.localEulerAngles.x <= _fwdTipAngle || transform.localEulerAngles.x > 180)
                        _accel = 0.5f;
                }
                else
                {
                    if (transform.localEulerAngles.x >= _bckTipAngle || transform.localEulerAngles.x < 180)
                    {
                        _accel = -0.5f;
                    }
                }
            }
            _angularVelocity += _accel;
        }
    }


    void FixedUpdate()
    {

        //Do a raycast off the ends of the platform to see if it hits anything
        if (_angularVelocity < 0)
        {
            if ((transform.localEulerAngles.x < _bckTipAngle && _bckTipAngle < 90) || (_bckTipAngle > 180 && transform.localEulerAngles.x > 180 && transform.localEulerAngles.x < _bckTipAngle))
            {
                transform.localEulerAngles = new Vector3(_bckTipAngle, 0, 0);
                _angularVelocity = 0; //if back edge hits the ground it stays there
            }
        }
        if (_angularVelocity > 0)
        {
            if (transform.localEulerAngles.x > _fwdTipAngle && transform.localEulerAngles.x < 180)
            {
                transform.localEulerAngles = new Vector3(_fwdTipAngle, 0, 0);
                _angularVelocity = 0; //if back edge hits the ground it stays there
            }
        }

        if (_angularVelocity != 0)
        {
            if (_angularVelocity > 0)
                transform.Rotate(Vector3.right, _angularVelocity, Space.Self);
            else
                transform.Rotate(Vector3.left, -_angularVelocity, Space.Self);
            //transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //the trigger is in tthe front edge. If the car enters it it flips the seesaw forward
        if (col.name.StartsWith("Coll"))
        {
            if (transform.InverseTransformPoint(col.transform.position).z > 0)
            { _angularVelocity = 0.5f; }
            else
            { _angularVelocity = -0.5f; }
        }
    }

    /*
    void PlaceMarker(string othr, Vector3 pos, Vector3 LookDirection)
    {
        GameObject rtn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rtn.name = othr + ": " + pos.ToString();
        rtn.GetComponent<Collider>().enabled = false;
        rtn.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        rtn.transform.LookAt(LookDirection);
        rtn.transform.position = pos + LookDirection * 0.5f;
    }*/

    private void PlaceMarker(Vector3 Pos)
    {
        if (PlayerManager.Type != "BuilderPlayer") return;
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Markers.Add(marker);
        Destroy(marker.GetComponent<Collider>());
        marker.tag = "Rut";
        marker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        marker.transform.position = Pos;
        //marker.transform.parent = this.transform;
    }
}

