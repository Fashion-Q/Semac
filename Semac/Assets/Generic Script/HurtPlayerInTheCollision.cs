using UnityEngine;

public class HurtPlayerInTheCollision : MonoBehaviour
{
    [SerializeField]
    private ICombateEntity entity;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
             collision.gameObject.GetComponent<ICombateEntity>().Hurt(entity.damage);
    }
}
