using UnityEngine;

public class GDKPresenceController : IPresenceProxy 
{
    public void Start() {}
    public void Update() {}
    public void OnApplicationQuit() {}

    public void UpdateActivity()
    {
        if (SGrid.Current != null)
        {
            GDKProxy.UpdateRichPresence(SGrid.Current.MyArea.ToString());
        }
        else
        {
            GDKProxy.UpdateRichPresence("Menus");
        }
    }
}