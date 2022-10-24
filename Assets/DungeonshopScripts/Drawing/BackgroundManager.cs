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
        [SerializeField] ComputeShader drawShader;
        Vector4 previousMousePosition;
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
            return blankLayer;
        }

        void dispatchShader(RenderTexture layer, int kernel)
        {
            drawShader.GetKernelThreadGroupSizes(kernel,
                out uint xGroupSize, out uint yGroupSize, out _);
            drawShader.Dispatch(kernel,
                Mathf.CeilToInt(layer.width / (float)xGroupSize),
                Mathf.CeilToInt(layer.height / (float)yGroupSize),
                1);
        }

        public void applyTexture(RenderTexture layer, RenderTexture overlayLayer)
        {
            int kernel = drawShader.FindKernel("ApplyTexture");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            dispatchShader(layer, kernel);
        }

        public void applyTextureWithNoLerp(RenderTexture layer, RenderTexture overlayLayer)
        {
            int kernel = drawShader.FindKernel("ApplyTextureWithNoLerp");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            dispatchShader(layer, kernel);
        }

        public void applyWhiteTexture(RenderTexture layer, int opacity)
        {
            int kernel = drawShader.FindKernel("ApplyWhiteTexture");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetFloat("opacity", opacity);
            dispatchShader(layer, kernel);
        }

        void Start()
        {
            canvasLayer = createBlankRenderTexture();
            displayLayer = createBlankRenderTexture();
            maskLayer = createBlankRenderTexture();
            gameObject.GetComponent<RawImage>().texture = displayLayer;
            applyWhiteTexture(maskLayer, 0);
            previousMousePosition = Input.mousePosition;
        }


        void uniteLayers()
        {
            applyWhiteTexture(canvasLayer, 1);
            foreach (Layer layer in Dungeonshop.LayerManager.Instance.getVisibleLayers())
            {
                applyTexture(canvasLayer, layer.background);
            }
        }

        void Update()
        {
            applyWhiteTexture(displayLayer, 0);
            if (Dungeonshop.LayerManager.Instance.getVisibleLayers().Count > 0)
            {
                uniteLayers();
                applyTextureWithNoLerp(displayLayer, canvasLayer);
                if (Input.GetMouseButton(0) && Dungeonshop.DrawingAreaInputHandler.Instance.isInsideDrawingArea())
                {
                    float size = Dungeonshop.UI.BrushSelectorManager.Instance.getSize();
                    float opacity = Dungeonshop.UI.BrushSelectorManager.Instance.getOpacity();
                    int kernel = 0;
                    RenderTexture layer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
                    switch (Dungeonshop.UI.BrushSelectorManager.Instance.drawingMode)
                    {
                        case DrawingMode.Color:
                            {

                                kernel = drawShader.FindKernel("UpdateColor");
                                drawShader.SetTexture(kernel, "originalLayer", canvasLayer);
                                drawShader.SetVector("overlayColor", Dungeonshop.UI.BrushSelectorManager.Instance.color);
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                kernel = drawShader.FindKernel("UpdateTexture");
                                drawShader.SetTexture(kernel, "originalLayer", canvasLayer);
                                drawShader.SetTexture(kernel, "overlayTexture", Dungeonshop.UI.BrushSelectorManager.Instance.texture);
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                kernel = drawShader.FindKernel("UpdateEraser");
                                break;
                            }
                        default: break;
                    }
                    drawShader.SetTexture(kernel, "canvas", maskLayer);
                    drawShader.SetVector("previousMousePosition", previousMousePosition);
                    drawShader.SetVector("mousePosition", Dungeonshop.DrawingAreaInputHandler.Instance.mousePosition());
                    drawShader.SetFloat("brushOpacity", opacity);
                    drawShader.SetFloat("brushSize", size);

                    dispatchShader(layer, kernel);
                    applyTexture(displayLayer, maskLayer);
                    dispatchShader(layer, kernel);
                    
                }
                else
                {
                    applyTexture(Dungeonshop.LayerManager.Instance.getCurrentLayer().background, maskLayer);
                    applyWhiteTexture(maskLayer, 0);
                }
                previousMousePosition = Dungeonshop.DrawingAreaInputHandler.Instance.mousePosition();
            }

        }


    }
}