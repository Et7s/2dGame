using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float checkRadius = 0.5f;
    public float dashingPower = 8f;
    public float dashingTime = 0.5f;
    public float dashingCooldown = 1f;
    public float AttackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int maxHealth = 100;
    int currentHP;
    public GameObject smert;

    public bool onGround;
    private bool m_FacingRight = true;
    private bool canDash = true;
    public bool isDashing;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform GroundCheck;
    public Transform AttackPoint;

    Vector2 movement;
    public LayerMask Ground;
    public LayerMask enemyLayers;

    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        currentHP = maxHealth;
        smert.SetActive(false);
    }
    private void Update()

    {
        if (isDashing)
        {
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        rb.velocity = new Vector2 (movement.x * moveSpeed, rb.velocity.y);
        Jump();
        CheckingGround();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (movement.x > 0 && !m_FacingRight)
        {
            
            Flip();
        }
        else if (movement.x < 0 && m_FacingRight)
        {
            
            Flip();
        }
        if (isDashing == true)
        {
            animator.SetBool("IsDash", true);
        }
        else 
        {
            animator.SetBool("IsDash", false);
        }
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
            
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }
    }
    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position,checkRadius,Ground);
        if (onGround == false)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("IsAttacking", true);
                animator.SetBool("Isjump", false);
            }
            else
            {
                animator.SetBool("Isjump", true);
                animator.SetBool("IsAttacking", false);
            }
        }
        else
        {
            animator.SetBool("Isjump", false);
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower,0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private void Flip()
    {
        
        m_FacingRight = !m_FacingRight;

        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void Attack()
    {
        animator.SetTrigger("IsAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange,enemyLayers);

        foreach(Collider2D enemy in  hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        animator.SetTrigger("Hit");
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetBool("IsDead", true);
        smert.SetActive(true);

    }
    public void Restart()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Snova()
    {
        SceneManager.LoadScene("Menu");
    }


}
