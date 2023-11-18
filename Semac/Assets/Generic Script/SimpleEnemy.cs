using UnityEngine;

public abstract class SimpleEnemy : ICombateEntity
{
    public bool IsPlayerObserved { get; set; } = false;
    public Transform playerTransform;

    public float GetPX { get { return playerTransform.position.x; } }
    public float GetPY { get { return playerTransform.position.y; } }
}