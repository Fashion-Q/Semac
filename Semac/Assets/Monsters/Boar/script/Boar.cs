using UnityEngine;

public class Boar : ICombateEntity
{
    [SerializeField] private BoxCollider2D childBoxColider;

    [SerializeField] private float horizontalSide;
    [SerializeField] private uint sleepTime, maxCountTime;
    [SerializeField] private uint countSleepTime, countStopMaxCountTime;
    [SerializeField] private bool IsWalking;

    private void FixedUpdate()
    {
        if(CanReceiveDamage)
        {
            if (IsAnimatorName("hurt"))
            {
                GetRB.velocity = new Vector2(0, 0);
                return;
            }
            if (countSleepTime > sleepTime)
            {
                GetRB.velocity = new Vector2(horizontalSide * (speed * Time.deltaTime), GetRB.velocity.y);
                countStopMaxCountTime++;
                if (!IsAnimatorName(IsWalking ? "walk" : "run"))
                    Trigger(IsWalking ? "walk" : "run");
                FlipX = horizontalSide == 1f;
                if (countStopMaxCountTime > maxCountTime)
                {
                    countSleepTime = 0;
                    countStopMaxCountTime = 0;
                    horizontalSide *= -1;
                    GetRB.velocity = Vector2.zero;
                    if (!IsAnimatorName("idle"))
                        Trigger("idle");
                }
            }
            else
                countSleepTime++;
        }
    }




    public override void Hurt(int damage)
    {
        if (CanReceiveDamage)
        {
            life -= damage;
            CanReceiveDamage = false;
            if (life <= 0)
            {
                Trigger("death");
                GetAudioSource.Play();
                GetRB.velocity = new Vector2(0, 0);
                childBoxColider.enabled = false;
            }
            else
            {
                Trigger("hurt");
                GetRB.velocity = new Vector2(0, 0);
                childBoxColider.enabled = false;
            }
        }
    }
    public void AfterHurt()
    {
        childBoxColider.enabled = true;
        CanReceiveDamage = true;
    }
    public void AfterDeath()
    {
        Destroy(gameObject);
    }
}
