using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // Import DOTween namespace
using Unity.Cinemachine;

public class CameraPhotoController : MonoBehaviour
{
    public CinemachineCamera virtualCamera; // Cinemachine camera của người chơi
    public Camera mainCamera; // Camera chính của người chơi
    public RawImage photoDisplay; // UI RawImage để hiển thị ảnh từ camera
    public GameObject photoFrame; // Khung của máy ảnh
    public RenderTexture photoRenderTexture; // RenderTexture để lưu ảnh chụp
    public GameObject photoPrefab; // Prefab của đối tượng hiển thị ảnh chụp
    public Transform photoContainer; // Container để chứa các ảnh chụp
    public float photoAnimationDuration = 1f; // Thời gian animation

    private PlayerInput playerInput;
    private InputAction photoAction;
    private InputAction switchCameraAction;
    private bool isPhotoMode = false;
    private int photoCount = 0;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        photoAction = playerInput.actions.FindAction("TakeAPhoto");
        switchCameraAction = playerInput.actions.FindAction("SwitchCam");

        // Đăng ký sự kiện cho action
        photoAction.performed += ctx => StartCoroutine(TakeScreenshot());
        switchCameraAction.performed += ctx => TogglePhotoMode();

        // Khởi tạo
        photoFrame.SetActive(false);
        photoDisplay.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị phá hủy
        photoAction.performed -= ctx => StartCoroutine(TakeScreenshot());
        switchCameraAction.performed -= ctx => TogglePhotoMode();
    }

    void TogglePhotoMode()
    {
        isPhotoMode = !isPhotoMode;

        if (isPhotoMode)
        {
            // Kích hoạt chế độ máy ảnh
            photoFrame.SetActive(true);
            photoDisplay.gameObject.SetActive(true);
        }
        else
        {
            // Trở lại chế độ bình thường
            photoFrame.SetActive(false);
            photoDisplay.gameObject.SetActive(false);
        }
    }

    IEnumerator TakeScreenshot()
    {
        if (!isPhotoMode) yield break;

        // Đợi cho tới cuối frame để đảm bảo camera đã render
        yield return new WaitForEndOfFrame();

        // Gán RenderTexture cho camera
        mainCamera.targetTexture = photoRenderTexture;

        // Tạo Texture để lưu ảnh chụp
        RenderTexture.active = photoRenderTexture;
        Texture2D screenShot = new Texture2D(photoRenderTexture.width, photoRenderTexture.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, photoRenderTexture.width, photoRenderTexture.height), 0, 0);
        screenShot.Apply();
        RenderTexture.active = null;

        // Hủy gán RenderTexture cho camera
        mainCamera.targetTexture = null;

        // Tạo đối tượng mới để hiển thị ảnh chụp
        GameObject newPhoto = Instantiate(photoPrefab, photoContainer);
        newPhoto.GetComponent<RawImage>().texture = screenShot;

        // Đặt vị trí ban đầu của ảnh chụp tại vị trí của photoFrame
        RectTransform newPhotoRect = newPhoto.GetComponent<RectTransform>();
        newPhotoRect.SetParent(photoFrame.transform, false);
        newPhotoRect.anchoredPosition = Vector2.zero;

        // Di chuyển ảnh đến vị trí trong container
        newPhotoRect.SetParent(photoContainer, true);
        newPhotoRect.DOAnchorPos(Vector2.zero, photoAnimationDuration).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            // Đẩy các ảnh cũ sang bên phải để tạo chỗ cho ảnh mới
            foreach (Transform child in photoContainer)
            {
                if (child == newPhoto.transform) continue;
                RectTransform childRect = child.GetComponent<RectTransform>();
                childRect.DOAnchorPosX(childRect.anchoredPosition.x + newPhotoRect.sizeDelta.x, photoAnimationDuration).SetEase(Ease.OutBounce);
            }
        });

        // Bật lại khung ảnh sau khi chụp
        photoFrame.SetActive(true);
    }
}
