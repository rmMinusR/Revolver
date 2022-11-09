using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadableUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameplate;
    [SerializeField] private Image icon;

    public void Write(Loadable src)
    {
        if (nameplate) nameplate.text = src.name;
        if (icon) icon.sprite = src.icon;

        transform.localPosition = src.position;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
