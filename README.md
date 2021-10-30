Rivers
======
[![Build status](https://ci.appveyor.com/api/projects/status/yak3xv2e06jgcg92/branch/master?svg=true)](https://ci.appveyor.com/project/Washi1337/rivers)

Rivers is a light-weight graphing library written in C#. It contains a model for directed and undirected graphs, as well as a whole bunch of standard algorithms to analyse graphs.

Rivers is released under the MIT license.

Binaries
========
- [Nightly builds](https://ci.appveyor.com/project/Washi1337/rivers/build/artifacts)

Features
========
- Easy to use graph class modelled using adjacency lists for each node. This has the following advantages:
    - Optimised for quick insertion of nodes and edges.
    - Minimal memory footprint.
    - Efficient for sparse graphs.
- Support for directed as well as undirected graphs. The default is directed.
- Support for grouping nodes into subgraphs.
- Various graph generators to help building up common graph structures.
- Various built-in node traversal and search algorithms (including breadth first, depth first and more).
- Pathfinding algorithms such as Dijkstra and A*
- Partitioning and isomorphism finder algorithms
- Connectivity analysis such as finding strongly connected components.
- Built-in dominator analysis. Useful for control flow graph analysis.
    - Construct dominator trees from CFGs.
    - Get dominance frontier.
- Dot file import/export support. 
- Compiled for .NET Standard 2.0


Quick starters guide
=====================

Creating a graph
----------------

The `Graph` class contains two collections representing the nodes and edges of the graph. They work like any other collection in .NET, and you can add and remove items, and iterate through them.

```csharp
var graph = new Graph();

// Add nodes
graph.Nodes.Add(new Node("1"));
graph.Nodes.Add("2");
graph.Nodes.Add("3");

// Add edges.
graph.Edges.Add(new Edge("1", "2"));
graph.Edges.Add("2", "3");
```

By default, graphs are directed. If you want an undirected graph, use the second constructor:
```csharp
var graph = new Graph(false);
```

Inspecting and editing nodes
----------------------------
The `Node` class has various properties that give more insight about a node in the graph. Get a node by its name through the indexer of the `Graph.Nodes` property.
- Inspect, add or remove incoming and outgoing edges on the fly using the `IncomingEdges` and `OutcomingEdges` collections.
- Store custom defined properties in the `UserData` property.

```csharp
// Obtain the node.
var node = graph.Nodes["1"];

// Add an edge from 1 to 2, 3 and 4.
node.OutgoingEdges.Add(new Edge(node, graph.Nodes["2"]));
node.OutgoingEdges.Add(graph.Nodes["3"]);
node.OutgoingEdges.Add("4");

// Set user data.
node.UserData["MyProperty"] = 1234;
```

Perform graph searches and traversals
-------------------------------------
There are various extension methods defined to perform searches in the graph. Example:

```csharp
using Rivers.Analysis.Traversal;

var result = myNode.DepthFirstSearch(n => n.Name.Contains("A"));
if (result == null) 
{
    // There is no path from myNode to a node with the letter "A".
}
```

If you are more interested in the actual traversal of the nodes, you can use one of the `ITraversal` implementations, along with various built-in hooks such as the `TraversalOrderRecorder`.

```csharp
using Rivers.Analysis.Traversal;

var traversal = new DepthFirstTraversal();
var order = new TraversalOrderRecorder();
traversal.Run(myNode);

// Loop over all nodes in the order that the depth first algorithm traverses the graph.
foreach (var node in order.GetTraversal()) 
{
    // ...
}

// Gets the index of a specific node during the traversal. 
// This is more efficient than order.GetTraversal().IndexOf(myOtherNode).
int index = order.GetIndex(myOtherNode);
```

Categorizing graphs
-------------------
Rivers provides a bunch of standard structural analysis algorithms that detect all kinds of types of graphs.

```csharp
using Rivers.Analysis;

var graph = // ...
bool cyclic = graph.IsCyclic();
bool connected = graph.IsConnected();
bool tree = graph.IsTree();
bool regular = graph.IsRegular();
bool complete = graph.IsComplete();
```

Generating graphs
-----------------
The `Rivers.Generators` namespace contains various graph generators for building common graph structures easily.

```csharp
using Rivers.Generators;

var generator = new PathGenerator(true, 5);
var pathGraph = generator.GenerateGraph();
// pathGraph now contains the graph:
// 1 -> 2 -> 3 -> 4 -> 5
```

Dominator analysis
------------------
Dominator information can be obtained by creating a new instance of the `DominatorInfo` class.

```csharp
using Rivers.Analysis;

var cfg = new Graph();
var rootNode = cfg.Nodes.Add("root");
var someOtherNode = cfg.Nodes.Add("other");
// ...

var info = new DominatorInfo(rootNode);

var idom = info.GetImmediateDominator(someOtherNode);
var frontier = info.GetDominanceFrontier(someOtherNode);
```

Dot file support
----------------
Importing and exporting to dot files can be done using the `DotReader` and `DotWriter` classes. You can then use a tool such as http://webgraphviz.com/ to visualise the graph.

```csharp
using Rivers.Serialization.Dot;

// Importing
var reader = new StreamReader("mygraph.dot");
var dotReader = new DotReader(reader);
var myGraph = reader.Read();

...

// Exporting
var writer = new StreamWriter("mygraph2.dot");
var dotWriter = new DotWriter(writer);
dotWriter.Write(myGraph);
```
