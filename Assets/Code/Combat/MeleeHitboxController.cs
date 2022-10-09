using UnityEngine;

public class MeleeHitboxController : MonoBehaviour
{

    public int damage = 10;
    public float meleeLingerTime = 0.3f;

    public void Initialize(int damage, float lingerTime)
    {
        this.damage = damage;
        meleeLingerTime = lingerTime;
    }

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
        // GameObject objectOfHurtbox = other.gameObject.transform.parent.gameObject;

        // // This assumes we only have Player and Enemy tags for hitbox/hurtbox!
        // if (objectOfHurtbox.tag != this.transform.parent.gameObject.tag) 
        // {
        //     objectOfHurtbox.GetComponent<HealthManager>().HurtEntity(damage);
        // }
    }
}
