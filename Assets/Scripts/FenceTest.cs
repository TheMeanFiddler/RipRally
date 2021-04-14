using UnityEngine;


    public class FenceTest:MonoBehaviour
    {
        public void LotsOfFences(string FenceType)
        {
            Transform Cntnr = GameObject.Find("Container").transform;
            Object FencePrefab = Resources.Load("Prefabs/" + FenceType + "/" + FenceType + "Prefab");
            {
                while (Cntnr.childCount > 0)
                {
                    Transform Chld = Cntnr.GetChild(0);
                    Chld.SetParent(null);
                    DestroyObject(Chld.gameObject);
                }

                for (int Idx = 0; Idx < 2000; Idx++)
                {

                    GameObject goLeftFence = (GameObject)UnityEngine.Object.Instantiate(FencePrefab, Vector3.zero, Quaternion.identity);
                    goLeftFence.transform.position = new Vector3(Idx,0, 0);
                    goLeftFence.transform.SetParent(Cntnr);


                }
            }
        }
    }

