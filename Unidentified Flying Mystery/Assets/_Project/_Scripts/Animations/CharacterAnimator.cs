using Kickstarter.Observer;using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour, IObserver<ThrustController.ThrustPower>
{
    private Animator animator;   
    private readonly int Flying = Animator.StringToHash(nameof(Flying));

    private void Awake()
    {
        animator = GetComponent<Animator>();
        transform.root.GetComponentInChildren<ThrustController>().AddObserver(this);
    }

    public void OnNotify(ThrustController.ThrustPower argument)
    {
        animator.SetFloat(Flying, argument.Thrust);
    }
}
