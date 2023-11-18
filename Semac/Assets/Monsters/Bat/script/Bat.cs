using UnityEngine;

public class Bat : SimpleEnemy
{
    [SerializeField] private BoxCollider2D childBoxColider;
    [SerializeField] private CircleCollider2D childCircleColider;
    private Vector2 firstPosition;
    [SerializeField]
    private float radiusCircleDetect;
    private float DirectionX { get; set; } = 0f;
    public float MaxDirLeft, MaxDirRight;
    private bool BackToFirstPosition { get; set; } = false;
    private bool BackLeftX { get; set; }
    private bool BackUpY { get; set; }
    private bool FixVariablesBackToPosition { get; set; } = false;

    private void Awake()
    {
        firstPosition = transform.position;
        childCircleColider.radius = radiusCircleDetect;
    }

    private void FixedUpdate()
    {
        if(CanReceiveDamage)
        {
            if(IsPlayerObserved && !BackToFirstPosition)
            {
                if(GetRB.bodyType == RigidbodyType2D.Kinematic)
                {
                    GetRB.bodyType = RigidbodyType2D.Dynamic;
                    Trigger("run");
                    DirectionX = GetPX < GSX ? -1f : 1f;
                    FlipX = DirectionX < 0 ? true : false;
                    FixVariablesBackToPosition = true;
                    childCircleColider.radius = 12f;
                }
                if(GSX < MaxDirLeft)
                {
                    DirectionX = 1;
                    FlipX = false;
                }
                if(GSX > MaxDirRight)
                {
                    DirectionX = -1;
                    FlipX = true;
                }
                GetRB.velocity = new Vector2((speed * DirectionX) * Time.deltaTime, GetRB.velocity.y > -15f ? GetRB.velocity.y : -15f);
            }
            else
            {
                if(FixVariablesBackToPosition)
                {
                    BackLeftX = GSX > firstPosition.x;
                    BackUpY = GSY < firstPosition.y;
                    GetRB.bodyType= RigidbodyType2D.Kinematic;
                    FixVariablesBackToPosition = false;
                    BackToFirstPosition = true;
                    GetRB.velocity = new Vector2(0, 0);
                    transform.Find("PlayerIsInArea").GetComponent<CircleCollider2D>().enabled = false;
                    if (!BackLeftX)
                        FlipX=false;
                    else
                        FlipX = true;
                }
                if(BackToFirstPosition)
                {
                    if(BackLeftX && GSX > firstPosition.x)
                    {
                        GSX -= (speed / 100) * Time.deltaTime;
                        if(GSX < firstPosition.x)
                            GSX = firstPosition.x;
                    }
                    if (!BackLeftX && GSX < firstPosition.x)
                    {
                        GSX += (speed / 100) * Time.deltaTime;
                        if (GSX > firstPosition.x)
                            GSX = firstPosition.x;
                    }
                    if(BackUpY && GSY < firstPosition.y)
                    {
                        GSY += (speed / 100) * Time.deltaTime;
                        if(GSY > firstPosition.y)
                            GSY = firstPosition.y;
                    }
                    if (!BackUpY && GSY > firstPosition.y)
                    {
                        GSY -= (speed / 100) * Time.deltaTime;
                        if (GSY < firstPosition.y)
                            GSY = firstPosition.y;
                    }

                    if (GSX == firstPosition.x && GSY == firstPosition.y)
                    {
                        Trigger("idle");
                        BackToFirstPosition = false;
                        transform.Find("PlayerIsInArea").GetComponent<CircleCollider2D>().radius = radiusCircleDetect;
                        transform.Find("PlayerIsInArea").GetComponent<CircleCollider2D>().enabled = true;
                    }
                }
            }
        }
    }
    public void AfterDeath()
    {
        Destroy(gameObject);
    }
    public override void Hurt(int damage)
    {
        if(CanReceiveDamage)
        {
            life -= damage;
            if(life <= 0 )
            {
                CanReceiveDamage = false;
                Trigger("death");
                GetAudioSource.Play();
                GetRB.velocity = new Vector2(0, 0);
                childBoxColider.enabled = false;
            }
        }
    }
}
