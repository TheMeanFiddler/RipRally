using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public bool HasEndSpacers;
    public int SelectedIdx = 0;
    public Transform SelectedScrollItem;
    [Tooltip("The content panel which is a child of this ScrollRect")]
    public Transform CntPnl;
    Transform[] ScrollItems;
    float[] points;
    [Tooltip("how many ScrollItemCount or pages are there within the content (steps)")]
    public int ScrollItemCount = 1;
    [Tooltip("How quickly the GUI snaps to each panel")]
    public float snapSpeed;
    public float inertiaCutoffMagnitude;
    float stepSize;

    ScrollRect scroll;
    bool LerpH;
    float targetH;
    [Tooltip("Snap horizontally")]
    public bool snapInH = true;

    bool LerpV;
    float targetV;
    [Tooltip("Snap vertically")]
    public bool snapInV = true;

    bool dragInit = true;
    int dragStartNearest;


    public void Init()
    {
        scroll = gameObject.GetComponent<ScrollRect>();
        scroll.inertia = true;
        ScrollItemCount = CntPnl.childCount;
        if (HasEndSpacers) ScrollItemCount -= 2;
        ScrollItems = new Transform[ScrollItemCount];
        points = new float[ScrollItemCount];
        stepSize = 1 / (float)(ScrollItemCount - 1);

        for (int i = 0; i < ScrollItemCount; i++)
        {
            ScrollItems[i] = CntPnl.GetChild(ScrollItemCount - i - 1);
            if (HasEndSpacers) ScrollItems[i] = CntPnl.GetChild(ScrollItemCount - i);
            points[i] = i * stepSize;
        }
    }

    void Update()
    {
        if (LerpH)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, snapSpeed * Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH)) LerpH = false;
        }
        if (LerpV)
        {
            scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, targetV, snapSpeed * Time.deltaTime);
            if (Mathf.Approximately(scroll.verticalNormalizedPosition, targetV)) LerpV = false;
        }
    }

    public void DragEnd()
    {
        int target = FindNearest(scroll.verticalNormalizedPosition, points);
        Debug.Log(target);
/*
        if (target == dragStartNearest && scroll.velocity.sqrMagnitude > inertiaCutoffMagnitude * inertiaCutoffMagnitude)
        {
            if (scroll.velocity.y < 0)
            {
                target = dragStartNearest + 1;
            }
            else if (scroll.velocity.y > 1)
            {
                target = dragStartNearest - 1;
            }
            target = Mathf.Clamp(target, 0, points.Length - 1);
        }
*/
        if (scroll.horizontal && snapInH && scroll.horizontalNormalizedPosition > 0f && scroll.horizontalNormalizedPosition < 1f)
        {
            targetH = points[target];
            LerpH = true;
        }
        if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f)
        {
            targetV = points[target];
            LerpV = true;
        }

        dragInit = true;
        SelectedIdx = target;
        SelectedScrollItem = ScrollItems[target];
    }
    public void DragEndH()
    {
        int target = FindNearest(scroll.horizontalNormalizedPosition, points);

        if (target == dragStartNearest && scroll.velocity.sqrMagnitude > inertiaCutoffMagnitude * inertiaCutoffMagnitude)
        {
            if (scroll.velocity.x < 0)
            {
                target = dragStartNearest + 1;
            }
            else if (scroll.velocity.x > 1)
            {
                target = dragStartNearest - 1;
            }
            target = Mathf.Clamp(target, 0, points.Length - 1);
        }

        if (scroll.horizontal && snapInH && scroll.horizontalNormalizedPosition > 0f && scroll.horizontalNormalizedPosition < 1f)
        {
            targetH = points[target];
            LerpH = true;
        }
        if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f)
        {
            targetV = points[target];
            LerpV = true;
        }
        dragInit = true;
    }

    public void OnDrag()
    {
        if (dragInit)
        {
            dragStartNearest = FindNearest(scroll.horizontalNormalizedPosition, points);
            dragInit = false;
        }

        LerpH = false;
        LerpV = false;
    }

    public void SnapTo(int target)
    {
        targetV = points[target];
        SelectedIdx = target;
        SelectedScrollItem = ScrollItems[target];
        scroll.verticalNormalizedPosition = points[target];
        //LerpV = true;

    }

    int FindNearest(float f, float[] array)
    {
        float distance = Mathf.Infinity;
        int output = 0;
        for (int index = 0; index < array.Length; index++)
        {
            if (Mathf.Abs(array[index] - f) < distance)
            {
                distance = Mathf.Abs(array[index] - f);
                output = index;
            }
        }
        return output;
    }
}