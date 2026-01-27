using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ToggleAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string animationName;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationName, 0, 1);
    }
    public void SetAnimation(string animationName) => this.animationName = animationName;
    public void PlayAnimation()
    {
        animator.SetFloat("Speed", -animator.GetFloat("Speed"));
        animator.Play(animationName, 0, Mathf.Clamp(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0, 1));
    }
}
