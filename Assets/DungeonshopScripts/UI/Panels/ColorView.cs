using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeonshop.UI
{
    public class ColorView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] RawImage colorImage;
        [SerializeField] Slider hueSlider;
        [SerializeField] Slider saturationSlider;
        [SerializeField] Slider lightnessSlider;
        float hue;
        float saturation;
        float lightness;
        Vector3 mousePosition;
        int width = 300;
        int height = 300;
        Color color;

        void updateRawImage()
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] colors = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[y * width + x] = Color.HSVToRGB(hue, ((float)x) / width, ((float)y) / height);
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
                hue = hueSlider.value / 360;
                updateRawImage();
            });

            saturationSlider.onValueChanged.AddListener(delegate
            {
                saturation = saturationSlider.value / 100;
            });

            lightnessSlider.onValueChanged.AddListener(delegate
            {
                lightness = lightnessSlider.value / 100;
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 drawingAreaPosition = colorImage.transform.position;
            int globalPositionX = (int)(Input.mousePosition.x - drawingAreaPosition.x);
            int globalPositionY = (int)(Input.mousePosition.y - drawingAreaPosition.y);
            Color tempColor = (colorImage.texture as Texture2D).GetPixel(globalPositionX, globalPositionY);
            BrushSelectorManager.Instance.updateColor(tempColor);
            float H, S, V;
            Color.RGBToHSV(tempColor, out H, out S, out V);
            hueSlider.value = H * 360;
            saturationSlider.value = S * 100;
            lightnessSlider.value = V * 100;
            
            Debug.Log(BrushSelectorManager.Instance.color);
        }
    }

}
