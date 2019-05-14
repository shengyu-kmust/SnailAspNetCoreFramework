using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Utilities;
using Xunit;

namespace XUnitTestProject1
{
    public class TreeBuilderTest
    {
        private List<string> list = new List<string>
        {
            "1",
            "1-1",
            "1-1-1",
            "1-1-2",
            "1-2",
            "1-2-1",
            "1-2-2",
            "2",
            "2-1",
            "2-1-1",
            "2-1-2",
            "2-2",
            "2-2-1",
            "2-2-2",
        };
        [Fact]
        public void Test()
        {
            var categories = new List<category>() {
                new category(1, "Sport", 0),
                new category(2, "Balls", 1),
                new category(3, "Shoes", 1),
                new category(4, "Electronics", 0),
                new category(5, "Cameras", 4),
                new category(6, "Lenses", 5),
                new category(7, "Tripod", 5),
                new category(8, "Computers", 4),
                new category(9, "Laptops", 8),
                new category(10, "Empty", 0),
                new category(0, "Broken", 999),

            };
            try
            {
                //var builder = new TreeBuilder<string>(list, (parent, child) => child.StartsWith(parent) && child != parent);
                //var a = builder.Build("1");

                var builder = new TreeBuilder<category>(categories, (parent, child) => parent.Id == child.ParentId);
                var a = builder.Build(new category(0,"ddd",1));

                //var root = categories.GenerateTree(c => c.Id, c => c.ParentId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

           


        }
        public class category
        {
            public category(int a,string b,int c)
            {
                Id = a;
                Name = b;
                ParentId = c;
            }
            public int Id;
            public int ParentId;
            public string Name;

            public List<category> Subcategories;
        }

        #region MyRegion

        public class TreeItem<T>
        {
            public T Item { get; set; }
            public IEnumerable<TreeItem<T>> Children { get; set; }
        }

        #endregion

        }

    internal static class GenericHelpers
    {
        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeBuilderTest.TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeBuilderTest.TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }
}
