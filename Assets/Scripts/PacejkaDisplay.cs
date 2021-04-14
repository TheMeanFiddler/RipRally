using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class PacejkaDisplay:MonoBehaviour
{
    public WheelController WC;
    public Texture LineTex;
    RectTransform _rt;
    GUIStyle NoBorderStyle;
    public Texture PointTex;
    AnimationCurve curve;
    float _ypos;
    float _xpos;
    float _width;
    float _height;
    Image _image;
    float maxx;
    float maxy;
    public float MaxForce;
    public float MaxSlip;   //The slip at the point of max force;

    Canvas canv;


    void Start()
    {
        canv = GetComponentInParent<Canvas>();
        _rt = GetComponent<RectTransform>();
        NoBorderStyle = new GUIStyle();
        NoBorderStyle.border = new RectOffset(0, 0, 0, 0);
        _ypos = Screen.height - _rt.anchoredPosition.y * canv.scaleFactor;
        _xpos = _rt.anchoredPosition.x * canv.scaleFactor;
        _width = _rt.rect.width * canv.scaleFactor;
        _height = _rt.rect.height * canv.scaleFactor;
        _image = GetComponent<Image>();
        curve = WC.fFriction.frictionCurve;
        maxx = curve.keys[curve.length - 2].time * 2f;
        maxy = curve.keys[curve.length - 2].value * 1.5f;
        Slider _slipSlider = transform.Find("SlipSlider").GetComponent<Slider>();
        _slipSlider.maxValue = maxx;
        _slipSlider.value = curve.keys[curve.length - 2].time;
        Slider _forceSlider = transform.Find("ForceSlider").GetComponent<Slider>();
        _forceSlider.maxValue = maxy;
        _forceSlider.value = curve.keys[curve.length - 2].value;
    }
    void OnGUI()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        try
        {
            float y;
            AnimationCurve curve = WC.fFriction.frictionCurve;
            for (int p = 0; p < 100; p++)
            {
                float x = (float)p / 100 * _width;
                float xval = (float)p / 100 * maxx;
                float yval = curve.Evaluate(xval);
                y = yval * _height / maxy;
                GUI.Box(new Rect(_xpos + x - 1f, _ypos - y - 1, 2, 2), LineTex, NoBorderStyle);
            }
            if (WC.isGrounded)
            {
                float pointxval = WC.SlipVectorMagnitude;
                float pointyval = curve.Evaluate(pointxval);
                float pointx = pointxval * _width / maxx;
                float pointy = pointyval * _height / maxy;
                GUI.Box(new Rect(_xpos + pointx - 4f, _ypos - pointy - 4, 8, 8), PointTex, NoBorderStyle);
                if (pointxval > curve.keys[curve.length - 2].time)
                    _image.color = new Color(1,0,0,0.3f); else _image.color = new Color(1, 1, 1, 0.3f);
            }
        }
        catch { }
    }

    public void OnMaxSlipChanged(float ms)
    {
        return;
        AnimationCurve curve = WC.fFriction.frictionCurve;
        Keyframe[] newKeys = new Keyframe[curve.keys.Length];
        curve.keys.CopyTo(newKeys, 0);
        float ratio = ms / curve.keys[curve.length - 2].time;
        for (int k= 0; k<newKeys.Length; k++) { newKeys[k].time = curve.keys[k].time * ratio; }
        AnimationCurve newCurve = new AnimationCurve(newKeys);
        for (int k = 0; k < newKeys.Length; k++) { newCurve.SmoothTangents(k, 0); }
        WC.fFriction.frictionCurve = newCurve;
    }

    public void OnMaxForceChanged(float f)
    {
        return;
        AnimationCurve curve = WC.fFriction.frictionCurve;
        Keyframe[] newKeys = new Keyframe[curve.keys.Length];
        curve.keys.CopyTo(newKeys, 0);
        float ratio = f / curve.keys[curve.length - 2].value;
        for (int k = 0; k < newKeys.Length; k++) { newKeys[k].value = curve.keys[k].value * ratio; }
        AnimationCurve newCurve = new AnimationCurve(newKeys);
        for (int k = 0; k < newKeys.Length; k++) { newCurve.SmoothTangents(k, 0); }
        WC.fFriction.frictionCurve = newCurve;
    }
}

