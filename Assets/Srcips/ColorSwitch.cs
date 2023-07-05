using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorSwitch : MonoBehaviour
{
    public int ID { get; private set; }

    private bool _completed;
    private bool _selected;
    private TextMeshPro _text;
    private SpriteRenderer _background;
    private SpriteRenderer _border;

    void Awake()
    {
        _border = transform.Find("Border").GetComponent<SpriteRenderer>();
        _background = transform.Find("Background").GetComponent<SpriteRenderer>();
        _text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetData(int id, Color color)
    {
        ID = id;
        _text.text = id.ToString();
        _background.color = color;
    }

    public void SetCompleted()
    {
        _completed = true;
        _text.text = "";
    }

    public void SetSelected(bool selected)
    {
        if (!_completed)
        {
            _selected = selected;
            if (_selected)
            {
                _border.color = Color.yellow;
            }
            else
            {
                _border.color = Color.black;
            }
        }
    }


}