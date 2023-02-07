using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop.UI
{

    public enum Mode
    {
        Drawing,
        Light,
        Selection
    }
    public class DrawingAreaInputHandler : MonoBehaviour
    {
        [HideInInspector] public static DrawingAreaInputHandler Instance;

        bool holdingOutsideDrawingArea;
        bool shifting;
        Mode mode;
        float size;

        [HideInInspector] public Vector3 mousePosition;
        public bool snap;
        [SerializeField] Checkbox snapCheckbox;
        [HideInInspector] public Vector3 mousePositionRelative;
        [HideInInspector] public bool insideDrawingArea;
        [HideInInspector] public bool isLeftClickPressed;
        [HideInInspector] public bool clickOnce;
        [HideInInspector] public bool isMiddleClickPressed;
        [HideInInspector] public bool isControlClicked;
        [HideInInspector] public Vector3 previousMousePosition;
        [HideInInspector] public Vector3 previousMousePositionRelative;
        [SerializeField] RectTransform drawingAreaTransform;
        [SerializeField] RectTransform viewingPortTransform;
        [SerializeField] LightHandler lightHandler;

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

        private void Start()
        {
            holdingOutsideDrawingArea = false;
            size = 1;
        }

        private Vector3 snapToGrid(Vector3 position)
        {
            if (snap)
            {
                float gridSize = 128 * size;
                return new Vector3(Mathf.Round(position.x / gridSize) * gridSize, Mathf.Round(position.y / gridSize) * gridSize, 0);
            }
            return position;
        }

        private void Update()
        {
            snap = snapCheckbox.checkValue;
            mousePosition = Input.mousePosition;
            mousePosition = snapToGrid(mousePosition);
            mousePositionRelative = snapToGrid(getMousePosition());
            insideDrawingArea = isInsideDrawingArea();
            isLeftClickPressed = Input.GetMouseButton(0);
            isMiddleClickPressed = Input.GetMouseButton(2);
            isControlClicked = Input.GetKey(KeyCode.LeftControl);
            BackgroundManager.Instance.uniteLayers();
            updateLights(mode == Mode.Selection);
            if (mode == Mode.Drawing)
            {
                BackgroundManager.Instance.UpdateBackground();
            }
            
            else if (mode == Mode.Light && insideDrawingArea && !isLeftClickPressed && lightHandler.lightInstance == null && lightHandler.lightMode == LightMode.Light)
            {
                lightHandler.createLight(size);
            }
            else if (mode == Mode.Light && insideDrawingArea && !isLeftClickPressed && lightHandler.lightInstance != null && lightHandler.lightMode == LightMode.Light)
            {
                lightHandler.lightInstance.GetComponent<ObjectInformation>().updatePosition(mousePosition);
            }
            else if (mode == Mode.Light && insideDrawingArea && isLeftClickPressed && lightHandler.lightInstance != null && lightHandler.lightMode == LightMode.Light)
            {
                Layer layer = CanvasManager.Instance.getCurrentLayer();
                if (layer.visible)
                {
                    layer.objects.Add(lightHandler.lightInstance);
                    lightHandler.lightInstance = null;
                }
            }

            if (mode == Mode.Light && lightHandler.lightInstance != null && lightHandler.lightMode == LightMode.Light)
            {
                lightHandler.updateLight();
            }
            if ((mode != Mode.Light || lightHandler.lightMode != LightMode.Light) && lightHandler.lightInstance != null)
            {
                Destroy(lightHandler.lightInstance);
            }
            if (mode == Mode.Selection)
            {
                SelectionManager.Instance.updateSelection();
            }
            if (mode == Mode.Selection && !isLeftClickPressed)
            {
                SelectionManager.Instance.isObjectSet = false;
            }
            if (mode == Mode.Selection && insideDrawingArea && isLeftClickPressed && SelectionManager.Instance.selectedObject != null && !SelectionManager.Instance.isObjectSet)
            {
                SelectionManager.Instance.selectedObject = null;
            }
            if (mode == Mode.Selection && insideDrawingArea && isLeftClickPressed && SelectionManager.Instance.selectedObject != null && SelectionManager.Instance.isObjectSet)
            {
                SelectionManager.Instance.selectedObject.GetComponent<ObjectInformation>().updatePosition(mousePosition);
            }
            if (mode != Mode.Selection && SelectionManager.Instance.selectedObject != null)
            {
                SelectionManager.Instance.selectedObject = null;
                SelectionManager.Instance.mode = SelectionMode.None;
            }

            shiftScreen();
            previousMousePosition = mousePosition;
            previousMousePositionRelative = mousePositionRelative;
        }

        public void changeMode(int newMode)
        {
            mode = (Mode) newMode;
        }

        public void updateLights(bool selectionMode)
        {
            foreach (Layer layer in CanvasManager.Instance.layers)
            {
                foreach (GameObject layerObject in layer.objects)
                {
                    layerObject.SetActive(false);
                    layerObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            foreach (Layer layer in CanvasManager.Instance.getVisibleLayers())
            {
                foreach (GameObject layerObject in layer.objects)
                {
                    layerObject.SetActive(true);
                    if (selectionMode)
                    {
                        layerObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }

        public bool isInsideDrawingArea()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            float minX = Math.Max(drawingAreaPosition.x - (drawingAreaTransform.rect.width / 2), viewingPortTransform.position.x - viewingPortTransform.rect.width / 2);
            float maxX = Math.Min(drawingAreaPosition.x + (drawingAreaTransform.rect.width / 2), viewingPortTransform.position.x + viewingPortTransform.rect.width / 2);
            float minY = Math.Max(drawingAreaPosition.y - (drawingAreaTransform.rect.height / 2), viewingPortTransform.position.y - viewingPortTransform.rect.height / 2);
            float maxY = Math.Min(drawingAreaPosition.y + (drawingAreaTransform.rect.height / 2), viewingPortTransform.position.y + viewingPortTransform.rect.height / 2);
            bool insideAreaX = Input.mousePosition.x <= maxX && Input.mousePosition.x >= minX;
            bool insideAreaY = Input.mousePosition.y <= maxY && Input.mousePosition.y >= minY;
            if (isLeftClickPressed && (!insideAreaX || !insideAreaY))
            {
                holdingOutsideDrawingArea = true;
                return false;
            }
            else if(holdingOutsideDrawingArea && insideAreaX && insideAreaY && !isLeftClickPressed)
            {
                holdingOutsideDrawingArea = false;
                return false;
            }
            else if(insideAreaX && insideAreaY && !holdingOutsideDrawingArea)
            {
                return true;
            }
            return false;
        }

        public Vector3 getMousePosition()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            float drawingAreaX = drawingAreaPosition.x - (drawingAreaTransform.rect.width / 2);
            float drawingAreaY = drawingAreaPosition.y - (drawingAreaTransform.rect.height / 2);
            float globalPositionX = Input.mousePosition.x;
            float globalPositionY = Input.mousePosition.y;
            globalPositionX = (BackgroundManager.Instance.width) / (drawingAreaTransform.rect.width) * (globalPositionX - drawingAreaX);
            globalPositionY = (BackgroundManager.Instance.height) / (drawingAreaTransform.rect.height) * (globalPositionY - drawingAreaY);
            return new Vector3(globalPositionX, globalPositionY, 0);
        }

        public void shiftScreen()
        {
            if(isMiddleClickPressed && !shifting)
            {
                shifting = true;
            }
            else if(isMiddleClickPressed && shifting)
            {
                drawingAreaTransform.SetPositionAndRotation(drawingAreaTransform.position + (mousePosition - previousMousePosition), drawingAreaTransform.rotation);
            }
            else if(!isMiddleClickPressed && shifting)
            {
                shifting = false;
            }
        }

        public void determineScroll()
        {
            float mouseDelta = Input.mouseScrollDelta.y;
            if (isControlClicked && lightHandler.lightInstance != null)
            {
                lightHandler.updateRotation(mouseDelta);

            }
            else
            {
                changeSize(mouseDelta);
            }
        }

        public void changeSize(float mouseDelta)
        {
            
            mouseDelta = mouseDelta >= 0 ? 1.1f : 0.9f;
            size *= mouseDelta;
            float scalar = size / drawingAreaTransform.GetComponent<ObjectInformation>().size;
            drawingAreaTransform.GetComponent<ObjectInformation>().size = size;
            Vector2 newDimensions = new Vector2(drawingAreaTransform.rect.width * scalar, drawingAreaTransform.rect.height * scalar);
            drawingAreaTransform.gameObject.GetComponent<RectTransform>().sizeDelta = newDimensions;
            foreach(Transform drawingAreaChild in drawingAreaTransform.transform)
            {
                drawingAreaChild.transform.localPosition = new Vector3(drawingAreaChild.transform.localPosition.x * mouseDelta, drawingAreaChild.transform.localPosition.y * mouseDelta, 0);
                Light2D light = drawingAreaChild.GetComponent<Light2D>();
                float lightScalar = size / drawingAreaChild.GetComponent<ObjectInformation>().size;
                drawingAreaChild.GetComponent<ObjectInformation>().size = size;
                light.pointLightInnerRadius = light.pointLightInnerRadius * lightScalar;
                light.pointLightOuterRadius = light.pointLightOuterRadius * lightScalar;
            }
        }
    }
}

