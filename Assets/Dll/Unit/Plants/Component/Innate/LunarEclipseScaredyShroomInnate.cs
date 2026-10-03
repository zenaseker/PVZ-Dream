
public class LunarEclipseScaredyShroomInnate : InnateBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Init(detail);
        this._detail._owner.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 5).Maxstack = 20;

    }
}