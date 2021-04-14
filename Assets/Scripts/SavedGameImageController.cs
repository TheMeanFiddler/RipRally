using UnityEngine;
using UnityEngine.UI;



    class SavedGameImageController: MonoBehaviour
    {
        public int GameId;
        public Material mat;

        void Update(){
        //line = gameObject.AddComponent<LineRenderer>();
        //line.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        //line.SetWidth(0.3F, 0.3F);
        //line.SetColors(Color.green, Color.green);
        //line.SetVertexCount(3);
            //Vector3 pointA = transform.position;
            //Vector3 pointB = transform.position + new Vector3(300,300,300);
            //Debug.DrawLine( pointA, pointB, Color.green);
        }

        void OnMouseDown()
        {
            Debug.Log("Mousedown");
        }

        void OnRenderObject()
        {
            if (!mat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            Vector3 pointA = new Vector3(0, 0, 9999);
            Vector3 pointB = transform.position + new Vector3(300, 300, 9999);
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(pointA);
            GL.Vertex(pointB);
            GL.End();
            GL.PopMatrix();
        }
    }

