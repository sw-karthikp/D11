using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMovingSlider : MonoBehaviour
{
    public RectTransform[] sliderPositions;

    public RectTransform MovingSlider;

    public Toggle[] toggles;

    public Ease _ease;

    private void OnEnable()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[index].onValueChanged.AddListener(delegate { OnToggleChanged(index); });

        }
    }

    void OnToggleChanged(int _index)
    {
        if (toggles[_index].isOn)
        {
            MovingSlider.DOKill();
            MovingSlider.DOMove(sliderPositions[_index].transform.position, 0.1f).SetEase(_ease);
            MovingSlider.DOSizeDelta(sliderPositions[_index].sizeDelta,0.1f).SetEase(_ease);
        }
    }
}
