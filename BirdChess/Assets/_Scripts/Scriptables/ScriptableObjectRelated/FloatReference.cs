using System;

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;
    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.value; }
        set
        {
            if (UseConstant)
            {
                ConstantValue = value;
            }
            else
            {
                Variable.value = value;
            }
        }
    }
}
