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

We also use extensive tests to ensure the quality of the library.

## Contribution

We encourage everyone to contribute to this project. Open-source lives from participation!
If you're not a programmer, you can help translating documentations. If you're a graphic artist you may help us with
illustrations. If you're not familiar with C#, do code reviews with us. If you're a theoretical computer scientists,
help us calculating bounds. If you encounter a bug, a vulnerability, a code quality issue, report it! Every input is
welcome.

If you'd like to donate to us. Point out a platform where someone can donate to us. I currently have no clue about this
topic.

## Collection List

Ticked means available in the newest package.

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
- [X] Queues
    - [X] **All-Types:** Thread-Safe, Blocking (Sammlung.Queues.Concurrent.BlockingDeque)
    - [X] Continuous Queue / Deque
        - [X] Non-Thread-Safe (Sammlung.Queues.ArrayDeque)
    - [X] Pointer Queue / Deque
        - [X] Non-Thread-Safe (Sammlung.Queues.LinkedDeque)
        - [X] Thread-Safe, Lock-Free (Sammlung.Queues.Concurrent.LockFreeLinkedDeque)
    - [ ] File-System Based Queue / Deque
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, with transactions
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
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free
    - [ ] BipBuffers
        - [ ] Non-Thread-Safe
        - [ ] Thread-Safe, Blocking
        - [ ] Thread-Safe, Lock-Free

## Literature

- [Algorithms and Data Structures - Kurt Mehlhorn, Peter Sanders](https://doi.org/10.1007/978-3-540-77978-0)
- [CAS-Based Lock-Free Algorithm for Shared Deques - Maged M. Michael](https://doi.org/10.1007/978-3-540-45209-6_92)
  
## Documentation

Our documentation can be found [here](https://schleicher-dev.github.io/Sammlung/).

## Contributors

- Ralf Schleicher



