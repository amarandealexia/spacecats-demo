using UnityEngine;

public class PhysicalScrap : MonoBehaviour
{
    public ScrapItem scrapData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScavengeManager manager = FindAnyObjectByType<ScavengeManager>();
            if (manager != null)
            {
                manager.LogFoundItem(scrapData);
            }
            Destroy(gameObject);
        }
    }
}