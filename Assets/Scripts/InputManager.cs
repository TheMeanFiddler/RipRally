using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public interface iInputManager
{
    float WMovement();
    float XMovement();
    float YMovement();
    float ZMovement();
    bool Brake();
    float BrakeForce { get; }
    void Dispose();
}

public class KeyboardSteerInput : iInputManager
{
    private GameObject AccelPad;
    private GameObject BrakePad;
    private GameObject canvas;
    private Transform _accelPedal;
    private Transform _brakePedal;
    private float steer;

    public KeyboardSteerInput()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _accelPedal = AccelPad.transform.Find("Pedal");
            GameObject.Destroy(AccelPad.GetComponentInChildren<InputSlidePedal>());
            BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakeSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
            _brakePedal = BrakePad.transform.Find("Pedal");
            GameObject.Destroy(BrakePad.GetComponentInChildren<InputSlidePedal>());

            BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
            AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        //BrakePad.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        //AccelPad.transform.localScale = new Vector3(0.3f, 0.3f, 0);

    }


    private float BrakeTime;

    public float WMovement()
    {
        return 0;
    }

    public float XMovement()
    {
        steer = 0;
        if (Input.GetKey(KeyCode.RightArrow)) steer = 0.53f;
        if (Input.GetKey(KeyCode.LeftArrow)) steer = -0.53f;
        if (Input.GetKey(KeyCode.Keypad1)) {
            if (steer == 0.53f) steer = 1.06f; else steer = 1.6f;
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (steer == -0.53f) steer = -1.06f; else steer = -1.6f;
        }
        return steer*40;
    }
    public float YMovement()
    {
        if (Input.GetKey(KeyCode.Space)) return 1;
        else if (Input.GetKey(KeyCode.LeftShift)) return -1;
        else return 0;
    }
    public float ZMovement()
    {
        return Input.GetAxis("Vertical");
    }

    public bool Brake()
    {

        return (Input.GetAxis("Brake") == 1);
    }
    float _brakeForce;
    public float BrakeForce
    {
        get
        {
            float rtn = 0;
            if (Input.GetAxis("Brake") == 1) _brakeForce = Mathf.Lerp(_brakeForce, 1, Time.deltaTime);
            if (Input.GetAxis("Brake") == 0) _brakeForce = Mathf.Lerp(_brakeForce, 0, Time.deltaTime);
            rtn = _brakeForce;
            _brakePedal.localScale = new Vector3(1, 1 - rtn, 1);
            return rtn;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        try { UnityEngine.Object.Destroy(AccelPad); }
        catch (Exception e) { Debug.Log(e.Message); }
        try { GameObject.Destroy(BrakePad); }
        catch { }
        System.GC.Collect();
    }
}

public class KeyboardAndMouseSteerInput : iInputManager
{
    public float Lasty = 200;
    public float xOffset = 500;
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        return Input.mousePosition.x - xOffset;
    }
    public float YMovement()
    {
        if (Input.GetKey(KeyCode.Space)) return 1;
        else if (Input.GetKey(KeyCode.LeftShift)) return -1;
        else return 0;
    }
    public float ZMovement()
    {
        return Input.GetAxis("Vertical");
    }

    public bool Brake()
    {
        return (Input.GetAxis("Brake") == 1);
    }
    public float BrakeForce { get { return 1; } }

    public void Dispose() { }
}

public class AndroidTiltInput : iInputManager
{
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        return Input.acceleration.x;
    }
    public float YMovement()
    {
        return Input.acceleration.y;
    }
    public float ZMovement()
    {
        return Input.acceleration.z;
    }
    public float Left()
    {
        return -Input.acceleration.x;
    }

    public bool Brake()
    {
        return (false);
    }
    public float BrakeForce { get { return 1; } }

    public void Dispose() { }
}

public class AndroidPressPedalsAndTiltSteer : iInputManager, IDisposable
{
    private GameObject canvas;
    private Transform _accelPedal;
    private Transform _brakePedal;
    private GameObject AccelPad;
    private GameObject BrakePad;
    InputPressPedal AccelPadController;
    InputPressPedal BrakePadController;
    //constructor
    public AndroidPressPedalsAndTiltSteer()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelPress"), Vector3.zero, Quaternion.identity, canvas.transform);
        _accelPedal = AccelPad.transform.Find("Pedal");
        AccelPadController = AccelPad.GetComponentInChildren<InputPressPedal>();
        BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakePress"), Vector3.zero, Quaternion.identity, canvas.transform);
        _brakePedal = BrakePad.transform.Find("Pedal");
        BrakePadController = BrakePad.GetComponentInChildren<InputPressPedal>();

        BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        BoxCollider2D BrakeColl = BrakePadController.GetComponent<BoxCollider2D>();
        BoxCollider2D AccelColl = AccelPadController.GetComponent<BoxCollider2D>();
        float MidPt = (Settings.Instance.BrakePos + Settings.Instance.AccelPos) / 2;
        if (Settings.Instance.BrakePos < Settings.Instance.AccelPos)
        {
            BrakeColl.size = new Vector2(MidPt + 400, BrakeColl.size.y);
            BrakeColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.BrakePos, 200);
            AccelColl.size = new Vector2((400 - MidPt), AccelColl.size.y);
            AccelColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.AccelPos, 200);
        }
        else
        {
            AccelColl.size = new Vector2(MidPt + 400, AccelColl.size.y);
            AccelColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.AccelPos, 200);
            BrakeColl.size = new Vector2((400 - MidPt), BrakeColl.size.y);
            BrakeColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.BrakePos, 200);
        }

    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        float _tilt = Mathf.Atan(Input.acceleration.x / Input.acceleration.y) * Mathf.Rad2Deg;
        if (Input.acceleration.y > 0) { if (Input.acceleration.x < 0) _tilt = 180 + _tilt; else _tilt = _tilt - 180; }
        return -_tilt;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        float Acc = BrakePadController.Value==1? -1: AccelPadController.Value;
        _accelPedal.localScale = new Vector3(1, 1 - Mathf.Abs(Acc), 1);
        return Acc;
    }

    public bool Brake()
    {
        return (BrakePadController.Value != 0 && BrakePadController.Value != 1);
    }

    public float BrakeForce {
        get
        {
            float rtn = BrakePadController.Value == 1? 0: BrakePadController.Value;
            _brakePedal.localScale = new Vector3(1, 1 - BrakePadController.Value, 1);
            return rtn;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        GameObject.Destroy(AccelPad);
        GameObject.Destroy(BrakePad);
        System.GC.Collect();
    }
}

public class AndroidSlidePedalsAndTiltSteer : iInputManager, IDisposable
{
    private GameObject canvas;
    private Transform _accelPedal;
    private Transform _brakePedal;

    //for separate pedals
    private GameObject AccelPad;
    private GameObject BrakePad;
    InputSlidePedal AccelPadController;
    InputSlidePedal BrakePadController;
    private bool SameSidePedals = true;

    //constructor
    public AndroidSlidePedalsAndTiltSteer()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }

        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelSlide"),Vector3.zero,Quaternion.identity, canvas.transform);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, -40);
        _accelPedal = AccelPad.transform.Find("Pedal");
        AccelPadController = AccelPad.GetComponentInChildren<InputSlidePedal>();
        BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakeSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _brakePedal = BrakePad.transform.Find("Pedal");
        BrakePadController = BrakePad.GetComponentInChildren<InputSlidePedal>();
        BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        BoxCollider2D BrakeColl = BrakePadController.GetComponent<BoxCollider2D>();
        BoxCollider2D AccelColl = AccelPadController.GetComponent<BoxCollider2D>();
        float MidPt = (Settings.Instance.BrakePos + Settings.Instance.AccelPos) / 2;
        if (Settings.Instance.BrakePos< Settings.Instance.AccelPos)
        {
            BrakeColl.size = new Vector2(MidPt+400, BrakeColl.size.y);
            BrakeColl.offset = new Vector2((MidPt-400) / 2 - Settings.Instance.BrakePos, 200);
            AccelColl.size = new Vector2((400-MidPt), AccelColl.size.y);
            AccelColl.offset = new Vector2((400+MidPt) / 2 - Settings.Instance.AccelPos, 200);
        }
        else
        {
            AccelColl.size = new Vector2(MidPt + 400, AccelColl.size.y);
            AccelColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.AccelPos, 200);
            BrakeColl.size = new Vector2((400 - MidPt), BrakeColl.size.y);
            BrakeColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.BrakePos, 200);
        }
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        //return Input.acceleration.x*2;
        float _tilt = Mathf.Atan(Input.acceleration.x / Input.acceleration.y) * Mathf.Rad2Deg;
        if (Input.acceleration.y > 0) { if (Input.acceleration.x < 0) _tilt = 180 + _tilt; else _tilt = _tilt - 180; }
        return -_tilt;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        float Acc;
        Acc = AccelPadController.Value;

        _accelPedal.localScale = new Vector3(1, 1 - Mathf.Abs(Acc), 1);
        //_reverseImg.enabled = Acc<0;
        return Acc;
    }

    public bool Brake()
    {
        return (BrakePadController.Value != 0);
    }
    public float BrakeForce
    {
        get
        {
            float brk;
            brk = BrakePadController.Value;
            _brakePedal.localScale = new Vector3(1, 1 - brk, 1);
            return brk;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        try { GameObject.Destroy(AccelPad); }
        catch(Exception e) { Debug.Log(e.Message); }
        try { GameObject.Destroy(BrakePad); }
        catch { }
        System.GC.Collect();
    }
}

