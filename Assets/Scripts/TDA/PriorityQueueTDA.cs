using System;
using UnityEngine;
using System.Collections.Generic;

public class PriorityQueueTDA<T>
{
    private List<(T Item, int Priority)> elements;

    public PriorityQueueTDA()
    {
        elements = new List<(T, int)>();
    }

    // Añadir un elemento con su prioridad
    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
        // Ordenar la lista en base a las prioridades (menor prioridad primero)
        elements.Sort((x, y) => x.Priority.CompareTo(y.Priority));
    }

    // Retirar el elemento de mayor prioridad (menor número)
    public T Dequeue()
    {
        if (IsEmpty())
           Debug.Log("La cola de prioridad está vacía.");

        T item = elements[0].Item;
        elements.RemoveAt(0); // Remover el primero de la lista
        return item;
    }

    // Consultar el elemento de mayor prioridad sin eliminarlo
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("La cola de prioridad está vacía.");

        return elements[0].Item;
    }

    // Verificar si la cola está vacía
    public bool IsEmpty()
    {
        return elements.Count == 0;
    }

    // Obtener el número de elementos en la cola
    public int Count()
    {
        return elements.Count;
    }
}