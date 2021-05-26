using UnityEngine;


public class RecoverPanel : MonoBehaviour
{
    GameObject _goVeh;
    GPS _gps;
    int _lastSegIdx;
    int layerMask = ~((1 << 8) + (1 << 10) + (1 << 13) + (1 << 2));
    //8 = car, 2 = Ignore Raycast
    public void Init(GPS gps, GameObject goVeh, int lastSegIdx)
    {
        _goVeh = goVeh;
        _gps = gps;
        _lastSegIdx = lastSegIdx;
    }

    void Update()
    {
        if (_goVeh == null) { RecoverNo_Click(); return; }
        RaycastHit hit;
        int SegIdx = _lastSegIdx;
        //raycast to see which segment is underneath us
        if (Physics.Raycast(_goVeh.transform.position + Vector3.up * 3 + _goVeh.transform.forward, Vector3.down, out hit, 8, layerMask))
        {
            if (hit.collider.name.Contains("Seg"))
            {
                SegIdx = System.Convert.ToInt16(hit.collider.name.Substring(7));
            }
        }
        if (SegIdx != _lastSegIdx)
        {
            _gps.RecoveryAllowed = true; Destroy(gameObject);
        }
    }

    public void RecoverYes_Click()
    {
        MusicPlayer.Instance.StepDown();
        GetComponent<Adverts>().PlayVideoRecover();
    }

    public void RecoverNo_Click()
    {
        Destroy(gameObject);
        DrivingPlayManager.Current.PlayerCarManager.DontRecover();
    }

    void Destroy()
    {
        _goVeh = null;
        _gps = null;
    }

}

