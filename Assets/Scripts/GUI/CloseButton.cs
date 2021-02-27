using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : ButtonBehavior
{
    public override void OnClick()
    {
        Application.Quit();
    }
}
