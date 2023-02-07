using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeonshop.UI
{
    public class ObjectView : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Dungeonshop.UI.SelectionManager.Instance.setSelection(gameObject.transform.parent.gameObject);
        }
    }

}