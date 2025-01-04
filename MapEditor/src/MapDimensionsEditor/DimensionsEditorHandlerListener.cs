using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.MapDimensionsEditor
{
    public interface DimensionsEditorHandlerListener
    {
        void OnDimensionsUpdated(int width, int height);
    }
}
