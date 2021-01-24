using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        // serialized on arr allows us to specify size and elements of arr in inspector
        [SerializeField] GameObject[] destroyOnHit = null; // arr of GameObjects
        [SerializeField] float lifeAfterImpact = 2;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            // GetAimLocation() is calling methods on another component, so we cannot place in Awake()
            transform.LookAt(GetAimLocation()); //actual projectile looks at instantiation, can

        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead()) // 2nd part protects against bug
            {
                transform.LookAt(GetAimLocation()); //projectile looks at target every frame
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage) // SetTarget is a unity method Animator.SetTarget
        {
            this.target = target; // called from another scrtipt, sets objects target to specified target
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime); // destroy after specified time
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2; // + vertical offset
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0; // SerializeField

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            // which to destroy v      v
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy); // destroy all for now
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}