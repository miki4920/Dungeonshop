using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dungeonshop
{
    public enum DrawingMode
    {
        Color,
        Texture,
        Eraser
    }

    public class BrushSelectorManager : ColorReceiver, TextureReceiver
    {
        public static new BrushSelectorManager Instance;
        [SerializeField] float defaultSize = 20f;
        [SerializeField] float defaultOpacity = 1;
        [SerializeField] Color defaultColor;
        [SerializeField] SliderLayout sizeSlider;
        [SerializeField] SliderLayout opacitySlider;
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
            drawingMode = DrawingMode.Color;
            color = defaultColor;
            textureColor = defaultColor;
            DrawingMode[] drawingModes = (DrawingMode[])Enum.GetValues(typeof(DrawingMode));
            for (int i = 0; i < drawingModes.Length; i++)
            {
                sizeDictionary[drawingModes[i]] = defaultSize;
                opacityDictionary[drawingModes[i]] = defaultOpacity;
            }
        }

        private void Update()
        {
            sizeDictionary[drawingMode] = sizeSlider.currentValue;
            opacityDictionary[drawingMode] = opacitySlider.currentValue / 100;
        }

        public void setSliders()
        {
            sizeSlider.updateValue(sizeDictionary[drawingMode]);
            opacitySlider.updateValue(opacityDictionary[drawingMode]*100);
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

        public void updateTexture(Texture2D texture)
        {
            this.texture = texture;
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

