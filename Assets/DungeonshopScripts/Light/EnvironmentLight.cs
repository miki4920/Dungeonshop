using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop.UI
{
    public class EnvironmentLight : ColorReceiver
    {
        [SerializeField] GameObject environmentLight;
        public override void updateColor(Color color)
        {
            environmentLight.GetComponent<Light2D>().color = color;
        }
    }
}
