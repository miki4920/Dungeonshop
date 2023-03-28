using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeonshop
{
    public class LayerDragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform currentTransform;
        public LayerManagerViewController viewController;
        private GameObject mainContent;
        private Vector3 currentPosition;

        private int totalChild;

        public void OnPointerDown(PointerEventData eventData)
        {
            currentPosition = currentTransform.position;
            mainContent = currentTransform.parent.gameObject;
            totalChild = mainContent.transform.childCount;
        }

        public void OnDrag(PointerEventData eventData)
        {
            currentTransform.position =
                new Vector3(currentTransform.position.x, eventData.position.y, currentTransform.position.z);

            for (int i = 0; i < totalChild; i++)
            {
                if (i != currentTransform.GetSiblingIndex())
                {
                    Transform otherTransform = mainContent.transform.GetChild(i);
                    int distance = (int) (otherTransform.position.y - currentTransform.position.y);
                    if (-5 <= distance && distance <= 5 && distance != 0)
                    {
                        Vector3 otherTransformOldPosition = otherTransform.position;
                        otherTransform.position = new Vector3(otherTransform.position.x, currentPosition.y,
                            otherTransform.position.z);
                        currentTransform.position = new Vector3(currentTransform.position.x, otherTransformOldPosition.y,
                            currentTransform.position.z);
                        if (0 < distance && distance <= 5)
                        {  
                            viewController.changeLayerPosition(otherTransform.GetSiblingIndex(), currentTransform.GetSiblingIndex());
                            BackgroundManager.Instance.uniteLayers();
                        }
                        else if (-5 <= distance && distance < 0)
                        {
                            viewController.changeLayerPosition(currentTransform.GetSiblingIndex(), otherTransform.GetSiblingIndex());
                            BackgroundManager.Instance.uniteLayers();
                        }
                        currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                        currentPosition = currentTransform.position;
                    }
                   
                    
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            currentTransform.position = currentPosition;
        }
    }
}
