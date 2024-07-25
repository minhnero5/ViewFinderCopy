using UnityEngine;

public class CameraSync : MonoBehaviour
{
    public Camera mainCamera; // Camera chính
    public Camera secondaryCamera; // Camera thứ hai

    void LateUpdate()
    {
        // Đồng bộ hóa vị trí và xoay
        secondaryCamera.transform.position = mainCamera.transform.position;
        secondaryCamera.transform.rotation = mainCamera.transform.rotation;

        // Đồng bộ hóa Field of View (FOV)
        secondaryCamera.fieldOfView = mainCamera.fieldOfView;
    }
}