using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public partial class WheelController : MonoBehaviour
{
    // Components
    [SerializeField]
    public Wheel wheel;
    [SerializeField]
    private WheelHit hit; // container for hit data
    [SerializeField]
    private Spring spring;
    [SerializeField]
    private Damper damper;
    [SerializeField]
    public Friction fFriction; // forward (longitudinal) friction
    [SerializeField]
    private Friction sFriction; // sideways (lateral) friction

    private Vector3 velocity;   // velocity at the center of the wheel
    private GameObject Marker;
    // Variables for calculating force application point and the force that is applied
    //This is a load of rubbish displaying the programmer's misunderstanding of basic physics
    //You cant just sum the forces and average out the Application points. You have to apply them separately
    //private Vector3 forcePoint;
    //private List<Vector3> forcePointList = new List<Vector3>();
    //private Vector3 forceSum;
    public float SideForceDampRate;
    private float SideForceDamped;
    //[SerializeField]
    //private List<RaycastHit> hitList = new List<RaycastHit>(); // holds all raycast hits

    // Raycast ignore layer
    public int scanIgnoreLayer = 20;
    public int scanIgnoreTriggerLayer = 11;
    LayerMask layerMask = ~(1 << 20 | Physics.IgnoreRaycastLayer | 1 << 11 | 1 << 13);
                        //scanIgnoreLayer                  scanIgnoreTriggerLayer  fENCElAYER

    // For motorcycles, trikes, etc...
    public bool singleWheel = false;

    // Wheel scan control params
    [SerializeField]
    private int initialScanResolution = 10; // resolution of the first scan pass
    [SerializeField]
    private int scanDepth = 2; // determines how many passes will happen
    [SerializeField]
    private int scanResolution = 4; // resolution of scan passes after the first scan
    [SerializeField]
    private int sideResolution = 2; // number of scan planes (side-to-side)

    // Control flags
    [SerializeField]
    private bool hasHit = false; // true if the wheel is touching ground
    [SerializeField]
    private bool tipOver = false; // true if vehicle on side
    public bool Replayer = false;
    public bool GravelSimulation = false;
    public bool WashboardSimulation = false;
    public float SlipVectorMagnitude;
    public Vector2 slipVectorNorm;

    // Debug variables
    public bool debug;
    //private List<DebugLine> debugScanLineList = new List<DebugLine>(); Removed becuase it was causing CG spikes

    // Parent object related
    [SerializeField]
    public GameObject parent;
    private Rigidbody parentRigidbody;

    public bool useRimCollider;
    SphereCollider _bottomoutCollider;

    // Vehicle side for mirroring directions
    public enum Side
    {
        Left = -1,
        Right = 1,
    }
    private Side vehicleSide;

    public float CircleCoefficient = 1f;

    private Queue<float> sideSpeeds = new Queue<float>();

    private void Awake()
    {
        // Fill in necessary values
        InitIfNull();
    }

    void Start()
    {

        wheel.Init(this);

        // Find parent
        parentRigidbody = parent.GetComponent<Rigidbody>();

        // Find vehicle side
        vehicleSide = DetermineSide(transform.position, parent.transform);

        // If single-wheel axis
        if (singleWheel) vehicleSide = Side.Right;

        hit.raycastHit = new RaycastHit();
        //Marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Marker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //Marker.GetComponent<Collider>().enabled = false;
        _bottomoutCollider = gameObject.AddComponent<SphereCollider>();
        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = 0f;
        material.staticFriction = 0f;
        material.bounciness = 0f;
        material.bounceCombine = PhysicMaterialCombine.Minimum;
        _bottomoutCollider.material = material;
        _bottomoutCollider.radius = radius;
        _bottomoutCollider.enabled = false;
    }

    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Keypad0) && gameObject.name == "WCRR") Debug.Log("WCRR fwd" + PrintFrictionCurve(fFriction.frictionCurve));
        // Find contact point with ground
        if(!Replayer)
        HitUpdate();
        // Update direction vectors needed for further calculations
        var steerQuaternion = Quaternion.AngleAxis(wheel.steerAngle, transform.up);
        wheel.up = steerQuaternion * Quaternion.AngleAxis(90 - wheel.camberAngle * (int)vehicleSide, transform.forward) * transform.right;
        wheel.forward = steerQuaternion * Quaternion.AngleAxis(-wheel.camberAngle * (int)vehicleSide, transform.forward) * transform.forward;
        wheel.right = Quaternion.AngleAxis(90.0f, wheel.up) * wheel.forward;
        wheel.inside = wheel.right * -(int)vehicleSide;

        // Find speeds relative to the wheel position (sideways and forward)
        fFriction.speed = Vector3.Dot(velocity, wheel.forward);
        sFriction.speed = Vector3.Dot(velocity, wheel.right);

        // Updated wheel components
        SuspensionUpdate();
        WheelUpdate();
        if (!Replayer)
        {
            FrictionUpdate();
            UpdateForces();
        }
    }

    /// <summary>
    /// Searches for wheel hit point by iterating WheelScan() function to the requested scan depth.
    /// </summary>

    private void HitUpdate()
    {
        UnityEngine.Profiling.Profiler.BeginSample("WCHitUpdate");

            hasHit = WheelScan(ref hit, true, true);


            // Forward vector of the hit
            hit.forwardDir = Quaternion.AngleAxis(steerAngle, transform.up) * Quaternion.AngleAxis(90.0f, transform.right) * hit.raycastHit.normal;
            hit.sidewaysDir = Quaternion.AngleAxis(90.0f, hit.raycastHit.normal) * hit.forwardDir * (int)vehicleSide;

        UnityEngine.Profiling.Profiler.EndSample();
    }

    /// <summary>
    /// Calculates spring and damper forces
    /// </summary>
    private void SuspensionUpdate()
    {
        UnityEngine.Profiling.Profiler.BeginSample("SuspUpdate");
        if (!Replayer)
        {
            spring.bottomOut = false;
            spring.prevLength = spring.length;

            if (hasHit && !tipOver)
            {
                spring.length = Vector3.Distance(Vector3.Project(transform.position, transform.up), Vector3.Project(hit.raycastHit.point, transform.up))
                    - Mathf.Abs(wheel.tireRadius * Mathf.Sin(hit.angleForward * Mathf.Deg2Rad))
                    - (hit.sideOffset / -(int)vehicleSide * wheel.width / 2f) * Mathf.Sin(wheel.camberAngle * -(int)vehicleSide * Mathf.Deg2Rad);

                // Check if spring has bottomed out
                if (spring.length < 0)
                {
                    //spring.length = 0;
                    spring.bottomOut = true;
                }
                // Check if spring is fully extended
                else if (spring.length > spring.maxLength)
                {
                    spring.length = spring.maxLength;
                }

                spring.availableLength = spring.maxLength - spring.length;
                spring.compressionPercent = spring.availableLength / spring.maxLength;
                spring.force = spring.maxForce * Mathf.Lerp(0f, spring.forceCurve.Evaluate(spring.compressionPercent), Mathf.Clamp01(spring.compressionPercent * 30f));
                //spring.force = spring.maxForce * spring.forceCurve.Evaluate(spring.compressionPercent);

                spring.velocity = (spring.length - spring.prevLength) / Time.fixedDeltaTime;

                damper.maxForce = spring.length < spring.prevLength ? damper.unitBumpForce : damper.unitReboundForce;

                // Bump
                if (spring.length <= spring.prevLength)
                {
                    damper.force = damper.unitBumpForce * damper.dampingCurve.Evaluate(Mathf.Abs(spring.velocity));
                }

                // Rebound
                else
                {
                    damper.force = -damper.unitReboundForce * damper.dampingCurve.Evaluate(Mathf.Abs(spring.velocity));
                }
            }
            else
            {
                spring.length = spring.maxLength;
                spring.availableLength = 0f;
                spring.compressionPercent = 1.0f;
                spring.force = 0.0f;

                damper.force = 0.0f;
            }
        }
        else //If Replayer
        {
            spring.compressionPercent = spring.length / spring.maxLength;
            spring.availableLength = spring.maxLength - spring.length;
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }

    /// <summary>
    /// Calculates wheel related parameters regarding position and rotation.
    /// </summary>
    private void WheelUpdate()
    {
        UnityEngine.Profiling.Profiler.BeginSample("WheelUpdate");
        // Claculate wheel velocity
        if (!Replayer)
            velocity = parentRigidbody.GetPointVelocity(wheel.worldPosition);

        // Calculate wheel position without tire deflection
        wheel.prevWorldPosition = wheel.worldPosition;
        if (!spring.bottomOut)
            wheel.worldPosition = transform.position - transform.up * spring.length - wheel.inside * wheel.rimOffset;
        else
        {
            wheel.worldPosition = hit.point + wheel.up * radius;
            spring.length = -Vector3.Distance(transform.position, wheel.worldPosition);
            spring.availableLength = spring.maxLength - spring.length;
            spring.compressionPercent = spring.availableLength / spring.maxLength;
        }
        // Calculate camber based on spring travel
        wheel.camberAngle = Mathf.Lerp(wheel.camberAtBottom, wheel.camberAtTop, spring.availableLength / spring.maxLength);

        // Tire load based off spring and damper forces
        wheel.tireLoad = Mathf.Clamp(spring.force + damper.force, 0.0f, Mathf.Infinity);
        if (hasHit) hit.force = wheel.tireLoad;

        // Calculate tire deflection
        //float newTireDeflection = Mathf.Clamp((1.0f - (wheel.contactPatchSurface / wheel.bottomOutTireArea))
        //    * ((wheel.tireLoad / wheel.tireStiffness) / 1000.0f), 0.0f, wheel.tireRadius - wheel.rimRadius);
        //wheel.tireDeflection = Mathf.Lerp(wheel.tireDeflection, newTireDeflection, 15f * Time.fixedDeltaTime);

        // Calculate visual rotation angle between 0 and 2pi rad
        wheel.rotationAngle = (wheel.rotationAngle % 360.0f) + (wheel.angularVelocity * Mathf.Rad2Deg * Time.fixedDeltaTime);

        // Calculate wheel rotations
        var steerQuaternion = Quaternion.AngleAxis(wheel.steerAngle, wheel.up);
        var camberQuaternion = Quaternion.AngleAxis(-(int)vehicleSide * wheel.camberAngle, transform.forward);
        var axleRotation = Quaternion.AngleAxis(wheel.rotationAngle, transform.right);

        // Set rotation   
        wheel.worldRotation = steerQuaternion * camberQuaternion * axleRotation * transform.rotation;

        // Apply rotation and position to visuals if assigned
        if (wheel.visual != null)
        {
            wheel.visual.transform.position = wheel.worldPosition;
            wheel.visual.transform.rotation = wheel.worldRotation;
        }

        if (useRimCollider && wheel.rimCollider != null)
        {
            wheel.rimCollider.transform.position = wheel.worldPosition;
            wheel.rimCollider.transform.rotation = wheel.worldRotation;
        }

        // Calculate wheel's angular velocity
        if (!Replayer)
        {
            if (hasHit)
            {
                wheel.freeRollingAngularVelocity = fFriction.speed / wheel.tireRadius;
            }
            else
            {
                wheel.freeRollingAngularVelocity = wheel.angularVelocity;
                Mathf.Clamp(wheel.freeRollingAngularVelocity, -100.0f, 100.0f);
            }

            wheel.angularVelocity = wheel.freeRollingAngularVelocity;
            wheel.angularVelocity += wheel.residualAngularVelocity;     //wheelspin
            //wheel.angularVelocity += ((wheel.motorTorque-fFriction.force) / wheel.inertia) * Time.fixedDeltaTime;     //Dont know what this was but it made the wheel spin the wrong way

            // Add brake acceleration
            wheel.angularVelocity -= Mathf.Sign(wheel.angularVelocity) * Mathf.Clamp(((wheel.brakeTorque - fFriction.force) / wheel.inertia)
                * Time.fixedDeltaTime, -Mathf.Abs(wheel.angularVelocity), Mathf.Abs(wheel.angularVelocity));
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    /// <summary>
    /// Does lateral and longitudinal slip and force calculations.
    /// </summary>
    private void FrictionUpdate()
    {
        //Calculate FrictionVelocity from Wheel.WorldPosition
        UnityEngine.Profiling.Profiler.BeginSample("FrictionUpdate");
        Vector3 dist = wheel.worldPosition - wheel.prevWorldPosition;
        Vector3 vel = dist / Time.fixedDeltaTime;

        float forwardDist = Vector3.Dot(dist, wheel.forward);
        float sideDist = Vector3.Dot(dist, wheel.right);

        fFriction.speed = forwardDist / Time.fixedDeltaTime;
        sFriction.speed = sideDist / Time.fixedDeltaTime;

        //*******************
        // Side slip
        //*******************

        if (hasHit)
        {
            // Fix for low speed sideways creep 
            float avgSideSpeed = 0;
            if (Mathf.Abs(sFriction.speed) > Mathf.Abs(fFriction.speed * 0.3f) && Mathf.Abs(sFriction.speed) < 0.2f)
            {
                sideSpeeds.Enqueue(sFriction.speed);
                if (sideSpeeds.Count > 10)
                {
                    sideSpeeds.Dequeue();
                }

                float sideSpeedSum = 0;
                for (int i = 0; i < sideSpeeds.Count; i++)
                {
                    sideSpeedSum += sideSpeeds.ElementAt(i);
                }
                avgSideSpeed = sideSpeedSum / sideSpeeds.Count;
            }
            else
            {
                sideSpeeds.Clear();
            }

            // Calculate accurate slip when possible
            //from Brian Beckman formula - calculated slip angle with an arbitrary scaling factor
            //Edy says this should be scaled up by the wheel velocity. I think its something to do with friction circle of forces
            float accurateSlip = fFriction.speed == 0 ? 0 : (Mathf.Atan(sFriction.speed / Mathf.Abs(fFriction.speed)) * Mathf.Rad2Deg / 100.0f);

            // Under low speeds calculate slip from speed since accurate slip is unstable at those speeds (divide by near zero)
            float lowSpeedSlip = Mathf.Sign(sFriction.speed + avgSideSpeed) * Mathf.Pow(Mathf.Abs(sFriction.speed + avgSideSpeed), 1.54f);

            // Depending on speed, get slip from the accurate and low speed slip
            //Jason - This doesnt look right because hes used vel.magnitude instead of sideways velocity
            sFriction.slip = Mathf.Lerp(lowSpeedSlip, accurateSlip, Mathf.Clamp01(vel.magnitude - 2.0f)) * sFriction.slipCoefficient;
        }
        else
        {
            sFriction.slip = 0f;
        }

        if (sFriction.ForcedSlip != null) sFriction.slip = (float)sFriction.ForcedSlip;

        //Jason - Moved this to after the forward calc because forward and side slip act together
        //sFriction.force = Mathf.Sign(sFriction.slip) * sFriction.frictionCurve.Evaluate(Mathf.Abs(sFriction.slip))
        //* wheel.tireLoad * sFriction.forceCoefficient;


            //*******************
            // Forward slip
            //*******************
        if (hasHit)
        {
            // Angular speed of the wheel in m/s
            float angularSpeed = wheel.angularVelocity * wheel.tireRadius;

            // Slip when calculating angular speed and linear speed ratio is viable
            //Jason - Brian Beckman formula is wrong:
            float dynamicSlip = fFriction.speed == 0 ? 0 : ((angularSpeed) - fFriction.speed) / Mathf.Abs(fFriction.speed);
            //Should be this - but doesnt make much difference:
            //float dynamicSlip = fFriction.speed == 0 ? 0 : angularSpeed / fFriction.speed - 1;

            // Slip when either angular speed or linear speed is too low for conventional calculation
            float staticSlip = ((fFriction.speed + angularSpeed) / 2f);

            // staticSlip when stationary and at low speeds, dynamicSlip when in motion
            fFriction.slip = Mathf.Lerp(staticSlip, dynamicSlip, Mathf.Abs(fFriction.speed / 3f)) * fFriction.slipCoefficient;
        }
        else
        {
            fFriction.slip = 0f;
        }

        //Jason - combine the slips to make a vector and use this instead of the separate slip values to evaluate the graph
        //I think this is what Brian Beckman says in http://phors.locost7.info/phors25.htm

        //Heres's the circle of slip where we combine the fslip and sslip. You can mnake it oval by changing the slip coefficients
        Vector2 slipVector = Mathf.Sign(sFriction.slip) * sFriction.slip * Vector2.right * sFriction.slipCoefficient + Mathf.Sign(fFriction.slip) * fFriction.slip * Vector2.up * fFriction.slipCoefficient;
        //This is the direction of the slip
        slipVectorNorm = slipVector.normalized;
        //Apply the Pacejca fslip curve. (the sslip curve gets ignored)
        SlipVectorMagnitude = slipVector.magnitude;
        float CombinedForce = fFriction.frictionCurve.Evaluate(slipVector.magnitude);
        Vector2 forceVector = slipVectorNorm * CombinedForce;
        float sFrictionCircularForce = forceVector.x * wheel.tireLoad * Mathf.Sign(sFriction.slip) * sFriction.forceCoefficient;
        float availableCircularForwardForce = forceVector.y * Mathf.Sign(fFriction.slip) * wheel.tireLoad * fFriction.forceCoefficient;

        //float sFrictionLinearForce = Mathf.Sign(sFriction.slip) * sFriction.frictionCurve.Evaluate(Mathf.Abs(sFriction.slip))
        //    * wheel.tireLoad * sFriction.forceCoefficient;

        sFriction.force = sFrictionCircularForce;

        //Allowed maximum force in forward direction before the wheel loses traction
        //float availableLinearForwardForce = Mathf.Sign(fFriction.slip) * fFriction.frictionCurve.Evaluate(Mathf.Abs(fFriction.slip))
        //    * wheel.tireLoad * fFriction.forceCoefficient;
        float availableForwardForce = availableCircularForwardForce; // * CircleCoefficient + availableLinearForwardForce * (1 - CircleCoefficient);

        wheel.rpm = wheel.angularVelocity * 9.5493f;

        // Forward force resulting from motorTorque
        var motorForceFromTorque = wheel.motorTorque / wheel.tireRadius;

        // Negating brake force resulting from brakeTorque
        var brakeForceFromTorque = (wheel.brakeTorque * Mathf.Clamp01(Mathf.Abs(fFriction.speed * 4)) / wheel.tireRadius);

        // Combination of motor and negating force
        var combinedForce = motorForceFromTorque - Mathf.Sign(fFriction.speed) * brakeForceFromTorque;
        // Forward force as a result of motor and brake forces acting together, up to the allowed traction limit
        fFriction.force = Mathf.Clamp(combinedForce, -Mathf.Abs(availableForwardForce), Mathf.Abs(availableForwardForce));

        /*
        #if UNITY_EDITOR
        #elif UNITY_ANDROID
                try
                {
                    if (slipVector.magnitude > fFriction.frictionCurve.keys[1].time)
                    {
                        Handheld.Vibrate();
                    }
                }
                catch { }
        #endif
        */
        /* never used
        if (Mathf.Abs(wheel.motorTorque) > wheel.brakeTorque && hasHit && !Replayer)
        {
            var surfaceTorque = Mathf.Sign(wheel.angularVelocity - wheel.freeRollingAngularVelocity)
                * ((Mathf.Abs(fFriction.force) * wheel.tireRadius) / wheel.inertia) * Time.fixedDeltaTime;
            //wheel.angularVelocity -= surfaceTorque;
        }
        */
        // Calculate residual (wheel-spinning) force that could not be put to ground
        //therefore calculate the Wheelspin velocity - just used to calculate the wheel angular velocity - which then gives the slip velocity 
        var residualForce = 0.0f;
        if (Mathf.Abs(combinedForce) > Mathf.Abs(availableForwardForce))
            residualForce = Mathf.Sign(combinedForce) * (Mathf.Abs(combinedForce) - Mathf.Abs(availableForwardForce));
        wheel.residualAngularVelocity = ((residualForce * wheel.tireRadius) / wheel.inertia) * Time.fixedDeltaTime;

        // Limit side and forward force if needed, useful for simulating slippery tires
        if (fFriction.maxForce > 0)
        {
            fFriction.force = Mathf.Clamp(fFriction.force, -sFriction.maxForce, sFriction.maxForce);
        }
        if (sFriction.maxForce > 0)
        {
            sFriction.force = Mathf.Clamp(sFriction.force, -sFriction.maxForce, sFriction.maxForce);
        }

        // Fill in WheelHit info for Unity wheelcollider compatibility
        if (hasHit)
        {
            hit.forwardSlip = fFriction.slip;
            hit.sidewaysSlip = sFriction.slip;
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }

    /// <summary>
    /// Updates force values, calculates force vector and applies it to the rigidbody.
    /// </summary>
    private void UpdateForces()
    {
        UnityEngine.Profiling.Profiler.BeginSample("UpdateForces");
        if (hasHit)
        {
            // Add forward tractive force                 
            //forceSum += hit.forwardDir * fFriction.force;
            //forcePointList.Add(wheel.worldPosition);
            //Jason
            parentRigidbody.AddForceAtPosition(hit.forwardDir * fFriction.force, hit.point);

            // Add side tractive force
            //forceSum += -hit.sidewaysDir * (int)vehicleSide * sFriction.force;
            //forcePointList.Add(wheel.worldPosition);
            //Jason
            parentRigidbody.AddForceAtPosition(-hit.sidewaysDir * (int)vehicleSide * sFriction.force, hit.point);

            // Add spring/damper force
            // raycastHit.normal cannot be used for horizontal force calculation because 
            // on edge of stepped surface normal will always be 90 deg.
            float normalAngle = AngleSigned(wheel.up, hit.raycastHit.normal, wheel.right);
            float horizontalModifier = 0;

            // Check that horizontalModifier is not actually detecting tilted ground
            if (Mathf.Abs((normalAngle + 90) - hit.angleForward) > 10.0f)
                horizontalModifier = -Mathf.Cos((hit.angleForward - normalAngle) * Mathf.Deg2Rad);

            // Add spring and damper forces
            float verticalModifier = Mathf.Sin((hit.angleForward - normalAngle) * Mathf.Deg2Rad);
            //forceSum += Mathf.Clamp(spring.force + damper.force, 0.0f, Mathf.Infinity) * hit.raycastHit.normal * verticalModifier;
            //forcePointList.Add(transform.position);
            //Jason - I Dont think this is right - maybe should apply the force at the top of the spring
            parentRigidbody.AddForceAtPosition(Mathf.Clamp(spring.force + damper.force, 0.0f, Mathf.Infinity) * hit.raycastHit.normal * verticalModifier, transform.position);

            // Add force that is result of hitting obstacles
            //forceSum += wheel.forward * wheel.tireLoad * horizontalModifier;
            //forcePointList.Add(hit.raycastHit.point);
            parentRigidbody.AddForceAtPosition(wheel.forward * wheel.tireLoad * horizontalModifier, hit.raycastHit.point);

            //Average out the forces - this is a load of bollocks
            //forcePoint = Vector3Average(forcePointList);
            // Apply force - bollocks
            //parentRigidbody.AddForceAtPosition(forceSum, forcePoint);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }


    //**********************************//
    // HELPER FUNCTIONS                 //
    //**********************************//

    /// <summary>
    /// Average of multiple Vector3's
    /// </summary>
    private Vector3 Vector3Average(List<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 v in vectors)
        {
            sum += v;
        }
        return sum / vectors.Count;
    }


    /// <summary>
    /// Determines on what side of the vehicle a point is. 
    /// </summary>
    /// <param name="pointPosition">Position of the point in question.</param>
    /// <param name="referenceTransform">Position of the reference transform.</param>
    /// <returns>Enum Side [Left,Right] (int)[-1,1]</returns>
    private Side DetermineSide(Vector3 pointPosition, Transform referenceTransform)
    {
        Vector3 relativePoint = referenceTransform.InverseTransformPoint(pointPosition);

        if (relativePoint.x < 0.0f)
        {
            return WheelController.Side.Left;
        }
        else
        {
            return WheelController.Side.Right;
        }
    }


    /// <summary>
    /// Calculates an angle between two vectors in relation a normal.
    /// </summary>
    /// <param name="v1">First Vector.</param>
    /// <param name="v2">Second Vector.</param>
    /// <param name="n">Angle around this vector.</param>
    /// <returns>Angle in degrees.</returns>
    private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }


    /// <summary>
    /// Main function for detecting wheel collision.
    /// Does a raycast scan along the wheel plane, forward to back, outside to inside.
    /// </summary>
    RaycastHit _raycastHit;

    private bool WheelScan(ref WheelHit resulthit, bool initial, bool final)
    {
        // Precalculate variables that are constant
        bool hasHit = false;

        float rayLength = wheel.tireRadius + 0.5f;//Jason added 0.5f because the collider fell through the floor

        
        float halfWidth = wheel.width / 2.0f;


        // Wheel sides
        Vector3 upRadius = wheel.up * wheel.tireRadius;
        Vector3 fwdRadius = wheel.forward * wheel.tireRadius;
        Vector3 wheelDown = -wheel.up;

        Ray ray = new Ray();

        // Loop for iterating through side-to-side scan planes
        //float s = 0;
        //sideOffset = s;
        //sideOffsetVect = wheel.inside * s * (int)vehicleSide;


        //float angle = 90;
        // Angles
        //float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        //float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        //float sin = 0;
        //float cos = 1;

        // Up ray Offset
        //Vector3 upOffset;
        //upOffset.x = sin * upRadius.x;
        //upOffset.y = sin * upRadius.y;
        //upOffset.z = sin * upRadius.z;

        // Forward ray offset
        //Vector3 forwardOffset;
        //forwardOffset.x = cos * fwdRadius.x;
        //forwardOffset.y = cos * fwdRadius.y;
        //forwardOffset.z = cos * fwdRadius.z;

        // Ray
        ray.origin = transform.position - transform.up * spring.maxLength + upRadius;
        ray.direction = wheelDown;
        bool grounded = Physics.Raycast(ray, out _raycastHit, rayLength, layerMask);

        // Debug
        //if (Debug.isDebugBuild)
        //debugScanLineList.Add(new DebugLine(ray.origin, wheel.up, rayLength));

        // Check if current hit is the best candidate
        if (grounded)
        {
            hasHit = true;
            resulthit.raycastHit = _raycastHit;
            if (GravelSimulation && Mathf.Abs(wheel.angularVelocity) > 5) resulthit.raycastHit.point += UnityEngine.Random.Range(0, 0.05f) * wheel.up;
            if (WashboardSimulation && Mathf.Abs(wheel.angularVelocity) > 5) resulthit.raycastHit.point += UnityEngine.Random.Range(0, 0.02f) * wheel.up;
            resulthit.angleForward = 90;
            resulthit.sideOffset = 0;
        }
        else
        { hasHit = false; }

        return hasHit;
    }




    /*****************************/
    /* CLASSES                   */
    /*****************************/

    /// <summary>
    /// All info related to longitudinal force calculation.
    /// </summary>
    [System.Serializable]
    public class Friction
    {
        public AnimationCurve frictionCurve;
        public Vector4 frictionParams;
        public float frictionParamsX;
        public float forceCoefficient = 1;
        public float slipCoefficient = 1;
        public float maxForce;
        public float slip;
        public float speed;
        public float force;
        public int _presetSelector;
        public float? ForcedSlip;
    }


    /// <summary>
    /// Contains everything wheel related, including rim and tire.
    /// </summary>
    [System.Serializable]
    public class Wheel
    {
        public float mass = 25.0f;
        public float rimRadius = 0.3f;
        public float rimOffset = 0f;
        public float tireRadius = 0.5f;
        public float width = 0.25f;
        public float tireStiffness = 200.0f;

        public float rpm;

        public Vector3 prevWorldPosition;
        public Vector3 worldPosition;
        public Quaternion worldRotation;

        public float camberAtTop = -4.0f;
        public float camberAtBottom = 7.0f;
        public float camberAngle;

        public float inertia;

        public float angularVelocity;
        public float freeRollingAngularVelocity;
        public float residualAngularVelocity;
        public float steerAngle;
        public float prevSteerAngle;    //Jason
        private float steerAngularVelocity;
        public float rotationAngle;
        public GameObject visual;
        public GameObject rim;
        public CapsuleCollider rimCollider;

        public Vector3 up;
        public Vector3 inside;
        public Vector3 forward;
        public Vector3 right;

        public float tireLoad;
        //public float tireDeflection;
        //public float tireArcLength;

        //public float hitWidth;
        //public float hitHeight;

        public float motorTorque;
        public float brakeTorque;

        /// <summary>
        /// Calculation of static parameters and creation of rim collider.
        /// </summary>
        public void Init(WheelController wc)
        {
            // Precalculate wheel variables
            inertia = 0.5f * mass * (tireRadius * tireRadius + tireRadius * tireRadius);

            // Calculate maximum deflection tire is allowed to have
            //var sagitta = tireRadius - rimRadius;
            //tireArcLength = Mathf.Sqrt(2 * tireRadius * sagitta - sagitta * sagitta);

            // Calculate tire area of a tire at maximum deflection
            //bottomOutTireArea = width * tireArcLength;

            // Dimensions (for area calculation) of a single scan hit
            //hitWidth = wc.sideResolution < 1 ? width : width / wc.sideResolution;
            //hitHeight = wc.initialScanResolution < 1 ? tireRadius * 2.0f : (tireRadius * 2.0f) / wc.initialScanResolution;

            if (wc.useRimCollider)
            {
                // Instantiate rim (prevent ground passing through the side of the wheel)
                rim = new GameObject();
                rim.name = "RimCollider";
                rim.transform.position = worldPosition;
                rim.transform.rotation = worldRotation;
                rim.transform.parent = wc.transform;
                rim.layer = 2;

                if (rim.AddComponent<CapsuleCollider>())
                {
                    rimCollider = rim.GetComponent<CapsuleCollider>();
                    rimCollider.direction = 0;
                    rimCollider.radius = rimRadius;
                    rimCollider.height = width;
                    PhysicMaterial material = new PhysicMaterial();
                    material.dynamicFriction = 0.2f;
                    material.staticFriction = 0.2f;
                    material.bounciness = 0f;
                    rimCollider.material = material;
                }
            }
        }
    }


    /// <summary>
    /// Suspension part.
    /// </summary>
    [System.Serializable]
    private class Damper
    {
        public AnimationCurve dampingCurve;
        public float unitBumpForce = 1200.0f;
        public float unitReboundForce = 1500.0f;

        public float force;
        public float maxForce;
    }


    /// <summary>
    /// Suspension part.
    /// </summary>
    [System.Serializable]
    private class Spring
    {
        public float maxLength = 0.3f;
        public AnimationCurve forceCurve;
        public float maxForce = 32000.0f;

        public bool bottomOut;
        public float length;
        public float prevLength;
        public float availableLength;
        public float compressionPercent;
        public float force;
        public float velocity;
    }

    /// <summary>
    /// Contains RaycastHit and extended hit data.
    /// </summary>
    [System.Serializable]
    public class WheelHit
    {
        public RaycastHit raycastHit;
        public float angleForward;
        public float sideOffset;

        /// <summary>
        /// The point of contact between the wheel and the ground.
        /// </summary>
        public Vector3 point
        {
            get
            {
                return raycastHit.point;
            }
        }

        /// <summary>
        /// The normal at the point of contact 
        /// </summary>
        public Vector3 normal
        {
            get
            {
                return raycastHit.normal;
            }
        }

        /// <summary>
        /// The direction the wheel is pointing in.
        /// </summary>
        public Vector3 forwardDir;

        /// <summary>
        /// Tire slip in the rolling direction.
        /// </summary>
        public float forwardSlip;

        /// <summary>
        /// The sideways direction of the wheel.
        /// </summary>
        public Vector3 sidewaysDir;

        /// <summary>
        /// The slip in the sideways direction.
        /// </summary>
        public float sidewaysSlip;

        /// <summary>
        /// The magnitude of the force being applied for the contact. [N]
        /// </summary>
        public float force;

        // WheelCollider compatibility variables
        public Collider collider
        {
            get
            {
                return raycastHit.collider;
            }
        }
    }


    /// <summary>
    /// Used to store information about each ray.
    /// </summary>
    private class DebugLine
    {
        public DebugLine(Vector3 p, Vector3 d, float l)
        {
            point = p;
            direction = d;
            length = l;
        }

        public Vector3 point;
        public Vector3 direction;
        public float length;
    }


    /// <summary>
    /// Generate Curve from B,C,D and E parameters of Pacejka's simplified magic formula
    /// </summary>
    public AnimationCurve GenerateFrictionCurve(Vector4 p)
    {
        AnimationCurve ac = new AnimationCurve();
        Keyframe[] frames = new Keyframe[50];
        for (int i = 0; i < frames.Length; i++)
        {
            float t = i / 49.0f;
            float v = CalculateFriction(t, p);
            ac.AddKey(t, v);
        }
        return ac;
    }

    public string PrintFrictionCurve(AnimationCurve c)
    {
        String Keys = "";
        foreach (Keyframe k in c.keys)
        {
            Keys += "\nnew Keyframe(" + k.time + "f, " + k.value + "f, " + k.inTangent + "f, " + k.outTangent + "f),";
        }
        return (Keys);
    }

    private float CalculateFriction(float slip, Vector4 p)
    {
        var B = p.x;
        var C = p.y;
        var D = p.z;
        var E = p.w;
        var t = Mathf.Abs(slip);
        return D * Mathf.Sin(C * Mathf.Atan(B * t - E * (B * t - Mathf.Atan(B * t))));
    }

    public void InitIfNull()
    {
        // Objects
        if (wheel == null) wheel = new Wheel();
        if (hit == null) hit = new WheelHit();
        if (spring == null) spring = new Spring();
        if (damper == null) damper = new Damper();
        if (fFriction == null) fFriction = new Friction();
        if (sFriction == null) sFriction = new Friction();

        // Curves
        if (springCurve == null) springCurve = GenerateDefaultSpringCurve();
        if (damperCurve == null) damperCurve = GenerateDefaultDamperCurve();
        if (sideFriction.frictionCurve == null) sideFriction.frictionCurve = SetFrictionParams(sideFriction, new Vector4(10.0f, 1.9f, 1.0f, 0.97f));
        if (forwardFriction.frictionCurve == null) forwardFriction.frictionCurve = SetFrictionParams(forwardFriction, new Vector4(10.0f, 1.9f, 1.0f, 0.97f));
    }

    private AnimationCurve GenerateDefaultSpringCurve()
    {
        AnimationCurve ac = new AnimationCurve();
        ac.AddKey(0.0f, 0.0f);
        ac.AddKey(1.0f, 1.0f);
        return ac;
    }

    private AnimationCurve GenerateDefaultDamperCurve()
    {
        AnimationCurve ac = new AnimationCurve();
        ac.AddKey(0f, 0f);
        ac.AddKey(10f, 40f);
        return ac;
    }

}