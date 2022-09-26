using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character// ke thua Character
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private LayerMask groundLayer; //phan lop tuong tac voi raycast
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArena;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;

    private float horizontal;//bien di chuyen ngang

    

    private int coin = 0;

    private Vector3 savePoint; //tao ra bien xac dinh vi tri sau khi chet

    // Update is called once per frame
    void Update()

    {


        
        if (isDeath)
        {
            return;
        }
        isGrounded = CheckGrounded();

        //-1 -> 0 -> 1
        horizontal = Input.GetAxisRaw("Horizontal");
        //verticle = Input.GetAxisRaw("Vertical");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }


        //Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            //horizontal > 0 -> tra ve 0, neu horizontal <= 0 -> tra ve la 180
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0)); // lat xoay nv
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        //idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    public override void OnInit()//duoc goi sau khi object duoc khoi tao len, dua ve trang thai dau tien => su dung cho "Invoke"
    {
        base.OnInit();
        isDeath = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();

    }
    protected override void OnDeath()// hoi ve protected ?????
    {
        base.OnDeath();

    }


    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer); // ban 1 tia raycast co cham vao box collider cua ground hay khong
        //nameof =>tra ve ten cua bat ky cai gi duoi dang string

        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);



    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void Instantiate()
    {
        
    }

    private void ResetAttack()
    {
        ChangeAnim("ilde");//idle
        isAttack = false;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    
    internal void SavePoint()// truy cap vao SavePoint => khi chet se di chuyen ve vi tri cua savePoint
    {
        savePoint = transform.position;


    }

    private void ActiveAttack()// khi attack se hien ra attackArena khien Enemy mat mau
    {
        attackArena.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArena.SetActive(false);

    }


    private void OnTriggerEnter2D(Collider2D collision)// ontrigger enter va ontrigger exit => va cham voi vat the nhung co the xuyen qua
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);// cham vao coin se bien mat ( huy doi tuong)
        }

        if (collision.tag == "DeathZone")// collision: su va cham
        {
            isDeath = true;

            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
    }
}
