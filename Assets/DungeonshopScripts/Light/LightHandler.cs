using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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
        [SerializeField] GameObject lightImagePrefab;
        [SerializeField] GameObject drawingArea;

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
            lightInstance.transform.SetParent(drawingArea.transform);
            lightInstance.name = "Light";
            lightInstance.AddComponent<Light2D>();
            GameObject lightImage = Instantiate(lightImagePrefab);
            lightImage.transform.SetParent(lightInstance.transform);
            lightImage.SetActive(false);
            updatePosition(position);
            updateLight(lightInstance);
        }

        public void updatePosition(Vector3 position)
        {
            lightInstance.transform.position = position;
            lightInstance.transform.GetChild(0).position = position;
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

        public void updateLights(bool selectionMode)
        {
            foreach (Layer layer in CanvasManager.Instance.layers)
            {
                foreach (GameObject light in layer.lights)
                {
                    light.SetActive(false);
                    light.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            foreach (Layer layer in CanvasManager.Instance.getVisibleLayers())
            {
                foreach (GameObject light in layer.lights)
                {
                    light.SetActive(true);
                    if (selectionMode)
                    {
                        light.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
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
