using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreGround = null;

        void Update()
        {
            //foreGround.Image.Color
            //   = healthComponent.Healthpoints.value;
            foreGround.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}