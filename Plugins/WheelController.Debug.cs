using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public partial class WheelController : MonoBehaviour
{

    /// <summary>
    /// Visual representation of the wheel and it's more important Vectors.
    /// </summary>
    void OnDrawGizmos()
    {
        // Draw spring travel
        Gizmos.color = Color.green;
        var forwardOffset = transform.forward * 0.07f;
        var springOffset = transform.up * spring.maxLength;
        Gizmos.DrawLine(transform.position - forwardOffset, transform.position + forwardOffset);
        Gizmos.DrawLine(transform.position - springOffset - forwardOffset, transform.position - springOffset + forwardOffset);
        Gizmos.DrawLine(transform.position, transform.position - springOffset);

        Vector3 interpolatedPos = Vector3.zero;

        // Set dummy variables when in inspector.
        if (!Application.isPlaying)
        {
            if(wheel.visual != null)
            {
                wheel.worldPosition = wheel.visual.transform.position;
                wheel.up = wheel.visual.transform.up;
                wheel.forward = wheel.visual.transform.forward;
                wheel.right = wheel.visual.transform.right;
            }
        }
        else if(parentRigidbody != null)
        {
            interpolatedPos = parentRigidbody.GetPointVelocity(wheel.worldPosition) * Time.deltaTime;
        }

        Gizmos.DrawSphere(wheel.worldPosition, 0.02f);

        // Draw wheel
        Gizmos.color = Color.green;
        DrawWheelGizmo(wheel.tireRadius, wheel.width, wheel.worldPosition + interpolatedPos, wheel.up, wheel.forward, wheel.right);

        if (debug && Application.isPlaying)
        {
            // Draw wheel anchor normals
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Ray(wheel.worldPosition, wheel.up));
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Ray(wheel.worldPosition, wheel.forward));
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Ray(wheel.worldPosition, wheel.right));
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(new Ray(wheel.worldPosition, wheel.inside));

            // Draw axle location
            if (spring.length < 0.01f) Gizmos.color = Color.red;
            else if (spring.length > spring.maxLength - 0.01f) Gizmos.color = Color.yellow;
            else Gizmos.color = Color.green;

            // Draw hit points
            //foreach(RaycastHit hit in hitList)
            //{
            //    Gizmos.color = Color.green;
            //    Gizmos.DrawSphere(hit.point, 0.07f);
            //}

            if (hasHit)
            {
                //Draw hit
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.raycastHit.point, 0.04f);
                Gizmos.DrawRay(new Ray(hit.raycastHit.point, hit.raycastHit.normal));

                //Draw hit forward and right
                Gizmos.color = Color.black;
                Gizmos.DrawRay(new Ray(hit.raycastHit.point, hit.forwardDir));
                Gizmos.DrawRay(new Ray(hit.raycastHit.point, hit.sidewaysDir));

                // Draw forces
                Gizmos.color = Color.yellow;

                // Draw contact points
                /*
                foreach (RaycastHit h in hitList)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(h.point, 0.02f);
                }
                */
            }

            /*
            if (debugScanLineList != null && debugScanLineList.Count > 0)
            {
                // Draw scan lines
                foreach (DebugLine l in debugScanLineList)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(l.point, l.point - l.direction * l.length);
                }
            }
            */
        }
    }


    /// <summary>
    /// Draw a wheel radius on both side of the wheel, interconected with lines perpendicular to wheel axle.
    /// </summary>
    private void DrawWheelGizmo(float radius, float width, Vector3 position, Vector3 up, Vector3 forward, Vector3 right)
    {
        var halfWidth = width / 2.0f;
        float theta = 0.0f;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = position + up * y + forward * x;
        Vector3 newPos = pos;

        for (theta = 0.0f; theta <= Mathf.PI * 2; theta += Mathf.PI / 12.0f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos = position + up * y + forward * x;
            Gizmos.DrawLine(pos - right * halfWidth, newPos - right * halfWidth);
            Gizmos.DrawLine(pos + right * halfWidth, newPos + right * halfWidth);
            Gizmos.DrawLine(pos - right * halfWidth, pos + right * halfWidth);
            pos = newPos;
        }
    }

}
