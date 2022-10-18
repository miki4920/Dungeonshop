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
        public int layer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                this.layer = -1;
            }
        }

        public void createNewLayer()
        {
            Layer blankLayer = new Layer(BackgroundManager.createBlankRenderTexture());
            if(this.layer >= 0) {
                layers.Insert(this.layer, blankLayer);
            }
            else {
                layers.Insert(0, blankLayer);
                this.layer = 0;
            }
        }

        public void deleteLayer(int layer)
        {
            layers.RemoveAt(layer);
            if(this.layer >= layers.Count)
            {
                this.layer = layers.Count - 1;
            }
        }

        public void deleteCurrentLayer()
        {
            this.deleteLayer(this.layer);
        }
        
        public Layer getCurrentLayer()
        {
            if (layer >= 0)
            {
                return layers[layer];
            }
            return null;
        }
    }
}