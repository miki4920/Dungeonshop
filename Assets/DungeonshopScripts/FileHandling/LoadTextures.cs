using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeonshop.UI;
using TMPro;

namespace Dungeonshop.UI
{
    public class LoadTextures : MonoBehaviour
    {
        [SerializeField] private string path;
        [SerializeField] GameObject textureTilePrefab;
        public List<GameObject> textures = new List<GameObject>();
        void Start()
        {
            Object[] loadedTextures = Resources.LoadAll(path);
            foreach (Object texture in loadedTextures)
            {
                GameObject temporaryTile = Instantiate(textureTilePrefab, gameObject.transform);
                temporaryTile.transform.GetChild(0).GetComponent<RawImage>().texture = (Texture2D)texture;
                temporaryTile.GetComponent<TabButton>().tabName = texture.name;
                temporaryTile.GetComponent<TabButton>().tabGroup = gameObject.GetComponent<TabGroup>();
                textures.Add(temporaryTile);
            }
            BrushSelectorManager.Instance.updateTexture((Texture2D) textures[0].transform.GetChild(0).GetComponent<RawImage>().texture);
        }

        public void updateTextureList(GameObject textureSearchBar)
        {
            string searchText = textureSearchBar.GetComponent<TMP_InputField>().text;
            foreach (GameObject textureObject in textures)
            {
                if(textureObject.GetComponent<TabButton>().tabName.Contains(searchText))
                {
                    textureObject.SetActive(true);
                }
                else
                {
                    textureObject.SetActive(false);
                }
                
            }
        }
    }
}

