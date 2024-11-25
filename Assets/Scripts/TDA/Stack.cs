using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack<T>
{
    int top = -1;
    T[] elements;
    int capacity;

    public Stack(T clase, int capacity)
    {
        this.capacity = capacity;
        elements = new T[capacity];

        for (int i = 0; i < capacity; i++)
        {
            Push(clase);
        }
    }

    public Stack(int capacity)
    {
        this.capacity = capacity;
        elements = new T[capacity];
    }



    public void Push(T newElement)
    {
        if (top == capacity - 1)
        {
            Resize(); // Aumentar el tamaño si la pila está llena
        }

        elements[++top] = newElement;
    }


    public T Pop()
    {
        if (top == -1)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        T element = elements[top];

        elements[top--] = default(T);

        return element;
    }


    public T Peek()
    {
        if (top == -1)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return elements[top];
    }


    public bool isEmpty()
    {

        return top == -1;


    }


    public int ShowStack()
    {
        int count = -1;
        for (int i = 0; i <= top; i++)
        {

            count++;
        }
        return count;
    }

    private void Resize()
    {
        capacity *= 2; // Duplicar la capacidad
        T[] newElements = new T[capacity]; // Crear un nuevo arreglo con la nueva capacidad
        Array.Copy(elements, newElements, elements.Length); // Copiar los elementos antiguos al nuevo arreglo
        elements = newElements; // Reemplazar el arreglo original con el nuevo
    }
}
