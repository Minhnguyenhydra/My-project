using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public Rigidbody2D rb;
    void Start()
    {
        OnInit();
    }

   public void OnInit()
    {
        rb.velocity = transform.right * 5f;
        Invoke(nameof(OnDespawn), 2f);
    }

   

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.gameObject.name);
        if (collision.tag == "Enemy")
            
        {
            collision.GetComponent<Character>().OnHit(30);
            OnDespawn();
        }
    }
}
