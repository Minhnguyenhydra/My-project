using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private IState currentState;
    [SerializeField] private GameObject attackArena;



    private bool isRight = true; // bien sang phai


    private Character target;
    public Character Target => target;


    private void Update()
    {
        if (currentState !=null)
        {
            currentState.OnExecute(this);
        }
    }

    

    public override void OnInit() // phuong thuc ghi de Onhit
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);

    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    

    public void ChangeState(IState newState)
    {
        // khi ma doi sang 1 state moi thi check state cu co = null hay khong?
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;// gan newState moi vao

        if (currentState != null)// neu currentState = null thi se truy vao state moi
        {
            currentState.OnEnter(this);
        }
    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if(Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }

        
      
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);

    }

    public bool IsTargetInRange() // kiem tra xem co trong vung danh hay ko
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // khi
    {
        if(collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }

    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
        //
    }
    private void ActiveAttack()// khi attack se hien ra attackArena khien Enemy mat mau
    {
        attackArena.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArena.SetActive(false);

    }
}


