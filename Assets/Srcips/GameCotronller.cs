using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCotronller : MonoBehaviour
{
 public Texture2D texture;
 private Pixel[,] _pixels;    
 private Camera _camera;
 private int _ID = 1;
 private Dictionary<Color, int> Colors = new Dictionary<Color, int>();
 private List<ColorSwitch> ColorSwitches = new List<ColorSwitch>();
 private Dictionary<int, List<Pixel>> PixelGroups = new Dictionary<int, List<Pixel>>();
 private RaycastHit2D[] Hits = new RaycastHit2D[1];
 private ColorSwitch _selectedColorSwitch;
 public GameObject colorBox;
 public GameObject picture;

    void Awake()
    {
        _camera = Camera.main;

        CreatePixelMap();
        CreateColorSwitches();
    }

    void CreatePixelMap()
    {
        Color[] colors = texture.GetPixels();
        
        
        _pixels = new Pixel[texture.width, texture.height];

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (colors[x + y * texture.width].a != 0)
                {
                    GameObject go = GameObject.Instantiate(Resources.Load("Pixel") as GameObject);
                    go.transform.position = new Vector3(x, y);

                    int id = _ID;
                    if (Colors.ContainsKey(colors[x + y * texture.width]))
                    {
                        id = Colors[colors[x + y * texture.width]];
                    }
                    else
                    {
                        Colors.Add(colors[x + y * texture.width], _ID);
                        _ID++;
                    }

                    _pixels[x, y] = go.GetComponent<Pixel>();
                    _pixels[x, y].SetData(colors[x + y * texture.width], id);

                    if (!PixelGroups.ContainsKey(id))
                    {
                        PixelGroups.Add(id, new List<Pixel>());                       
                    }
                    PixelGroups[id].Add(_pixels[x, y]);
                    go.transform.SetParent(picture.transform);
                }
            }
        }
    }

    void CreateColorSwitches()
    {     
        foreach (KeyValuePair<Color, int> kvp in Colors)
        {
            GameObject spawnColorswitch = GameObject.Instantiate(Resources.Load("ColorSwitch") as GameObject);

            float offset = 1.2f;
            spawnColorswitch.transform.position = new Vector2(kvp.Value * 10 * offset, -10);
            ColorSwitch colorswitch = spawnColorswitch.GetComponent<ColorSwitch>();
            colorswitch.SetData(kvp.Value, kvp.Key);

            ColorSwitches.Add(colorswitch);
            spawnColorswitch.transform.SetParent(colorBox.transform);
        }
    }

    void DeselectAllColorSwitches()
    {
        for (int n = 0; n < ColorSwitches.Count; n++)
        {
            ColorSwitches[n].SetSelected(false);
        }
    }

    void Update()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(mousePos.x);
        int y = Mathf.FloorToInt(mousePos.y);

        Pixel hoveredPixel = null;
        if (x >= 0 && x < _pixels.GetLength(0) && y >= 0 && y < _pixels.GetLength(1))
        {
            if (_pixels[x, y] != null)
            {
                hoveredPixel = _pixels[x, y];
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                int hitCount = Physics2D.RaycastNonAlloc(mousePos, Vector2.zero, Hits);
                for (int n = 0; n < hitCount; n++)
                {
                    if (Hits[n].collider.CompareTag("ColorSwitch"))
                    {
                        SelectColorSwitch(Hits[n].collider.GetComponent<ColorSwitch>());
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (hoveredPixel != null && !hoveredPixel.IsFilledIn)
                {
                    if (_selectedColorSwitch != null && _selectedColorSwitch.ID == hoveredPixel.ID)
                    {
                        hoveredPixel.Fill();
                        if (CheckIfSelectedComplete())
                        {
                            _selectedColorSwitch.SetCompleted();
                        }
                    }
                }
            }
        }
    }
    
    void SelectColorSwitch(ColorSwitch _switch)
    {
        if (_selectedColorSwitch != null)
        {
            for (int n = 0; n < PixelGroups[_selectedColorSwitch.ID].Count; n++)
            {
                PixelGroups[_selectedColorSwitch.ID][n].SetSelected(false);
            }

            _selectedColorSwitch.SetSelected(false);
        }

        _selectedColorSwitch = _switch;
        _selectedColorSwitch.SetSelected(true);

        for (int n = 0; n < PixelGroups[_selectedColorSwitch.ID].Count; n++)
        {
            PixelGroups[_selectedColorSwitch.ID][n].SetSelected(true);
        }
    }
    bool CheckIfSelectedComplete()
    {
        if (_selectedColorSwitch != null)
        {
            for (int n = 0; n < PixelGroups[_selectedColorSwitch.ID].Count; n++)
            {
                if (PixelGroups[_selectedColorSwitch.ID][n].IsFilledIn == false)
                    return false;
            }
        }
        return true;
    }

}
