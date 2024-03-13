using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int maxHP = 100;
    [SerializeField] [Range(0, 100)] private int hp = 100;
    [SerializeField] private bool isDead = false;

    protected Animator animator = null;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount) {
        this.hp -= damageAmount;

        if (this.hp <= 0) {
            this.hp = 0;
            this.isDead = true;
            if (animator != null) {
                animator.SetBool("IsDead", true);
            }
            HandleDeath();
        } else {
            this.isDead = false;
            if (animator != null) {
                animator.SetTrigger("Hit");
            }
        }
    }

    public bool IsDead() { return this.isDead; }

    public virtual void HandleDeath() {
        // do nothing
    }
}
