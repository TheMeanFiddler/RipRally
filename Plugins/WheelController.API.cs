using UnityEngine;
using System.Collections;

/// <summary>
/// API for WheelController
/// </summary>
public partial class WheelController : MonoBehaviour
{

    #region UnityDefault

    /// <summary>
    /// Returns the position and rotation of the wheel.
    /// </summary>
    public void GetWorldPose(out Vector3 pos, out Quaternion quat)
    {
        pos = wheel.worldPosition;
        quat = wheel.worldRotation;
    }

    /// <summary>
    /// Brake torque on the wheel axle. [Nm]
    /// Must be positive (zero included).
    /// </summary>
    public float brakeTorque
    {
        get
        {
            return wheel.brakeTorque;
        }
        set
        {
            if (value >= 0)
            {
                wheel.brakeTorque = value;
            }
            else
            {
                wheel.brakeTorque = 0;
                Debug.LogWarning("Brake torque must be positive and so was set to 0.");
            }
        }
    }


    /// <summary>
    /// Is the tractive surface touching the ground?
    /// Returns false if vehicle tipped over / tire sidewall is in contact.
    /// </summary>
    public bool isGrounded
    {
        get
        {
            return hasHit;
        }
        set
        {
            hasHit = value;
        }
    }

    /// <summary>
    /// Mass of the wheel. [kg]
    /// Typical values would be in range [20, 200]
    /// </summary>
    public float mass
    {
        get
        {
            return wheel.mass;
        }
        set
        {
            wheel.mass = value;
        }
    }

    /// <summary>
    /// Motor torque on the wheel axle. [Nm]
    /// Can be positive or negative based on direction.
    /// </summary>
    public float motorTorque
    {
        get
        {
            return wheel.motorTorque;
        }
        set
        {
            wheel.motorTorque = value;
        }
    }

    /// <summary>
    /// Equal to tireRadis but exists because of compatibility with inbuilt WheelCollider.
    /// Radius of the complete wheel. [meters]
    /// Must be larger than 0.
    /// </summary>
    public float radius
    {
        get
        {
            return tireRadius;
        }
        set
        {
            tireRadius = value;
        }
    }

    /// <summary>
    /// Radius of the rim. [meters]
    /// </summary>
    public float rimRadius
    {
        get
        {
            return wheel.rimRadius;
        }
        set
        {
            wheel.rimRadius = value;
        }
    }

    /// <summary>
    /// Side offset of the rim. Positive value will result if wheel further from the vehicle. [meters]
    /// </summary>
    public float rimOffset
    {
        get
        {
            return wheel.rimOffset;
        }
        set
        {
            wheel.rimOffset = value;
        }
    }

    /// <summary>
    /// Radius (height) of the tire. [meters]
    /// </summary>
    public float tireRadius
    {
        get
        {
            return wheel.tireRadius;
        }
        set
        {
            wheel.tireRadius = value;
        }
    }

    /// <summary>
    /// Stiffness of tire in N/mm of tire deflection on flat ground. Usually 100-400.
    /// </summary>
    public float tireStiffness
    {
        get
        {
            return wheel.tireStiffness;
        }
        set
        {
            wheel.tireStiffness = value;
        }
    }

    /// <summary>
    /// Width of the wheel. [meters]
    /// </summary>
    public float tireWidth
    {
        get
        {
            return wheel.width;
        }
        set
        {
            wheel.width = value;
        }
    }

    /// <summary>
    /// Rotations per minute of the wheel around the axle. [rpm]
    /// </summary>
    public float rpm
    {
        get
        {
            return wheel.rpm;
        }
    }


    /// <summary>
    /// Steer angle around the wheel's up axis (with add-ons ignored). [deg]
    /// </summary>
    public float steerAngle
    {
        get
        {
            return wheel.steerAngle;
        }
        set
        {
            wheel.steerAngle = value;
        }
    }


