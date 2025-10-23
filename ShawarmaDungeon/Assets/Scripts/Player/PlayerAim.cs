using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform aimOrigin;
    [SerializeField] private float rotOffset;

    [SerializeField] private Camera cam;
    [SerializeField] private Animator anim;

    Vector2 lookDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        lookDir = mousePos - (Vector2)aimOrigin.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - rotOffset;

        aimOrigin.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        anim.SetFloat("AimX", lookDir.normalized.x);
        anim.SetFloat("AimY", lookDir.normalized.y);
    }
}
