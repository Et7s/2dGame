using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyidti : MonoBehaviour
{
    public Transform[] targets; // Массив с таргетами, к которым будет двигаться враг
    public float speed = 3f; // Скорость движения врага
    public float attackRange = 1f; // Расстояние для атаки
    public float detectionRange = 5f; // Расстояние обнаружения игрока
    public float attackDelay = 2f; // Задержка между атаками
    public int maxHealth = 100;
    int currentHealth;
    public Transform currentTarget; // Текущий таргет, к которому движется враг
    private Animator animator; // Компонент аниматора
    private bool isAttacking = false; // Флаг для определения, идет ли атака
    public int Damage = 1;

    void Start()
    {
        currentTarget = targets[0]; // Начинаем с первого таргета
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeTargetWithDelay());
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAttacking)
        {
            MoveToTarget();
            
        }

        CheckForPlayer();
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    void CheckForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            currentTarget = playerCollider.transform;
            if (Vector2.Distance(transform.position, playerCollider.transform.position) <= attackRange)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay); // Задержка между атаками

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
        if (playerCollider != null)
        {
            // Нанесение урона игроку
            playerCollider.GetComponent<PlayerMovement>().TakeDamage(Damage);
            
        }

        isAttacking = false;
    }
    void OnDrawGizmosSelected()
    {
        if (transform.position == null)
            return;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    IEnumerator ChangeTargetWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f)); // Задержка перед сменой таргета
            int nextTargetIndex = Random.Range(0, targets.Length);
            currentTarget = targets[nextTargetIndex];
            yield return new WaitForSeconds(3f); // Задержка перед возвратом к первому таргету
            currentTarget = targets[0];
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }
}
