using UnityEngine;
using System.Collections;

public class GlobalObjects : Singleton<GlobalObjects>{
    public GameObject SelectedObject;
    public Sprite DefaultSelectionSprite;
    public UnityEngine.UI.Slider speedSlider;
    protected GlobalObjects()
    {

    }
}
