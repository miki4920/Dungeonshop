using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace Dungeonshop.UI
{
    public class LightHandler : ColorReceiver
    {
        public static LightHandler LightInstance;

        [SerializeField] SliderLayout radiusOuterSlider;
        float radiusOuter;
        [SerializeField] SliderLayout radiusInnerSlider;
        float radiusInner;
        [SerializeField] SliderLayout intensitySlider;
        float intensity;
        [SerializeField] SliderLayout falloffStrengthSlider;
        float falloffStrength;
        [SerializeField] SliderLayout angleSlider;
        float angle;
        Color color;
        bool restoreValues;
        GameObject lightInstance;
        FieldInfo m_FalloffField = typeof(Light2D).GetField("m_FalloffIntensity", BindingFlags.NonPublic | BindingFlags.Instance);

        private void Awake()
        {
            if (LightInstance != null && LightInstance != this)
            {
                Destroy(this);
            }
            else
            {
                LightInstance = this;
            }
        }

        public override void updateColor(Color color)
        {
            this.color = color;
        }

        public GameObject createLight(Vector3 positon)
        {
            GameObject newLight = new GameObject();
            newLight.name = "Light";
            newLight.transform.position = positon;
            newLight.AddComponent<Light2D>();
            updateLight(newLight);
            return newLight;
        }

        public void updateLight(GameObject light)
        {
            Light2D lightComponent = light.GetComponent<Light2D>();
            lightComponent.pointLightOuterRadius = radiusOuterSlider.currentValue * 256;
            lightComponent.pointLightInnerRadius = radiusInnerSlider.currentValue * 256;
            lightComponent.intensity = intensitySlider.currentValue;
            // Needs to be done via reflection because setter doesn't exist
            m_FalloffField.SetValue(lightComponent, falloffStrengthSlider.currentValue);
            lightComponent.color = color;
            lightComponent.blendStyleIndex = 1;
            lightComponent.overlapOperation = Light2D.OverlapOperation.Additive;

    }

    void Update()
        {
            if(lightInstance == null)
            {
                radiusOuter = radiusOuterSlider.currentValue;
                radiusInner = radiusInnerSlider.currentValue;
                intensity = intensitySlider.currentValue;
                falloffStrength = falloffStrengthSlider.currentValue;
                angle = angleSlider.currentValue;
            }
            else if (restoreValues)
            {
                restoreValues = false;
                radiusOuterSlider.updateValue(radiusOuter);
                radiusInnerSlider.updateValue(radiusInner);
                intensitySlider.updateValue(intensity);
                falloffStrengthSlider.updateValue(falloffStrength);
                angleSlider.updateValue(angle);
            }
            else
            {
                updateLight(lightInstance);
            }
        }
    }
}
