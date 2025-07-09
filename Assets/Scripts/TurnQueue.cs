using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Turn_based_Combat;
using UnityEngine;

public class TurnQueue
{
    private List<TurnEntry> entries;

    public TurnQueue()
    {
        entries = new List<TurnEntry>();
    }
    
    private struct TurnEntry : IComparable<TurnEntry>
    {
        public Character character { get; }
        public Turn turn { get; }

        public TurnEntry(Character character, Turn turn)
        {
            this.character = character;
            this.turn = turn;
        }

        public int CompareTo(TurnEntry other)
        {
            return other.character.initiative.CompareTo(character.initiative);
        }
    }

    public void Enqueue(Character character, Turn turn)
    {
        entries.Add(new TurnEntry(character, turn));
    }

    public (Character character, Turn turn) Dequeue()
    {
        if (entries.Count == 0)
            Debug.LogError("Queue is empty");
        var entry = entries[0];
        return (entry.character, entry.turn);
    }
    
    public (Character character, Turn turn) Peek()
    {
        if (entries.Count == 0)
            Debug.LogError("Queue is empty");

        var entry = entries[0];
        return (entry.character, entry.turn);
    }

    public void Sort() => entries.Sort();

    public void Remove(Character character)
    {
        entries.RemoveAll(entry => entry.character == character);
    }

    public bool Contains(Character character)
    {
        return entries.Any(entry => entry.character == character);
    }

    public int Count => entries.Count;
    
    public void Clear() => entries.Clear();

    public void Requeue(Character character, Turn turn)
    {
        Remove(character);
        Enqueue(character, turn);
    }

    public IEnumerator<(Character character, Turn turn)> GetEnumerator()
    {
        foreach (var entry in entries)
        {
            yield return (entry.character, entry.turn);
        }
    }

}
