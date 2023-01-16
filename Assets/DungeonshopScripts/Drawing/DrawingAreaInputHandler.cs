using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeonshop
{
    public class DrawingAreaInputHandler : MonoBehaviour
    {
        [HideInInspector] public static DrawingAreaInputHandler Instance;

        bool holdingOutsideDrawingArea;

        [HideInInspector] public Vector3 mousePosition;
        [HideInInspector] public bool insideDrawingArea;
        [HideInInspector] public bool isPressed;
        [HideInInspector] public Vector3 previousMousePosition;

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
            mousePosition = getMousePosition();
            insideDrawingArea = isInsideDrawingArea();
            isPressed = Input.GetMouseButton(0);
            BackgroundManager.Instance.UpdateBackground();
            previousMousePosition = mousePosition;
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
            if (Input.GetMouseButton(0) && (!insideAreaX || !insideAreaY))
            {
                holdingOutsideDrawingArea = true;
                return false;
            }
            else if(holdingOutsideDrawingArea && insideAreaX && insideAreaY && !Input.GetMouseButton(0))
            {
                holdingOutsideDrawingArea = false;
                return false;
            }
            else if(Input.GetMouseButton(0) && insideAreaX && insideAreaY && !holdingOutsideDrawingArea)
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

        public void changeSize()
        {
            float mouseDelta = Input.mouseScrollDelta.y;
            mouseDelta = mouseDelta >= 0 ? 1.1f : 0.9f;
            
            drawingAreaTransform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(drawingAreaTransform.rect.width * mouseDelta, drawingAreaTransform.rect.height * mouseDelta);
        }
    }
}

