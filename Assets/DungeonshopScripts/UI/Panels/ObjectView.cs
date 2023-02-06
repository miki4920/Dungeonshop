using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeonshop.UI
{
    public class ObjectView : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Dungeonshop.UI.SelectionManager.Instance.setSelection(gameObject.transform.parent.gameObject);
        }
    }

}