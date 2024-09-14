using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSort : MonoBehaviour
{
    [SerializeField] Element element = null;
    [SerializeField] Button buttonSort = null;
    [SerializeField] Button buttonShuffle = null;
    [SerializeField] Button buttonRandom = null;
    [SerializeField] Text textTimer = null;
    private Element[] array = new Element[10];
    private float startPosX = -7.8f;
    private const float posY = 2.5f;
    private float Scale = 3.0f;
    private const float offsetScale = 1.14f;
    private bool clickTwoElements = false;
    private byte firstClickElementID = 0;
    private byte numElementsMoving = 0;
    private float timeSorting = 0.0f;
    private bool timerSorting = false;

    private void OnEnable()
    {
        Element.elementClick += ClickElementEvent;
        Element.elementMove += isMoving;

        buttonShuffle.onClick.AddListener(ShuffleArray);
        buttonSort.onClick.AddListener(SortArray);
        buttonRandom.onClick.AddListener(InitRandomArray);
    }
    private void OnDisable()
    {
        Element.elementClick -= ClickElementEvent;
        Element.elementMove -= isMoving;
    }
    private void Start()
    {
        InitArray();
        ShowArray();
    }
    private void InitArray(bool random = false)
    {
        float componentColor = 0.0f;
        System.Random rnd = new System.Random();

        for (byte i = 0; i < array.Length; i++)
        {
            if (random) Destroy(array[i].gameObject);
            array[i] = Instantiate(this.element, new Vector3(startPosX, posY, 0.0f), Quaternion.identity) as Element;
            array[i].elementID = i;
            array[i].transform.localScale = new Vector3(Scale, Scale, 1.0f);

            componentColor = 0.1f * i;
            array[i].InitColor = new Color(componentColor, componentColor, componentColor);

            if (random) Scale = rnd.Next(3, 10); else Scale *= offsetScale;
        }
    }
    private void ShowArray()
    {
        float elementPosX = startPosX;
        float offsetPosX = Mathf.Abs(startPosX * 2) / (array.Length - 1);

        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.localPosition = new Vector3(elementPosX, posY, 0.0f);
            elementPosX += offsetPosX;
        }
    }
    private void SwapElementArray(ref Element element1, ref Element element2)
    {
        byte tempID = element1.elementID;
        element1.elementID = element2.elementID;
        element2.elementID = tempID;

        Element temp = element1;
        element1 = element2;
        element2 = temp;
    }
    private void ShuffleArray()
    {
        if (numElementsMoving == 0)
        {
            System.Random rnd = new System.Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                SwapElementArray(ref array[n], ref array[k]);
            }
            ShowArray();
        }
    }
    void InitRandomArray()
    {
        if (numElementsMoving == 0)
        {
            InitArray(true);
            ShowArray();
        }
    }
    private void SortArray()
    {
        StartCoroutine(StartSortingTimer());
        StartCoroutine(SelectionSortArray());
    }
    void isMoving(byte elementID, bool isMoving)
    {
        numElementsMoving = isMoving ? ++numElementsMoving : --numElementsMoving;
        if (timerSorting == false)
        {
            if (numElementsMoving != 0)
            {
                buttonRandom.interactable = false;
                buttonShuffle.interactable = false;
                buttonSort.interactable = false;
            }
            else
            {
                
            }
        }
    }
    private void ClickElementEvent(byte elementID)
    {
        if (numElementsMoving == 0 && timerSorting == false)
        {
            if (clickTwoElements)
            {
                firstClickElementID = elementID;
                clickTwoElements = true;
            }
            else
            {
                clickTwoElements = false;
                if (firstClickElementID != elementID)
                {
                    array[firstClickElementID].MoveTo(array[elementID].transform.localPosition);
                    array[elementID].MoveTo(array[firstClickElementID].transform.localPosition);
                    SwapElementArray(ref array[firstClickElementID], ref array[elementID]);
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        float minute = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float millisSeconds = (timeToDisplay % 1) * 1000;

        textTimer.text = string.Format("{0:00}:{1:00}:{2:000}", minute, seconds, millisSeconds);
        if (minute == 10) timerSorting = false;
    }
    private IEnumerator StartSortingTimer()
    {
        timerSorting = false;
        timeSorting = 0;
        while (timerSorting)
        {
            timeSorting += Time.deltaTime;
            DisplayTime(timeSorting);
            yield return null;
        }
    }
    private IEnumerator SelectionSortArray()
    {
        buttonRandom.interactable = false;
        buttonShuffle.interactable = false;
        buttonSort.interactable = false;

        for (int i = 0; i < array.Length - 1; i++)
        {
            int min_idx = i;
            array[i].SetColor(Color.green);

            for (int j = i + 1; j < array.Length; j++)
            {
                array[j].SetColor(Color.green);
                yield return new WaitForSeconds(1);
                array[j].InitColor = array[j].InitColor;

                if (array[j].transform.localScale.x < array[min_idx].transform.localScale.x)
                {
                    array[j].SetColor(Color.blue);
                    if (min_idx != i) array[min_idx].InitColor = array[min_idx].InitColor;
                    min_idx = j;
                }
            }
            if (min_idx != i)
            {
                array[min_idx].MoveTo(array[i].transform.localPosition);
                array[i].MoveTo(array[min_idx].transform.localPosition);

                while (numElementsMoving != 0)
                {
                    yield return null;
                }
                array[i].InitColor = array[i].InitColor;
                SwapElementArray(ref array[min_idx], ref array[i]);
                ShowArray();
            }
            else
            {
                array[i].SetColor(Color.blue);
            }
        }
        array[array.Length - 1].SetColor(Color.blue);
        for (int j = 0; j < array.Length; j++)
        {
            array[j].InitColor = array[j].InitColor;
            yield return new WaitForSeconds(0.1f);
        }
        buttonRandom.interactable = true;
        buttonShuffle.interactable = true;
        buttonSort.interactable = true;
        timerSorting = false;
    }
}