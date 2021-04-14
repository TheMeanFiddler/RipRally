using UnityEngine;
using System.Collections;

public class PlayerSelectorNotUsed : MonoBehaviour {
    GameObject Cam;
    GameObject btnPlayerLeft;
    GameObject btnPlayerRight;
    GameObject Selector;
    GameObject MenuCar;
    GameObject Roadworks;
    Vector3 SelectorPos;
    Vector3 SelectorTargetPos;

	// Use this for initialization
	void Start () {
        Cam = GameObject.Find ("CameraUI");
        Selector = GameObject.Find("PlayerSelector");
        btnPlayerLeft = GameObject.Find("btnPlayerLeft");
        btnPlayerRight = GameObject.Find("btnPlayerRight");
        SelectorTargetPos = Selector.transform.position;
        if(PlayerManager.Type == "CarPlayer")SelectorTargetPos=SelectorTargetPos+Vector3.right*5;
		MenuCar = GameObject.Find ("MenuCar");
        Roadworks = GameObject.Find("MenuRoadworksSign");
		//Hide all the gameobjects that have a dot in them, because these are the damaged meshes
		foreach(Transform child in MenuCar.transform){
			if(child.gameObject.name.Contains(".")) {
				child.gameObject.GetComponent<MeshRenderer>().enabled=false;
			}
		}
        ChangeColor(PlayerManager.Color);
	}
	
	// Update is called once per frame
	void Update () {
		MenuCar.transform.Rotate(Vector3.up,1);
        Roadworks.transform.Rotate(Vector3.up, 1);
        if (Vector3.Distance(Selector.transform.position, SelectorTargetPos) > 0.005f)
        {
            Selector.transform.position = Vector3.Lerp(Selector.transform.position, SelectorTargetPos, Time.deltaTime * 2);
        }
	}

    //Called when you click the left or right buttton
    public void ChangePlayer(string Dir)
    {
        if (Dir == "L") { SelectorTargetPos += Vector3.right * 5; }
        if (Dir == "R") { SelectorTargetPos -= Vector3.right * 5; }
        switch ((int)SelectorTargetPos.x)
        {
            case 0:
                PlayerManager.Type = "CarPlayer";
                break;
            case -5:
                PlayerManager.Type = "BuilderPlayer";
                break;
        }
        btnPlayerLeft.SetActive((int)SelectorTargetPos.x <= -5);
        btnPlayerRight.SetActive((int)SelectorTargetPos.x >= 0);
    }


	//Called when you click one of the colour buttons:
	public void ChangeColor(string color){
        GameObject.Find("GameStarterPanel").GetComponent<HomeScreen>().ChangePlayerColor(color);
		PlayerManager.Color = color;
		Material Mat = (Material)Resources.Load("Prefabs/Materials/" + color, typeof(Material));
		GameObject car = GameObject.Find("MenuCar");
		foreach(Transform child in car.transform){
			//Change the colour
			if(child.gameObject.GetComponent<Renderer>()!=null){
				
				Material[] CarMats = child.gameObject.GetComponent<Renderer>().sharedMaterials;
				for(int idx=0;idx<CarMats.Length;idx++) {
					if(CarMats[idx].name.Contains("Bodywork")){
						CarMats[idx] = Mat;
						child.gameObject.GetComponent<Renderer>().sharedMaterials = CarMats;
						idx = CarMats.Length;
					}
				}
			}
		}
	}
}
