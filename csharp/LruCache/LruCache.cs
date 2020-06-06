namespace LruCache
{
    using System.Collections.Generic;

    class Node<K, V>
    {
        public K Key { get; }
        public V Value { get; }
        public Node<K, V> Next { get; set; }
        public Node<K, V> Previous { get; set; }

        public Node(K key, V value)
        {
            Key = key;
            Value = value;
            Next = null;
            Previous = null;
        }
    }

    public class LruCache<K, V>
    {
        private readonly int capacity;
        private readonly Dictionary<K, Node<K, V>> dictionary;
        private Node<K, V> leastRecentlyAccessed;
        private Node<K, V> mostRecentlyAccessed;

        public LruCache(int capacity)
        {
            this.capacity = capacity;
            this.dictionary = new Dictionary<K, Node<K, V>>();
            this.leastRecentlyAccessed = null;
            this.mostRecentlyAccessed = null;
        }

        public object Get(K key)
        {
            Node<K, V> node;

            // Return null if we don't have a value for the given key
            if (!this.dictionary.TryGetValue(key, out node))
            {
                return null;
            }

            // Update least recently accessed to point to the next least recently accessed
            if (node == this.leastRecentlyAccessed)
            {
                this.leastRecentlyAccessed = node.Next;
            }

            // Update most recently accessed to point to the node with the given key
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }

            node.Next = null;
            node.Previous = this.mostRecentlyAccessed;
            this.mostRecentlyAccessed.Next = node;
            this.mostRecentlyAccessed = node;

            return node.Value;
        }

        public void Set(K key, V value)
        {
            var node = new Node<K, V>(key, value);

            // Special case for first node added
            if (this.mostRecentlyAccessed == null && this.leastRecentlyAccessed == null)
            {
                this.mostRecentlyAccessed = node;
                this.leastRecentlyAccessed = node;
                this.dictionary.Add(key, node);
                return;
            }

            // If the cache is at capacity remove the least recently accessed node
            if (this.dictionary.Count == this.capacity)
            {
                this.dictionary.Remove(this.leastRecentlyAccessed.Key);
                this.leastRecentlyAccessed = this.leastRecentlyAccessed.Next;

                if (this.leastRecentlyAccessed != null)
                {
                    this.leastRecentlyAccessed.Previous = null;
                }
            }

            // Update most recently accessed to point at the new node
            node.Previous = this.mostRecentlyAccessed;
            this.mostRecentlyAccessed.Next = node;
            this.mostRecentlyAccessed = node;

            this.dictionary.Add(key, node);
        }
    }
}