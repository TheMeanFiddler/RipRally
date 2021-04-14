using UnityEngine;
public abstract class aSceneryComponent:MonoBehaviour
{
    protected PlaceableObject _placeableObj;

    public abstract void Init(PlaceableObject p);
}

