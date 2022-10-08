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
        [SerializeField] float brushSize = 10f;
        [SerializeField] BrushSizeSlider brushSlider;
        [SerializeField, Range(0.01f, 1)] float interpolationInterval = 0.1f;
        Vector4 previousMousePosition;
        RenderTexture canvasLayer;

        public static RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(Screen.width, Screen.height, 24);
            blankLayer.filterMode = FilterMode.Point;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            return blankLayer;
        }

        void Start()
        {
            canvasLayer = Dungeonshop.LayerManager.instance.getCurrentLayer().background;
            brushSlider.slider.SetValueWithoutNotify(brushSize);
            int initBackgroundKernel = drawShader.FindKernel("InitBackground");
            drawShader.SetVector("_BackgroundColour", backgroundColor);
            drawShader.SetTexture(initBackgroundKernel, "_Canvas", canvasLayer);
            drawShader.SetFloat("_CanvasWidth", canvasLayer.width);
            drawShader.SetFloat("_CanvasHeight", canvasLayer.height);
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
            canvasLayer = Dungeonshop.LayerManager.instance.getCurrentLayer().background;
            if (!brushSlider.isInUse && Input.GetMouseButton(0))
            {
                int updateKernel = drawShader.FindKernel("Update");
                drawShader.SetVector("_PreviousMousePosition", previousMousePosition);
                drawShader.SetVector("_MousePosition", Input.mousePosition);
                drawShader.SetBool("_MouseDown", Input.GetMouseButton(0));
                drawShader.SetFloat("_BrushSize", brushSize);
                drawShader.SetTexture(updateKernel, "_BrushColour", brushColor);
                drawShader.SetFloat("_StrokeSmoothingInterval", interpolationInterval);
                drawShader.SetTexture(updateKernel, "_Canvas", canvasLayer);
                drawShader.SetFloat("_CanvasWidth", canvasLayer.width);
                drawShader.SetFloat("_CanvasHeight", canvasLayer.height);

                drawShader.GetKernelThreadGroupSizes(updateKernel,
                    out uint xGroupSize, out uint yGroupSize, out _);
                drawShader.Dispatch(updateKernel,
                    Mathf.CeilToInt(canvasLayer.width / (float)xGroupSize),
                    Mathf.CeilToInt(canvasLayer.height / (float)yGroupSize),
                    1);
            }

            previousMousePosition = Input.mousePosition;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(canvasLayer, dest);
        }

        public void OnBrushSizeChanged(float newValue)
        {
            brushSize = newValue;
        }
    }
}