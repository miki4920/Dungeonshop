using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerViewController : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>();
    void Start()
    {
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        children[0].SetActive(true);
    }

    public void changeActivePanel(GameObject panel)
    {
        foreach(GameObject child in children)
        {
            child.SetActive(false);
            if (child == panel)
            {
                child.SetActive(true);
            }
        }
    }
}
