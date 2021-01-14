using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            // update text with format - : 0.0 is decimal places
            GetComponent<Text>().text = String.Format("Remaining {0:0.0}%", health.GetPercentage());
        }
    }
}
