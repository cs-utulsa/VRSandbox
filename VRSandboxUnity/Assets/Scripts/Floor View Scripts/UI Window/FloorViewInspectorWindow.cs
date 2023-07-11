using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorViewInspectorWindow : MonoBehaviour
{
    public GameObject RoomUIWindowPrefab;
    public GameObject AllRoomsUIWindowPrefab;

    public TabGroup SelectedRoomsTabGroup;
    public GameObject UIWindowGroup;

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

        GameObject newRoomUI = Instantiate(RoomUIWindowPrefab, UIWindowGroup.transform);
        newRoomUI.SetActive(false);

        SelectedRoomsTabGroup.AddTab(room.gameObject.name, newRoomUI);

        /*if(_selectedRooms.Count > 1)
        {
            GameObject allRoomsUI = Instantiate(AllRoomsUIWindowPrefab, UIWindowGroup.transform);
            _allRoomsTab = SelectedRoomsTabGroup.AddTab("Overall", allRoomsUI, true);
        }*/
    }

    public void RoomDeselected(Room room)
    {
        _selectedRooms.Remove(room);
        SelectedRoomsTabGroup.RemoveTab(null);

        /*if(_selectedRooms.Count <= 1)
        {
            SelectedRoomsTabGroup.RemoveTab(_allRoomsTab);
        }*/
    }
}
