using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Remotion.Linq.Clauses;

namespace Web.Utilities
{
    public class TreeBuilder<T> where T : class
    {
        private List<T> _allItems;

        /// <summary>
        /// 第一个T:parent
        /// 第二个T:child
        /// </summary>
        private Func<T, T, bool> _childPredicate;
        public TreeBuilder(List<T> allItems, Func<T, T, bool> childPredicate)
        {
            _allItems = allItems;
            _childPredicate = childPredicate;
        }

        public IEnumerable<TreeNode<T>> GetTreeNode(T root)
        {
            return from item in _allItems
                        where _childPredicate(root, item)
                        select new TreeNode<T>()
                        {
                            Value = item,
                            ChildNode = GetTreeNode(item)
                        };
        }

        public IEnumerable<TreeNode<T>> Build(T t)
        {
            return GetTreeNode(t);
        }
    }

    public class TreeNode<T>
    {
        public T Value { get; set; }
        public IEnumerable<TreeNode<T>> ChildNode { get; set; }
    }
}
