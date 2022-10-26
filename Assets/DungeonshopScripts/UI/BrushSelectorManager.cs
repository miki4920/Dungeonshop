using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonshop.UI
{
    public enum DrawingMode
    {
        Color,
        Texture,
        Eraser
    }
    public class BrushSelectorManager : MonoBehaviour
    {
        public static BrushSelectorManager Instance;
        [SerializeField] float defaultSize = 20f;
        [SerializeField] float defaultOpacity = 1;
        [SerializeField] Texture2D defaultTexture;
        [SerializeField] Color defaultColor;
        [SerializeField] Slider sizeSlider;
        [SerializeField] Slider opacitySlider;
        [HideInInspector] public DrawingMode drawingMode;
        Dictionary<DrawingMode, float> sizeDictionary = new Dictionary<DrawingMode, float>();
        Dictionary<DrawingMode, float> opacityDictionary = new Dictionary<DrawingMode, float>();
        Dictionary<DrawingMode, GameObject> panelDictionary = new Dictionary<DrawingMode, GameObject>();

        [HideInInspector] public Color color;
        [HideInInspector] public Texture2D texture;

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
        }

        void Start()
        {
            drawingMode = DrawingMode.Color;
            texture = defaultTexture;
            color = defaultColor;
            DrawingMode[] drawingModes = (DrawingMode[]) Enum.GetValues(typeof(DrawingMode));
            Transform[] children = transform.Cast<Transform>().ToArray();
            for(int i=0; i < drawingModes.Length; i++)
            {
                sizeDictionary[drawingModes[i]] = defaultSize;
                opacityDictionary[drawingModes[i]] = defaultOpacity;
                // Add 1 to i to offset parent object transform being contained in the list
                panelDictionary[drawingModes[i]] = children[i+1].gameObject;
                panelDictionary[drawingModes[i]].gameObject.SetActive(false);
            }
            panelDictionary[drawingMode].SetActive(true);
            setSliders();
            sizeSlider.onValueChanged.AddListener(delegate
            {
                sizeDictionary[drawingMode] = sizeSlider.value;
            });

            opacitySlider.onValueChanged.AddListener(delegate
            {
                opacityDictionary[drawingMode] = opacitySlider.value/100;
            });
        }

        public void setSliders()
        {
            sizeSlider.SetValueWithoutNotify(sizeDictionary[drawingMode]);
            opacitySlider.SetValueWithoutNotify(opacityDictionary[drawingMode]*100);
        }

        public void changeDrawingMode(DrawingMode newDrawingMode)
        {
            panelDictionary[drawingMode].SetActive(false);
            drawingMode = newDrawingMode;
            panelDictionary[drawingMode].SetActive(true);
            setSliders();
        }

        public void updateColor(Color newColor)
        {
            color = newColor;
        }

        public void updateTexture(Texture2D newTexture)
        {
            texture = newTexture;
        }

        public float getOpacity()
        {
            return opacityDictionary[drawingMode];
        }

        public float getSize()
        {
            return sizeDictionary[drawingMode];
        }


    }
}

