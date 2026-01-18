using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class SteamPresenceController : IPresenceProxy 
{
    public void Start() {}
    public void Update() {}
    public void OnApplicationQuit() {}

    public void UpdateActivity()
    {
#if !DISABLESTEAMWORKS
        if (SGrid.Current != null)
        {
            if (!Steamworks.SteamFriends.SetRichPresence(SGrid.Current.MyArea.ToString(), ""))
            {
                Debug.LogError("[Steam] Failed to set Steam Rich Presence");
            }
        }
        else
        {
            if (!Steamworks.SteamFriends.SetRichPresence("Menus", ""))
            {
                Debug.LogError("[Steam] Failed to set Steam Rich Presence");
            }
        }
#endif
    }
}