using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Use()
    {
        Destroy(gameObject);
    }
}
