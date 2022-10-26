using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class DrawingAreaInputHandler : MonoBehaviour
    {
        public static DrawingAreaInputHandler Instance;
        public bool holdingOutsideDrawingArea;
        [SerializeField] RectTransform drawingAreaTransform;
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

        public bool isInsideDrawingArea()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            bool insideArea = Input.mousePosition.x <= Screen.width - drawingAreaPosition.x && Input.mousePosition.x >= drawingAreaPosition.x;
            if (Input.GetMouseButton(0) && !insideArea)
            {
                holdingOutsideDrawingArea = true;
                return false;
            }
            else if(holdingOutsideDrawingArea && insideArea && !Input.GetMouseButton(0))
            {
                holdingOutsideDrawingArea = false;
                return false;
            }
            else if(Input.GetMouseButton(0) && insideArea && !holdingOutsideDrawingArea)
            {
                return true;
            }
            return false;
        }

        public Vector3 mousePosition()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            float globalPositionX = Input.mousePosition.x - drawingAreaPosition.x;
            return new Vector3(globalPositionX, Input.mousePosition.y, 0);
        }
    }
}

