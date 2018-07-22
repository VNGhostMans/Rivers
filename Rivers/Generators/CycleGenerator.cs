﻿using System;

namespace Rivers.Generators
{
    /// <summary>
    /// Provides a mechanism for generating cycle graphs. 
    /// </summary>
    public class CycleGenerator : IGraphGenerator
    {
        /// <summary>
        /// Creates a new cycle graph generator.
        /// </summary>
        /// <param name="directed">Determines whether the generated graphs should be directed or not.</param>
        /// <param name="length">The length of the cycles generated graphs will have.</param>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when the given length is negative.</exception>
        public CycleGenerator(bool directed, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

            Directed = directed;
            Length = length;
        }

        /// <summary>
        /// Gets a value indicating whether the generator should generate directed or undirected graphs. 
        /// </summary>
        public bool Directed
        {
            get;
        }
        
        /// <summary>
        /// Gets the length of the cycle that generated graphs will have. 
        /// </summary>
        public int Length
        {
            get;
        }
        
        /// <inheritdoc />
        public Graph GenerateGraph()
        {
            var g = new Graph(Directed);

            for (int i = 1; i <= Length; i++)
            {
                g.Nodes.Add(i.ToString());
                if (i > 1)
                    g.Edges.Add((i - 1).ToString(), i.ToString());
            }

            if (Length > 0)
                g.Edges.Add(Length.ToString(), "1");
            
            return g;
        }
    }
}