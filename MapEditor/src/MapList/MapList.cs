using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MapEditor.src.ExtensionMethods;
using System.Text.RegularExpressions;

namespace MapEditor.src.MapList
{
    public partial class MapList : ObservableUserControl<MapListListener>
    {
        private TreeNode selectedNode;
        private ContextMenuStrip folderContextMenu;
        private ContextMenuStrip fileContextMenu;
        private string oldName = "";
        private string oldPath = "";

        public MapList()
        {
            InitializeComponent();
        }

        private void MapList_Load(object sender, EventArgs e)
        {
            SetupMapTreeView();
            mapTreeView.Nodes.Clear();
            PopulateMapTreeView();
            mapTreeView.ExpandAll();
            mapTreeView.Sort();
        }

        private void SetupMapTreeView()
        {
            ImageList imageList = new ImageList();
            imageList.Images.Add("folder", Image.FromFile("./Resources/Images/folder-icon.png"));
            imageList.Images.Add("file", Image.FromFile("./Resources/Images/file-icon.png"));
            imageList.Images.Add("file-selected", Image.FromFile("./Resources/Images/file-icon-selected.png"));
            mapTreeView.ImageList = imageList;

            SetupNodeContextMenu();
        }

        private void SetupNodeContextMenu()
        {
            folderContextMenu = new ContextMenuStrip();

            folderContextMenu.Items.Add("Add New Folder");
            folderContextMenu.Items[0].Click += (sender, e) => {
                TreeNode selectedNode = mapTreeView.SelectedNode;
                string path = selectedNode.FullPath;
                int newFolderIndex = GetNewFolderIndex($"./Resources/{path}");
                string newFolderName = newFolderIndex == 0 ? "New folder" : $"New folder ({newFolderIndex})";
                string newFolderDirectory = $"./Resources/{path}/{newFolderName}";
                Directory.CreateDirectory(newFolderDirectory);
                if (Directory.Exists(newFolderDirectory))
                {
                    TreeNode newNode = new TreeNode(newFolderName);
                    newNode.Tag = "folder";
                    newNode.ImageKey = "folder";
                    newNode.SelectedImageKey = "folder";
                    newNode.Name = newFolderName;
                    selectedNode.Nodes.Add(newNode);
                    mapTreeView.Sort();
                }
                else
                {
                    MessageBox.Show("Unknown error creating folder");
                }
            };

            folderContextMenu.Items.Add("Add New Map");
            folderContextMenu.Items[1].Click += (sender, e) => {
                TreeNode selectedNode = mapTreeView.SelectedNode;
                string path = selectedNode.FullPath;
                int newMapIndex = GetNewMapIndex($"./Resources/{path}");
                string newMapName = newMapIndex == 0 ? "New map" : $"New map ({newMapIndex})";
                string newMapFilePath = $"./Resources/{path}/{newMapName}.map";
                FileStream fs = File.Create(newMapFilePath);
                fs.Close();
                if (File.Exists(newMapFilePath))
                {
                    TreeNode newNode = new TreeNode(newMapName);
                    newNode.Tag = "file";
                    newNode.ImageKey = "file";
                    newNode.SelectedImageKey = "file";
                    newNode.Name = newMapName;
                    selectedNode.Nodes.Add(newNode);
                    mapTreeView.Sort();
                }
                else
                {
                    MessageBox.Show("Unknown error creating map");
                }
            };

            folderContextMenu.Items.Add("Rename");
            folderContextMenu.Items[2].Click += (sender, e) => {
                mapTreeView.LabelEdit = true;
                oldName = mapTreeView.SelectedNode.Name;
                oldPath = mapTreeView.SelectedNode.FullPath;
                mapTreeView.SelectedNode.BeginEdit();
            };

            fileContextMenu = new ContextMenuStrip();

            fileContextMenu.Items.Add("Rename");
            fileContextMenu.Items[0].Click += (sender, e) => {
                mapTreeView.LabelEdit = true;
                oldName = mapTreeView.SelectedNode.Name;
                oldPath = mapTreeView.SelectedNode.FullPath;
                mapTreeView.SelectedNode.BeginEdit();
            };
        }

        private int GetNewFolderIndex(string path)
        {
            string name = "/New folder";
            string current = name;
            int i = 0;
            while (Directory.Exists($"{path}/{current}")) {
                i++;
                current = $"{name} ({i})";
            }
            return i;
        }

        private int GetNewMapIndex(string path)
        {
            string name = "/New map";
            string current = name;
            int i = 0;
            while (File.Exists($"{path}/{current}.map"))
            {
                i++;
                current = $"{name} ({i})";
            }
            return i;
        }

