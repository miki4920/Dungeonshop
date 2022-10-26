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

        public void applyTexture(string kernelName, RenderTexture layer, RenderTexture overlayLayer = null, float opacity = 0, float size = 0, float previousMousePosition = 0, float mousePosition = 0, Color? overlayColor = null)
        {
            int kernel = layerShader.FindKernel(kernelName);
            layerShader.SetTexture(kernel, "canvas", layer);
            layerShader.SetFloat("opacity", opacity);
            layerShader.SetFloat("size", size);
            layerShader.SetFloat("previousMousePosition", previousMousePosition);
            layerShader.SetFloat("mousePosition", mousePosition);
            if (overlayLayer != null)
            {
                layerShader.SetTexture(kernel, "overlayTexture", overlayLayer);
            }
            if (overlayColor != null)
            {
                layerShader.SetVector("overlayColor", overlayColor.Value);
            }
            dispatchShader(layer, kernel);
        }
    }

}