using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastDistance;
    public float attackDistance;
    public float moveSpeed;
    public float timer;

    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance;
    private bool animState;
    private bool inRange;
    private bool cooling;
    private float intTimer;

    [SerializeField] public Collider2D hitbox;


    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastDistance, rayCastMask);
            RayCastDebugger();
        }

        if (hit.collider != null)
        {
            EnemyLogic();
        }

        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            animator.SetInteger("AnimState", 0);
            StopAttack();
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            animator.SetBool("Attack", false);
        }
    }

    private void Attack()
    {
        timer = intTimer;

        animator.SetInteger("AnimState", 1);
        animator.SetBool("Attack", true);
        
    }

    private void StopAttack()
    {
        cooling = false;

        animator.SetInteger("AnimState", 1);
        animator.SetBool("Attack", false);
    }

    private void Move()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyBandit_Attack"))
        {
            animator.SetInteger("AnimState", 2);
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
        }
    }

    private void RayCastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastDistance, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastDistance, Color.green);
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            inRange = true;
            animator.SetInteger("AnimState", 1);
        }
    }
}
