public interface IPresenceProxy
{
    public void Start();
    public void Update();
    public void OnApplicationQuit();

    public void UpdateActivity();
}