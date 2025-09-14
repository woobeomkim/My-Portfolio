using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); 
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.i.playerId];
    }

    private void OnMove(InputValue value)
    {
        if (!GameManager.i.isLive)
            return;
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        //// 1. 힘을 준다
        //rigid.AddForce(inputVec);

        //// 2. 속도 제어
        //rigid.velocity = inputVec;

        // 3. 위치 이동

        if (!GameManager.i.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = (inputVec.x < 0) ? true : false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.i.isLive)
            return;

        GameManager.i.health -= Time.deltaTime * 10;

        if (GameManager.i.health <=0)
        {
            for (int i = 2; i < transform.childCount; i++) 
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.i.GameOver();
        }
    }
}
