using UnityEngine;
using System.Collections;

public class RoadSegmentUI : MonoBehaviour {
    public RoadSegment seg;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            seg.CreateTerrainMinimap();
        }
    }
}
