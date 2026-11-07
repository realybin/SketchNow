using System.Windows.Controls;

using SketchNow.Input.StylusPlugIns;

namespace SketchNow.Controls;

public sealed class DrawingInkCanvas : InkCanvas
{
    public DrawingInkCanvas()
    {
        StylusPlugIns.Add(new StylusPointForwardingPlugIn());
    }
}
