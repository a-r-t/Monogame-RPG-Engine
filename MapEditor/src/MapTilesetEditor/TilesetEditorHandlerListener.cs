using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.MapTilesetEditor
{
    public interface TilesetEditorHandlerListener
    {
        void OnTilesetInfoUpdated(string tilesetName, int scale);
    }
}
