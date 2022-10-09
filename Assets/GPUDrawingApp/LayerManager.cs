using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dungeonshop;

namespace Dungeonshop
{
    public class LayerManager : MonoBehaviour
    {
        public static LayerManager Instance;
        public List<Layer> layers = new List<Layer>();
        [SerializeField] public int layer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                layer = 0;
                layers.Add(new Layer(Dungeonshop.BackgroundManager.createBlankRenderTexture()));
                layers.Add(new Layer(Dungeonshop.BackgroundManager.createBlankRenderTexture()));
            }
        }

        void Start()
        {
            
        }
        
        public Layer getCurrentLayer()
        {
            return layers[1];
        }
    }
}