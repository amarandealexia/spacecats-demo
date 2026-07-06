using UnityEngine;
using UnityEngine.EventSystems;

public class FallingNote : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rectTransform;
    private bool isFalling = false;
    private bool hasLanded = false;
    
    [Header("Drop Settings")]
    public float fallSpeed = 500f;       
    public float stopYPosition = -400f;  

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip landSound;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isFalling)
        {
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y -= fallSpeed * Time.deltaTime;

            if (newPos.y <= stopYPosition)
            {
                newPos.y = stopYPosition;
                isFalling = false; 

                if (!hasLanded)
                {
                    if (audioSource != null && landSound != null)
                    {
                        audioSource.PlayOneShot(landSound);
                    }
                    hasLanded = true;
                }

                Destroy(gameObject, 0.5f); 
                Debug.Log("Tutorial note destroyed safely.");
                return; 
            }

            rectTransform.anchoredPosition = newPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isFalling = true;
        Debug.Log("Note clicked! Falling now...");
    }
}