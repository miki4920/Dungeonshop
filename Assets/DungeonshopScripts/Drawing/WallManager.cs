using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop
{
    public class WallManager : ColorReceiver, TextureReceiver
    {
        [SerializeField] SliderLayout widthSlider;
        [SerializeField] GameObject wallPrefab;
        [SerializeField] GameObject drawingArea;
        Texture2D texture;
        Color color;
        [HideInInspector] public GameObject wallInstance;

        public void updateTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void updateColor(Color color)
        {
            this.color = color;
        }

        public void createWall(float size, Vector3 position)
        {
            position.z = 0;
            ShadowCaster2D;
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0.5f), 256);
            wallInstance = Instantiate(wallPrefab);
            wallInstance.GetComponent<SpriteRenderer>().sprite = sprite;
            wallInstance.GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            wallInstance.AddComponent<ObjectInformation>();
            wallInstance.GetComponent<ObjectInformation>().size = size;
            wallInstance.transform.position = position;
            wallInstance.transform.SetParent(drawingArea.transform);
            RectTransform rect = wallInstance.GetComponent<RectTransform>();
            rect.localScale = new Vector2((rect.position - position).magnitude, texture.height*widthSlider.currentValue);
            updateWall(position);
        }
        
        public void updateWall(Vector3 position)
        {
            position.z = 0;
            RectTransform rect = wallInstance.GetComponent<RectTransform>();
            rect.localScale = new Vector2(Vector3.Distance(rect.position, position), rect.localScale.y) ;
            float AngleRad = Mathf.Atan2(position.y - wallInstance.transform.position.y, position.x - wallInstance.transform.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            wallInstance.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
        }
    }
}