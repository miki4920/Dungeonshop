using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dungeonshop;

namespace Dungeonshop
{
    public class LayerManager : MonoBehaviour
    {
        public static LayerManager instance;
        List<Layer> layers = new List<Layer>();
        Layer currentLayer;
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
            }
        }

        void Start()
        {
            currentLayer = Dungeonshop.BackgroundManager.createBlankRenderTexture();
            layer = 0;
            layers.Add(currentLayer);
            layers.Add(Dungeonshop.BackgroundManager.createBlankRenderTexture());
        }
        
        public Layer getCurrentLayer()
        {
            return layers[currentLayer];
        }
    }
}