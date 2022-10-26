using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class DrawingAreaInputHandler : MonoBehaviour
    {
        public static DrawingAreaInputHandler Instance;
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

        public bool isInsideDrawingArea()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            return Input.mousePosition.x <= Screen.width - drawingAreaPosition.x && Input.mousePosition.x >= drawingAreaPosition.x;
        }

        public Vector3 mousePosition()
        {
            Vector3 drawingAreaPosition = drawingAreaTransform.position;
            float globalPositionX = Input.mousePosition.x - drawingAreaPosition.x;
            return new Vector3(globalPositionX, Input.mousePosition.y, 0);
        }
    }
}

