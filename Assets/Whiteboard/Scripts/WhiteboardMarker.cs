using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip; // Tip of the marker
    [SerializeField] private int _penSize = 5; // Size of the marker drawing area

    private Renderer _renderer;
    private Color[] _colors; // Color buffer for drawing
    private float _tipHeight;

    private RaycastHit _touch; // Raycast hit for detecting the whiteboard
    private Whiteboard _whiteboard; // Reference to the whiteboard being drawn on
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    [SerializeField] private Color _markerColor = Color.black; // Current marker color

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        UpdateColor(_markerColor); // Initialize with default color
        _tipHeight = _tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        // Cast a ray from the marker tip to detect the whiteboard
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }

                    transform.rotation = _lastTouchRot;

                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }

        _whiteboard = null;
        _touchedLastFrame = false;
    }

    /// <summary>
    /// Updates the marker's drawing color.
    /// </summary>
    /// <param name="newColor">The new color to set for the marker.</param>
    public void UpdateColor(Color newColor)
    {
        _markerColor = newColor;
        _renderer.material.color = _markerColor; // Change the tip's visual color

        // Update the color buffer used for drawing
        _colors = Enumerable.Repeat(_markerColor, _penSize * _penSize).ToArray();
    }

    /// <summary>
    /// Detects collision with objects and changes the marker's color.
    /// </summary>
    /// <param name="other">The collider of the object the marker collides with.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only change color if the collided object has the "ColorChanger" tag
        if (other.CompareTag("ColorChanger"))
        {
            Renderer collidedRenderer = other.GetComponent<Renderer>();
            if (collidedRenderer != null)
            {
                Color newColor = collidedRenderer.material.color; // Get the object's color
                UpdateColor(newColor); // Update the marker's color
            }
        }
    }
}
