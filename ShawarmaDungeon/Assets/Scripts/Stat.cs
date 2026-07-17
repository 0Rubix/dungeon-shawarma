[System.Serializable]
public class Stat
{
    public float value;
    private float? modifiedValue;

    public float GetModifiedValue() => modifiedValue ?? value;

    public void ModifyValue(float newValue) => modifiedValue = newValue;

    public void ResetValue() => modifiedValue = null;
}
