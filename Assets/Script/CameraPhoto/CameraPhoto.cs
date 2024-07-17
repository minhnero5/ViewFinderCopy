using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraPhoto : MonoBehaviour
{
    public CinemachineCamera playerCam;
    public CinemachineCamera photoCam;
    public Image photoDisplay;
    public int photoWidth = 1920;
    public int photoHeight = 1080;

    private PlayerInput photoInput;
    private InputAction photoAction;
    private InputAction switchCamAction;
    private Texture2D photoTexture;
    // Start is called before the first frame update
    void Start()
    {
        photoInput = GetComponent<PlayerInput>();
        photoAction = photoInput.actions.FindAction("TakeAPhoto");
        Debug.Log(photoAction);
        switchCamAction = photoInput.actions.FindAction("SwitchCam");
        photoAction.performed += ctx => TakeAShot();
        switchCamAction.performed += ctx => SwitchCamera();
    }

    private void OnDestroy()
    {
        photoAction.performed -= ctx => TakeAShot();
        switchCamAction.performed -= ctx => SwitchCamera();
    }
    // Update is called once per frame
    void Update()
    {
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
      
    }

    private void SwitchCamera()
    {
        if (playerCam.Priority.Value > photoCam.Priority.Value)
        {
            playerCam.Priority.Value = 0;
            photoCam.Priority.Value = 10;
            photoCam.transform.position= Vector3.zero;
        }
        else
        {
            playerCam.Priority.Value = 10;
            photoCam.Priority.Value = 0;
            //playerCam.transform.rotation = new Quaternion(0,0,0);
        }
    }

    private void TakeAShot()
    {
        
    }
}
