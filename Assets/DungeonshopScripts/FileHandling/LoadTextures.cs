using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace Dungeonshop
{
    public class LoadTextures : MonoBehaviour
    {
        [SerializeField] private string path;
        [SerializeField] GameObject textureTilePrefab;
        [SerializeField] GameObject textureReceiver;
        List<GameObject> textures = new List<GameObject>();
        public bool sprite;
        void Start()
        {
            Object[] loadedTextures = Resources.LoadAll(path);
            foreach (Object texture in loadedTextures)
            {
                GameObject temporaryTile = Instantiate(textureTilePrefab, gameObject.transform);
                temporaryTile.GetComponent<TextureView>().updateTexture((Texture2D) texture);
                temporaryTile.GetComponent<TextureView>().textureReceiver = textureReceiver.GetComponent<TextureReceiver>();
                temporaryTile.GetComponent<TabButton>().tabName = texture.name;
                temporaryTile.GetComponent<TabButton>().tabGroup = gameObject.GetComponent<TabGroup>(); 
                textures.Add(temporaryTile);
            }
            textureReceiver.GetComponent<TextureReceiver>().updateTexture((Texture2D)textures[0].transform.GetChild(0).GetComponent<RawImage>().texture);

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

