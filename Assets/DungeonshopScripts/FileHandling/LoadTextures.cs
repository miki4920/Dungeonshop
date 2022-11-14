using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeonshop.UI;
using TMPro;

namespace Dungeonshop.Files
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
                temporaryTile.GetComponent<RawImage>().texture = (Texture2D)texture;
                temporaryTile.GetComponent<TextureView>().textureName = texture.name;
                textures.Add(temporaryTile);
            }
        }

        public void updateTextureList(GameObject textureSearchBar)
        {
            string searchText = textureSearchBar.GetComponent<TMP_InputField>().text;
            foreach (GameObject textureObject in textures)
            {
                if(textureObject.GetComponent<TextureView>().textureName.Contains(searchText))
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

