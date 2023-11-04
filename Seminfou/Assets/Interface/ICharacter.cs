using UnityEngine;

public abstract class ICharacter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D GetRB => rb;
    public bool AnimatorIsName(string name) => animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    public float CurrentAnimationTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    public void Trigger(string str) => animator.SetTrigger(str);
    public void BoolAnimation(string str, bool boolAnimation) => animator.SetBool(str,boolAnimation);
    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
        set { spriteRenderer.flipX = value;}
    }
    public float SpeedX { get; set; }
}
