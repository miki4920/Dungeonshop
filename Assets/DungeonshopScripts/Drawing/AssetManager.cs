using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class AssetManager : ColorReceiver, TextureReceiver
    {
        [SerializeField] SliderLayout rotationSlider;
        [SerializeField] SliderLayout widthSlider;
        [SerializeField] SliderLayout heightSlider;
        [SerializeField] GameObject assetPrefab;
        [SerializeField] GameObject drawingArea;
        Texture2D texture;
        Color color;
        [HideInInspector] public GameObject assetInstance;
     
        public void updateTexture(Texture2D texture)
        {
            this.texture = texture;
            if(assetInstance != null)
            {
                float size = assetInstance.GetComponent<ObjectInformation>().size;
                assetInstance.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 256);
                RectTransform rect = assetInstance.GetComponent<RectTransform>();
                rect.localScale = new Vector2(texture.width * widthSlider.currentValue * size, texture.height * heightSlider.currentValue * size);
            }
        }

        public override void updateColor(Color color)
        {
            this.color = color;
            if (assetInstance != null)
            {
                assetInstance.GetComponent<SpriteRenderer>().color = color;
            }
        }

        public void createAsset(float size, Vector3 position)
        {
            position.z = 0;
            Material material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 256);
            assetInstance = Instantiate(assetPrefab);
            assetInstance.GetComponent<Renderer>().material = material;
            assetInstance.GetComponent<Renderer>().sortingLayerName = "UI";
            assetInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            assetInstance.GetComponent<SpriteRenderer>().sprite = sprite;
            assetInstance.GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            assetInstance.GetComponent<SpriteRenderer>().color = color;

            assetInstance.AddComponent<ObjectInformation>();
            assetInstance.GetComponent<ObjectInformation>().size = size;

            assetInstance.transform.position = position;
            assetInstance.transform.SetParent(drawingArea.transform);

            RectTransform rect = assetInstance.GetComponent<RectTransform>();
            rect.localScale = new Vector2(texture.width * widthSlider.currentValue * size, texture.height * heightSlider.currentValue * size);
        }

        public void updateRotation(float mouseDelta)
        {
            mouseDelta = mouseDelta >= 0 ? 5 : -5;
            assetInstance.transform.Rotate(new Vector3(0, 0, mouseDelta));
        }

        public void updateDimensions()
        {
            float size = assetInstance.GetComponent<ObjectInformation>().size;
            RectTransform rect = assetInstance.GetComponent<RectTransform>();
            rect.localScale = new Vector2(texture.width * widthSlider.currentValue * size, texture.height * heightSlider.currentValue * size);
        }

        public void updateAsset(Vector3 position)
        {
            position.z = 0;
            RectTransform rect = assetInstance.GetComponent<RectTransform>();
            rect.position = position;
            updateDimensions();
            
        }
    }
}

