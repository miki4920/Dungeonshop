using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

namespace Dungeonshop.UI
{
    public class ColorView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] RawImage colorImage;
        [SerializeField] RawImage currentColor;
        [SerializeField] Slider hueSlider;
        [SerializeField] Slider saturationSlider;
        [SerializeField] Slider lightnessSlider;
        int width = 300;
        int height = 300;

        void updateRawImage()
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] colors = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[y * width + x] = Color.HSVToRGB(hueSlider.value / 360, ((float)x) / width, ((float)y) / height);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, width, height, colors);
            texture.Apply();
            colorImage.texture = texture;
        }


        void Start()
        {
            updateRawImage();
            hueSlider.onValueChanged.AddListener(delegate
            {
                updateRawImage();
                updateColor();
            });

            saturationSlider.onValueChanged.AddListener(delegate
            {
                updateColor();
            });

            lightnessSlider.onValueChanged.AddListener(delegate
            {
                updateColor();
            });
        }

        public void updateColor()
        {
            Color color = Color.HSVToRGB(hueSlider.value / 360, saturationSlider.value / 100, lightnessSlider.value / 100);
            BrushSelectorManager.Instance.updateColor(color);
            currentColor.color = color;
        }

        public void updateColorOnClick()
        {
            Vector3 drawingAreaPosition = colorImage.transform.position;
            float globalPositionX = (Mathf.Clamp(Input.mousePosition.x - drawingAreaPosition.x, 0, width) / width);
            float globalPositionY = (1 - (Mathf.Clamp(drawingAreaPosition.y - Input.mousePosition.y, 0, height) / height));
            saturationSlider.SetValueWithoutNotify((int)(globalPositionX * 100));
            lightnessSlider.SetValueWithoutNotify((int)(globalPositionY * 100));
            updateColor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            updateColorOnClick();
        }
    }

}
