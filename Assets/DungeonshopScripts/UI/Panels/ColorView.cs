using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

namespace Dungeonshop.UI
{
    public class ColorView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] RawImage colorImage;
        [SerializeField] RawImage currentColor;
        [SerializeField] Slider hueSlider;
        [SerializeField] Slider saturationSlider;
        [SerializeField] Slider lightnessSlider;
        [SerializeField] TMP_Text hueText;
        [SerializeField] TMP_Text saturationText;
        [SerializeField] TMP_Text lightnessText;

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
                    colors[y * width + x] = Color.HSVToRGB(hueSlider.value / 360, ((float)x) / width, ((float)y) / height);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, width, height, colors);
            texture.Apply();
            colorImage.texture = texture;
        }

        void updateSaturation()
        {
            int saturationWidth = (int) saturationSlider.GetComponent<RectTransform>().rect.width;
            int saturationHeight = (int)saturationSlider.GetComponent<RectTransform>().rect.height;
            Texture2D texture = new Texture2D(saturationWidth, saturationHeight);
            Color[] colors = new Color[saturationWidth * saturationHeight];
            for (int x = 0; x < saturationWidth; x++ )
            {
                for(int y = 0; y < saturationHeight; y++)
                {
                    colors[y * width + x] = Color.HSVToRGB(hueSlider.value / 360, ((float)x) / saturationWidth, 1);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, saturationWidth, saturationHeight, colors);
            texture.Apply();
            saturationSlider.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

        void updateLightness()
        {
            int lightnessWidth = (int)lightnessSlider.GetComponent<RectTransform>().rect.width;
            int lightnessHeight = (int)lightnessSlider.GetComponent<RectTransform>().rect.height;
            Texture2D texture = new Texture2D(lightnessWidth, lightnessHeight);
            Color[] colors = new Color[lightnessWidth * lightnessHeight];
            for (int x = 0; x < lightnessWidth; x++)
            {
                for (int y = 0; y < lightnessHeight; y++)
                {
                    colors[y * width + x] = Color.HSVToRGB(hueSlider.value/360, 0, ((float)x) / lightnessWidth);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(0, 0, lightnessWidth, lightnessHeight, colors);
            texture.Apply();
            lightnessSlider.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }


        void Start()
        {
            width = (int) colorImage.GetComponent<RectTransform>().rect.width;
            height =(int) colorImage.GetComponent<RectTransform>().rect.height;

            updateRawImage();
            updateSaturation();
            updateLightness();
            updateColor();
            hueSlider.onValueChanged.AddListener(delegate
            {
                updateRawImage();
                updateSaturation();
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
            hueText.text = hueSlider.value.ToString();
            saturationText.text = saturationSlider.value.ToString();
            lightnessText.text = lightnessSlider.value.ToString();
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
