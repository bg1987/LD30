using UnityEngine;
using System.Collections;

public class GlobalObjects : Singleton<GlobalObjects>{
    public GameObject SelectedObject;
    public Sprite DefaultSelectionSprite;

    protected GlobalObjects()
    {

    }
}
