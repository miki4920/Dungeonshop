using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dungeonshop.UI
{
    public enum DrawingMode
    {
        Color,
        Texture,
        Eraser
    }

    public class BrushSelectorManager : ColorReceiver
    {
        public static new BrushSelectorManager Instance;
        [SerializeField] float defaultSize = 20f;
        [SerializeField] float defaultOpacity = 1;
        [SerializeField] Color defaultColor;
        [SerializeField] Slider sizeSlider;
        [SerializeField] Slider opacitySlider;
        [SerializeField] TMP_Text sizeText;
        [SerializeField] TMP_Text opacityText;
        [HideInInspector] public DrawingMode drawingMode;
        Dictionary<DrawingMode, float> sizeDictionary = new Dictionary<DrawingMode, float>();
        Dictionary<DrawingMode, float> opacityDictionary = new Dictionary<DrawingMode, float>();

        [HideInInspector] public Color color;
        [HideInInspector] public Color textureColor;
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
            sizeText.text = defaultSize.ToString();
            opacityText.text = (defaultOpacity * 100).ToString();
            drawingMode = DrawingMode.Texture;
            color = defaultColor;
            textureColor = defaultColor;
            DrawingMode[] drawingModes = (DrawingMode[]) Enum.GetValues(typeof(DrawingMode));
            for (int i = 0; i < drawingModes.Length; i++)
            {
                sizeDictionary[drawingModes[i]] = defaultSize;
                opacityDictionary[drawingModes[i]] = defaultOpacity;
            }
            setSliders();
            sizeSlider.onValueChanged.AddListener(delegate
            {
                sizeDictionary[drawingMode] = sizeSlider.value;
                sizeText.text = sizeSlider.value.ToString();
            });

            opacitySlider.onValueChanged.AddListener(delegate
            {
                opacityDictionary[drawingMode] = opacitySlider.value/100;
                opacityText.text = opacitySlider.value.ToString();
            });
        }

        public void setSliders()
        {
            sizeSlider.SetValueWithoutNotify(sizeDictionary[drawingMode]);
            sizeText.text = sizeSlider.value.ToString();
            opacitySlider.SetValueWithoutNotify(opacityDictionary[drawingMode]*100);
            opacityText.text = opacitySlider.value.ToString();
        }

        public void changeDrawingMode(DrawingMode newDrawingMode)
        {
            drawingMode = newDrawingMode;
            setSliders();
        }

        public void changeDrawingModeInt(int newDrawingMode)
        {
            drawingMode = (DrawingMode) newDrawingMode;
            setSliders();
        }

        public override void updateColor(Color newColor)
        {
            if(drawingMode == DrawingMode.Color)
            {
                color = newColor;
            }
            else if(drawingMode == DrawingMode.Texture)
            {
                textureColor = newColor;
            }
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

