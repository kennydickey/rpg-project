using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            // update text with format - : 0.0 is decimal places
            GetComponent<Text>().text = string.Format("{0:0}", baseStats.GetLevel());
        }

    }
}
