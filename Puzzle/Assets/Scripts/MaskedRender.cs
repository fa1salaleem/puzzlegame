using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedRender : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteMask maskSprite;

    public static MaskedRender CreateMask(Sprite _sp, string _maskPath)
    {
        GameObject GO = Instantiate(Resources.Load("Prefabs/MaskedRender")) as GameObject;
        MaskedRender maskedRender = GO.GetComponent<MaskedRender>();
        maskedRender.spriteRenderer.sprite = _sp;
        maskedRender.maskSprite.sprite = Resources.Load<Sprite>(_maskPath);
        return maskedRender;
    }
}
