using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dungeonshop;
using UnityEngine.Rendering.Universal;
using System;

namespace Dungeonshop
{
    public class CanvasManager : ColorReceiver
    {
        public static CanvasManager Instance;
        [HideInInspector] public List<Layer> layers = new List<Layer>();
        public GameObject environmentLight;
        [HideInInspector] public int layer;
        Dictionary<Guid,Stack<RenderTexture>> layerUndoDictionary;
        public Guid InstanceID;
        Stack<Guid> layerUndoList;

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
                layerUndoList = new Stack<Guid>();
                layerUndoDictionary = new Dictionary<Guid, Stack<RenderTexture>>();
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
            blankLayer.layerID = Guid.NewGuid();
            layerUndoDictionary[blankLayer.layerID] = new Stack<RenderTexture>();
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

        public void addUndoHistory()
        {
            Layer layer = getCurrentLayer();
            layerUndoList.Push(layer.layerID);
            layerUndoDictionary[layer.layerID].Push(copyRenderTexture(layer.background));
        }

        public void undo()
        {
            if(layerUndoList.Count == 0)
            {
                return;
            }
            Guid guid = layerUndoList.Pop();
            for (int i = 0; i < layers.Count; i++) {
                if(layers[i].layerID == guid)
                {
                    layers[i].background.Release();
                    layers[i].background = layerUndoDictionary[guid].Pop();
                    BackgroundManager.Instance.uniteLayers();
                }
            }

        }

        private RenderTexture copyRenderTexture(RenderTexture source)
        {
            RenderTexture copiedRenderTexture = new RenderTexture(source.width, source.height, source.depth, source.graphicsFormat);
            copiedRenderTexture.filterMode = source.filterMode;
            copiedRenderTexture.wrapMode = source.wrapMode;
            copiedRenderTexture.autoGenerateMips = source.autoGenerateMips;
            copiedRenderTexture.useMipMap = source.useMipMap;
            copiedRenderTexture.useDynamicScale = source.useDynamicScale;
            copiedRenderTexture.enableRandomWrite = source.enableRandomWrite;
            copiedRenderTexture.memorylessMode = source.memorylessMode;
            copiedRenderTexture.antiAliasing = source.antiAliasing;
            copiedRenderTexture.anisoLevel = source.anisoLevel;
            copiedRenderTexture.name = source.name;

            Graphics.CopyTexture(source, copiedRenderTexture);

            return copiedRenderTexture;
        }

        public override void updateColor(Color color)
        {
            environmentLight.GetComponent<Light2D>().color = color;
        }
    }
}