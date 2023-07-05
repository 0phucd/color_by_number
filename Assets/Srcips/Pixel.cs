using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pixel : MonoBehaviour
{
public int ID { get; private set; }

 private TextMeshPro _text;
 private Color _pixelColor;

 private SpriteRenderer _background;
 private SpriteRenderer _border;
public bool IsFilledIn
{
    get
    {
        if (_background.color == _pixelColor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

void Awake()
{
    _border = transform.Find("Border").GetComponent<SpriteRenderer>();
    _background = transform.Find("Background").GetComponent<SpriteRenderer>();
    _text = transform.Find("Text").GetComponent<TextMeshPro>();
}

public void SetData(Color color, int colorID)
{
    ID = colorID;
    _pixelColor = color;
    _border.color = new Color(0.95f, 0.95f, 0.95f, 1);
    _text.text = colorID.ToString();

    _background.color = Color.Lerp(new Color(_pixelColor.grayscale, _pixelColor.grayscale, _pixelColor.grayscale), Color.white, 0.85f);
}

public void SetSelected(bool selected)
{
    if (selected)
    {
        if (!IsFilledIn)
        {
            _background.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }
    else
    {
        if (!IsFilledIn)
        {
            _background.color = Color.Lerp(new Color(_pixelColor.grayscale, _pixelColor.grayscale, _pixelColor.grayscale), Color.white, 0.85f);
        }
    }
}

public void Fill()
{
    if (!IsFilledIn)
    {
        _border.color = _pixelColor;
        _background.color = _pixelColor;
        _text.text = "";
    }
}

    
}