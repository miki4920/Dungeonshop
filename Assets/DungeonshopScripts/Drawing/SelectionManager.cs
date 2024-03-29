using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop
{
    public enum SelectionMode
    {
        Light,
        Wall,
        None
    }
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance;
        public bool isObjectSet;
        public GameObject selectedObject;
        public GameObject lightPanel;
        public SelectionMode mode;
        public LightManager lightHandler;
        bool setObjectValues;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            mode = SelectionMode.None;
        }

        public void setSelection(GameObject selection)
        {
            selectedObject = selection;
            isObjectSet = true;
            setObjectValues = false;
        }

        public void disableTabs()
        {
            lightHandler.gameObject.SetActive(false);
        }

        public void updateSelection()
        {
            if (selectedObject == null)
            {
                disableTabs();
            }
            else if(selectedObject != null)
            {
                if(selectedObject.GetComponent<Light2D>() != null)
                {
                    if(!setObjectValues)
                    {
                        lightHandler.gameObject.SetActive(true);
                        lightHandler.lightInstance = selectedObject;
                        mode = SelectionMode.Light;
                        setObjectValues = true;
                        lightHandler.updateSliders();
                    }
                    else if(setObjectValues)
                    {
                        lightHandler.updateLight();
                    }
                }
            }
        }
    }

    
}

