using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLayout : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] float minimum;
    [SerializeField] float maximum;
    [SerializeField] float defaultValue;
    [HideInInspector] public float currentValue;

    public void Start()
    {
        if(minimum == default(float))
        {
            minimum = 0;
        }
        if(maximum == default(float))
        {
            maximum = float.MaxValue;
        }
        slider.minValue = minimum;
        slider.maxValue = maximum;
        updateValue(defaultValue);
    }

    public void updateValue(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        slider.SetValueWithoutNotify(value);
        inputField.SetTextWithoutNotify(value.ToString());
        currentValue = value;
    }

    public void convertTextToFloat(TMP_InputField textField)
    {
        float.TryParse(textField.text, out float value);
        if(minimum <= value && value <= maximum)
        {
            updateValue(value);
        }
        else
        {
            inputField.SetTextWithoutNotify(currentValue.ToString());
        }
    }

    public void convertSliderToFloat(Slider slider) 
    {
        updateValue(slider.value);
    }

}
