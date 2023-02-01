using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

namespace Dungeonshop.UI
{
    public enum LightMode
    {
        Environment,
        Light
    }
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
        [HideInInspector] public LightMode lightMode;
        bool restoreValues;
        [HideInInspector] public GameObject lightInstance;
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

        public void changeMode(int newMode)
        {
            lightMode = (LightMode) newMode;
        }

        public void createLight(Vector3 position)
        {
            lightInstance = new GameObject();
            lightInstance.name = "Light";
            lightInstance.AddComponent<Light2D>();
            updatePosition(position);
            updateLight(lightInstance);
        }

        public void updatePosition(Vector3 position)
        {
            lightInstance.transform.position = position;
        }

        public void updateRotation(float mouseDelta)
        {
            mouseDelta = mouseDelta >= 0 ? 5 : -5;
            lightInstance.transform.Rotate(new Vector3(0, 0, mouseDelta));
        }

        public void updateLight(GameObject light)
        {
            Light2D lightComponent = light.GetComponent<Light2D>();
            lightComponent.pointLightOuterRadius = radiusOuterSlider.currentValue * 256;
            lightComponent.pointLightInnerRadius = Mathf.Min(radiusInnerSlider.currentValue, radiusOuterSlider.currentValue) * 256;
            lightComponent.intensity = intensitySlider.currentValue;
            // Needs to be done via reflection because setter doesn't exist
            m_FalloffField.SetValue(lightComponent, falloffStrengthSlider.currentValue);
            lightComponent.color = color;
            lightComponent.blendStyleIndex = 0;
            lightComponent.overlapOperation = Light2D.OverlapOperation.Additive;
            lightComponent.pointLightOuterAngle = angleSlider.currentValue;
            lightComponent.pointLightInnerAngle = angleSlider.currentValue;

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
