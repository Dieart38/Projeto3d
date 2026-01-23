using UnityEngine;

public class DynamicCam : MonoBehaviour
{
     [Header("Camera")]
    public GameObject camB;
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                camB.SetActive(true);
                break;

        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                camB.SetActive(false);
                break;

        }
    }
}
