using UnityEngine;

public class DetectEnemyPlayerCollision : MonoBehaviour
{
    [SerializeField]
    private SimpleEnemy simpleEnemy;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
             collision.gameObject.GetComponent<ICombateEntity>().Hurt(simpleEnemy.damage);
    }
}
