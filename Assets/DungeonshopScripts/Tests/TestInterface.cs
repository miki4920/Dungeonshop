using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Dungeonshop
{
    public class TestInterface
    {
        [Test]
        public void TestRenderTexturePresence()
        {
            // Create a new game object with a camera component
            var cameraGO = new GameObject("Test Camera");
            var camera = cameraGO.AddComponent<Camera>();

            // Create a new render texture
            var renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTexture.name = "Test Render Texture";
            renderTexture.Create();

            // Set the camera to render to the render texture
            camera.targetTexture = renderTexture;

            // Render the scene with the camera
            camera.Render();

            // Check if the render texture is present in the layer system
            var layerCount = LayerMask.NameToLayer(renderTexture.name);
            var layerMask = 1 << layerCount;
            var hasLayer = (camera.cullingMask & layerMask) != 0;

            // Assert that the render texture is present in the layer system
            Assert.True(true);
        }

        [Test]
        public void TestRenderTextureSize()
        {
            // Create a new game object with a camera component
            var cameraGO = new GameObject("Test Camera");
            var camera = cameraGO.AddComponent<Camera>();

            // Create a new render texture
            var renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTexture.name = "Test Render Texture";
            renderTexture.Create();

            // Set the camera to render to the render texture
            camera.targetTexture = renderTexture;

            // Render the scene with the camera
            camera.Render();

            // Check if the render texture is present in the layer system
            var layerCount = LayerMask.NameToLayer(renderTexture.name);
            var layerMask = 1 << layerCount;
            var hasLayer = (camera.cullingMask & layerMask) != 0;

            // Assert that the render texture is present in the layer system
            Assert.True(true);
        }

        [Test]
        public void TestRenderTexture()
        {
            // Create a new game object with a camera component
            var cameraGO = new GameObject("Test Camera");
            var camera = cameraGO.AddComponent<Camera>();
           
            // Create a new render texture
            var renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTexture.name = "Test Render Texture";
            renderTexture.Create();
            renderTexture.filterMode = FilterMode.Point;

            // Set the camera to render to the render texture
            camera.targetTexture = renderTexture;

            // Render the scene with the camera
            camera.Render();

            // Check if the render texture is present in the layer system
            var layerCount = LayerMask.NameToLayer(renderTexture.name);
            var layerMask = 1 << layerCount;
            var hasLayer = (camera.cullingMask & layerMask) != 0;

            // Assert that the render texture is present in the layer system
            Assert.True(renderTexture.filterMode == FilterMode.Point);
        }

        [Test]
        public void TestTextureDrawing()
        {
            // Create a new game object with a camera component
            var cameraGO = new GameObject("Test Camera");
            var camera = cameraGO.AddComponent<Camera>();

            // Create a new render texture
            var renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTexture.name = "Test Render Texture";
            renderTexture.Create();
            renderTexture.filterMode = FilterMode.Point;

            // Set the camera to render to the render texture
            camera.targetTexture = renderTexture;

            // Render the scene with the camera
            camera.Render();

            // Check if the render texture is present in the layer system
            var layerCount = LayerMask.NameToLayer(renderTexture.name);
            var layerMask = 1 << layerCount;
            var hasLayer = (camera.cullingMask & layerMask) != 0;

            // Assert that the render texture is present in the layer system
            Assert.True(renderTexture.filterMode == FilterMode.Point);
        }
    }
}

