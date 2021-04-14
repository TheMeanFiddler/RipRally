using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour
{
    GameObject GUICanvas;
    float Rot;
    private iInputManager InputManager;
    bool _lerping;
    Vector3 _lerpStartPos;
    Vector3 _lerpEndPos;
    float _lerpStartTime;

    // Use this for initialization
    void Start()
    {
        if (Road.Instance.BuilderPos != Vector3.zero)
        {
            transform.position = Road.Instance.BuilderPos;
            transform.rotation = Road.Instance.BuilderRot;
        }
        else
        { transform.rotation = Quaternion.Euler(0, 300, 0); }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        _lerping = false;
    }

    public void Init()
    {
        Debug.Log("BldrCtrlr.Init");
        GUICanvas = (GameObject)Instantiate(Resources.Load("Prefabs/BuilderGUICanvas"), new Vector3(354f, 183f, 0), Quaternion.identity);



#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        InputManager = new AndroidDPadInput();
#endif
#if UNITY_EDITOR
        InputManager = new KeyboardInput();

#endif
#if UNITY_STANDALONE_WIN
            InputManager = InputFactory.ChooseInputManager();
#endif

    }

    // Update is called once per frame
    //FixedUpdate is called for every physics calculation
    void FixedUpdate()
    {


        if (_lerping == false)
        {
            float ym = InputManager.YMovement();
            if (transform.position.y > 300 && ym > 0) ym = 0;
            if (transform.position.y < 0 && ym < 0) ym = 0;
            transform.Rotate(Vector3.up, InputManager.XMovement());
            transform.Translate(new Vector3(InputManager.WMovement(), ym , InputManager.ZMovement()), Space.Self);
        }
        else
        {
            transform.position = Vector3.Slerp(_lerpStartPos, _lerpEndPos, Time.time - _lerpStartTime);
            transform.LookAt(new Vector3(_lerpEndPos.x, transform.position.y, _lerpEndPos.z));
            if (Vector3.Distance(transform.position, _lerpEndPos) < 30f) _lerping = false;
        }

    }
    /// <summary>
    /// Lerp til you are 5m away from destination
    /// </summary>
    /// <param name="Dest"></param>
    public void LerpTowards(Vector3 Dest)
    {
        _lerpStartPos = transform.position;
        _lerpEndPos = Dest;
        _lerpStartTime = Time.time;
        _lerping = true;
    }



}
