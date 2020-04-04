using UnityEngine;
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [SerializeField]
    private GameObject createOrJoinRoomCanvas;
    [SerializeField]
    private GameObject currentRoomCanvas;
    [SerializeField]
    private GameObject setNickNameCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchCanvases()
    {
        createOrJoinRoomCanvas.SetActive(!createOrJoinRoomCanvas.activeSelf);
        currentRoomCanvas.SetActive(!currentRoomCanvas.activeSelf);
    }

    public void SwitchToCreateRoom()
    {
        createOrJoinRoomCanvas.SetActive(true);
        setNickNameCanvas.SetActive(false);
    }
}
