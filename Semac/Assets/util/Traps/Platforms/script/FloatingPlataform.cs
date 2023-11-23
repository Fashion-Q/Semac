using UnityEngine;

public class PlataformaFlutuante : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float horizontalSide,speedX;
    [SerializeField] private uint sleepTime, maxCountTime;
    [SerializeField] private uint countSleepTime,countStopMaxCountTime;

    private void FixedUpdate()
    {
        
        if (countSleepTime > sleepTime)
        {
            GSX += horizontalSide * (speedX * Time.deltaTime);
            countStopMaxCountTime++;
            if(!IsAnimatorName("run"))
                anim.SetTrigger("run");
            if(countStopMaxCountTime > maxCountTime)
            {
                countSleepTime = 0;
                countStopMaxCountTime = 0;
                horizontalSide *= -1;
                spriteRenderer.flipX = horizontalSide == -1f;
                if(!IsAnimatorName("idle"))
                    anim.SetTrigger("idle");
            }
        }
        else
            countSleepTime++;
    }


    public bool IsAnimatorName(string name) => anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    public float GSX
    {
        get { return transform.position.x; }
        set { transform.position = new Vector2(value, GSY); }
    }
    public float GSY
    {
        get { return transform.position.y; }
        set { transform.position = new Vector2(GSX, value); }
    }
}
