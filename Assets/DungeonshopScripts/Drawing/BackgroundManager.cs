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
            gameObject.GetComponent<RawImage>().texture = canvasLayer;
            previousMousePosition = Input.mousePosition;
        }


        void uniteLayers()
        {
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyWhiteTexture", canvasLayer, opacity: 1);
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyWhiteTexture", displayLayer, opacity: 0);
            Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTexture", displayLayer, overlayLayer: Dungeonshop.LayerManager.Instance.getCurrentLayer().background);
            foreach (Layer layer in Dungeonshop.LayerManager.Instance.getVisibleLayers())
            {
                
                if (layer == Dungeonshop.LayerManager.Instance.getCurrentLayer())
                {
                    switch (Dungeonshop.UI.BrushSelectorManager.Instance.drawingMode)
                    {

                        case DrawingMode.Color:
                            {
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTextureBasedOnMask", displayLayer, overlayLayer: Dungeonshop.UI.BrushSelectorManager.Instance.texture, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                Dungeonshop.ShaderManager.Instance.applyTexture("ApplyEraserBasedOnMask", displayLayer, maskLayer: maskLayer);
                                break;
                            }
                        default: break;
                    }
                    Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: displayLayer);
                }
                else
                {
                    Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: layer.background);
                }
            }
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
                    Dungeonshop.ShaderManager.Instance.applyTexture("UpdateMask", maskLayer, size: size, opacity: opacity, previousMousePosition: previousMousePosition, mousePosition: mousePosition);
                }
                else if(!Input.GetMouseButton(0))
                {
                    RenderTexture layer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
                    switch (Dungeonshop.UI.BrushSelectorManager.Instance.drawingMode)
                    {
                        
                        case DrawingMode.Color:
                            {
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                Dungeonshop.ShaderManager.Instance.applyTexture("ApplyTextureBasedOnMask", layer, overlayLayer: Dungeonshop.UI.BrushSelectorManager.Instance.texture, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                Dungeonshop.ShaderManager.Instance.applyTexture("ApplyEraserBasedOnMask", layer, maskLayer: maskLayer);
                                break;
                            }
                        default: break;
                    }
                    Dungeonshop.ShaderManager.Instance.applyTexture("ApplyWhiteTexture", maskLayer, opacity: 0);
                }
                previousMousePosition = Dungeonshop.DrawingAreaInputHandler.Instance.mousePosition();
            }

        }


    }
}