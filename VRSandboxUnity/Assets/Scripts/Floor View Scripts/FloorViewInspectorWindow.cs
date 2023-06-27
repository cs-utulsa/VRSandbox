using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorViewInspectorWindow : MonoBehaviour
{
    public TabGroup SelectedRoomsTabGroup;

    private List<Room> _selectedRooms;
    private Tab _allRoomsTab;

    private void Awake()
    {
        var roomObjects = FindObjectsOfType<Room>();

        foreach(Room room in roomObjects)
        {
            room.OnRoomSelected += RoomSelected;
            room.OnRoomDeselected += RoomDeselected;
        }
    }

    public void RoomSelected(Room room)
    {
        _selectedRooms.Add(room);
        SelectedRoomsTabGroup.AddTab(room.gameObject.name, null);

        if(_selectedRooms.Count > 1)
        {
            _allRoomsTab = SelectedRoomsTabGroup.AddTab("Overall", null, true);
        }
    }

    public void RoomDeselected(Room room)
    {
        _selectedRooms.Remove(room);
        SelectedRoomsTabGroup.RemoveTab(null);

        if(_selectedRooms.Count <= 1)
        {
            SelectedRoomsTabGroup.RemoveTab(_allRoomsTab);
        }
    }
}