    /// <summary>
    /// Returns Raycast info of the wheel's hit.
    /// Always check if the function returns true before using hit info
    /// as data will only be updated when wheel is hitting the ground (isGrounded == True).
    /// </summary>
    /// <param name="h">Standard Unity RaycastHit</param>
    /// <returns></returns>
    public bool GetGroundHit(out WheelHit hit)
    {
        hit = this.hit;
        return hasHit;
    }

    #endregion

    #region Geometry

    /// <summary>
    /// Camber angle of the wheel. [deg]
    /// Negative angle means that the top of the wheel in closer to the vehicle than the bottom.
    /// </summary>
    public float camber
    {
        get
        {
            return wheel.camberAngle;
        }
    }

    /// <summary>
    /// Camber at top of spring travel. [deg]
    /// Negative angle means that the top of the wheel in closer to the vehicle than the bottom.
    /// </summary>
    public float camberAtTop
    {
        get
        {
            return wheel.camberAtTop;
        }
        set
        {
            wheel.camberAtTop = value;
        }
    }

    /// <summary>
    /// Camber at bottom of spring travel. [deg]
    /// Negative angle means that the top of the wheel in closer to the vehicle than the bottom.
    /// </summary>
    public float camberAtBottom
    {
        get
        {
            return wheel.camberAtBottom;
        }
        set
        {
            wheel.camberAtBottom = value;
        }
    }

    #endregion

    #region Spring

    /// <summary>
    /// Returns value in range [0,1] where 1 means spring is fully compressed.
    /// </summary>
    public float springCompression
    {
        get
        {
            return 1f - spring.compressionPercent;
        }
        set
        {
            spring.compressionPercent = 1f - value;
        }
    }


    /// <summary>
    /// Spring velocity in relation to local vertical axis. [m/s]
    /// Positive on rebound (extension), negative on bump (compression).
    /// </summary>
    public float springVelocity
    {
        get
        {
            return spring.velocity;
        }
    }

    /// <summary>
    /// Current spring force. [N]
    /// Can be written to for use in Anti-roll Bar script or similar.
    /// </summary>
    public float springForce
    {
        get
        {
            return spring.force;
        }
        set
        {
            spring.force = value;
        }
    }

    /// <summary>
    /// Maximum spring force. [N]
    /// </summary>
    public float springMaximumForce
    {
        get
        {
            return spring.maxForce;
        }
        set
        {
            spring.maxForce = value;
        }
    }

    /// <summary>
    /// Spring force curve in relation to spring length.
    /// </summary>
    public AnimationCurve springCurve
    {
        get
        {
            return spring.forceCurve;
        }
        set
        {
            spring.forceCurve = value;
        }
    }
   
    /// <summary>
    /// Length of the spring when fully extended.
    /// </summary>
    public float springLength
    {
        get
        {
            return spring.maxLength;
        }
        set
        {
            spring.maxLength = value;
        }
    }

    public float springLengthInst
    {
        get
        {
            return spring.length;
        }
        set
        {
            spring.length = value;
        }
    }

    #endregion

    #region Damper

    /// <summary>
    /// Current damper force.
    /// </summary>
    public float damperForce
    {
        get
        {
            return damper.force;
        }
    }

    /// <summary>
    /// Rebounding force at 1 m/s spring velocity
    /// </summary>
    public float damperUnitReboundForce
    {
        get
        {
            return damper.unitReboundForce;
        }
        set
        {
            damper.unitReboundForce = value;
        }
    }

    /// <summary>
    /// Bump force at 1 m/s spring velocity
    /// </summary>
    public float damperUnitBumpForce
    {
        get
        {
            return damper.unitBumpForce;
        }
        set
        {
            damper.unitBumpForce = value;
        }
    }

    /// <summary>
    /// Damper force curve in relation to spring velocity.
    /// </summary>
    public AnimationCurve damperCurve
    {
        get
        {
            return damper.dampingCurve;
        }
        set
        {
            damper.dampingCurve = value;
        }
    }

    #endregion

    #region Friction

    /// <summary>
    /// Returns _Friction object with longitudinal values.
    /// </summary>
    public Friction forwardFriction
    {
        get
        {
            return fFriction;
        }
        set
        {
            fFriction = value;
        }
    }

