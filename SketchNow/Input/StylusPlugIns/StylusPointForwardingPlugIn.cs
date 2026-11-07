using System.Windows.Input.StylusPlugIns;

namespace SketchNow.Input.StylusPlugIns;

internal sealed class StylusPointForwardingPlugIn : StylusPlugIn
{
    protected override void OnStylusDown(RawStylusInput rawStylusInput)
    {
        base.OnStylusDown(rawStylusInput);
        ForwardStylusPoints(rawStylusInput);
    }

    protected override void OnStylusMove(RawStylusInput rawStylusInput)
    {
        base.OnStylusMove(rawStylusInput);
        ForwardStylusPoints(rawStylusInput);
    }

    protected override void OnStylusUp(RawStylusInput rawStylusInput)
    {
        base.OnStylusUp(rawStylusInput);
        ForwardStylusPoints(rawStylusInput);
    }

    private static void ForwardStylusPoints(RawStylusInput rawStylusInput)
    {
        // Setting points marks the input as modified in WPF, even when their values are unchanged.
        rawStylusInput.SetStylusPoints(rawStylusInput.GetStylusPoints());
    }
}
