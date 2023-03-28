using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{

    private void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    public void PrivacyPolicy()
    {
        OpenLink("https://www.floatingislandstudios.com/privacy-policy/ssa");
    }
    public void Facebook()
    {
        OpenLink("https://www.facebook.com/FloatingIslandStudiosGr/");
    }
    public void Instagram()
    {
        OpenLink("https://www.instagram.com/floatingislandstudios/");
    }
    public void Twitter()
    {
        OpenLink("https://twitter.com/Floating0Island");
    }
    public void Website()
    {
        OpenLink("https://www.floatingislandstudios.com/mobile-games");
    }

}
