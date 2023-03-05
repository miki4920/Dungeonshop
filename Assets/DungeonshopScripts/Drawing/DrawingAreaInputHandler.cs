using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dungeonshop
{

    public enum Mode
    {
        Drawing,
        Light,
        Wall,
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
        [HideInInspector] public Vector3 mousePositionGrid;
        public bool snap;
        [SerializeField] Checkbox snapCheckbox;
        [HideInInspector] public Vector3 mousePositionRelative;
        [HideInInspector] public bool insideDrawingArea;
        [HideInInspector] public bool isLeftClickPressed;
        [HideInInspector] public bool isLeftClickClicked;
        [HideInInspector] public bool clickOnce;
        [HideInInspector] public bool isMiddleClickPressed;
        [HideInInspector] public bool isRightClickClicked;
        [HideInInspector] public bool isControlClicked;
        [HideInInspector] public bool isDeleteClicked;
        [HideInInspector] public Vector3 previousMousePosition;
        [HideInInspector] public Vector3 previousMousePositionRelative;
        [SerializeField] RectTransform drawingAreaTransform;
        [SerializeField] RectTransform viewingPortTransform;
        [SerializeField] LightManager lightManager;
        [SerializeField] WallManager wallManager;

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
            drawingAreaTransform.sizeDelta = new Vector2(BackgroundManager.Instance.width, BackgroundManager.Instance.height);
        }

        private Vector3 convertToLocal(Vector3 position)
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            float drawingAreaX = drawingAreaPosition.x - (drawingAreaTransform.rect.width / 2);
            float drawingAreaY = drawingAreaPosition.y - (drawingAreaTransform.rect.height / 2);
            position.x -= drawingAreaX;
            position.y -= drawingAreaY;
            return position;
        }

        private Vector3 convertToGlobal(Vector3 position)
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            position.x += (drawingAreaPosition.x - (drawingAreaTransform.rect.width / 2));
            position.y += (drawingAreaPosition.y - (drawingAreaTransform.rect.height / 2));
            return position;
        }

        private Vector3 snapToGrid(Vector3 position)
        {
            if (snap)
            {
                float gridSize = 128 * size;
                Vector3 local = convertToLocal(position) / gridSize;
                local = new Vector3(Mathf.Round(local.x) * gridSize, Mathf.Round(local.y) * gridSize, local.z);
                local = convertToGlobal(local);
                return local;
            }
            return position;
        }

        private void Update()
        {
            snap = snapCheckbox.checkValue;
            mousePosition = Input.mousePosition;
            mousePositionGrid = snapToGrid(mousePosition);
            mousePositionRelative = getMousePosition();
            insideDrawingArea = isInsideDrawingArea();
            isLeftClickPressed = Input.GetMouseButton(0);
            isLeftClickClicked = Input.GetMouseButtonDown(0);
            isMiddleClickPressed = Input.GetMouseButton(2);
            isRightClickClicked = Input.GetMouseButtonDown(1);
            isControlClicked = Input.GetKey(KeyCode.LeftControl);
            isDeleteClicked = Input.GetKey(KeyCode.Delete);
            BackgroundManager.Instance.uniteLayers();
            updateLights(mode == Mode.Selection);
            if (mode == Mode.Drawing)
            {
                BackgroundManager.Instance.UpdateBackground();
            }

            else if (mode == Mode.Light && insideDrawingArea && !isLeftClickPressed && lightManager.lightInstance == null && lightManager.lightMode == LightMode.Light)
            {
                lightManager.createLight(size);
            }
            else if (mode == Mode.Light && insideDrawingArea && !isLeftClickPressed && lightManager.lightInstance != null && lightManager.lightMode == LightMode.Light)
            {
                lightManager.lightInstance.GetComponent<ObjectInformation>().updatePosition(mousePositionGrid);
            }
            else if (mode == Mode.Light && insideDrawingArea && isLeftClickPressed && lightManager.lightInstance != null && lightManager.lightMode == LightMode.Light)
            {
                Layer layer = CanvasManager.Instance.getCurrentLayer();
                if (layer.visible)
                {
                    layer.objects.Add(lightManager.lightInstance);
                    lightManager.lightInstance = null;
                }
            }
            else if (mode == Mode.Wall && insideDrawingArea && isLeftClickClicked)
            {
                wallManager.createWall(size, mousePositionGrid);
            }
            else if (mode == Mode.Wall && insideDrawingArea && isRightClickClicked && wallManager.wallInstance.Count != 0)
            {
                Destroy(wallManager.wallInstance.Last());
                wallManager.wallInstance.RemoveAt(wallManager.wallInstance.Count - 1);
                wallManager.wallInstance.Clear();
            }
            else if (mode == Mode.Wall && insideDrawingArea && !isLeftClickPressed && wallManager.wallInstance.Count != 0)
            {
                wallManager.updateWall(mousePositionGrid);
            }

            if (mode == Mode.Light && lightManager.lightInstance != null && lightManager.lightMode == LightMode.Light)
            {
                lightManager.updateLight();
            }
            if ((mode != Mode.Light || lightManager.lightMode != LightMode.Light) && lightManager.lightInstance != null)
            {
                Destroy(lightManager.lightInstance);
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
                SelectionManager.Instance.selectedObject.GetComponent<ObjectInformation>().updatePosition(mousePositionGrid);
            }
            if (mode == Mode.Selection && isDeleteClicked && SelectionManager.Instance.selectedObject != null)
            {
                Destroy(SelectionManager.Instance.selectedObject);
                SelectionManager.Instance.selectedObject = null;
                SelectionManager.Instance.mode = SelectionMode.None;
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
                layer.objects = layer.objects.Where(item => item != null).ToList();
                foreach (GameObject layerObject in layer.objects)
                {
                    if (layerObject != null)
                    {
                        layerObject.SetActive(false);
                        layerObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    
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
            if (isControlClicked && lightManager.lightInstance != null)
            {
                lightManager.updateRotation(mouseDelta);

            }
            else if(isControlClicked && SelectionManager.Instance.lightHandler.lightInstance != null)
            {
                SelectionManager.Instance.lightHandler.updateRotation(mouseDelta);
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
                if(drawingAreaChild.GetComponent<Light2D>() != null)
                {
                    Light2D light = drawingAreaChild.GetComponent<Light2D>();
                    float lightScalar = size / drawingAreaChild.GetComponent<ObjectInformation>().size;
                    drawingAreaChild.GetComponent<ObjectInformation>().size = size;
                    light.pointLightInnerRadius = light.pointLightInnerRadius * lightScalar;
                    light.pointLightOuterRadius = light.pointLightOuterRadius * lightScalar;
                }
                else if(drawingAreaChild.GetComponent<SpriteRenderer>() != null)
                {
                    RectTransform rect = drawingAreaChild.GetComponent<RectTransform>();
                    rect.localScale = rect.localScale * size / drawingAreaChild.GetComponent<ObjectInformation>().size;
                    drawingAreaChild.GetComponent<ObjectInformation>().size = size;
                }
            }
        }
    }
}

