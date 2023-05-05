using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Dungeonshop
{
    public class ColorView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] RawImage colorImage;
        [SerializeField] RawImage currentColor;
        [SerializeField] SliderLayout hueSlider;
        [SerializeField] SliderLayout saturationSlider;
        [SerializeField] SliderLayout lightnessSlider;
        [SerializeField] GameObject colorReceiverObject;
        float hue;
        float saturation;
        float lightness;
        ColorReceiver colorReceiver;

        int width;
        int height;

        void updateRawImage()
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] colors = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[y * width + x] = Color.HSVToRGB(hue / 360, ((float)x) / width, ((float)y) / height);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, width, height, colors);
            texture.Apply();
            colorImage.texture = texture;
        }

        void updateSaturation()
        {
            Transform slider = saturationSlider.transform.GetChild(0);
            int saturationWidth = (int) slider.GetComponent<RectTransform>().rect.width;
            int saturationHeight = (int) slider.GetComponent<RectTransform>().rect.height;
            Texture2D texture = new Texture2D(saturationWidth, saturationHeight);
            Color[] colors = new Color[saturationWidth * saturationHeight];
            for (int x = 0; x < saturationWidth; x++ )
            {
                for(int y = 0; y < saturationHeight; y++)
                {
                    colors[y * saturationWidth + x] = Color.HSVToRGB(hue / 360, ((float)x) / saturationWidth, 1);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, saturationWidth, saturationHeight, colors);
            texture.Apply();
            slider.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

        void updateLightness()
        {
            Transform slider = lightnessSlider.transform.GetChild(0);
            int lightnessWidth = (int) slider.GetComponent<RectTransform>().rect.width;
            int lightnessHeight = (int) slider.GetComponent<RectTransform>().rect.height;
            Texture2D texture = new Texture2D(lightnessWidth, lightnessHeight);
            Color[] colors = new Color[lightnessWidth * lightnessHeight];
            for (int x = 0; x < lightnessWidth; x++)
            {
                for (int y = 0; y < lightnessHeight; y++)
                {
                    colors[y * lightnessWidth + x] = Color.HSVToRGB(hue/360, 0, ((float)x) / lightnessWidth);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, lightnessWidth, lightnessHeight, colors);
            texture.Apply();
            slider.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }


        void Start()
        {
            width = (int) colorImage.GetComponent<RectTransform>().rect.width;
            height =(int) colorImage.GetComponent<RectTransform>().rect.height;
            colorReceiver = colorReceiverObject.GetComponent<ColorReceiver>();
            hue = hueSlider.currentValue;
            saturation = saturationSlider.currentValue;
            lightness = lightnessSlider.currentValue;
            updateRawImage();
            updateSaturation();
            updateLightness();
            updateColor();
        }

        void Update()
        {
            if(hueSlider.currentValue != hue)
            {
                hue = hueSlider.currentValue;
                updateRawImage();
                updateSaturation();
                updateColor();
                
            }
            if (saturationSlider.currentValue != saturation)
            {
                saturation = saturationSlider.currentValue;
                updateColor();
            }
            if (lightnessSlider.currentValue != lightness)
            {
                lightness = lightnessSlider.currentValue;
                updateColor();
            }
        }

        public void updateColor()
        {
            Color color = Color.HSVToRGB(hue / 360, saturation / 100, lightness / 100);
            colorReceiver.updateColor(color);
            currentColor.color = color;
        }

        public void setColor(Color color)
        {
            Color.RGBToHSV(color, out hue, out saturation, out lightness);
            hueSlider.updateValue(hue * 360);
            saturationSlider.updateValue(saturation * 100);
            lightnessSlider.updateValue(lightness * 100);
        }

        public void updateColorOnClick()
        {
            Vector3 drawingAreaPosition = colorImage.transform.position;
            float globalPositionX = (Mathf.Clamp(Input.mousePosition.x - drawingAreaPosition.x, 0, width) / width);
            float globalPositionY = (1 - (Mathf.Clamp(drawingAreaPosition.y - Input.mousePosition.y, 0, height) / height));
            saturationSlider.updateValue((int) (globalPositionX * 100));
            lightnessSlider.updateValue((int) (globalPositionY * 100));
            updateColor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            updateColorOnClick();
        }
    }

}
