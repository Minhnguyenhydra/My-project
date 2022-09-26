using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float hp;
    private string currentAnimName;// check ten anim


    public bool IsDead => hp <= 0; //cho? ve, gan giong return;

    private void Start()
    {
        OnInit();
    }


    public virtual void OnInit()// ham khoi tao => chu dong goi no bat ky luc nao
    {
        hp = 100;
    }
    public virtual void OnDespawn()// ham huy , ke thua sau;
    {

    }
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }
    protected void ChangeAnim(string animName)// protected => sap xep cac ham ............ hoi lai thay giao
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damage)
    {
        if(hp >= damage)
        {
            hp -= damage;
            if(hp <= damage)
            {
                OnDeath();
            }
        }
    }
      
    
   
}
