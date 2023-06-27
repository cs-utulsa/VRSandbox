using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color DefaultRoomColor;
    public Color HoveredRoomColor;
    public Color SelectedRoomColor;
    public Color HoveredSelectedRoomColor;

    private void Awake()
    {
        Instance = this;
    }
}
