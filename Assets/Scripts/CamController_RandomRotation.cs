using UnityEngine;
using System.Collections;

public class CamController_RandomRotation : MonoBehaviour {
	public GameObject car;
	private bool camRotationDirection;
	// Use this for initialization
	void Start () {
		//car = GameObject.Find ("car");
		camRotationDirection = false;
	}
	
	// Update is called once per frame
	void Update () {
		float camRotationSpeed;
		if(camRotationDirection==false){camRotationSpeed=0.2f;}
		else {camRotationSpeed=-0.2f;}
		transform.RotateAround (car.transform.position, new Vector3(0,1,0), camRotationSpeed);
		if (Vector3.Distance(transform.position, car.transform.position) > 100)
		{
			if (Random.value>0.5f)
			{transform.position = car.transform.position + car.GetComponent<Rigidbody>().velocity*2;}
			else {
				transform.position =  car.transform.position + Random.insideUnitSphere*10;
			}
			transform.position = new Vector3(transform.position.x, Mathf.Abs(transform.position.y*3), transform.position.z);
			if (Random.value>0.5f){camRotationDirection=true;}
			else {camRotationDirection=false;}
		}
		transform.LookAt (car.transform.position);
	}
}
