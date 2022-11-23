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
        [HideInInspector] public int layer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                layer = -1;
            }
        }

        public Layer createNewLayer()
        {
            Layer blankLayer = new Layer(BackgroundManager.Instance.createBlankRenderTexture());
            if (this.layer >= 0) {
                layers.Insert(this.layer+1, blankLayer);
                layer++;
            }
            else {
                layers.Add(blankLayer);
                layer = 0;
            }
            return blankLayer;
        }

        public void deleteLayer(int layer)
        {
            layers.RemoveAt(layer);
            if(this.layer >= layers.Count)
            {
                this.layer = layers.Count - 1;
            }
        }
        
        public List<Layer> getVisibleLayers()
        {
            List<Layer> visibleLayers = new List<Layer>();
            foreach(Layer layer in this.layers)
            {
                if(layer.visible)
                {
                    visibleLayers.Add(layer);
                }
            }
            return visibleLayers;
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