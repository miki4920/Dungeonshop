using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox : MonoBehaviour
{
    [HideInInspector] public bool checkValue;
    [SerializeField] bool defaultValue;
    void Start()
    {
        checkValue = defaultValue;
        updateState(defaultValue);
    }

    void updateState(bool value)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(value);
    }

    public void switchState()
    {
        checkValue = !checkValue;
        updateState(checkValue);
    }
}
