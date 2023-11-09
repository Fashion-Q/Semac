using UnityEngine;

public abstract class ICombateEntity : MonoBehaviour
{
    public float speed;
    public int life,damage;
    public bool CanReceiveDamage { get; set; } = true;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private CapsuleCollider2D capsuleCollider;

    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
        set { spriteRenderer.flipX = value; }
    }
    public CapsuleCollider2D GetCapsula => capsuleCollider;
    public Rigidbody2D GetRB  => rb;
    public bool IsAnimatorName(string name) => animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    public void Trigger(string name) => animator.SetTrigger(name);
    public float AnimationNormalizedTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    public float GetSetX
    {
        get { return transform.position.x; }
        set { transform.position = new Vector3(value, GetSetY, 0f); }
    }
    public float GetSetY
    {
        get { return transform.position.y; }
        set { transform.position = new Vector3(GetSetX, value, 0f); }
    }
    public Vector3 GetSetXY
    {
        get { return transform.position; }
    }
    public abstract void Hurt(int damage);
}