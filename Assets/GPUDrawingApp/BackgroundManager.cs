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
        Vector4 previousMousePosition;
        RenderTexture canvasLayer;
        Texture2D whiteTexture;

        public static RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            return blankLayer;
        }

        public static Texture2D createWhiteTexture()
        {
            Texture2D temporaryWhiteTexture = new Texture2D(Screen.width, Screen.height);
            Color color = new Color(1, 1, 1, 1);
            Color[] pixelArray = temporaryWhiteTexture.GetPixels();
            for(int i = 0; i < pixelArray.Length; i++)
            {
                pixelArray[i] = color;
            }
            temporaryWhiteTexture.SetPixels(pixelArray);
            return temporaryWhiteTexture;
        }

        void Start()
        {
            canvasLayer = createBlankRenderTexture();
            whiteTexture = createWhiteTexture();
            foreach (Layer layer in Dungeonshop.LayerManager.Instance.layers)
            {
                int applyTextureKernel;
                if (layer.Equals(Dungeonshop.LayerManager.Instance.getCurrentLayer()))
                {
                    applyTextureKernel = drawShader.FindKernel("ApplyTexture");
                }
                else
                {
                    applyTextureKernel = drawShader.FindKernel("ApplyTextureTransparent");
                }
                canvasLayer = layer.background;
                drawShader.SetTexture(applyTextureKernel, "canvas", canvasLayer);
                drawShader.SetFloat("canvasWidth", canvasLayer.width);
                drawShader.SetFloat("canvasHeight", canvasLayer.height);
                drawShader.SetTexture(applyTextureKernel, "overlayTexture", whiteTexture);
                drawShader.GetKernelThreadGroupSizes(applyTextureKernel,
                    out uint xGroupSize, out uint yGroupSize, out _);
                drawShader.Dispatch(applyTextureKernel,
                    Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                    Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                    1);
                drawShader.Dispatch(applyTextureKernel,
                    Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                    Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                    1);
            }
            previousMousePosition = Input.mousePosition;
        }

        void uniteLayers()
        {
            for (int i = 0; i <= Dungeonshop.LayerManager.Instance.layer; i++)
            {
                int applyTextureKernel = drawShader.FindKernel("ApplyTexture");
                RenderTexture overlayLayer = Dungeonshop.LayerManager.Instance.layers[i].background;
                drawShader.SetTexture(applyTextureKernel, "canvas", canvasLayer);
                drawShader.SetTexture(applyTextureKernel, "overlayTexture", overlayLayer);
                drawShader.SetFloat("canvasWidth", canvasLayer.width);
                drawShader.SetFloat("canvasHeight", canvasLayer.height);
                drawShader.GetKernelThreadGroupSizes(applyTextureKernel,
                    out uint xGroupSize, out uint yGroupSize, out _);
                drawShader.Dispatch(applyTextureKernel,
                    Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                    Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                    1);
            }
        }


        void Update()
        {
            uniteLayers();
            if (Input.GetMouseButton(0))
            {
                int updateKernel = drawShader.FindKernel("Update");
                drawShader.SetVector("previousMousePosition", previousMousePosition);
                drawShader.SetVector("mousePosition", Input.mousePosition);
                drawShader.SetFloat("brushSize", brushSize);
                drawShader.SetTexture(updateKernel, "overlayTexture", brushColor);
                drawShader.SetFloat("strokeSmoothingInterval", interpolationInterval);
                drawShader.SetTexture(updateKernel, "canvas", Dungeonshop.LayerManager.Instance.getCurrentLayer().background);
                drawShader.SetFloat("canvasWidth", Dungeonshop.LayerManager.Instance.getCurrentLayer().background.width);
                drawShader.SetFloat("canvasHeight", Dungeonshop.LayerManager.Instance.getCurrentLayer().background.height);
                drawShader.GetKernelThreadGroupSizes(updateKernel,
                    out uint xGroupSize, out uint yGroupSize, out _);
                drawShader.Dispatch(updateKernel,
                    Mathf.CeilToInt(Dungeonshop.LayerManager.Instance.getCurrentLayer().background.width / (float)xGroupSize),
                    Mathf.CeilToInt(Dungeonshop.LayerManager.Instance.getCurrentLayer().background.height / (float)yGroupSize),
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