public class AndroidTiltPedalsAndSteeringWheel : iInputManager, IDisposable
{
    private GameObject canvas;
    private GameObject AccelPad;
    private GameObject BrakePad;
    private GameObject SteerPad;
    private Transform _accelPedal;
    private Transform _brakePedal;
    SteeringWheelPadController SteerPadController;
    private float _restAngle;
    private float _tilt;

    //constructor
    public AndroidTiltPedalsAndSteeringWheel()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _accelPedal = AccelPad.transform.Find("Pedal");
        GameObject.Destroy(AccelPad.GetComponentInChildren<InputSlidePedal>());
        BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakeSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _brakePedal = BrakePad.transform.Find("Pedal");
        GameObject.Destroy(BrakePad.GetComponentInChildren<InputSlidePedal>());
        SteerPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/SteeringWheelPad"), Vector3.zero, Quaternion.identity, canvas.transform);
        SteerPadController = SteerPad.GetComponent<SteeringWheelPadController>();
        BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        SteerPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.SteeringPosX, Settings.Instance.SteeringPosY);
        _restAngle = -Mathf.Atan2(Input.acceleration.y, Input.acceleration.z) * Mathf.Rad2Deg;
        //Main.Instance.PopupMsg(_restAngle.ToString());
        //125 degrees rest = 2.18rad
        //150 is full accel = 2.62rad = rest + 0.436
        //100 is full brake = 1.75rad = rest - 0.436
        //<100 is reverse
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        float rtn = SteerPadController.Value*50;
        return rtn;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        _tilt = (-Mathf.Atan2(Input.acceleration.y, Input.acceleration.z)-2.18f)* 2.29183f;
        float Acc = _tilt > 0f || _tilt < -1f ? _tilt: 0;
        _accelPedal.localScale = new Vector3(1, 1 - Mathf.Abs(Acc), 1);
        return Acc;
    }

    public bool Brake()
    {
        return _tilt < 0 && _tilt > -1;
    }

    public float BrakeForce
    {
        get
        {
            float rtn = (_tilt < 0 && _tilt > -1)? -_tilt: 0;
            _brakePedal.localScale = new Vector3(1, 1 - rtn, 1);
            return rtn;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        GameObject.Destroy(BrakePad);
        GameObject.Destroy(AccelPad);
        GameObject.Destroy(SteerPad);
        System.GC.Collect();
    }
}


public class AndroidSlidePedalsAndSteeringWheel : iInputManager, IDisposable
{
    private GameObject canvas;
    private GameObject AccelPad;
    private GameObject BrakePad;
    private GameObject SteerPad;
    private Transform _accelPedal;
    private Transform _brakePedal;
    InputSlidePedal AccelPadController;
    InputSlidePedal BrakePadController;
    SteeringWheelPadController SteerPadController;

