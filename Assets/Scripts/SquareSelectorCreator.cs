using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material opponentSquareMaterial;
    [SerializeField] private GameObject selectorPrefab;
    [SerializeField] private AudioOutputController audioOutput;
    private List<GameObject> instantiatedSelectors = new List<GameObject>();
    public void ShowSelection(Dictionary<Vector3, bool> squareData)
    {
        ClearSelection();
        audioOutput.PlaySelectorShine();
        foreach(var data in squareData)
        {
            Quaternion rotation = Quaternion.Euler (90, 0, 0);
            GameObject selector = Instantiate(selectorPrefab, data.Key, rotation);
            instantiatedSelectors.Add(selector);
            if(data.Value)
            {
                selector.GetComponent<Renderer>().material = freeSquareMaterial;
            }
            else
            {
                selector.GetComponent<Renderer>().material = opponentSquareMaterial;
                selector.transform.localScale += new Vector3(10f, 10f, 10f);
            }
            Color objectColor = selector.GetComponent<Renderer>().material.color;
            selector.GetComponent<Renderer>().material.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
            FadeCoroutine(selector);
            foreach(var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetSingleMaterial(data.Value ? freeSquareMaterial : opponentSquareMaterial);
            }
        }
    }
    private void FadeCoroutine(GameObject selector)
    {
        StartCoroutine(InstantiateFade(true, selector));
    }
    IEnumerator InstantiateFade(bool fadeAway, GameObject selector)
    {
        Color objectColor = selector.GetComponent<Renderer>().material.color;

        for (float i = 0; i <= 1; i += Time.deltaTime*3)
        {
            selector.GetComponent<Renderer>().material.color = new Color(objectColor.r, objectColor.g, objectColor.b, i);
            yield return null;
        }
    }
    public void ClearSelection()
    {
        foreach(var selector in instantiatedSelectors)
        {
            Destroy(selector.gameObject.gameObject);
        }
        instantiatedSelectors.Clear();
    }
}
