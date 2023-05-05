using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeonshop
{
    public class ObjectView : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Dungeonshop.SelectionManager.Instance.setSelection(gameObject.transform.parent.gameObject);
        }
    }

}