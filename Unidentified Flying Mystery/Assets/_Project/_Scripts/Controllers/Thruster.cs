using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class Thruster : Observable, IInputReceiver, IThruster
{
    [SerializeField] private FloatInput thrusterInput;
    [SerializeField] private float powerMultiplier;
    [SerializeField] private Vector3 forcePlacement;
    [SerializeField, Range(-1, 1),] private int rotationDirection;

    private float rawInput;
    public float ThrusterPower { get; private set; }
    
    // Cached References and Constant Values
    private Rigidbody body;
    private const float minThrust = 0;
    private const float maxThrust = 1;
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        thrusterInput.RegisterInput(OnThrusterInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        thrusterInput.DeregisterInput(OnThrusterInputChange, playerIdentifier);
    }

    private void OnThrusterInputChange(float input)
    {
        rawInput = input;
    }
    #endregion
    
    #region UnityEvents
    private void Awake()
    {
        body = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    { 
        AddThrusterPower();
    }
    #endregion

    private void AddThrusterPower()
    {
        float power = rawInput * powerMultiplier;
        if (rawInput == 0 && ThrusterPower > 0)
            power = -ThrusterPower;
        ThrusterPower += power;
        ThrusterPower = Mathf.Clamp(ThrusterPower, minThrust, maxThrust);
        body.AddForce(power * transform.forward, ForceMode.VelocityChange);
        body.AddTorque(body.transform.TransformDirection(forcePlacement) * (ThrusterPower * rotationDirection), ForceMode.Force);
        
        NotifyObservers(rawInput == 0 ? Thrust.Inactive : Thrust.Active);
    }

    public enum Thrust
    {
        Active,
        Inactive,
    }
}

public interface IThruster
{
    public float ThrusterPower { get; }
}