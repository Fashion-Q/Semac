using System.Collections;
using UnityEngine;

public class Player : ICharacter
{
    private bool IsJumping { get; set; }
    private const float impulso = 200;
    public void Update()
    {
        Jump();
    }
    public void FixedUpdate()
    {
        Move();
    }

    public float habilitarTempoPlataforma = 0.5f;
    private bool poderDesabilitarPlataforma = true;
    IEnumerator HabilitarLayerPlataforma()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        poderDesabilitarPlataforma = false;
         yield return new WaitForSeconds(habilitarTempoPlataforma);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        poderDesabilitarPlataforma = true;
    }

    public void Jump()
    {
        if(Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && !IsJumping)
        {
            if (poderDesabilitarPlataforma)
            {
                StartCoroutine(HabilitarLayerPlataforma());
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !IsJumping)
        {
            IsJumping = true;
            GetRB.velocity = new Vector2(GetRB.velocity.x,0);
            GetRB.AddForce(Vector2.up * impulso,ForceMode2D.Impulse);
            if (!AnimatorIsName("jumping"))
                Trigger("jumping");
        }
        if (IsJumping && GetRB.velocity.y <= 0f && !AnimatorIsName("falling"))
        {
            Trigger("falling");
        }
    }

    private void Move()
    {
        SpeedX = 0;
        if(Input.GetKey(KeyCode.A)  && !Input.GetKey(KeyCode.D))
        {
            SpeedX = -300;
            if(!AnimatorIsName("run") && !IsJumping)
                Trigger("run");
            if (!FlipX)
                FlipX = true;
        }
        if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            SpeedX = 300;
            if (!AnimatorIsName("run") && !IsJumping)
                Trigger("run");
            if (FlipX)
                FlipX = false;
        }
        if (SpeedX == 0 && !IsJumping && !AnimatorIsName("idle"))
        {
            Trigger("idle");
        }
        GetRB.velocity = new Vector2(SpeedX * Time.deltaTime, GetRB.velocity.y > -15f ? GetRB.velocity.y : -15f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(IsJumping && collision.gameObject.CompareTag("floor") && AnimatorIsName("falling"))
        {
            IsJumping = false;
            Trigger("idle");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsJumping && collision.gameObject.CompareTag("floor") && !AnimatorIsName("falling"))
        {
            IsJumping = true;
            Trigger("falling");
        }
    }
}
