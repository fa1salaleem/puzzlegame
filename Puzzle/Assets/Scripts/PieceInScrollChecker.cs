using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInScrollChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PuzzlePiece pp = other.gameObject.GetComponent<PuzzlePiece>();
        if (pp == null)
        {
            return;
        }
        else
        {
            pp.inScroll = true;
            pp.SetLocalScale(pp.scrollScale);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PuzzlePiece pp = other.gameObject.GetComponent<PuzzlePiece>();
        if (pp == null)
        {
            return;
        }
        else
        {
            pp.inScroll = false;
            pp.SetLocalScale(1.0f);
        }
    }
}
