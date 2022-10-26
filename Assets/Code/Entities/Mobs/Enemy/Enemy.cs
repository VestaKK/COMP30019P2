using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Mob
{
    public delegate void DamageEvent();
    public event DamageEvent OnTakeDamage;
    public GameObject item;
    [SerializeField] private int _score;

    public Enemy() : base() {}
    public override void TakeDamage(AttackInfo info) {
        if (isDead)
            return;

        Health -= info.Damage;

        OnTakeDamage.Invoke();

        if (Health <= 0)
        {
            if (deathClip != null)
                audioSource.PlayOneShot(deathClip, 1f);
            OnDeath();
            return;
        }

        HealthBar.SetProgress(Health / MaxHealth);

        if (takeDamageClip.Length > 0)
            audioSource.PlayOneShot(takeDamageClip[Random.Range(0, takeDamageClip.Length)], 1f);
    }

    public override void OnDeath()
    {
        isDead = true;
        // die
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        MobController.Animator.SetTrigger("DeathAnimation");
        Destroy(HealthBar.gameObject);
        GameManager.AddToScore(_score);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        if (item != null)
            Instantiate(item, transform.position + Vector3.up, Quaternion.identity);
        Destroy(this.gameObject);
    }

    // Probably don't want to set
    public Player Player { get => GameManager.CurrentPlayer; set => GameManager.CurrentPlayer = value; }
}