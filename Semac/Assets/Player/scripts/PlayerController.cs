using System.Collections;
using UnityEngine;

public class PlayerController : ICombateEntity
{
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask layerEnemy;
    private RaycastHit2D IsGround { get; set; }
    [SerializeField] private LayerMask layerGround;
    private readonly Vector3 offSetGroundRayCastLeft = new(-0.07f, -0.75f, 0);
    private readonly Vector3 offSetGroundRayCastRight = new(0.07f, -0.75f, 0);
    public const float rayCastGroundDistance = 0.25f;
    public float FixRayCastGroundDistance { get; set; } = 0;
    private void Awake()
    {
        CheckGround();
    }

    private void Update()
    {
        CheckGround();
        if (IsAnimatorName("death"))
        {
            if (AnimationNormalizedTime >= 1f && speed != 0)
            {
                speed = 0;
            }
            return;
        }
        if (IsHurting)
            return;
        speed = 0;
        if (IsAnimatorName("attack_base"))
            return;
        if (Input.GetKey(KeyCode.S))
            Crouch();
        else
            StandUp();
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            MoveLeft();
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            MoveRight();
        if (Input.GetMouseButtonDown(0))
            BaseAttack();
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (IsAnimatorName("crouch") && Input.GetKeyDown(KeyCode.Space))
            TurnOffPhysicsPlayerAndPlataform();
        Jumping();
        Falling();
        Idle();

    }
    private void FixedUpdate()
    {
        GetRB.velocity = new Vector2(speed * Time.deltaTime,
            GetRB.velocity.y > -15f ? GetRB.velocity.y : -15f);
    }
    private void CheckGround()
    {
        IsGround = Physics2D.Raycast(transform.position + offSetGroundRayCastLeft, Vector2.down, rayCastGroundDistance + FixRayCastGroundDistance, layerGround);
        Debug.DrawRay(transform.position + offSetGroundRayCastLeft, Vector2.down * (rayCastGroundDistance + FixRayCastGroundDistance), Color.red);
        if (!IsGround.collider)
        {
            IsGround = Physics2D.Raycast(transform.position + offSetGroundRayCastRight, Vector2.down, rayCastGroundDistance + FixRayCastGroundDistance, layerGround);
            Debug.DrawRay(transform.position + offSetGroundRayCastRight, Vector2.down * (rayCastGroundDistance + FixRayCastGroundDistance), Color.red);
        }
    }
    /*
     // Incluir apenas a camada 5
        layerGround = 1 << 5;
     // Incluir a camada 5 e camada 7
        layerGround = (1 << 5) | (1 << 7);
     */
    public void TurnOffPhysicsPlayerAndPlataform()
    {
        if (!Physics2D.GetIgnoreLayerCollision(6, 7))
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);
            layerGround = 1 << 9;
            StartCoroutine(TurnOnPhysicsPlayerAndPlataform());
        }
    }
    IEnumerator TurnOnPhysicsPlayerAndPlataform()
    {
        yield return new WaitForSeconds(0.2f);
        if (Physics2D.GetIgnoreLayerCollision(6, 7))
        {
            Physics2D.IgnoreLayerCollision(6, 7, false);
            layerGround = (1 << 7) | (1 << 9);
        }
    }
    public void BaseAttack()
    {
        if ((IsAnimatorName("idle") || IsAnimatorName("crouch") || IsAnimatorName("run")))
        {
            Trigger("attack_base");
        }
    }
    [SerializeField] private Vector2 AreaDamage;
    public void DamageBaseAttack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(FlipX ? GSXY + new Vector3(-0.7f, 0, 0)
            : new Vector2(0.7f + GSX, GSY), AreaDamage, 0f, layerEnemy);
        foreach (Collider2D target in targets)
        {
            target.gameObject.GetComponent<ICombateEntity>().Hurt(damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(FlipX ? GSXY + new Vector3(-0.7f, 0, 0) :
            new Vector2(0.7f + GSX, GSY), AreaDamage);
    }
    public void StandUp()
    {
        if (IsAnimatorName("crouch"))
        {
            Trigger("idle");
            FixRayCastGroundDistance = 0f;
        }
    }
    public void Crouch()
    {
        if (IsAnimatorName("idle") && !IsAnimatorName("run") && !IsAnimatorName("crouch"))
        {
            Trigger("crouch");
            FixRayCastGroundDistance = 0f;
        }
    }
    public void MoveLeft()
    {
        speed = -300;
        if (!FlipX)
            FlipX = true;
        if (!IsAnimatorName("run") && IsGround.collider && !IsAnimatorName("jumping"))
        {
            Trigger("run");
            GetRB.velocity = new Vector2(GetRB.velocity.x, 0);
            FixRayCastGroundDistance = 0f;
        }
    }
    public void MoveRight()
    {
        speed = 300;
        if (FlipX)
            FlipX = false;
        if (!IsAnimatorName("run") && IsGround.collider && !IsAnimatorName("jumping"))
        {
            Trigger("run");
            GetRB.velocity = new Vector2(GetRB.velocity.x, 0);
            FixRayCastGroundDistance = 0;
        }
    }
    public void Idle()
    {
        if (speed == 0 && !IsAnimatorName("idle") && IsGround.collider &&
            !IsAnimatorName("crouch") && !IsAnimatorName("jumping")||
            (IsAnimatorName("jumping") && GetRB.velocity.y < 4f) && IsGround.collider)
            {
            if (IsAnimatorName("falling"))
                FixRayCastGroundDistance = 0f;
            Trigger("idle");
        }
    }
    public void Jumping()
    {
        if (!IsGround.collider && !IsAnimatorName("jumping") &&
            !IsAnimatorName("falling") && GetRB.velocity.y > 4f)
        {
            Trigger("jumping");
        }
    }
    public void Jump()
    {
        if (IsGround && !IsAnimatorName("crouch") && !IsAnimatorName("jumping"))
        {
            GetRB.velocity = new Vector2(GetRB.velocity.x, 0);
            GetRB.AddForce(JumpForce * Vector2.up, ForceMode2D.Impulse);
            FixRayCastGroundDistance = -0.08f;
        }
    }
    public void Falling()
    {
        if (GetRB.velocity.y <= 0
            && !IsGround.collider
            && !IsAnimatorName("falling"))
        {
            Trigger("falling");
        }
    }
    public override void Hurt(int damage)
    {
        if (CanReceiveDamage)
        {
            life -= damage;
            speed = FlipX ? 100f : -100f;
            GetRB.velocity = Vector2.zero;
            GetRB.AddForce(new Vector2(speed, 75f), ForceMode2D.Impulse);
            CanReceiveDamage = false;
            StartCoroutine(FreezeReceiveDamage(1));
            if (life <= 0)
                Trigger("death");
            else
            {
                Trigger("hurt");
                IsHurting = true;
            }
        }
    }
    private IEnumerator FreezeReceiveDamage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CanReceiveDamage = true;
    }
    [SerializeField] private void AfterHurt() => IsHurting = false;
    private bool IsHurting { get; set; } = false;
}