    /// <summary>
    /// Returns _Friction object with lateral values.
    /// </summary>
    public Friction sideFriction
    {
        get
        {
            return sFriction;
        }
        set
        {
            sFriction = value;
        }
    }


    /// <summary>
    /// Sets BCDE values for friction and generates and sets a friction curve from those values.
    /// </summary>
    /// <param name="f">Forward (Longitudinal) or Sideways (Lateral) friction.</param>
    /// <param name="p">Friction parameters in Vector4 format [Vector4(B,C,D,E)].</param>
    public AnimationCurve SetFrictionParams(Friction f, Vector4 p)
    {
        f.frictionParams = p;
        return f.frictionCurve = GenerateFrictionCurve(p);
    }

    #endregion

    #region Misc

    /// <summary>
    /// Returns true if wheel detects that the vehicle has tipped over.
    /// </summary>
    public bool TipOver
    {
        get
        {
            return tipOver;
        }
    }

    /// <summary>
    /// Returns Enum [Side] with the corresponding side of the vehicle a wheel is at [Left, Right]
    /// </summary>
    public Side VehicleSide
    {
        get
        {
            return DetermineSide(transform.position, parent.transform);
        }
    }

    /// <summary>
    /// Returns vehicle speed in meters per second [m/s], multiply by 3.6 for [kph] or by 2.24 for [mph].
    /// </summary>
    public float speed
    {
        get
        {
            return fFriction.speed;
        }
    }

    /// <summary>
    /// Function to adjust parameters related to wheel scan (ground detection).
    /// </summary>
    /// <param name="depth">How many iterations will be performed?</param>
    /// <param name="initialResolution">How many scan rays will be cast in the first iteration?</param>
    /// <param name="subseqResolution">How many scan rays will be cast in the next iterations?</param>
    /// <param name="sideToSideResolution">How many scan planes will there be side-to-side?</param>
    public void SetScanParams(int depth, int initialResolution, int subseqResolution, int sideToSideResolution)
    {
        if(scanDepth > 0)
            scanDepth = depth;
        else
            Debug.LogWarning("Too shallow scan depth. Minimum is 1 iteration.");

        if(initialResolution > 0)
            initialScanResolution = initialResolution;
        else
            Debug.LogWarning("Too low initial scan resolution, must be >= 1.");

        if (subseqResolution > 0)
            scanResolution = subseqResolution;
        else
            Debug.LogWarning("Too low subsequential scan resolution. Minimum is 1.");

        if (sideResolution > 0)
            sideResolution = sideToSideResolution;
        else
            Debug.LogWarning("Too low side to side scan resolution. Minimum is 1.");
    }

    /// <summary>
    /// Returns Vector4[scanDepth, initialScanResolution, scanResolution, sideResolution].
    /// </summary>
    public Vector4 ScanParams
    {
        get
        {
            return new Vector4(scanDepth, initialScanResolution, scanResolution, sideResolution);
        }
    }

    /// <summary>
    /// Returns wheel's parent object.
    /// </summary>
    public GameObject Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;
        }
    }

    /// <summary>
    /// Returns object that represents wheel's visual representation. Can be null in case the object is not assigned (not mandatory).
    /// </summary>
    public GameObject Visual
    {
        get
        {
            return wheel.visual;
        }
        set
        {
            wheel.visual = value;
        }
    }

    /// <summary>
    /// Returns velocity at the wheel's center position in [m/s].
    /// </summary>
    public Vector3 pointVelocity
    {
        get
        {
            return parentRigidbody.GetPointVelocity(wheel.worldPosition);
        }
    }

    /// <summary>
    /// Returns angular velocity of the wheel in radians. Multiply by wheel radius to get linear speed.
    /// </summary>
    public float angularVelocity
    {
        get
        {
            return wheel.angularVelocity;
        }
        set
        {
            wheel.angularVelocity = value;
        }
    }

    public float SpringComp
    {
        get
        {
            return spring.length;
        }
        set
        {
            spring.length = value;
        }
    }

    #endregion
}
