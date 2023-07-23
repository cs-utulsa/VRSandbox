using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class Room : MonoBehaviour
{
    public event Action<Room> OnRoomSelected;
    public event Action<Room> OnRoomDeselected;

    /// <summary>
    /// Collider representing the floor of the room. should be flat
    /// </summary>
    public Collider RoomCollider;

    public Scene RoomViewScene;

    private XRBaseInteractable _roomInteractable;
    private Material _roomMaterial;
    private bool _hovered, _selected;

    private GameObject _interactionMenu;

    private void Awake()
    {
        if(RoomCollider == null)
        {
            RoomCollider = GetComponent<Collider>();
        }

        _roomInteractable = GetComponent<XRBaseInteractable>();

        _roomInteractable.selectEntered.AddListener(RoomSelected);
        _roomInteractable.hoverEntered.AddListener(RoomHovered);
        _roomInteractable.hoverExited.AddListener(RoomUnhovered);

        _roomMaterial = GetComponent<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        _roomMaterial.color = ColorManager.instance.DefaultRoomColor;
    }

    private void RoomSelected(SelectEnterEventArgs args)
    {
        _selected = !_selected;
        SetColor();
        _interactionMenu.SetActive(_selected);

        if(_selected)
        {
            OnRoomSelected(this);
        }
        else
        {
            OnRoomDeselected(this);
        }
    }

    private void RoomHovered(HoverEnterEventArgs args)
    {
        _hovered = true;
        SetColor();
    }

    private void RoomUnhovered(HoverExitEventArgs args)
    {
        _hovered = false;
        SetColor();
    }

    private void SetColor()
    {
        if(_hovered && _selected)
        {
            _roomMaterial.color = ColorManager.instance.HoveredSelectedRoomColor;
        }
        else if (_hovered)
        {
            _roomMaterial.color = ColorManager.instance.HoveredRoomColor;
        }
        else if (_selected)
        {
            _roomMaterial.color = ColorManager.instance.SelectedRoomColor;
        }
        else
        {
            _roomMaterial.color = ColorManager.instance.DefaultRoomColor;
        }
    }
}
