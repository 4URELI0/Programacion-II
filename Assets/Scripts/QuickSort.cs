using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuickSort : MonoBehaviour
{
    public int[] rand;
    public Text text1;
    public Text text2;
    public Button btn = null;
    public Button btn2 = null;
    private void OnEnable()
    {
        btn.onClick.AddListener(QuickSortMostrar);
        btn2.onClick.AddListener(ResetRandomValues);
    }
    private void Start()
    {
        rand = new int[10];
        GenerateRandomValues();
    }
    /*Genera valores aleatorio y actualiza a text1*/
    private void GenerateRandomValues()
    {
        text1.text = "";
        text2.text = "";
        for (int i = 0; i < 10; ++i)
        {
            rand[i] = Random.Range(0, 100);
            text1.text += " " + rand[i];
        }
    }
    public void ResetRandomValues()
    {
        GenerateRandomValues();
    }
    /*Limpiamos el text2 antes de mostrar los valores ordenados*/
    public void QuickSortMostrar()
    {
        Quicksort(rand);
        for (int i = 0; i < 10; ++i)
        {
            text2.text += " " + rand[i];
        }
    }
    private void Quicksort(int[] array)
    {
        Quicksort(array, 0, array.Length - 1);
    }
    private void Quicksort(int[] array, int start, int ende)
    {
        if (start >= ende)
        {
            return;
        }
        int num = array[start];
        int i = start;
        int j = ende;
        while (i < j)
        {
            while (i < j && array[j] >= num)
            {
                j--;
            }
            array[i] = array[j];
            while(i < j && array[i] < num)
            {
                i++;
            }
            array[j] = array[i];
        }
        array[i] = num;
        Quicksort(array, start, i - 1);
        Quicksort(array, i + 1, ende);
    }
}