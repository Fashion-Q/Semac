using UnityEngine;

public abstract class ICombateEntity : MonoBehaviour
{
    public float speed;
    public int life,damage;
    public bool CanReceiveDamage { get; set; } = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private AudioSource audioSource;

    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
        set { spriteRenderer.flipX = value; }
    }
    public CapsuleCollider2D GetCapsula => capsuleCollider;
    public AudioSource GetAudioSource => audioSource;
    public Rigidbody2D GetRB  => rb;
    public bool IsAnimatorName(string name) => animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    public void Trigger(string name) => animator.SetTrigger(name);
    public float AnimationNormalizedTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    public float GSX
    {
        get { return transform.position.x; }
        set { transform.position = new Vector3(value, GSY, 0f); }
    }
    public float GSY
    {
        get { return transform.position.y; }
        set { transform.position = new Vector3(GSX, value, 0f); }
    }
    public Vector3 GSXY
    {
        get { return transform.position; }
    }
    public abstract void Hurt(int damage);
}