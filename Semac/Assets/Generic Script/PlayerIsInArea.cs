using UnityEngine;

public class PlayerIsInArea : MonoBehaviour
{
    [SerializeField]
    private SimpleEnemy simpleEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            simpleEnemy.IsPlayerObserved = true;
            simpleEnemy.playerTransform = collision.transform;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            simpleEnemy.IsPlayerObserved = false;
        }
    }
}
