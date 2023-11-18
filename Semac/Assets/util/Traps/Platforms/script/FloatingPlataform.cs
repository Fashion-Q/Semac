using UnityEngine;

public class PlataformaFlutuante : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float horizontalSide,speedX;
    [SerializeField] private uint sleepTime, maxCountTime;
    [SerializeField] private uint countSleepTime,countStopMaxCountTime;

    private void FixedUpdate()
    {
        
        if (countSleepTime > sleepTime)
        {
            rb.velocity = new Vector2(horizontalSide * (speedX * Time.deltaTime), rb.velocity.y);
            countStopMaxCountTime++;
            if(!IsAnimatorName("run"))
                anim.SetTrigger("run");
            if(countStopMaxCountTime > maxCountTime)
            {
                countSleepTime = 0;
                countStopMaxCountTime = 0;
                horizontalSide *= -1;
                spriteRenderer.flipX = horizontalSide == -1f;
                rb.velocity = Vector2.zero;
                if(!IsAnimatorName("idle"))
                    anim.SetTrigger("idle");
            }
        }
        else
            countSleepTime++;
    }


    public bool IsAnimatorName(string name) => anim.GetCurrentAnimatorStateInfo(0).IsName(name);
}
