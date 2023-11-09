using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : ICombateEntity
{
    public bool IsJumping  = false;
    [SerializeField]
    private LayerMask enemy;

    private void Update()
    {
        if(!IsAnimatorName("attack_base") && !IsAnimatorName("hurt") &&!IsAnimatorName("death"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            if (Input.GetKey(KeyCode.S))
                Crouch();
            if (IsAnimatorName("crouch") && Input.GetKeyDown(KeyCode.Space))
                TurnOffPhysicsPlayerAndPlataform();
            if (Input.GetMouseButtonDown(0))
                BaseAttack();
            if (Input.GetKeyUp(KeyCode.S))
                StandUp();
        }
    }
    private void FixedUpdate()
    {
        if (IsAnimatorName("death"))
            return;
        if(!IsAnimatorName("hurt"))
            speed = 0;
        if (!IsAnimatorName("attack_base") && !IsAnimatorName("hurt"))
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                MoveLeft();
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                MoveRight();
        }
            Falling();
            GetRB.velocity = new Vector2(speed * Time.deltaTime, 
                GetRB.velocity.y > -15f ? GetRB.velocity.y : -15f);
            Others();
    }
    public void TurnOffPhysicsPlayerAndPlataform()
    {
        if(!Physics2D.GetIgnoreLayerCollision(6, 7))
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);
            StartCoroutine(TurnOnPhysicsPlayerAndPlataform());
        }
    }
     IEnumerator TurnOnPhysicsPlayerAndPlataform()
    {
        yield return new WaitForSeconds(0.15f);
        if(Physics2D.GetIgnoreLayerCollision(6, 7))
        {
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }
    }
    public void BaseAttack()
    {
        if((IsAnimatorName("idle") || IsAnimatorName("crouch") || IsAnimatorName("run")))
        {
            Trigger("attack_base");
        }
    }
    public void DamageBaseAttack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(FlipX ? GetSetXY + new Vector3(-0.7f, 0, 0)
            : new Vector2(0.7f + GetSetX,GetSetY), new Vector2(1.3f, 1.7f), 0f, enemy);
        foreach (Collider2D target in targets)
        {
            target.gameObject.GetComponent<ICombateEntity>().Hurt(damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(FlipX ?  GetSetXY + new Vector3(-0.7f, 0,0) :
            new Vector2(0.7f + GetSetX, GetSetY), new Vector2(1.3f, 1.7f));
    }
    public void StandUp()
    {
        if(IsAnimatorName("crouch"))
        {
            Trigger("idle");
            IsJumping = false;
        }
    }
    public void Crouch()
    {
        if(IsAnimatorName("idle") && !IsAnimatorName("run") && !IsAnimatorName("crouch"))
            Trigger("crouch");
    }
    public void MoveLeft()
    {
        speed = -300;
        if(!FlipX)
            FlipX = true;
        if (!IsAnimatorName("run") && !IsJumping)
        {
            Trigger("run");
            GetRB.velocity = new Vector2(GetRB.velocity.x, 0);
        }
    }
    public void MoveRight()
    {
        speed = 300;
        if (FlipX)
            FlipX = false;
        if (!IsAnimatorName("run") && !IsJumping)
        {
            Trigger("run");
            GetRB.velocity = new Vector2(GetRB.velocity.x, 0);
        }
    }
    public void Others()
    {
        if (speed == 0 && !IsAnimatorName("idle") && !IsJumping && 
            !IsAnimatorName("crouch") && !IsAnimatorName("attack_base"))
        {
            Trigger("idle");
            IsJumping = false;
        }
    }
    public void Jump()
    {
        if (!IsJumping && !IsAnimatorName("crouch"))
        {
            Trigger("jumping");
            GetRB.velocity = new Vector2(GetRB.velocity.x,0);
            GetRB.AddForce(200 * Vector2.up,ForceMode2D.Impulse);
            IsJumping = true;
        }
    }
    public void Falling()
    {
        if (GetRB.velocity.y <= 0 && IsJumping && IsAnimatorName("jumping"))
            Trigger("falling");
    }
    public override void Hurt(int damage)
    {
        if(CanReceiveDamage && life > 0)
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
                Trigger("hurt");
        }
    }
    private IEnumerator FreezeReceiveDamage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CanReceiveDamage = true;
    }
    public void AfterHurt()
    {
        Trigger("falling");
        IsJumping = true;
    }
    public void AfterDeath()
    {
        GetRB.velocity = new Vector2(0,0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor") && IsJumping && IsAnimatorName("falling"))
        {
            IsJumping = false;
            Trigger("idle");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("floor") && !IsJumping && !IsAnimatorName("falling"))
        {
            IsJumping = true;
            Trigger("falling");
        }
    }
    public void JumpButton()
    {
        Jump();
    }
}
