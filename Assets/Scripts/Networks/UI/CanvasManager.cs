using UnityEngine;
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [SerializeField]
    private GameObject createOrJoinRoomCanvas;
    [SerializeField]
    private GameObject currentRoomCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchCanvases()
    {
        createOrJoinRoomCanvas.SetActive(!createOrJoinRoomCanvas.activeSelf);
        currentRoomCanvas.SetActive(!currentRoomCanvas.activeSelf);
    }
}
