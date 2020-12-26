# ![Sammlung](Misc/Logo/LogoWithName.svg)

Sammlung (/'zamlʊŋ/) is german for *collection* and is pronounced *some-loong*.

This repository contains a library for C# containing a plethora of collection types which are not provided by the 
.NET standard libraries. The NuGet package will be provided for both **.NET Framework** and **.NET Core**.

## Rationale

There are a bunch of collection NuGet packages out there, which contain a single collection type.
These collections are doing well most of the time and they do have their raison d'être. But these
container types are naïve implementations in most cases.

## Vision

Sammlung has the vision to be the go-to NuGet package, when it comes to collections beyond the standard 
collections. We aim to deliver high-end default collection implementations as well as multiple options
for specific collection types. Therefore the programmer has the choice to use a particular implementation
if needed.

Moreover we are aiming for collections which perform on the file system. In this case we prefer
cache oblivious algorithms.

For every collection class there should be a benchmark, testing the performance of the particular 
container. When possible we deliver theoretical bounds for the particular operations of a collection.

## Provided Collections

 - Bidirectional Dictionaries (Single-Threaded, Thread-Safe)
 - Multi-Key Dictionaries (Single-Threaded, Thread-Safe)
 - Circular Buffer
   - Fixed size variant (Single-Threaded)
 - Heaps / Priority Queues
   - Binary heap (Single-Threaded, Thread-Safe)

## Further Documentation

1. [Bidirectional Dictionaries](Docs/BidiDictionaries.md)
1. [Multi-Key Dictionaries](Docs/MultiKeyDictionaries.md)
1. [Circular Buffers](Docs/CircularBuffers.md)
1. [Heaps / Priority Queues](Docs/Heaps.md)

## Missing Collection Types
 - [ ] Circular Buffers
    - [ ] Circular Buffer w/ fixed size
      - [ ] Concurrent
    - [ ] Circular Buffer w/ growing behavior
      - [ ] Single-Threaded
      - [ ] Concurrent
 - [ ] Heaps
    - [ ] Pairing Heap
      - [ ] Single-Threaded
      - [ ] Concurrent
    - [ ] Fibonacci Heap
      - [ ] Single-Threaded
      - [ ] Concurrent
    - [ ] Brodal Queue
      - [ ] Single-Threaded
      - [ ] Concurrent
 - [ ] Queues
   - [ ] Queue w/ growing circular buffer
 - [ ] Graphs
   - [ ] Undirected Graph
   - [ ] Directed Graph
   - [ ] Multi Graph
   - [ ] Algorithms on Graphs (poss. Separate NuGet package)
     - [ ] Breadth-first search (BFS)
     - [ ] Depth-first search (DFS)
     - [ ] Dijkstra-Algorithm
     - [ ] Kruskal-Algorithm
     - [ ] Minimum Spanning tree (MST)
 - [ ] Search Trees
   - [ ] Binary Search Tree
   - [ ] ABSearchTree :: (a,b)-tree
   - [ ] RedBlackSearchTree :: red-black tree


