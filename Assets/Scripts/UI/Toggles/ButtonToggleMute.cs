public class ButtonToggleMute : ButtonToggle
{
    new void Start()
    {
        base.Start();
        UIEvents.UpdateVolumeMute += selfUpdate;
    }
    void OnDestroy()
    {
        UIEvents.UpdateVolumeMute -= selfUpdate;
    }
}
