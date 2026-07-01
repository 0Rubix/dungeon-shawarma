using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private SpriteRenderer hand;
    [SerializeField] private Sprite handSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchHandGFX(Sprite sprite)
    {
        if (sprite == null)
        {
            hand.sprite = handSprite;
            return;
        }

        hand.sprite = sprite;
    }
}
