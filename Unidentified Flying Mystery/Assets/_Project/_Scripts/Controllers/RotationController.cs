using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class RotationController : MonoBehaviour, IInputReceiver, IObserver<ThrustController.ThrustPower>
{
    [SerializeField] private Vector2Input rotationInput;
    [SerializeField] private float torqueForce;

    private Vector2 rawInput;
    private bool canRotate = true;

    private Rigidbody body;
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        rotationInput.RegisterInput(OnRotationInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        rotationInput.DeregisterInput(OnRotationInputChange, playerIdentifier);
    }

    private void OnRotationInputChange(Vector2 input)
    {
        rawInput = input;
    }
    #endregion
    
    #region UnityEvents
    private void Awake()
    {
        body = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        transform.root.GetComponentInChildren<ThrustController>().AddObserver(this);
    }

    private void FixedUpdate()
    {
        if (!canRotate)
            return;
        body.AddTorque(0, rawInput.x * torqueForce, 0, ForceMode.Acceleration);
    }
    #endregion

    public void OnNotify(ThrustController.ThrustPower argument)
    {
        canRotate = argument.Thrust == 0;
    }
}
