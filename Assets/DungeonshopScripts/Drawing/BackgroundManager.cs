using UnityEngine;
using UnityEngine.UI;
using Dungeonshop;

namespace Dungeonshop
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance;
        public RawImage canvas;
        public int width;
        public int height;
        RenderTexture canvasLayer;
        RenderTexture displayLayer;
        RenderTexture maskLayer;
        RenderTexture gridLayer;
        public Checkbox showGridCheckbox;
        public bool wasLeftClickPressed;

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
            canvasLayer = createBlankRenderTexture();
            displayLayer = createBlankRenderTexture();
            maskLayer = createBlankRenderTexture();
            gridLayer = createBlankRenderTexture();
            ShaderManager.Instance.applyTexture("ApplyGrid", gridLayer, overlayColor: Color.black);
            canvas.texture = canvasLayer;
        }

        public RenderTexture createBlankRenderTexture()
        {
            RenderTexture blankLayer = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            blankLayer.filterMode = FilterMode.Bilinear;
            blankLayer.enableRandomWrite = true;
            blankLayer.Create();
            ShaderManager.Instance.applyTexture("ApplyWhiteTexture", blankLayer, opacity: 0);
            return blankLayer;
        }

        public void uniteLayers()
        {
            ShaderManager.Instance.applyTexture("ApplyWhiteTexture", canvasLayer, opacity: CanvasManager.Instance.getVisibleLayers().Count > 0 ? 1 : 0);
            ShaderManager.Instance.applyTexture("ApplyWhiteTexture", displayLayer, opacity: 0);
            foreach (Layer layer in CanvasManager.Instance.getVisibleLayers())
            {
                if (layer == CanvasManager.Instance.getCurrentLayer())
                {
                    ShaderManager.Instance.applyTexture("ApplyTexture", displayLayer, overlayLayer: canvasLayer);
                    ShaderManager.Instance.applyTexture("ApplyTexture", displayLayer, overlayLayer: layer.background);
                    switch (BrushSelectorManager.Instance.drawingMode)
                    {

                        case DrawingMode.Color:
                            {
                                ShaderManager.Instance.applyTexture("ApplyColorBasedOnMask", displayLayer, overlayColor: BrushSelectorManager.Instance.color, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                ShaderManager.Instance.applyTexture("ApplyTextureBasedOnMask", displayLayer, overlayLayer: BrushSelectorManager.Instance.texture, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                ShaderManager.Instance.applyTexture("ApplyEraserBasedOnMask", displayLayer, maskLayer: maskLayer);
                                break;
                            }
                        default: break;
                    }
                    ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: displayLayer);
                }
                else
                {
                    ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: layer.background);
                }
            }
            if(showGridCheckbox.checkValue)
            {
                ShaderManager.Instance.applyTexture("ApplyTexture", canvasLayer, overlayLayer: gridLayer);
            }
        }

        public void UpdateBackground()
        {
            DrawingAreaInputHandler inputBoard = DrawingAreaInputHandler.Instance;
            if (CanvasManager.Instance.getVisibleLayers().Count > 0)
            {

                if (inputBoard.insideDrawingArea && inputBoard.isLeftClickPressed)
                {
                    uniteLayers();
                    float size = BrushSelectorManager.Instance.getSize();
                    float opacity = BrushSelectorManager.Instance.getOpacity();
                    ShaderManager.Instance.applyTexture("UpdateMask", maskLayer, size: size, opacity: opacity, previousMousePosition: inputBoard.previousMousePositionRelative, mousePosition: inputBoard.mousePositionRelative);
                    wasLeftClickPressed = true;
                }
                else if(!inputBoard.isLeftClickPressed && wasLeftClickPressed)
                {
                    wasLeftClickPressed = false;
                    CanvasManager.Instance.addUndoHistory();
                    RenderTexture layer = CanvasManager.Instance.getCurrentLayer().background;
                    switch (BrushSelectorManager.Instance.drawingMode)
                    {
                        
                        case DrawingMode.Color:
                            {
                                ShaderManager.Instance.applyTexture("ApplyColorBasedOnMask", layer, overlayLayer: BrushSelectorManager.Instance.texture, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Texture:
                            {
                                ShaderManager.Instance.applyTexture("ApplyTextureBasedOnMask", layer, overlayColor: BrushSelectorManager.Instance.color, maskLayer: maskLayer);
                                break;
                            }
                        case DrawingMode.Eraser:
                            {
                                ShaderManager.Instance.applyTexture("ApplyEraserBasedOnMask", layer, maskLayer: maskLayer);
                                break;
                            }
                        default: break;
                    }
                    ShaderManager.Instance.applyTexture("ApplyWhiteTexture", maskLayer, opacity: 0);
                }
            }
        }


    }
}