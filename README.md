# ![Sammlung](Images/LogoWithName.svg)

Sammlung (/'zamlʊŋ/) is german for *collection* and is pronounced *some-loong*.

This repository contains a library for C# containing a plethora of collection types which are not provided by the .NET
standard libraries. The NuGet package will be provided for both **.NET Framework** and **.NET Core**.

## Rationale

There are a bunch of collection NuGet packages out there, which contain a single collection type. These collections are
doing well most of the time and they do have their raison d'être. But these container types are naïve implementations in
most cases.

## Vision

Sammlung has the vision to be the go-to NuGet package, when it comes to collections beyond the standard collections.

We aim to deliver:

- high-end default collection implementations
- multiple options for a container type, if reasonable
- container types which operate memory-aware and perform on the file system, in this case we prefer cache-oblivious
  algorithms.

For every collection class there should be a benchmark, testing the performance of the particular container. When
possible we deliver theoretical bounds for the particular operations of a collection.

## Contributing

We encourage everyone to contribute to this project. Open-source lives from participation!
If you're not a programmer, you can help translating documentations. If you're a graphic artist you may help us with
illustrations. If you're not familiar with C#, do code reviews with us. If you're a theoretical computer scientists,
help us calculating bounds. If you encounter a bug, a vulnerability, a code quality issue, report it! Every input is
welcome.

If you'd like to donate to us. Point out a platform where someone can donate to us. I currently have no clue about this
topic.

## Provided Collections

- Bidirectional Dictionaries (Single-Threaded, Thread-Safe)
- Multi-Key Dictionaries (Single-Threaded, Thread-Safe)
- Circular Buffer
    - Fixed size variant (Single-Threaded)
- Heaps / Priority Queues
    - Binary heap (Single-Threaded, Thread-Safe)

## Missing Collection Types

- [ ] Dictionaries
    - [ ] Bidirectional Dictionary
        - [X] Non-Thread-Safe (Sammlung.Dictionaries.BidiDictionary)
        - [X] Thread-Safe, Blocking (Sammlung.Dictionaries.Concurrent.BlockingBidiDictionary)
        - [ ] Thread-Safe, Lock-Free
    - [ ] Multi-Key Dictionary
        - [X] Non-Thread-Safe (Sammlung.Dictionaries.MultiKeyDictionary)
        - [X] Thread-Safe, Blocking (Sammlung.Dictionaries.Concurrent.BlockingMultiKeyDictionary)
        - [ ] Thread-Safe, Lock-Free
- [ ] Heaps
    - [ ] Binary Heap
        - [X] Non-Thread-Safe (Sammlung.Heaps.BinaryHeap)
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free
    - [ ] Pairing Heap
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free
    - [ ] Fibonacci Heap
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free
    - [ ] Brodal Queue
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free
- [ ] Queues
    - [ ] Continuous Queue
        - [X] Non-Thread-Safe (Sammlung.Queues.ArrayDeque)
        - [X] Thread-Safe, Blocking (Sammlung.Queues.Concurrent.BlockingArrayDeque)
        - [ ] Thread-Safe, Lock-Free (Maybe not possible)
    - [ ] Continuous Deque
        - [X] Non-Thread-Safe (Sammlung.Queues.ArrayDeque)
        - [X] Thread-Safe, Blocking (Sammlung.Queues.Concurrent.BlockingArrayDeque)
        - [ ] **Maybe not possible:** Thread-Safe, Lock-Free
    - [ ] Pointer Queue
        - [X] Non-Thread-Safe (Sammlung.Queues.LinkedDeque)
        - [ ] Thread-Safe, Lock-Free
    - [ ] Pointer Deque
        - [X] Non-Thread-Safe (Sammlung.Queues.LinkedDeque)
        - [ ] Thread-Safe, Lock-Free
- [ ] Queues
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
- [ ] Circular Buffers
    - [ ] Circular Buffer
        - [ ] Single-Threaded
        - [ ] Blocking / Non-Blocking
    - [ ] BipBuffers
        - [ ] Single-Threaded
        - [ ] Non-Blocking

## Documentation

1. [Bidirectional Dictionaries](Wiki/Documentation/BidiDictionaries.md)
1. [Multi-Key Dictionaries](Wiki/Documentation/MultiKeyDictionaries.md)
1. [Circular Buffers](Wiki/Documentation/CircularBuffers.md)
1. [Heaps / Priority Queues](Wiki/Documentation/Heaps.md)

## Literature

- [Algorithms and Data Structures - Kurt Mehlhorn, Peter Sanders](https://www.springer.com/de/book/9783540779773) ::
  ISBN: 978-3-540-77977-3

## Contributors

- Ralf Schleicher



