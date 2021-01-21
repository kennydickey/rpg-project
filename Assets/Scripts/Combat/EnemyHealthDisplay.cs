using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if(fighter.GetTarget() == null) // if no health information from target
            {
                GetComponent<Text>().text = "not available";
                return;
            }
            // since we use RPG.Combat, we can specify using Resouces to use Health
            Health health = fighter.GetTarget(); // simply our fighter's target, which is also of type Health, GetPercentage gets the actual health v                             v
            GetComponent<Text>().text = String.Format("{0:0.0}%/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
