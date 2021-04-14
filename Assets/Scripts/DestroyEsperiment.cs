using UnityEngine;


    public class DestroyEsperiment: MonoBehaviour
    {
        void OnDestroy()
    {
        Debug.Log("Destroyyyyyyy " + this.name);
    }
    }

