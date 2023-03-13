using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop
{
    public enum LightMode
    {
        Environment,
        Light
    }
    public class LightManager : ColorReceiver
    {
        [SerializeField] SliderLayout radiusOuterSlider;
        [SerializeField] SliderLayout radiusInnerSlider;
        [SerializeField] SliderLayout intensitySlider;
        [SerializeField] SliderLayout falloffStrengthSlider;
        [SerializeField] SliderLayout angleSlider;

        Color color;

        [HideInInspector] public LightMode lightMode;
        [HideInInspector] public GameObject lightInstance;

        [SerializeField] GameObject lightImagePrefab;
        [SerializeField] GameObject drawingArea;
        [SerializeField] ColorView colorView;

        public override void updateColor(Color color)
        {
            this.color = color;
        }

        public void changeMode(int newMode)
        {
            lightMode = (LightMode) newMode;
        }

        public void createLight(float size)
        {
            lightInstance = new GameObject();
            lightInstance.transform.SetParent(drawingArea.transform);
            lightInstance.name = "Light";
            lightInstance.AddComponent<Light2D>();
            lightInstance.GetComponent<Light2D>().shadowsEnabled = true;
            lightInstance.GetComponent<Light2D>().shadowIntensity = 1;
            lightInstance.AddComponent<ObjectInformation>();
            lightInstance.GetComponent<ObjectInformation>().size = size;
            GameObject lightImage = Instantiate(lightImagePrefab);
            lightImage.transform.SetParent(lightInstance.transform);
            lightImage.SetActive(false);
        }

        public void updateRotation(float mouseDelta)
        {
            mouseDelta = mouseDelta >= 0 ? 5 : -5;
            lightInstance.transform.Rotate(new Vector3(0, 0, mouseDelta));
        }

        public void updateSliders()
        {
            Light2D light = lightInstance.GetComponent<Light2D>();
            radiusOuterSlider.updateValue(light.pointLightOuterRadius / 256 / lightInstance.GetComponent<ObjectInformation>().size);
            radiusInnerSlider.updateValue(light.pointLightInnerRadius / 256 / lightInstance.GetComponent<ObjectInformation>().size);
            intensitySlider.updateValue(light.intensity);
            falloffStrengthSlider.updateValue(light.falloffIntensity);
            angleSlider.updateValue(light.pointLightOuterAngle);
            colorView.setColor(light.color);
        }

        public void updateLight()
        {
            Light2D lightComponent = lightInstance.GetComponent<Light2D>();
            lightComponent.pointLightOuterRadius = radiusOuterSlider.currentValue * 256 * lightComponent.GetComponent<ObjectInformation>().size;
            lightComponent.pointLightInnerRadius = Mathf.Min(radiusInnerSlider.currentValue, radiusOuterSlider.currentValue) * 256 * lightComponent.GetComponent<ObjectInformation>().size;
            lightComponent.intensity = intensitySlider.currentValue;
            lightComponent.falloffIntensity = falloffStrengthSlider.currentValue;
            lightComponent.color = color;
            lightComponent.blendStyleIndex = 0;
            lightComponent.overlapOperation = Light2D.OverlapOperation.Additive;
            lightComponent.pointLightOuterAngle = angleSlider.currentValue;
            lightComponent.pointLightInnerAngle = angleSlider.currentValue;

        }
    }


}
