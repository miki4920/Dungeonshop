using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        [HideInInspector] public Vector3 mousePosition;
        [HideInInspector] public Vector3 mousePositionRelative;
        [HideInInspector] public bool insideDrawingArea;
        [HideInInspector] public bool isLeftClickPressed;
        private bool singleClick;
        [HideInInspector] public bool clickOnce;
        [HideInInspector] public bool isMiddleClickPressed;
        [HideInInspector] public Vector3 previousMousePosition;
        [HideInInspector] public Vector3 previousMousePositionRelative;
        [SerializeField] RectTransform drawingAreaTransform;
        [SerializeField] RectTransform viewingPortTransform;

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
        }

        private void Update()
        {
            mousePosition = Input.mousePosition;
            mousePositionRelative = getMousePosition();
            insideDrawingArea = isInsideDrawingArea();
            isLeftClickPressed = Input.GetMouseButton(0);
            isMiddleClickPressed = Input.GetMouseButton(2);
            if (mode == Mode.Drawing)
            {
                BackgroundManager.Instance.UpdateBackground();
            }
            else if (mode == Mode.Light && insideDrawingArea && isLeftClickPressed && !singleClick)
            {
                LightHandler.LightInstance.createLight(mousePosition);
                singleClick = true;
            }
            else if (!isLeftClickPressed)
            {
                singleClick = false;
            }
            shiftScreen();
            previousMousePosition = mousePosition;
            previousMousePositionRelative = mousePositionRelative;
        }

        public void changeMode(int newMode)
        {
            mode = (Mode) newMode;
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
            else if(isLeftClickPressed && insideAreaX && insideAreaY && !holdingOutsideDrawingArea)
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

        public void changeSize()
        {
            float mouseDelta = Input.mouseScrollDelta.y;
            mouseDelta = mouseDelta >= 0 ? 1.1f : 0.9f;
            //TODO: Make so that the zooming in zooms to a mouse pointer, rather than centre
            //TODO: Prevent moving the canvas off the screen
            Vector2 oldDimensions = drawingAreaTransform.gameObject.GetComponent<RectTransform>().sizeDelta;
            Vector2 newDimensions = new Vector2(drawingAreaTransform.rect.width * mouseDelta, drawingAreaTransform.rect.height * mouseDelta);
            drawingAreaTransform.gameObject.GetComponent<RectTransform>().sizeDelta = newDimensions;
        }
    }
}

