using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class LocomotionController : MonoBehaviour, IInputReceiver, IObserver<ThrustController.ThrustPower>
{
    [SerializeField] private Vector2Input movementInput;
    [SerializeField] private float movementForce;
    [SerializeField] private float movementSpeed;
    
    private Vector3 rawInput;
    private bool canMove = true;

    private Rigidbody body;
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
    }
    
    private void OnMovementInputChange(Vector2 input)
    {
        rawInput = new Vector3(input.x, 0, input.y);
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
        if (!canMove)
            return;
        if (Vector3.ProjectOnPlane(body.velocity, Vector3.up).sqrMagnitude > movementSpeed * movementSpeed)
            return;
        body.AddForce(transform.TransformDirection(rawInput) * movementForce, ForceMode.Acceleration);
    }
    #endregion

    public void OnNotify(ThrustController.ThrustPower argument)
    {
        canMove = argument.Thrust == 0;
    }
}