    //constructor
    public AndroidSlidePedalsAndSteeringWheel()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _accelPedal = AccelPad.transform.Find("Pedal");
        AccelPadController = AccelPad.GetComponentInChildren<InputSlidePedal>();
        BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakeSlide"), Vector3.zero, Quaternion.identity, canvas.transform);
        _brakePedal = BrakePad.transform.Find("Pedal");
        BrakePadController = BrakePad.GetComponentInChildren<InputSlidePedal>();
        SteerPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/SteeringWheelPad"), Vector3.zero, Quaternion.identity, canvas.transform);
        SteerPadController = SteerPad.GetComponent<SteeringWheelPadController>();
        AccelPadController.SteeringColl = SteerPad.GetComponent<BoxCollider2D>();
        BrakePadController.SteeringColl = SteerPad.GetComponent<BoxCollider2D>();
        BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        SteerPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.SteeringPosX, Settings.Instance.SteeringPosY);
        BoxCollider2D BrakeColl = BrakePadController.GetComponent<BoxCollider2D>();
        BoxCollider2D AccelColl = AccelPadController.GetComponent<BoxCollider2D>();
        float MidPt = (Settings.Instance.BrakePos + Settings.Instance.AccelPos) / 2;
        if (Settings.Instance.BrakePos < Settings.Instance.AccelPos)
        {
            BrakeColl.size = new Vector2(MidPt + 400, BrakeColl.size.y);
            BrakeColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.BrakePos, 200);
            AccelColl.size = new Vector2((400 - MidPt), AccelColl.size.y);
            AccelColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.AccelPos, 200);
        }
        else
        {
            AccelColl.size = new Vector2(MidPt + 400, AccelColl.size.y);
            AccelColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.AccelPos, 200);
            BrakeColl.size = new Vector2((400 - MidPt), BrakeColl.size.y);
            BrakeColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.BrakePos, 200);
        }
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        float rtn = SteerPadController.Value*50;
        return rtn;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        float Acc = BrakePadController.Value == 1 ? -1 : AccelPadController.Value;
        _accelPedal.localScale = new Vector3(1, 1 - Mathf.Abs(Acc), 1);
        return Acc;
    }

    public bool Brake()
    {
        return (BrakePadController.Value != 0 && BrakePadController.Value != 1);
    }

    public float BrakeForce
    {
        get
        {
            float rtn = BrakePadController.Value == 1 ? 0 : BrakePadController.Value;
            _brakePedal.localScale = new Vector3(1, 1 - BrakePadController.Value, 1);
            return rtn;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        GameObject.Destroy(BrakePad);
        GameObject.Destroy(AccelPad);
        GameObject.Destroy(SteerPad);
        System.GC.Collect();
    }
}

public class AndroidPressPedalsAndSteeringWheel : iInputManager, IDisposable
{
    private GameObject canvas;
    private GameObject AccelPad;
    private GameObject BrakePad;
    private GameObject SteerPad;
    private Transform _accelPedal;
    private Transform _brakePedal;
    InputPressPedal AccelPadController;
    InputPressPedal BrakePadController;
    SteeringWheelPadController SteerPadController;

    //constructor
    public AndroidPressPedalsAndSteeringWheel()
    {
        canvas = GameObject.Find("ControlCanvas");
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        AccelPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/AccelPress"), Vector3.zero, Quaternion.identity, canvas.transform);
        _accelPedal = AccelPad.transform.Find("Pedal");
        AccelPadController = AccelPad.GetComponentInChildren<InputPressPedal>();
        BrakePad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/BrakePress"), Vector3.zero, Quaternion.identity, canvas.transform);
        _brakePedal = BrakePad.transform.Find("Pedal");
        BrakePadController = BrakePad.GetComponentInChildren<InputPressPedal>();
        SteerPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/SteeringWheelPad"), Vector3.zero, Quaternion.identity, canvas.transform);
        SteerPadController = SteerPad.GetComponent<SteeringWheelPadController>();
        AccelPadController.SteeringColl = SteerPad.GetComponent<BoxCollider2D>();
        BrakePadController.SteeringColl = SteerPad.GetComponent<BoxCollider2D>();
        BrakePad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 0);
        AccelPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 0);
        SteerPad.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.SteeringPosX, Settings.Instance.SteeringPosY);
        BoxCollider2D BrakeColl = BrakePadController.GetComponent<BoxCollider2D>();
        BoxCollider2D AccelColl = AccelPadController.GetComponent<BoxCollider2D>();
        float MidPt = (Settings.Instance.BrakePos + Settings.Instance.AccelPos) / 2;
        if (Settings.Instance.BrakePos < Settings.Instance.AccelPos)
        {
            BrakeColl.size = new Vector2(MidPt + 400, BrakeColl.size.y);
            BrakeColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.BrakePos, 200);
            AccelColl.size = new Vector2((400 - MidPt), AccelColl.size.y);
            AccelColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.AccelPos, 200);
        }
        else
        {
            AccelColl.size = new Vector2(MidPt + 400, AccelColl.size.y);
            AccelColl.offset = new Vector2((MidPt - 400) / 2 - Settings.Instance.AccelPos, 200);
            BrakeColl.size = new Vector2((400 - MidPt), BrakeColl.size.y);
            BrakeColl.offset = new Vector2((400 + MidPt) / 2 - Settings.Instance.BrakePos, 200);
        }
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        float rtn = SteerPadController.Value*50;
        return rtn;
    }

    public float YMovement()
    {
        return 0;
    }

    public float ZMovement()
    {
        float Acc = BrakePadController.Value == 1 ? -1 : AccelPadController.Value;
        _accelPedal.localScale = new Vector3(1, 1 - Mathf.Abs(Acc), 1);
        return Acc;
    }

    public bool Brake()
    {
        return (BrakePadController.Value != 0 && BrakePadController.Value != 1);
    }

    public float BrakeForce
    {
        get
        {
            float rtn = BrakePadController.Value == 1 ? 0 : BrakePadController.Value;
            _brakePedal.localScale = new Vector3(1, 1 - BrakePadController.Value, 1);
            return rtn;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        GameObject.Destroy(BrakePad);
        GameObject.Destroy(AccelPad);
        GameObject.Destroy(SteerPad);
        System.GC.Collect();
    }
}

