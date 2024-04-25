using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BlockColor
{
    Red,
    Blue
}

public class Block : MonoBehaviour
{
    public BlockColor color;
    public Material redMaterial;
    public Material blueMaterial;

    void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        switch (color)
        {
            case BlockColor.Red:
                meshRenderer.material = redMaterial;
                break;
            case BlockColor.Blue:
                meshRenderer.material = blueMaterial;
                break;
        }
    }

}
