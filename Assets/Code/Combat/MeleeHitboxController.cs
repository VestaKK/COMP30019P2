using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxController : MonoBehaviour
{

    public int damage = 10;
    public float meleeLingerTime = 0.3f;

    // Update is called once per frame
    private void Update()
    {
        meleeLingerTime -= Time.deltaTime;
        if (meleeLingerTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Event trigger on collide with a hurtbox
    public void OnTriggerEnter(Collider other)
    {
        GameObject objectOfHurtbox = other.gameObject.transform.parent.gameObject;
        if (objectOfHurtbox.tag == "Enemy")
        {
            objectOfHurtbox.GetComponent<EnemyHealthManager>().HurtEnemy(damage);
        }
    }
}
