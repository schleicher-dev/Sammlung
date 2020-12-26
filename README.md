# README

Die C#-System-Bibliotheken lassen einige Datenstrukturen vermissen, welche in anderen 
Programmiersprachen mitgeliefert werden. Daher wurde die vorliegende Bibliothek erstellt,
um nach und nach noch mehr Datenstrukturen für C# in der Breite bereit zu stellen.

## Wunschliste

Die Wunschliste umfasst mehrere abstrakte Datenstrukturen. Die Unterpunkte sind spezielle 
Implementierungen dieser Datenstrukturen.

### Datenstrukturen
 - [ ] IMultiKeyDictionary - Multi-Key Dictionary - Mehrere Schlüssel, ein Wert
   - [X] MultiKeyDictionary
     - [X] Implementiert
     - [ ] Ausgiebig getestet
     - [ ] Dokumentiert
   - [X] ConcurrentMultiKeyDictionary
     - [X] Implementiert
     - [ ] Ausgiebig getestet
     - [ ] Dokumentiert
 - [ ] IHeap - Heap / PriorityQueue
   - [X] BinaryHeap
     - [X] Implementiert
     - [X] Ausgiebig getestet
     - [ ] Dokumentiert
   - [X] PairingHeap / FibonacciHeap
 - [ ] IRingBuffer - RingPuffer
   - [ ] RingBuffer
 - [ ] Queue
   - [ ] Queue mit wachsendem RingBuffer
 - [ ] Graphen
   - [ ] UnDiGraph :: Ungerichteter Graph
   - [ ] DiGraph :: Gerichteter Graph
   - [ ] MultiGraph :: Multi Graph
 - [ ] ISearchTree :: Suchbaum
   - [ ] BinarySearchTree
   - [ ] ABSearchTree :: (a,b)-tree
   - [ ] RedBlackSearchTree :: red-black tree
 - [X] IBidiDictionary - Zweiwege Dictionary - Bijektives Mapping
   - [X] BidiDictionary
     - [X] Implementiert
     - [X] Ausgiebig getestet
     - [X] Dokumentiert
   - [X] ConcurrentBidiDictionary
     - [X] Implementiert
     - [X] Ausgiebig getestet
     - [X] Dokumentiert


