using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeonshop;

namespace Dungeonshop
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] ComputeShader drawShader;
        [SerializeField] Texture2D brushColor;
        [SerializeField, Range(1, 1000)] float brushSize = 10f;
        [SerializeField, Range(0.01f, 1)] float interpolationInterval = 0.01f;
        [SerializeField, Range(0.01f, 1)] float brushOpacity = 1;
        Vector4 previousMousePosition;
        RenderTexture canvasLayer;
        RenderTexture maskLayer;

        public static RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            return blankLayer;
        }

        void applyTexture(RenderTexture layer, RenderTexture overlayLayer, int kernel)
        {
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            drawShader.SetFloat("canvasWidth", layer.width);
            drawShader.SetFloat("canvasHeight", layer.height);
            drawShader.GetKernelThreadGroupSizes(kernel,
                out uint xGroupSize, out uint yGroupSize, out _);
            drawShader.Dispatch(kernel,
                Mathf.CeilToInt(layer.width / (float)xGroupSize),
                Mathf.CeilToInt(layer.height / (float)yGroupSize),
                1);
        }

        void applyWhiteTexture(RenderTexture layer, int opacity)
        {
            int kernel = drawShader.FindKernel("ApplyWhiteTexture");
            drawShader.SetTexture(kernel, "canvas", layer);
            drawShader.SetFloat("canvasWidth", layer.width);
            drawShader.SetFloat("canvasHeight", layer.height);
            drawShader.SetFloat("opacity", opacity);
            drawShader.GetKernelThreadGroupSizes(kernel,
                out uint xGroupSize, out uint yGroupSize, out _);
            drawShader.Dispatch(kernel,
                Mathf.CeilToInt(layer.width / (float)xGroupSize),
                Mathf.CeilToInt(layer.height / (float)yGroupSize),
                1);
        }

        void Start()
        {
            canvasLayer = createBlankRenderTexture();
            maskLayer = createBlankRenderTexture();
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
            int kernel = drawShader.FindKernel("ApplyTexture");
            for (int i = 0; i <= Dungeonshop.LayerManager.Instance.layer; i++)
            {
                Layer layer = Dungeonshop.LayerManager.Instance.layers[i];
                applyTexture(canvasLayer, layer.background, kernel);
                if (i == Dungeonshop.LayerManager.Instance.layer)
                {
                    break;
                }
            }
        }

        void Update()
        {
            uniteLayers();
            if (Input.GetMouseButton(0))
            {
                RenderTexture layer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
                int kernel = drawShader.FindKernel("Update");
                drawShader.SetVector("previousMousePosition", previousMousePosition);
                drawShader.SetVector("mousePosition", Input.mousePosition);
                drawShader.SetFloat("brushSize", brushSize);
                drawShader.SetTexture(kernel, "overlayTexture", brushColor);
                drawShader.SetFloat("strokeSmoothingInterval", interpolationInterval);
                drawShader.SetTexture(kernel, "canvas", layer);
                drawShader.SetFloat("canvasWidth", layer.width);
                drawShader.SetFloat("canvasHeight", layer.height);
                drawShader.SetFloat("brushOpacity", brushOpacity);
                drawShader.GetKernelThreadGroupSizes(kernel,
                    out uint xGroupSize, out uint yGroupSize, out _);
                drawShader.Dispatch(kernel,
                    Mathf.CeilToInt(layer.width / (float)xGroupSize),
                    Mathf.CeilToInt(layer.height / (float)yGroupSize),
                    1);
            }
            previousMousePosition = Input.mousePosition;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(canvasLayer, dest);
        }
    }
}