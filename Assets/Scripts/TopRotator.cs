using UnityEngine;
using System.Linq;


public class TopRotator : MonoBehaviour
{
    Mesh _mesh;
    Vector3[] _verts;
    Vector3[] _newVerts;
    Vector3 _pivotPos;
    float _maxY;
    public float _yThreshold = 0.5f;

    void Start()
    {
        _mesh = transform.GetComponentInChildren<MeshFilter>().mesh;
        _verts = _mesh.vertices;
        _newVerts = _mesh.vertices;
        //Find the middle of the top and set this as the pivot
        _maxY = _verts.OrderBy(v => v.y).LastOrDefault().y;
        _pivotPos = Vector3.up * _maxY;
    }

    public void RotateTop(Quaternion q)
    {
        Vector3 _eulr = q.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(0, _eulr.y, 0));
        for (int n = 0; n < _verts.Length; n++)
        {

            if (_verts[n].y > _yThreshold)
            {
                _newVerts[n] = VectGeom.RotatePointAroundPivot(_verts[n], _pivotPos, q.eulerAngles * (_verts[n].y-_yThreshold)/(_maxY-_yThreshold));
                _newVerts[n] = new Vector3(_verts[n].x, _newVerts[n].y, _verts[n].z);
            }

        }
        _mesh.vertices = _newVerts;
    }
}



