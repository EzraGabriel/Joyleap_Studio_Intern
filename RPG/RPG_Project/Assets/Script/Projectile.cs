using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;

        GameObject instigator = null;
        Health target = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetLocation());
        }


        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                return;
            }
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target)
            {
                return;
            }
            if (target.IsDead())
            {
                return;
            }
            target.takeDamage(instigator, damage);
            speed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}