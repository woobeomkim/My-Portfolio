using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    Collider2D col;
    Animator animator;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.i.isLive)
            return;
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.i.isLive)
            return;
        if (!isLive) return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.i.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health >0)
        {
            animator.SetTrigger("Hit");
            // .. Live , Hit Action
            AudioManager.i.PlaySfx(Sfx.Hit);
        }
        else
        {
            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            animator.SetBool("Dead", true);
            StartCoroutine(Dead());
            GameManager.i.kill++;
            GameManager.i.GetExp();

            if(GameManager.i.isLive)
                AudioManager.i.PlaySfx(Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.i.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        dirVec = dirVec.normalized;
        rigid.AddForce(dirVec * 3, ForceMode2D.Impulse);
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