        private void PopulateMapTreeView()
        {
            Queue<string> paths = new Queue<string>();
            string rootDir = Config.GameMapFilesPath;
            string[] rootPathParts = rootDir.Split(Path.DirectorySeparatorChar);
            int rootPathPartsCount = rootPathParts.Length;
            string lastFolder = rootPathParts.Last();
            mapTreeView.Nodes.Add(lastFolder, lastFolder);
            TreeNode rootFolderNode = mapTreeView.Nodes[lastFolder];
            rootFolderNode.ImageKey = "folder";
            rootFolderNode.SelectedImageKey = "folder";
            rootFolderNode.Tag = "folder";
            paths.Enqueue(rootDir);

           
            while (paths.Count > 0)
            {
                string path = paths.Dequeue();
                string[] pathParts = path.Split(Path.DirectorySeparatorChar);

                TreeNode temp = mapTreeView.Nodes[pathParts[rootPathPartsCount - 1]];
                int traversalDepth = File.Exists(path) ? pathParts.Length - 1 : pathParts.Length;
                for (int i = rootPathPartsCount; i < traversalDepth; i++)
                {
                    temp = temp.Nodes[pathParts[i]];
                }
                if (Directory.Exists(path))
                {
                    foreach (string subdirPath in GetSubdirsInDir(path))
                    {
                        string[] subdirPathParts = subdirPath.Split(Path.DirectorySeparatorChar);
                        paths.Enqueue(subdirPath);
                        string subdirName = subdirPathParts[subdirPathParts.Length - 1];
                        temp.Nodes.Add(subdirName, subdirName);
                        temp.Nodes[subdirName].ImageKey = "folder";
                        temp.Nodes[subdirName].SelectedImageKey = "folder";
                        temp.Nodes[subdirName].Tag = "folder";
                    }

                    foreach (string filePath in GetFilesInDir(path))
                    {
                        paths.Enqueue(filePath);
                    }
                }
                else if (File.Exists(path))
                {
                    string fileName = Path.GetFileNameWithoutExtension(pathParts[pathParts.Length - 1]);
                    temp.Nodes.Add(fileName, fileName);
                    temp.Nodes[fileName].ImageKey = "file";
                    temp.Nodes[fileName].SelectedImageKey = "file";
                    temp.Nodes[fileName].Tag = "file";
                }
            }
        }

        private string[] GetSubdirsInDir(string dir)
        {
            return Directory.GetDirectories(dir);
        }

        private string[] GetFilesInDir(string dir)
        {
            return Directory.GetFiles(dir);
        }

        private bool IsMapNode(TreeNode node)
        {
            return (string)node.Tag == "file";
        }

        // node double click
        private void mapTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (mapTreeView.SelectedNode != null && IsMapNode(mapTreeView.SelectedNode) && mapTreeView.SelectedNode != selectedNode)
            {
                // set previous selected node back to the standard file icon
                if (selectedNode != null)
                {
                    selectedNode.ImageKey = "file";
                    selectedNode.SelectedImageKey = "file";
                }

                selectedNode = mapTreeView.SelectedNode;

                // set newly selected node to the selected file icon
                selectedNode.ImageKey = "file-selected";
                selectedNode.SelectedImageKey = "file-selected";

                // this chops off root path since it is already included in config
                string fullPath = selectedNode.FullPath;
                string[] fullPathParts = fullPath.Split(Path.DirectorySeparatorChar);
                string modifiedFullPath = string.Join(Path.DirectorySeparatorChar.ToString(), fullPathParts.Skip(1));

                foreach (MapListListener listener in listeners)
                {
                    listener.OnMapSelected(modifiedFullPath);
                }
            }
        }

        // node single click
        private void mapTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode selectedNode = e.Node;
                mapTreeView.SelectedNode = e.Node;
                if ((string)selectedNode.Tag == "folder") 
                {
                    folderContextMenu.Show(mapTreeView, e.Location);
                }
                else if ((string)selectedNode.Tag == "file")
                {
                    fileContextMenu.Show(mapTreeView, e.Location);
                }
            }
        }

        private void mapTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        // this method is triggered after node label is edited but before it is actually committed
        // so it triggers the "afterAfterNodeLabelEdit" so the node label edit can be reacted on AFTER it has been committed
        private void mapTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            mapTreeView.LabelEdit = false;
            BeginInvoke(new Action(() => AfterAfterNodeLabelEdit(e.Node)));
        }


        private void AfterAfterNodeLabelEdit(TreeNode node)
        {
            string newName = node.Text;
            if (newName != "" && newName != mapTreeView.SelectedNode.Name) // no name change occurred
            {
                if ((string)node.Tag == "folder")
                {
                    Directory.Move($"./Resources/{oldPath}", $"./Resources/{mapTreeView.SelectedNode.FullPath}");
                }
                else if ((string)node.Tag == "file")
                {
                    Directory.Move($"./Resources/{oldPath}.map", $"./Resources/{mapTreeView.SelectedNode.FullPath}.map");
                }
            }
            else
            {
                mapTreeView.SelectedNode.Name = oldName;
                mapTreeView.SelectedNode.Text = oldName;
            }
        }
    }
}
