using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using System.Linq;

namespace Dungeonshop
{
    public class WallManager : ColorReceiver, TextureReceiver
    {
        [SerializeField] SliderLayout widthSlider;
        [SerializeField] Checkbox blocksShadow;
        [SerializeField] GameObject wallPrefab;
        [SerializeField] GameObject drawingArea;
        Texture2D texture;
        Color color;
        [HideInInspector] public List<GameObject> wallInstance;
        private FieldInfo _shapePathField;
        private FieldInfo _shapeHash;

        public void Awake()
        {
            _shapeHash = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
            _shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
            wallInstance = new List<GameObject>();
        }

        public void updateWallTextures()
        {
            if (wallInstance.Count != 0 && wallInstance.Last().GetComponent<SpriteRenderer>().sprite.texture != texture)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0.5f), 256);
                foreach (GameObject wall in wallInstance)
                {
                    wall.GetComponent<SpriteRenderer>().sprite = sprite;
                    RectTransform rect = wall.GetComponent<RectTransform>();
                    rect.localScale = new Vector2(rect.localScale.x, texture.height * widthSlider.currentValue * wall.GetComponent<ObjectInformation>().size);
                    wall.GetComponent<Renderer>().material.mainTextureScale = new Vector2(rect.localScale.x / texture.width / wall.GetComponent<ObjectInformation>().size / widthSlider.currentValue, 1);
                }
            }
        }

        public void updateTexture(Texture2D texture)
        {
            this.texture = texture;
            updateWallTextures();
        }

        public override void updateColor(Color color)
        {
            this.color = color;
        }

        private Vector3[] Vector2ToVector3(Vector2[] vector2s)
        {
            Vector3[] vector3s = new Vector3[vector2s.Length];

            for (int i = 0; i < vector2s.Length; i++)
            {
                vector3s[i] = vector2s[i];
            }

            return vector3s;
        }

        public void updateShadowcaster()
        {
            _shapePathField.SetValue(wallInstance.Last().GetComponent<ShadowCaster2D>(), Vector2ToVector3(wallInstance.Last().GetComponent<PolygonCollider2D>().points));

            unchecked
            {
                int hashCode = (int)2166136261 ^ _shapePathField.GetHashCode();
                hashCode = hashCode * 16777619 ^ (wallInstance.Last().GetComponent<PolygonCollider2D>().points.GetHashCode());
                _shapeHash.SetValue(wallInstance.Last().GetComponent<ShadowCaster2D>(), hashCode);
            }
        }

        public void createWall(float size, Vector3 position)
        {
            position.z = 0;
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0.5f), 256);
            wallInstance.Add(Instantiate(wallPrefab));
            Material material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
            wallInstance.Last().GetComponent<Renderer>().material = material;
            wallInstance.Last().GetComponent<Renderer>().sortingLayerName = "UI";
            wallInstance.Last().GetComponent<SpriteRenderer>().sortingOrder = 2;
            wallInstance.Last().GetComponent<SpriteRenderer>().sprite = sprite;
            wallInstance.Last().GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            wallInstance.Last().GetComponent<SpriteRenderer>().color = color;
            wallInstance.Last().AddComponent<ObjectInformation>();
            wallInstance.Last().GetComponent<ObjectInformation>().size = size;
            wallInstance.Last().transform.position = position;
            wallInstance.Last().transform.SetParent(drawingArea.transform);
            RectTransform rect = wallInstance.Last().GetComponent<RectTransform>();
            rect.localScale = new Vector2((rect.position - position).magnitude, texture.height * widthSlider.currentValue * size);
            updateShadowcaster();
            updateWall(position);
        }
        
        public void updateWall(Vector3 position)
        {
            updateWallTextures();
            position.z = 0;
            RectTransform rect = wallInstance.Last().GetComponent<RectTransform>();
            rect.localScale = new Vector2(Vector3.Distance(rect.position, position)+rect.localScale.y/3, rect.localScale.y);
            float AngleRad = Mathf.Atan2(position.y - wallInstance.Last().transform.position.y, position.x - wallInstance.Last().transform.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            wallInstance.Last().transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            float size = wallInstance.Last().GetComponent<ObjectInformation>().size;
            wallInstance.Last().GetComponent<Renderer>().material.mainTextureScale = new Vector2(rect.localScale.x / texture.width / size / widthSlider.currentValue, 1);

            if (!blocksShadow.checkValue)
            {
                wallInstance.Last().GetComponent<ShadowCaster2D>().enabled = false;
                wallInstance.Last().GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
            }
            else
            {
                wallInstance.Last().GetComponent<ShadowCaster2D>().enabled = true;
                wallInstance.Last().GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
            }
        }
    }
}