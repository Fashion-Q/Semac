using System;
using UnityEngine;

public class Bat : SimpleEnemy
{
    [SerializeField]
    private BoxCollider2D childBoxColider;
    [SerializeField]
    private CircleCollider2D childCircleColider;
    private Vector2 firstPosition;
    [SerializeField]
    private float radiusCircleDetect;
    private float DirectionX { get; set; } = 0f;
    private float DirectionY { get; set; } = 0f;
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
                    DirectionX = GetPX < GetSetX ? -1f : 1f;
                    FlipX = DirectionX < 0 ? true : false;
                    FixVariablesBackToPosition = true;
                    childCircleColider.radius = 12f;
                }
                if(GetSetX < MaxDirLeft)
                {
                    DirectionX = 1;
                    FlipX = false;
                }
                if(GetSetX > MaxDirRight)
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
                    BackLeftX = GetSetX > firstPosition.x ? true: false;
                    BackUpY = GetSetY < firstPosition.y ? true: false;
                    GetRB.bodyType= RigidbodyType2D.Kinematic;
                    FixVariablesBackToPosition = false;
                    BackToFirstPosition = true;
                    GetRB.velocity = new Vector2(0, 0);
                    transform.Find("DetectEnemyPlayerArea").GetComponent<CircleCollider2D>().enabled = false;
                    if (!BackLeftX)
                        FlipX=false;
                    else
                        FlipX = true;
                }
                if(BackToFirstPosition)
                {
                    if(BackLeftX && GetSetX > firstPosition.x)
                    {
                        GetSetX -= (speed / 100) * Time.deltaTime;
                        if(GetSetX < firstPosition.x)
                            GetSetX = firstPosition.x;
                    }
                    if (!BackLeftX && GetSetX < firstPosition.x)
                    {
                        GetSetX += (speed / 100) * Time.deltaTime;
                        if (GetSetX > firstPosition.x)
                            GetSetX = firstPosition.x;
                    }
                    if(BackUpY && GetSetY < firstPosition.y)
                    {
                        GetSetY += (speed / 100) * Time.deltaTime;
                        if(GetSetY > firstPosition.y)
                            GetSetY = firstPosition.y;
                    }
                    if (!BackUpY && GetSetY > firstPosition.y)
                    {
                        GetSetY -= (speed / 100) * Time.deltaTime;
                        if (GetSetY < firstPosition.y)
                            GetSetY = firstPosition.y;
                    }

                    if (GetSetX == firstPosition.x && GetSetY == firstPosition.y)
                    {
                        Trigger("idle");
                        BackToFirstPosition = false;
                        transform.Find("DetectEnemyPlayerArea").GetComponent<CircleCollider2D>().radius = radiusCircleDetect;
                        transform.Find("DetectEnemyPlayerArea").GetComponent<CircleCollider2D>().enabled = true;
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
                GetRB.velocity = new Vector2(0, 0);
                childBoxColider.enabled = false;
            }
        }
    }
}
