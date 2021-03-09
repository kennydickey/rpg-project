using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            if (Mathf.Approximately(healthComponent.GetFraction(), 0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return; // returns out of update function as false only to be tested again each update()
            }

            rootCanvas.enabled = true;

            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);

        }

    }
}