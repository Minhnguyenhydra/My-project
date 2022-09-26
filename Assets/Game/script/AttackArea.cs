using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")// neu va cham Player hoac Enemy thi se giam damge
        {
            collision.GetComponent<Character>().OnHit(30f);
        }
    }
}
