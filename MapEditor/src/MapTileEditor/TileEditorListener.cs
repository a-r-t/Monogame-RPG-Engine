using MapEditor.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.MapTileEditor
{
    public interface TileEditorListener
    {
        void OnTileEditorLoad(Map map);
        void OnTileEdited(int index, Tile tile);
    }
}
