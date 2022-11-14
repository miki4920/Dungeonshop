using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class ShaderManager : MonoBehaviour
    {
        public static ShaderManager Instance;
        public ComputeShader layerShader;
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

        void dispatchShader(RenderTexture layer, int kernel)
        {
            layerShader.GetKernelThreadGroupSizes(kernel,
                out uint xGroupSize, out uint yGroupSize, out _);
            layerShader.Dispatch(kernel,
                Mathf.CeilToInt(layer.width / (float)xGroupSize),
                Mathf.CeilToInt(layer.height / (float)yGroupSize),
                1);
        }

        public void applyTexture(string kernelName, RenderTexture layer, Texture overlayLayer = null, RenderTexture maskLayer = null, float opacity = 0, float size = 0, Vector4? previousMousePosition = null, Vector4? mousePosition = null, Color? overlayColor = null)
        {
            int kernel = layerShader.FindKernel(kernelName);
            layerShader.SetTexture(kernel, "canvas", layer);
            layerShader.SetFloat("opacity", opacity);
            layerShader.SetFloat("size", size);
            if (overlayLayer != null)
            {
                layerShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            }
            if (maskLayer != null)
            {
                layerShader.SetTexture(kernel, "mask", maskLayer);
            }
            if (overlayColor != null)
            {
                layerShader.SetVector("overlayColor", overlayColor.Value);
            }
            if(previousMousePosition != null)
            {
                layerShader.SetVector("previousMousePosition", previousMousePosition.Value);
            }
            if(mousePosition != null)
            {
                layerShader.SetVector("mousePosition", mousePosition.Value);
            }
            dispatchShader(layer, kernel);
        }
    }

}