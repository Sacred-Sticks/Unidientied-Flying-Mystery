using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class ThrustController : Observable, IInputReceiver
{
    [SerializeField] private FloatInput leftThrustInput;
    [SerializeField] private FloatInput rightThrustInput;
    [SerializeField] private FloatInput altitudeInput;
    [SerializeField] private float thrustForce;
    [SerializeField] private float torqueForce;
    [SerializeField] private float altitudeForce;

    private float rawLeftThrustInput;
    private float rawRightThrustInput;
    private float rawAltitudeInput;

    private Rigidbody body;
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        leftThrustInput.RegisterInput(OnLeftThrusterInputChange, playerIdentifier);
        rightThrustInput.RegisterInput(OnRightThrusterInputChange, playerIdentifier);
        altitudeInput.RegisterInput(OnAltitudeInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        leftThrustInput.DeregisterInput(OnLeftThrusterInputChange, playerIdentifier);
        rightThrustInput.DeregisterInput(OnRightThrusterInputChange, playerIdentifier);
        altitudeInput.DeregisterInput(OnAltitudeInputChange, playerIdentifier);
    }

    private void OnLeftThrusterInputChange(float input)
    {
        rawLeftThrustInput = input;
    }

    private void OnRightThrusterInputChange(float input)
    {
        rawRightThrustInput = input;
    }

    private void OnAltitudeInputChange(float input)
    {
        rawAltitudeInput = input;
    }
    #endregion
    
    #region UnityEvents
    private void Awake()
    {
        body = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AdjustAltitude();
        VelocityHandler();
        RotationHandler();
    }
    #endregion

    private void AdjustAltitude()
    {
        body.AddForce(Vector3.up * (rawAltitudeInput * altitudeForce), ForceMode.Acceleration);
    }

    private void VelocityHandler()
    {
        if (rawLeftThrustInput == 0 && rawRightThrustInput == 0) 
            return;
        body.AddForce(transform.forward * thrustForce, ForceMode.Acceleration);
    }

    private void RotationHandler()
    {
        float direction = rawRightThrustInput - rawLeftThrustInput;
        if (direction == 0) 
            return;
        body.AddTorque(0, direction * torqueForce, 0, ForceMode.Acceleration);
    }
}
