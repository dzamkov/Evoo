using System;
using System.Collections.Generic;

namespace Evoo
{
    /// <summary>
    /// Represents a bounded (plane shaped) area divided into faces, edges and vertices with custom data. The terrain
    /// implements the algorithim detailed here http://www.cognigraph.com/ROAM_homepage/ROAM2/.
    /// </summary>
    public class Terrain<V, E>
    {
        public Terrain(V[] Corners, E[] Edges, E RootEdge)
        {
            // Root diamond
            Diamond root = new Diamond();
            root.ParentRelationA = 0;
            root.ParentRelationB = 0;
            root.EdgeData = RootEdge;

            // "Greater diamonds" contain terrain corner information
            Diamond[] greaterdiamonds = new Diamond[4];
            for (int t = 0; t < 4; t++)
            {
                Diamond cur = greaterdiamonds[t] = new Diamond();
                root.Ancestors[t] = cur;
                cur.VertexData = Corners[t];
            }
            greaterdiamonds[1].Children[0] = root;
            greaterdiamonds[3].Children[0] = root;
            
        }

        /// <summary>
        /// A recursive square shaped area.
        /// </summary>
        public class Diamond
        {
            public Diamond()
            {
                this.Ancestors = new Diamond[4];
                this.Children = new Diamond[4];
            }

            public Diamond[] Children;
            public Diamond[] Ancestors;
            public V VertexData;
            public E EdgeData;
            public byte ParentRelationA;
            public byte ParentRelationB;

            public Diamond ParentA
            {
                get
                {
                    return this.Ancestors[DiamondParentA];
                }
            }

            public Diamond ParentB
            {
                get
                {
                    return this.Ancestors[DiamondParentB];
                }
            }

            /// <summary>
            /// Gets a border of the diamond. This border is a parent of the specified
            /// kid.
            /// </summary>
            public Diamond GetBorder(byte K)
            {
                return K < 2 ?
                    (this.ParentA.Children[((K * 2) + 1 + this.ParentRelationA) % 4]) :
                    (this.ParentB.Children[((K * 2) + 1 + this.ParentRelationB) % 4]);
            }

            /// <summary>
            /// Splits the diamond, causing the 4 child diamonds to come into existance.
            /// </summary>
            public void Split()
            {
                
            }
        }

        public const byte DiamondAnsector = 0;
        public const byte DiamondParentA = 1;
        public const byte DiamondOlderAnsector = 2;
        public const byte DiamondParentB = 3;

        private Diamond _Root;
    }
}

