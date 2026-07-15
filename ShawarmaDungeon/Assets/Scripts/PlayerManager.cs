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
    [SerializeField] private RuntimeAnimatorController handController;
    [SerializeField] private Animator handAnim;

    public delegate void HeldItemchanged(Item item);
    public HeldItemchanged onHeldItemChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryManager.instance.onInventoryChanged += UpdateHandGFX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchHandGFX(Item item)
    {
        onHeldItemChanged?.Invoke(item);

        handAnim.runtimeAnimatorController = handController;

        if (item == null)
        {
            hand.sprite = handSprite;
            return;
        }

        hand.sprite = item.icon;
    }

    private void UpdateHandGFX(int slot, Item item, int amount)
    {
        if (slot != InventoryManager.instance.hotbarSlot) return;

        SwitchHandGFX(item);
    }
}
