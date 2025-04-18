using System;

[Serializable]
public struct Size
{
    public Size(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public float Width;
    public float Height;

    public override string ToString()
    {
        return $"Width: {Width}, Height: {Height}";
    }
}