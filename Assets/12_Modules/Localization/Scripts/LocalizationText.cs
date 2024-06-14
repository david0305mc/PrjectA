using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocalizationText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (LocalizationManager.Instance != null)
        {
            string curStr = string.Empty;
            var _text = GetComponent<Text>();
            if (_text)
            {
                curStr = _text.text;
                _text.text = LocalizationManager.Instance.GetText(curStr);
            }
        }
        else
        {
            Debug.LogWarning("LocalizationManaget is null");
        }
    }
}
