using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestPanel : MonoBehaviour {

    void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(false);
#else
        gameObject.SetActive(false);
#endif
    }
}
