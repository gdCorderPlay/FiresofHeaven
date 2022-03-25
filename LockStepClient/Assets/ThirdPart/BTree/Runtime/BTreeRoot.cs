
using BTreeFrame;
namespace Battle.Logic.AI.BTree
{
    public class BTreeRoot
    {
        public BTreeNode m_TreeRoot { get; private set; }

        public void CreateBevTree()
        {
            //BTreeNodeSerialization.WriteXML(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadXML("Btree");
           // var _config = BTreeNodeSerialization.ReadXML("Btree_test2");
            var _config = BTreeNodeSerialization.ReadBinary("GDAI");
            BTreeNodeFactory.Init();
            m_TreeRoot = BTreeNodeFactory.CreateBTreeRootFromConfig(_config);
        }

        public BTreeRoot(string treeConfig)
        {
            var _config = TreeHelper.CreateTreeConfig(treeConfig);

            m_TreeRoot = BTreeNodeFactory.CreateBTreeRootFromConfig(_config);
        }

        public void Tick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            if (m_TreeRoot.Evaluate(_input))
            {
                m_TreeRoot.Tick(_input, ref _output);
            }
            else
            {
                m_TreeRoot.Transition(_input);
            }
        }
    }
    
}