public class AndroidDPadInput : iInputManager
{
    private GameObject canvas;
    private GameObject dPad;

    //constructor
    public AndroidDPadInput()
    {
        canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (canvas == null)
        {
            canvas = new GameObject();
            canvas.AddComponent<Canvas>();
        }
        dPad = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/DPad"), new Vector3(50f, 50f, 0), Quaternion.identity);
        dPad.transform.SetParent(canvas.transform);
        dPad.transform.SetAsFirstSibling();
        dPad.transform.localScale = Vector3.one * 1.5f;
        dPad.transform.localPosition = new Vector3(-280, -130, 0);
    }
    public float WMovement()
    {
        return dPad.GetComponent<DPadController>().Strafe;
    }
    public float XMovement()
    {
        return dPad.GetComponent<DPadController>().x / 2;
    }
    public float YMovement()
    {
        return dPad.GetComponent<DPadController>().y / 2;
    }
    public float ZMovement()
    {
        return dPad.GetComponent<DPadController>().z / 2;
    }
    public bool Brake()
    {
        return false;
    }
    public float BrakeForce { get { return 1; } }

    public void Dispose() { }
}

public class ReplayerInput : iInputManager
{
    public float xMovement;
    public float yMovement;
    public float zMovement;
    public float fRAngularVelocity;
    public bool brake;

    //Constructor
    public ReplayerInput()
    {
        GameObject.Destroy(GameObject.Find("pPad"));
    }
    public float WMovement()
    {
        return 0;
    }
    public float XMovement()
    {
        return xMovement;
    }

    public float YMovement()
    {
        return yMovement;
    }

    public float ZMovement()
    {
        return zMovement;
    }

    public float FRAngularVelocity()
    {
        return fRAngularVelocity;
    }

    public bool Brake()
    {
        return brake;
    }
    public float BrakeForce { get { return 0; } }

    public void Dispose() { }
}

/// <summary>
/// Used for BuilderController
/// </summary>
public class KeyboardInput : iInputManager
{
    public float WMovement()
    {
        if (Input.GetKey(KeyCode.Keypad1))
            return 1;
        else if (Input.GetKey(KeyCode.RightShift))
            return -1;
        else
            return 0;
    }
    public float XMovement()
    {
        float h = Input.GetAxis("Horizontal");
        return h;
    }
    public float YMovement()
    {
        if (Input.GetKey(KeyCode.Space)) return 1;
        else if (Input.GetKey(KeyCode.LeftShift)) return -1;
        else return 0;
    }
    public float ZMovement()
    {
        return Input.GetAxis("Vertical");
    }

    public bool Brake()
    {

        return (Input.GetAxis("Brake") == 1);
    }

    public float BrakeForce
    {
        get
        {
            return 0;
        }
    }

    public void Dispose()
    {

    }

}

public class InputFactory
{
    public static iInputManager ChooseInputManager()
    {
#if UNITY_EDITOR
        return new KeyboardSteerInput();
        //return new AndroidSlidePedalsAndSteeringWheel();

#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        if(Settings.Instance.AccelControl == "ThumbSlide")
        {
            if(Settings.Instance.SteerControl == "Tilt")
                return new AndroidSlidePedalsAndTiltSteer();
            else
                return new AndroidSlidePedalsAndSteeringWheel();
        }
        if(Settings.Instance.AccelControl == "ThumbPress")
        {
            if(Settings.Instance.SteerControl == "Tilt")
                return new AndroidPressPedalsAndTiltSteer();
            else
                return new AndroidPressPedalsAndSteeringWheel();
         }
        if(Settings.Instance.AccelControl == "Tilt")
         {
                return new AndroidTiltPedalsAndSteeringWheel();
         }
        return new AndroidSlidePedalsAndTiltSteer();
#else
        return new KeyboardSteerInput();
#endif



    }
}

public struct InputStruct
{
    public float Time;
    public float Accel;
    public float Brake;
    public float Steer;
    public byte Event;
}
