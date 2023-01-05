using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TabInputHelper : MonoBehaviour
{
    public List<GameObject> intractableTabObjects;
    int currentTabIndex;
    private void OnEnable()
    {
        foreach (var item in intractableTabObjects)
        {
            TMP_InputField currentInputField = item.GetComponent<TMP_InputField>();
            Button currentButton = item.GetComponent<Button>();
            Toggle toggle = item.GetComponent<Toggle>();
            if (currentInputField)
            {
                currentInputField.onSelect.AddListener((val) => OnEntryDetected(currentInputField.gameObject));
            }
            else if (currentButton)
            {
                currentButton.onClick.AddListener(() => OnButtonDetected(currentButton.gameObject));
            }
            else if (toggle)
            {
                toggle.onValueChanged.AddListener((bo) => OnToggleDetected(toggle.gameObject));
            }
        }
    }
    private void OnDisable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in intractableTabObjects)
        {
            TMP_InputField currentInputField = item.GetComponent<TMP_InputField>();
            Button currentButton = item.GetComponent<Button>();
            Toggle toggle = item.GetComponent<Toggle>();
            if (currentInputField)
            {
                currentInputField.onSelect.RemoveListener((val) => OnEntryDetected(currentInputField.gameObject));
            }
            else if (currentButton)
            {
                currentButton.onClick.RemoveListener(() => OnButtonDetected(currentButton.gameObject));
            }
            else if (toggle)
            {
                toggle.onValueChanged.RemoveListener((bo) => OnToggleDetected(toggle.gameObject));
            }
        }

        //CheckInput();
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentTabIndex >= intractableTabObjects.Count) return;
            TMP_InputField currentInputField = intractableTabObjects[currentTabIndex].GetComponent<TMP_InputField>();
            Button currentButton = intractableTabObjects[currentTabIndex].GetComponent<Button>();
            Toggle toggle = intractableTabObjects[currentTabIndex].GetComponent<Toggle>();
            if (currentInputField)
            {
                currentInputField.onEndEdit.Invoke(currentInputField.text);
            }
            ChangeToNextInput();
            void ChangeToNextInput()
            {
                currentTabIndex++;
                if (currentTabIndex >= intractableTabObjects.Count) return;
                currentInputField = intractableTabObjects[currentTabIndex].GetComponent<TMP_InputField>();
                currentButton = intractableTabObjects[currentTabIndex].GetComponent<Button>();
                Toggle toggle = intractableTabObjects[currentTabIndex].GetComponent<Toggle>();
                if (currentInputField)
                {
                    EventSystem.current.SetSelectedGameObject(currentInputField.gameObject);
                    currentInputField.ActivateInputField();
                }
                else if (currentButton)
                {
                    EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                }
                else if (toggle)
                {
                    EventSystem.current.SetSelectedGameObject(toggle.gameObject);
                }
            }
        }
    }
    public void OnEntryDetected(GameObject go)
    {
        if (intractableTabObjects.Contains(go))
        {
            currentTabIndex = intractableTabObjects.IndexOf(go);
        }
        else
        {
            currentTabIndex = 0;
        }
    }
    public void OnButtonDetected(GameObject go)
    {
        if (intractableTabObjects.Contains(go))
        {
            currentTabIndex = intractableTabObjects.IndexOf(go);
        }
        else
        {
            currentTabIndex = 0;
        }
    }
    public void OnToggleDetected(GameObject go)
    {
        if (intractableTabObjects.Contains(go))
        {
            currentTabIndex = intractableTabObjects.IndexOf(go);
        }
        else
        {
            currentTabIndex = 0;
        }
    }
}