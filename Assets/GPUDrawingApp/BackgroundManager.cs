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

        public static RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            return blankLayer;
        }

        void Start()
        {
            canvasLayer = Dungeonshop.LayerManager.Instance.getCurrentLayer().background;
            int initBackgroundKernel = drawShader.FindKernel("InitBackground");
            drawShader.SetTexture(initBackgroundKernel, "canvas", canvasLayer);
            drawShader.SetFloat("canvasWidth", canvasLayer.width);
            drawShader.SetFloat("canvasHeight", canvasLayer.height);
            drawShader.SetTexture(initBackgroundKernel, "brushColor", brushColor);
            drawShader.GetKernelThreadGroupSizes(initBackgroundKernel,
                out uint xGroupSize, out uint yGroupSize, out _);
            drawShader.Dispatch(initBackgroundKernel,
                Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                1);
            drawShader.Dispatch(initBackgroundKernel,
                Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                1);


            previousMousePosition = Input.mousePosition;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                foreach (Layer layer in Dungeonshop.LayerManager.Instance.layers)
                {
                    canvasLayer = layer.background;
                    int updateKernel = drawShader.FindKernel("Update");
                    drawShader.SetVector("_PreviousMousePosition", previousMousePosition);
                    drawShader.SetVector("_MousePosition", Input.mousePosition);
                    drawShader.SetBool("_MouseDown", Input.GetMouseButton(0));
                    drawShader.SetFloat("_BrushSize", brushSize);
                    drawShader.SetTexture(updateKernel, "brushColor", brushColor);
                    drawShader.SetFloat("_StrokeSmoothingInterval", interpolationInterval);
                    drawShader.SetTexture(updateKernel, "canvas", canvasLayer);
                    drawShader.SetFloat("canvasWidth", canvasLayer.width);
                    drawShader.SetFloat("canvasHeight", canvasLayer.height);

                    drawShader.GetKernelThreadGroupSizes(updateKernel,
                        out uint xGroupSize, out uint yGroupSize, out _);
                    drawShader.Dispatch(updateKernel,
                        Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                        Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                        1);
                    if (layer.Equals(Dungeonshop.LayerManager.Instance.getCurrentLayer()))
                    {
                        break;
                    }
                }
                
            }
            previousMousePosition = Input.mousePosition;


        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(canvasLayer, dest);
        }
    }
}