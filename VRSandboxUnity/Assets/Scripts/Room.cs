using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class Room : MonoBehaviour
{
    /// <summary>
    /// Collider representing the floor of the room. should be flat
    /// </summary>
    public Collider RoomCollider;

    /// <summary>
    /// All sensors currently used by this room
    /// </summary>
    public Sensor[] Sensors;

    public Canvas SensorTextCanvas;

    private XRBaseInteractable _roomInteractable;
    private Material _roomMaterial;
    private bool _hovered, _selected;

    private void Awake()
    {
        if(RoomCollider == null)
        {
            RoomCollider = GetComponent<Collider>();
        }

        _roomInteractable = GetComponent<XRBaseInteractable>();

        _roomInteractable.selectEntered.AddListener(RoomSelected);
        _roomInteractable.selectExited.AddListener(RoomUnselected);
        _roomInteractable.hoverEntered.AddListener(RoomHovered);
        _roomInteractable.hoverExited.AddListener(RoomUnhovered);

        _roomMaterial = GetComponent<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SensorTextCanvas.gameObject.SetActive(false);

        foreach(Sensor sensor in Sensors)
        {
            sensor.InitializeSensor(this);
            sensor.StartDisplayVisual();
        }

        _roomMaterial.color = ColorManager.Instance.DefaultRoomColor;
    }

    private void RoomSelected(SelectEnterEventArgs args)
    {
        _selected = true;
        SetColor();
    }

    private void RoomUnselected(SelectExitEventArgs args)
    {
        _selected = false;
        SetColor();
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

    private void DisplaySensorText()
    {

    }

    private void SetColor()
    {
        if(_hovered && _selected)
        {
            _roomMaterial.color = ColorManager.Instance.HoveredSelectedRoomColor;
        }
        else if (_hovered)
        {
            _roomMaterial.color = ColorManager.Instance.HoveredRoomColor;
        }
        else if (_selected)
        {
            _roomMaterial.color = ColorManager.Instance.SelectedRoomColor;
        }
        else
        {
            _roomMaterial.color = ColorManager.Instance.DefaultRoomColor;
        }
    }
}
