using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Dungeonshop;
using Dungeonshop.UI;

namespace Dungeonshop
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance;
        Vector3 previousMousePosition;
        RenderTexture canvasLayer;
        RenderTexture displayLayer;
        RenderTexture maskLayer;

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

        public RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(1152, Screen.height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyWhiteTexture", blankLayer, opacity: 0);
            return blankLayer;
        }
        void Start()
        {
            canvasLayer = createBlankRenderTexture();
            displayLayer = createBlankRenderTexture();
            maskLayer = createBlankRenderTexture();
            gameObject.GetComponent<RawImage>().texture = displayLayer;
            previousMousePosition = Input.mousePosition;
        }


        void uniteLayers()
        {
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyWhiteTexture", canvasLayer, opacity: 0);
            foreach (Layer layer in Dungeonshop.LayerManager.Instance.getVisibleLayers())
            {
                Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: layer.background);
            }
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTexture", displayLayer, overlayLayer: canvasLayer);
        }

        void Update()
        {
            if (Dungeonshop.LayerManager.Instance.getVisibleLayers().Count > 0)
            {
                uniteLayers();
                if (Dungeonshop.DrawingAreaInputHandler.Instance.isInsideDrawingArea())
                {
                    Vector3 mousePosition = Dungeonshop.DrawingAreaInputHandler.Instance.mousePosition();
                    float size = Dungeonshop.UI.BrushSelectorManager.Instance.getSize();
                    float opacity = Dungeonshop.UI.BrushSelectorManager.Instance.getOpacity();
                    RenderTexture layer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
                    switch (Dungeonshop.UI.BrushSelectorManager.Instance.drawingMode)
                    {
                        case DrawingMode.Color:
                            {
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                break;
                            }
                        default: break;
                    }             
                }
                else if(!Input.GetMouseButton(0))
                {

                }
                previousMousePosition = Dungeonshop.DrawingAreaInputHandler.Instance.mousePosition();
            }

        }


    }
}