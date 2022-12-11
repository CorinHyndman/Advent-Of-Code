using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022
{
    public class TreeNode
    {
        public int FileSize;
        public string FileName;
        private readonly List<TreeNode> children = new();

        public TreeNode(string name, int size)
        {
            FileName = name;
            FileSize = size;
        }

        public TreeNode this[int i]
        {
            get { return children[i]; }
        }

        public TreeNode Parent { get; private set; }


        public ReadOnlyCollection<TreeNode> Children
        {
            get { return children.AsReadOnly(); }
        }

        public TreeNode AddChild(string name, int size)
        {
            var node = new TreeNode(name, size) { Parent = this };
            children.Add(node);
            return node;
        }

        public TreeNode EnterChild(string filename)
        {
            foreach (TreeNode node in children)
            {
                if (node.FileName == filename)
                {
                    return node;
                }
            };
            return this;
        }
        public TreeNode[] GetDirectories()
        {
            return children.Where(x => x.FileSize is 0).ToArray();
        }

        public List<int> GetTotalSizes()
        {
            if (children.Count is 0)
            {
                return new List<int>(FileSize);
            }

            List<int> totalSize = new();
            foreach (TreeNode node in children)
            {
                totalSize.AddRange(node.GetTotalSizes());
            }
            return totalSize;
        }
    }
}
