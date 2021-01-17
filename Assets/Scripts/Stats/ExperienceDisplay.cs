using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            // update text with format - : 0.0 is decimal places
            GetComponent<Text>().text = string.Format("+{0:0.0}", experience.GetPoints());
        }

    }
}
