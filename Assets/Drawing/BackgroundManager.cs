using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeonshop;

namespace Dungeonshop
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] ComputeShader drawShader;
        [SerializeField] Texture2D brushColor;
        float brushSize = 10f;
        [SerializeField, Range(0.01f, 1)] float interpolationInterval = 0.01f;
        float brushOpacity = 1;
        Vector4 previousMousePosition;
        RenderTexture canvasLayer;
        RenderTexture displayLayer;
        RenderTexture maskLayer;

        public static RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            return blankLayer;
        }
        
        public void setBrushSize(float size)
        {
            brushSize = size;
        }

        public void setBrushOpacity(float opacity)
        {
            brushOpacity = opacity/100;
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

        void applyTexture(RenderTexture layer, RenderTexture overlayLayer)
        {
            int kernel = drawShader.FindKernel("ApplyTexture");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            dispatchShader(layer, kernel);
        }

        void applyTextureWithNoLerp(RenderTexture layer, RenderTexture overlayLayer)
        {
            int kernel = drawShader.FindKernel("ApplyTextureWithNoLerp");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            dispatchShader(layer, kernel);
        }

        void applyWhiteTexture(RenderTexture layer, int opacity)
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
            for (int i = 0; i < Dungeonshop.LayerManager.Instance.layers.Count; i++)
            {
                Layer layer = Dungeonshop.LayerManager.Instance.layers[i];
                int opacity;
                if (i == 0)
                {
                    opacity = 1;
                }
                else
                {
                    opacity = 0;
                }
                applyWhiteTexture(layer.background, opacity);
                
            }
            previousMousePosition = Input.mousePosition;
        }


        void uniteLayers()
        {
            for (int i = 0; i <= Dungeonshop.LayerManager.Instance.layer; i++)
            {
                Layer layer = Dungeonshop.LayerManager.Instance.layers[i];
                applyTexture(canvasLayer, layer.background);
            }
        }

        void Update()
        {
            uniteLayers();
            applyTextureWithNoLerp(displayLayer, canvasLayer);
            if (Input.GetMouseButton(0))
            {
                RenderTexture layer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
                int kernel = drawShader.FindKernel("Update");
                drawShader.SetVector("previousMousePosition", previousMousePosition);
                drawShader.SetVector("mousePosition", Input.mousePosition);
                drawShader.SetFloat("brushSize", brushSize);
                drawShader.SetTexture(kernel, "overlayTexture", brushColor);
                drawShader.SetFloat("strokeSmoothingInterval", interpolationInterval);
                drawShader.SetTexture(kernel, "canvas", maskLayer);
                drawShader.SetTexture(kernel, "originalLayer", canvasLayer);
                drawShader.SetFloat("brushOpacity", brushOpacity);
                dispatchShader(layer, kernel);
                applyTexture(displayLayer, maskLayer);
            }
            else
            {
                applyTexture(Dungeonshop.LayerManager.Instance.getCurrentLayer().background, maskLayer);
                applyWhiteTexture(maskLayer, 0);
            }
            previousMousePosition = Input.mousePosition;
        }


    }
}