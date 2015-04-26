﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace VTL.ListView
{
    public class Row : MonoBehaviour
    {

        private ListViewManager listViewManager;

        public bool isSelected = false;

        public Guid guid; 

        public Color unselectedColor = Color.white; //new Color(0, 0, 0, 100 / 255);
        public Color selectedColor = new Color(0.1f, 0.1f, 0.1f, 0.4f);

        private Image image;

        public List<GameObject> rowElements = new List<GameObject>();

        void Start()
        {
            listViewManager = transform.parent.
                              transform.parent.
                              transform.parent.gameObject.GetComponent<ListViewManager>();

            image = gameObject.GetComponent<Image>();
        }

        public void Initialize(object[] fieldData, Guid _guid,
                               List<HeaderElementInfo> headerElementInfo, 
                               GameObject RowElementPrefab)
        {
            guid = _guid;
            transform.localScale = Vector3.one;

            rowElements = new List<GameObject>();

            for (int i=0; i<fieldData.Length; i++)
            {
                rowElements.Add(Instantiate(RowElementPrefab));
                rowElements[i].transform.SetParent(transform);
                rowElements[i].transform.localScale = Vector3.one;
                rowElements[i].GetComponentInChildren<LayoutElement>().preferredWidth = 
                    headerElementInfo[i].preferredWidth;
                Text rowElementText = rowElements[i].GetComponentInChildren<Text>();
                rowElementText.text = 
                    StringifyObject(fieldData[i], headerElementInfo[i].formatString, headerElementInfo[i].dataType); 
                rowElementText.alignment = 
                    headerElementInfo[i].horizontalAlignment == HorizontalAlignment.Left ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
            }
        }

        private static string StringifyObject(object obj, string formatString, DataType dataType)
        {
            if (dataType == DataType.String)
                return (string)obj;
            else if (formatString != null)
            {
                if (dataType == DataType.Int)
                    return ((int)obj).ToString(formatString);
                else if (dataType == DataType.Float)
                    return ((float)obj).ToString(formatString);
                else if (dataType == DataType.Double)
                    return ((double)obj).ToString(formatString);
                else if (dataType == DataType.DateTime)
                    return ((DateTime)obj).ToString(formatString);
                else if (dataType == DataType.TimeSpan)
                    return ((TimeSpan)obj).ToString();
                else
                    return obj.ToString();
            }
            else
                return obj.ToString();
        }

        public void SetFields(object[] fieldData, Guid _guid, bool selected,
                              List<HeaderElementInfo> headerElementInfo)
        {
            guid = _guid;
            isSelected = selected;
            image.color = isSelected ? selectedColor : unselectedColor;

            Debug.Log(guid.ToString() + ", " + isSelected.ToString());

            for (int i = 0; i < headerElementInfo.Count; i++)
            {
                rowElements[i].GetComponentInChildren<Text>().text =
                    StringifyObject(fieldData[i], headerElementInfo[i].formatString, headerElementInfo[i].dataType); 
            }
        }

        public void SetFields(Dictionary<string, object> rowData, Guid _guid, bool selected,
                              List<HeaderElementInfo> headerElementInfo)
        {
            guid = _guid;
            isSelected = selected;
            image.color = isSelected ? selectedColor : unselectedColor;

            for (int i = 0; i < headerElementInfo.Count; i++)
            {
                rowElements[i].GetComponentInChildren<Text>().text =
                    StringifyObject(rowData[headerElementInfo[i].text], headerElementInfo[i].formatString, headerElementInfo[i].dataType);
            }
        }

        public void OnSelectEvent()
        {
            isSelected = !isSelected;
            image.color = isSelected ? selectedColor : unselectedColor;
            listViewManager.SetRowSelection(guid, isSelected);
        }
    }
}
